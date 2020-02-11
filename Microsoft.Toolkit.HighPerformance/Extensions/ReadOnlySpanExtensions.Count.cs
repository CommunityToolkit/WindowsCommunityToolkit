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
        /// Counts the number of occurrences of a given character into a target <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance to read.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="span"/>.</returns>
        [Pure]
        public static int Count<T>(this ReadOnlySpan<T> span, T value)
            where T : struct, IEquatable<T>
        {
            // Special vectorized version when using the char type
            if (typeof(T) == typeof(char))
            {
                ref T r0 = ref MemoryMarshal.GetReference(span);
                ref char r1 = ref Unsafe.As<T, char>(ref r0);
                int length = span.Length;
                char c = Unsafe.As<T, char>(ref value);

                return Count(ref r1, length, c);
            }

            int result = 0;

            /* Fast loop for all the other types.
             * The Equals<T> call is automatically inlined, if possible. */
            foreach (var item in span)
            {
                bool equals = item.Equals(value);
                result += Unsafe.As<bool, byte>(ref equals);
            }

            return result;
        }

        /// <summary>
        /// Counts the number of occurrences of a given character into a target search space
        /// </summary>
        /// <param name="r0">A <see cref="char"/> reference to the start of the search space</param>
        /// <param name="length">The number of items in the search space</param>
        /// <param name="c">The character to look for</param>
        /// <returns>The number of occurrences of <paramref name="c"/> in the search space</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private static int Count(ref char r0, int length, char c)
        {
            int i = 0, result = 0;

            /* Only execute the SIMD-enabled branch if the Vector<T> APIs
             * are hardware accelerated on the current CPU.
             * Vector<char> is not supported, but the type is equivalent to
             * ushort anyway, as they're both unsigned 16 bits integers. */
            if (Vector.IsHardwareAccelerated)
            {
                int end = length - Vector<ushort>.Count;

                var partials = Vector<short>.Zero;
                var vc = new Vector<ushort>(c);

                /* Run the fast path if the input span is short enough.
                 * Since a Vector<short> is being used to sum the partial results,
                 * it means that a single SIMD value can count up to 32767 without
                 * overflowing. In the worst case scenario, the same character appears
                 * always at the offset aligned with the same SIMD value in the current
                 * register. Therefore, if the input span is longer than that minimum
                 * threshold, additional checks need to be performed to avoid overflows.
                 * The check is moved outside of the loop to enable a branchless version
                 * of this method if the input span is short enough.
                 * Otherwise, the safe but slower variant is used. */
                if (length <= short.MaxValue)
                {
                    for (; i <= end; i += Vector<ushort>.Count)
                    {
                        ref char ri = ref Unsafe.Add(ref r0, i);

                        /* Load the current Vector<ushort> register.
                         * Vector.Equals sets matching positions to all 1s, and
                         * Vector.BitwiseAnd results in a Vector<ushort> with 1
                         * in positions corresponding to matching characters,
                         * and 0 otherwise. The final += is also calling the
                         * right vectorized instruction automatically. */
                        var vi = Unsafe.As<char, Vector<ushort>>(ref ri);
                        var ve = Vector.Equals(vi, vc);
                        var vs = Unsafe.As<Vector<ushort>, Vector<short>>(ref ve);

                        partials -= vs;
                    }
                }
                else
                {
                    for (; i <= end; i += Vector<ushort>.Count)
                    {
                        ref char ri = ref Unsafe.Add(ref r0, i);

                        // Same as before
                        var vi = Unsafe.As<char, Vector<ushort>>(ref ri);
                        var ve = Vector.Equals(vi, vc);
                        var vs = Unsafe.As<Vector<ushort>, Vector<short>>(ref ve);

                        partials -= vs;

                        // Additional checks to avoid overflows
                        if (i % ((short.MaxValue + 1) / 2) == 0)
                        {
                            result += Vector.Dot(partials, Vector<short>.One);
                            partials = Vector<short>.Zero;
                        }
                    }
                }

                // Compute the horizontal sum of the partial results
                result += Vector.Dot(partials, Vector<short>.One);
            }
            else
            {
                result = 0;
            }

            // Iterate over the remaining characters and count those that match
            for (; i < length; i++)
            {
                /* Skip a conditional jump by assigning the comparison
                 * result to a variable and reinterpreting a reference to
                 * it as a byte reference. The byte value is then implicitly
                 * cast to int before adding it to the result. */
                bool equals = Unsafe.Add(ref r0, i) == c;
                result += Unsafe.As<bool, byte>(ref equals);
            }

            return result;
        }
    }
}
