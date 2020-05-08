// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Helpers.Internals
{
    /// <summary>
    /// Helpers to process sequences of values by reference.
    /// </summary>
    internal static partial class SpanHelper
    {
        /// <summary>
        /// Counts the number of occurrences of a given value into a target search space.
        /// </summary>
        /// <param name="r0">A <typeparamref name="T"/> reference to the start of the search space.</param>
        /// <param name="length">The number of items in the search space.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <typeparam name="T">The type of value to look for.</typeparam>
        /// <returns>The number of occurrences of <paramref name="value"/> in the search space</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(ref T r0, IntPtr length, T value)
            where T : IEquatable<T>
        {
            if (!Vector.IsHardwareAccelerated)
            {
                return CountSequential(ref r0, length, value);
            }

            // Special vectorized version when using a supported type
            if (typeof(T) == typeof(byte) ||
                typeof(T) == typeof(sbyte) ||
                typeof(T) == typeof(bool))
            {
                ref sbyte r1 = ref Unsafe.As<T, sbyte>(ref r0);
                sbyte target = Unsafe.As<T, sbyte>(ref value);

                return CountSimd(ref r1, length, target, (IntPtr)sbyte.MaxValue);
            }

            if (typeof(T) == typeof(char) ||
                typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(short))
            {
                ref short r1 = ref Unsafe.As<T, short>(ref r0);
                short target = Unsafe.As<T, short>(ref value);

                return CountSimd(ref r1, length, target, (IntPtr)short.MaxValue);
            }

            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(uint))
            {
                ref int r1 = ref Unsafe.As<T, int>(ref r0);
                int target = Unsafe.As<T, int>(ref value);

                return CountSimd(ref r1, length, target, (IntPtr)int.MaxValue);
            }

            if (typeof(T) == typeof(long) ||
                typeof(T) == typeof(ulong))
            {
                ref long r1 = ref Unsafe.As<T, long>(ref r0);
                long target = Unsafe.As<T, long>(ref value);

                return CountSimd(ref r1, length, target, (IntPtr)int.MaxValue);
            }

            return CountSequential(ref r0, length, value);
        }

        /// <summary>
        /// Implements <see cref="Count{T}"/> with a sequential search.
        /// </summary>
        [Pure]
#if NETCOREAPP3_1
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static unsafe int CountSequential<T>(ref T r0, IntPtr length, T value)
            where T : IEquatable<T>
        {
            int result = 0;

            IntPtr offset = default;

            // Main loop with 8 unrolled iterations
            while ((byte*)length >= (byte*)8)
            {
                result += Unsafe.Add(ref r0, offset + 0).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 1).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 2).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 3).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 4).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 5).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 6).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 7).Equals(value).ToInt();

                length -= 8;
                offset += 8;
            }

            if ((byte*)length >= (byte*)4)
            {
                result += Unsafe.Add(ref r0, offset + 0).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 1).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 2).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 3).Equals(value).ToInt();

                length -= 4;
                offset += 4;
            }

            // Iterate over the remaining values and count those that match
            while ((byte*)length > (byte*)0)
            {
                result += Unsafe.Add(ref r0, offset).Equals(value).ToInt();

                length -= 1;
                offset += 1;
            }

            return result;
        }

        /// <summary>
        /// Implements <see cref="Count{T}"/> with a vectorized search.
        /// </summary>
        [Pure]
#if NETCOREAPP3_1
        [MethodImpl(MethodImplOptions.AggressiveOptimization)]
