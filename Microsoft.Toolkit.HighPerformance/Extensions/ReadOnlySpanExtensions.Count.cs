// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ReadOnlySpan{T}"/> type.
    /// </summary>
    public static partial class ReadOnlySpanExtensions
    {
        /// <summary>
        /// Counts the number of occurrences of a given value into a target <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to read.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="span"/>.</returns>
        [Pure]
        public static int Count<T>(this ReadOnlySpan<T> span, T value)
            where T : IEquatable<T>
        {
            // Special vectorized version when using a supported type
            if (typeof(T) == typeof(byte) ||
                typeof(T) == typeof(sbyte) ||
                typeof(T) == typeof(bool))
            {
                ref T r0 = ref MemoryMarshal.GetReference(span);
                ref sbyte r1 = ref Unsafe.As<T, sbyte>(ref r0);
                int length = span.Length;
                sbyte target = Unsafe.As<T, sbyte>(ref value);

                return Count<sbyte, SbyteConverter>(ref r1, length, target, sbyte.MaxValue);
            }

            if (typeof(T) == typeof(char) ||
                typeof(T) == typeof(ushort) ||
                typeof(T) == typeof(short))
            {
                ref T r0 = ref MemoryMarshal.GetReference(span);
                ref short r1 = ref Unsafe.As<T, short>(ref r0);
                int length = span.Length;
                short target = Unsafe.As<T, short>(ref value);

                return Count<short, ShortConverter>(ref r1, length, target, short.MaxValue);
            }

            if (typeof(T) == typeof(int) ||
                typeof(T) == typeof(uint))
            {
                ref T r0 = ref MemoryMarshal.GetReference(span);
                ref int r1 = ref Unsafe.As<T, int>(ref r0);
                int length = span.Length;
                int target = Unsafe.As<T, int>(ref value);

                return Count<int, IntConverter>(ref r1, length, target, int.MaxValue);
            }

            if (typeof(T) == typeof(long) ||
                typeof(T) == typeof(ulong))
            {
                ref T r0 = ref MemoryMarshal.GetReference(span);
                ref long r1 = ref Unsafe.As<T, long>(ref r0);
                int length = span.Length;
                long target = Unsafe.As<T, long>(ref value);

                return Count<long, LongConverter>(ref r1, length, target, int.MaxValue);
            }

            int result = 0;

            // Fast loop for all the other types (see below for more details)
            foreach (var item in span)
            {
                bool equals = item.Equals(value);
                result += Unsafe.As<bool, byte>(ref equals);
            }

            return result;
        }

        /// <summary>
        /// Counts the number of occurrences of a given value into a target search space.
        /// </summary>
        /// <param name="r0">A <see cref="char"/> reference to the start of the search space.</param>
        /// <param name="length">The number of items in the search space.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <param name="limit">The limit for consecutive SIMD operations without the risk of overflows.</param>
        /// <typeparam name="T">The type of value to look for.</typeparam>
        /// <typeparam name="TIntConverter">The type implementing <see cref="IIntConverter{T}"/> to use.</typeparam>
        /// <returns>The number of occurrences of <paramref name="value"/> in the search space</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Count<T, TIntConverter>(ref T r0, int length, T value, int limit)
            where T : unmanaged, IEquatable<T>
            where TIntConverter : unmanaged, IIntConverter<T>
        {
            int i = 0, result = 0;

            // Only run the SIMD-enabled branch if the Vector<T> APIs are hardware accelerated
            if (Vector.IsHardwareAccelerated)
            {
                int end = length - Vector<T>.Count;

                var partials = Vector<T>.Zero;
                var vc = new Vector<T>(value);

                var converter = default(TIntConverter);

                /* Run the fast path if the input span is short enough.
                 * There are two branches here because if the search space is large
                 * enough, the partial results could overflow if the target value
                 * is present too many times. This upper limit depends on the type
                 * being used, as the smaller the type is, the shorter the supported
                 * fast range will be. In the worst case scenario, the same value appears
                 * always at the offset aligned with the same SIMD value in the current
                 * register. Therefore, if the input span is longer than that minimum
                 * threshold, additional checks need to be performed to avoid overflows.
                 * The check is moved outside of the loop to enable a branchless version
                 * of this method if the input span is guaranteed not to cause overflows.
                 * Otherwise, the safe but slower variant is used. */
                if (length <= limit)
                {
                    for (; i <= end; i += Vector<T>.Count)
                    {
                        ref T ri = ref Unsafe.Add(ref r0, i);

                        /* Load the current Vector<T> register, and then use
                         * Vector.Equals to check for matches. This API sets the
                         * values corresponding to matching pairs to all 1s.
                         * Since the input type is guaranteed to always be signed,
                         * this means that a value with all 1s represents -1, as
                         * signed numbers are represented in two's complement.
                         * So we can subtract this intermediate value to the
                         * partial results, which effectively sums 1 for each match. */
                        var vi = Unsafe.As<T, Vector<T>>(ref ri);
                        var ve = Vector.Equals(vi, vc);

                        partials -= ve;
                    }
                }
                else
                {
                    for (; i <= end; i += Vector<T>.Count)
                    {
                        ref T ri = ref Unsafe.Add(ref r0, i);

                        // Same as before
                        var vi = Unsafe.As<T, Vector<T>>(ref ri);
                        var ve = Vector.Equals(vi, vc);

                        partials -= ve;

                        // Additional checks to avoid overflows
                        if (i % ((limit + 1) / 2) == 0)
                        {
                            result += converter.Convert(Vector.Dot(partials, Vector<T>.One));
                            partials = Vector<T>.Zero;
                        }
                    }
                }

                // Compute the horizontal sum of the partial results
                result += converter.Convert(Vector.Dot(partials, Vector<T>.One));
            }

            // Iterate over the remaining values and count those that match
            for (; i < length; i++)
            {
                /* Skip a conditional jump by assigning the comparison
                 * result to a variable and reinterpreting a reference to
                 * it as a byte reference. The byte value is then implicitly
                 * cast to int before adding it to the result. Additionally,
                 * the JIT will automatically inline the calls to Equals<T>. */
                bool equals = Unsafe.Add(ref r0, i).Equals(value);
                result += Unsafe.As<bool, byte>(ref equals);
            }

            return result;
        }

        /// <summary>
        /// A contract for converters that turn values into <see cref="int"/> values.
        /// </summary>
        /// <typeparam name="T">The type of input values to convert.</typeparam>
        /// <remarks>The <see cref="Convert"/> method should be marked with <see cref="MethodImplOptions.AggressiveInlining"/>.</remarks>
        private interface IIntConverter<in T>
            where T : unmanaged
        {
            /// <summary>
            /// Converts a <typeparamref name="T"/> value into an <see cref="int"/> value.
            /// </summary>
            /// <param name="value">The input <typeparamref name="T"/> value to convert.</param>
            /// <returns>The <see cref="int"/> conversion of <paramref name="value"/>.</returns>
            int Convert(T value);
        }

        /// <summary>
        /// A type implementing the <see cref="IIntConverter{T}"/> contract for <see cref="sbyte"/> values.
        /// </summary>
        private readonly struct SbyteConverter : IIntConverter<sbyte>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Convert(sbyte value) => value;
        }

        /// <summary>
        /// A type implementing the <see cref="IIntConverter{T}"/> contract for <see cref="short"/> values.
        /// </summary>
        private readonly struct ShortConverter : IIntConverter<short>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Convert(short value) => value;
        }

        /// <summary>
        /// A type implementing the <see cref="IIntConverter{T}"/> contract for <see cref="int"/> values.
        /// </summary>
        private readonly struct IntConverter : IIntConverter<int>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Convert(int value) => value;
        }

        /// <summary>
        /// A type implementing the <see cref="IIntConverter{T}"/> contract for <see cref="long"/> values.
        /// </summary>
        private readonly struct LongConverter : IIntConverter<long>
        {
            /// <inheritdoc/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public int Convert(long value) => (int)value;
        }
    }
}
