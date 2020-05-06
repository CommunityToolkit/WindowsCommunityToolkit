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
        [MethodImpl(MethodImplOptions.NoInlining)]
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

                return CountSimd(ref r1, length, target, sbyte.MaxValue);
            }

            if (typeof(T) == typeof(char) ||
                typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(short))
            {
                ref short r1 = ref Unsafe.As<T, short>(ref r0);
                short target = Unsafe.As<T, short>(ref value);

                return CountSimd(ref r1, length, target, short.MaxValue);
            }

            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(uint))
            {
                ref int r1 = ref Unsafe.As<T, int>(ref r0);
                int target = Unsafe.As<T, int>(ref value);

                return CountSimd(ref r1, length, target, int.MaxValue);
            }

            if (typeof(T) == typeof(long) ||
                typeof(T) == typeof(ulong))
            {
                ref long r1 = ref Unsafe.As<T, long>(ref r0);
                long target = Unsafe.As<T, long>(ref value);

                return CountSimd(ref r1, length, target, int.MaxValue);
            }

            return CountSequential(ref r0, length, value);
        }

        /// <summary>
        /// Implements <see cref="Count{T}"/> with a sequential search.
        /// </summary>
        [Pure]
        [MethodImpl(MethodImplOptions.NoInlining)]
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
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static unsafe int CountSimd<T>(ref T r0, IntPtr length, T value, int max)
            where T : unmanaged, IEquatable<T>
        {
            int result = 0;

            IntPtr offset = default;

            // Only run the SIMD-enabled branch if the Vector<T> APIs are hardware accelerated
            if (Vector.IsHardwareAccelerated)
            {
                var partials = Vector<T>.Zero;
                var vc = new Vector<T>(value);

                // Run the fast path if the input span is short enough.
                // There are two branches here because if the search space is large
                // enough, the partial results could overflow if the target value
                // is present too many times. This upper limit depends on the type
                // being used, as the smaller the type is, the shorter the supported
                // fast range will be. Therefore, if the input span is longer than that
                // minimum threshold, additional checks need to be performed to avoid overflows.
                // This value is equal to the maximum (signed) numerical value for the current
                // type, divided by the number of value that can fit in a register, minus 1.
                // This is because the partial results are accumulated with a dot product,
                // which sums them horizontally while still working on the original type.
                // The check is moved outside of the loop to enable a branchless version
                // of this method if the input span is guaranteed not to cause overflows.
                // Otherwise, the safe but slower variant is used.
                // Note that even the slower vectorized version is still much
                // faster than just doing a linear search without using vectorization.
                int threshold = (max / Vector<T>.Count) - 1;

                if ((byte*)length <= (byte*)threshold)
                {
                    while ((byte*)length >= (byte*)Vector<T>.Count)
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

                        length -= Vector<T>.Count;
                        offset += Vector<T>.Count;
                    }
                }
                else
                {
                    for (int j = 0; (byte*)length >= (byte*)Vector<T>.Count; j++)
                    {
                        ref T ri = ref Unsafe.Add(ref r0, offset);

                        var vi = Unsafe.As<T, Vector<T>>(ref ri);
                        var ve = Vector.Equals(vi, vc);

                        partials -= ve;

                        length -= Vector<T>.Count;
                        offset += Vector<T>.Count;

                        // Additional checks to avoid overflows
                        if (j == threshold)
                        {
                            j = 0;
                            result += CastToInt(Vector.Dot(partials, Vector<T>.One));
                            partials = Vector<T>.Zero;
                        }
                    }
                }

                // Compute the horizontal sum of the partial results
                result += CastToInt(Vector.Dot(partials, Vector<T>.One));
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

            throw new ArgumentException($"Invalid input type {typeof(T)}", nameof(value));
        }
    }
}