#endif
        private static unsafe int CountSimd<T>(ref T r0, IntPtr length, T value, IntPtr max)
            where T : unmanaged, IEquatable<T>
        {
            int result = 0;

            IntPtr offset = default;

            // Skip the initialization overhead if there are not enough items
            if ((byte*)length >= (byte*)Vector<T>.Count)
            {
                var vc = new Vector<T>(value);

                do
                {
                    // Calculate the maximum sequential area that can be processed in
                    // one pass without the risk of numeric overflow in the dot product
                    // to sum the partial results. We also backup the current offset to
                    // be able to track how many items have been processed, which lets
                    // us avoid updating a third counter (length) in the loop body.
                    IntPtr
                        chunkLength = Min(length, max),
                        initialOffset = offset;

                    var partials = Vector<T>.Zero;

                    while ((byte*)chunkLength >= (byte*)Vector<T>.Count)
                    {
                        ref T ri = ref Unsafe.Add(ref r0, offset);

                        // Load the current Vector<T> register, and then use
                        // Vector.Equals to check for matches. This API sets the
                        // values corresponding to matching pairs to all 1s.
                        // Since the input type is guaranteed to always be signed,
                        // this means that a value with all 1s represents -1, as
                        // signed numbers are represented in two's complement.
                        // So we can just subtract this intermediate value to the
                        // partial results, which effectively sums 1 for each match.
                        var vi = Unsafe.As<T, Vector<T>>(ref ri);
                        var ve = Vector.Equals(vi, vc);

                        partials -= ve;

                        chunkLength -= Vector<T>.Count;
                        offset += Vector<T>.Count;
                    }

                    result += CastToInt(Vector.Dot(partials, Vector<T>.One));

                    length = Subtract(length, Subtract(offset, initialOffset));
                }
                while ((byte*)length >= (byte*)Vector<T>.Count);
            }

            // Optional 8 unrolled iterations. This is only done when a single SIMD
            // register can contain over 8 values of the current type, as otherwise
            // there could never be enough items left after the vectorized path
            if (Vector<T>.Count > 8 &&
                (byte*)length >= (byte*)8)
            {
                result += Unsafe.Add(ref r0, offset + 0).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 1).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 2).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 3).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 4).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 5).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 6).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 7).Equals(value).ToInt();

                length -= 8;
                offset += 8;
            }

            // Optional 4 unrolled iterations
            if (Vector<T>.Count > 4 &&
                (byte*)length >= (byte*)4)
            {
                result += Unsafe.Add(ref r0, offset + 0).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 1).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 2).Equals(value).ToInt();
                result += Unsafe.Add(ref r0, offset + 3).Equals(value).ToInt();

                length -= 4;
                offset += 4;
            }

            // Iterate over the remaining values and count those that match
            while ((byte*)length > (byte*)0)
            {
                result += Unsafe.Add(ref r0, offset).Equals(value).ToInt();

                length -= 1;
                offset += 1;
            }

            return result;
        }

        /// <summary>
        /// Returns the minimum between two <see cref="IntPtr"/> values.
        /// </summary>
        /// <param name="a">The first <see cref="IntPtr"/> value.</param>
        /// <param name="b">The second <see cref="IntPtr"/> value</param>
        /// <returns>The minimum between <paramref name="a"/> and <paramref name="b"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe IntPtr Min(IntPtr a, IntPtr b)
        {
            if (sizeof(IntPtr) == 4)
            {
                return (IntPtr)Math.Min((int)a, (int)b);
            }

            return (IntPtr)Math.Min((long)a, (long)b);
        }

        /// <summary>
        /// Returns the difference between two <see cref="IntPtr"/> values.
        /// </summary>
        /// <param name="a">The first <see cref="IntPtr"/> value.</param>
        /// <param name="b">The second <see cref="IntPtr"/> value</param>
        /// <returns>The difference between <paramref name="a"/> and <paramref name="b"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static unsafe IntPtr Subtract(IntPtr a, IntPtr b)
        {
            if (sizeof(IntPtr) == 4)
            {
                return (IntPtr)((int)a - (int)b);
            }

            return (IntPtr)((long)a - (long)b);
        }

        /// <summary>
        /// Casts a value of a given type to <see cref="int"/>.
        /// </summary>
        /// <typeparam name="T">The input type to cast.</typeparam>
        /// <param name="value">The input <typeparamref name="T"/> value to cast to <see cref="int"/>.</param>
        /// <returns>The <see cref="int"/> cast of <paramref name="value"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int CastToInt<T>(T value)
            where T : unmanaged
        {
            if (typeof(T) == typeof(sbyte))
            {
                return Unsafe.As<T, sbyte>(ref value);
            }

            if (typeof(T) == typeof(short))
            {
                return Unsafe.As<T, short>(ref value);
            }

            if (typeof(T) == typeof(int))
            {
                return Unsafe.As<T, int>(ref value);
            }

            if (typeof(T) == typeof(long))
            {
                return (int)Unsafe.As<T, long>(ref value);
            }

            throw new NotSupportedException($"Invalid input type {typeof(T)}");
        }
    }
}
