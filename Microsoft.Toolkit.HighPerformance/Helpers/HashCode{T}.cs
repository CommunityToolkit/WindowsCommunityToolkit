// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Helpers
{
    /// <summary>
    /// Combines the hash code of sequences of <typeparamref name="T"/> values into a single hash code.
    /// </summary>
    /// <typeparam name="T">The type of values to hash.</typeparam>
    public struct HashCode<T>
#if NETSTANDARD2_1
        where T : notnull
#else
        /* .NET Standard 2.0 doesn't have the API to check at runtime whether a
         * type satisfies the unmanaged constraint, se we enforce that at compile
         * time and only expose the APIs of this class in that case. */
        where T : unmanaged
#endif
    {
        /// <summary>
        /// Gets a content hash from the input <see cref="ReadOnlySpan{T}"/> instance using the xxHash32 algorithm.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance</param>
        /// <returns>The xxHash32 value for the input <see cref="ReadOnlySpan{T}"/> instance</returns>
        /// <remarks>The xxHash32 is only guaranteed to be deterministic within the scope of a single app execution</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Combine(ReadOnlySpan<T> span)
        {
            int hash = CombineValues(span);

            return HashCode.Combine(hash);
        }

        /// <summary>
        /// Gets a content hash from the input <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance</param>
        /// <returns>The hash code for the input <see cref="ReadOnlySpan{T}"/> instance</returns>
        /// <remarks>The returned hash code is not processed through <see cref="HashCode"/> APIs.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [SuppressMessage("StyleCop.CSharp.LayoutRules", "SA1515", Justification = "Compiler directive instead of whitespace")]
        internal static int CombineValues(ReadOnlySpan<T> span)
        {
            ref T r0 = ref MemoryMarshal.GetReference(span);

#if NETSTANDARD2_1
            /* If typeof(T) is not unmanaged, iterate over all the items one by one.
             * This check is always known in advance either by the JITter or by the AOT
             * compiler, so this branch will never actually be executed by the code. */
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                return CombineValues(ref r0, span.Length);
            }
#endif
            // Get the info for the target memory area to process
            ref byte rb = ref Unsafe.As<T, byte>(ref r0);
            long byteSize = (long)span.Length * Unsafe.SizeOf<T>();

            // Fast path if the source memory area is not large enough
            if (byteSize < BytesProcessor.MinimumSuggestedSize)
            {
                return CombineValues(ref r0, span.Length);
            }

            // Use the fast vectorized overload if the input span can be reinterpreted as a sequence of bytes
            return byteSize <= int.MaxValue
                ? BytesProcessor.CombineBytes(ref rb, unchecked((int)byteSize))
                : BytesProcessor.CombineBytes(ref rb, byteSize);
        }

        /// <summary>
        /// Gets a content hash from the input memory area.
        /// </summary>
        /// <param name="r0">A <typeparamref name="T"/> reference to the start of the memory area.</param>
        /// <param name="length">The size of the memory area.</param>
        /// <returns>The hash code for the input values.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static int CombineValues(ref T r0, int length)
        {
            int
                hash = 0, i = 0,
                end8 = length - 8;

            // Main loop with 8 unrolled iterations
            for (; i <= end8; i += 8)
            {
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 0).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 1).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 2).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 3).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 4).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 5).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 6).GetHashCode());
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i + 7).GetHashCode());
            }

            // Handle the leftover items
            for (; i < length; i++)
            {
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i).GetHashCode());
            }

            return hash;
        }
    }

    /// <summary>
    /// Combines the hash code of sequences of <see cref="byte"/> values into a single hash code.
    /// </summary>
    /// <remarks>
    /// This type is just a wrapper for the <see cref="CombineBytes(ref byte, int)"/> method,
    /// which is not defined within <see cref="HashCode{T}"/> for performance reasons.
    /// Because <see cref="HashCode{T}"/> is a generic type, each contained method will be JIT
    /// compiled into a different executable, even if it's not directly using the generic type
    /// parameters of the declaring type at all. Moving this method into a separate, non-generic
    /// type allows the runtime to always reuse a single JIT compilation.
    /// </remarks>
    internal static class BytesProcessor
    {
        /// <summary>
        /// The minimum suggested size for memory areas to process using the APIs in this class
        /// </summary>
        public const int MinimumSuggestedSize = 512;

        /// <summary>
        /// Gets a content hash from a given memory area.
        /// </summary>
        /// <param name="r0">A <see cref="byte"/> reference to the start of the memory area.</param>
        /// <param name="length">The size in bytes of the memory area.</param>
        /// <returns>The hash code for the contents of the source memory area.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int CombineBytes(ref byte r0, long length)
        {
            /* This method takes a long as the length parameter, which is needed in case
             * the target area is a large sequence of items with a byte size greater than
             * one. To maximize efficiency, the target memory area is divided into
             * contiguous blocks as large as possible, and the SIMD implementation
             * is executed on all of them one by one. The partial results
             * are accumulated with the usual hash function. */
            int
                hash = 0,
                runs = unchecked((int)(length / int.MaxValue)),
                trailing = unchecked((int)(length - (runs * (long)int.MaxValue)));

            // Process chunks of int.MaxValue consecutive bytes
            for (int i = 0; i < runs; i++)
            {
                int partial = CombineBytes(ref r0, int.MaxValue);

                hash = unchecked((hash * 397) ^ partial);

                r0 = ref Unsafe.Add(ref r0, int.MaxValue);
            }

            // Process the leftover elements
            int tail = CombineBytes(ref r0, trailing);

            hash = unchecked((hash * 397) ^ tail);

            return hash;
        }

        /// <summary>
        /// Gets a content hash from a given memory area.
        /// </summary>
        /// <param name="r0">A <see cref="byte"/> reference to the start of the memory area.</param>
        /// <param name="length">The size in bytes of the memory area.</param>
        /// <returns>The hash code for the contents of the source memory area.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.NoInlining)]
        public static int CombineBytes(ref byte r0, int length)
        {
            int hash = 0, i = 0;

            // Dedicated SIMD branch, if available
            if (Vector.IsHardwareAccelerated)
            {
                /* Test whether the total number of bytes is at least
                 * equal to the number that can fit in a single SIMD register.
                 * If that is not the case, skip the entire SIMD branch, which
                 * also saves the unnecessary computation of partial hash
                 * values from the accumulation register, and the loading
                 * of the prime constant in the secondary SIMD register. */
                if (length >= 8 * Vector<int>.Count * sizeof(int))
                {
                    var vh = Vector<int>.Zero;
                    var v397 = new Vector<int>(397);

                    /* First upper bound for the vectorized path.
                     * The first loop has sequences of 8 SIMD operations unrolled,
                     * so assuming that a SIMD register can hold 8 int values at a time,
                     * it processes 8 * Vector<int>.Count * sizeof(int), which results in
                     * 128 bytes on SSE registers, 256 on AVX2 and 512 on AVX512 registers. */
                    var end256 = length - (8 * Vector<int>.Count * sizeof(int));

                    for (; i <= end256; i += 8 * Vector<int>.Count * sizeof(int))
                    {
                        ref byte ri0 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 0) + i);
                        var vi0 = Unsafe.ReadUnaligned<Vector<int>>(ref ri0);
                        var vp0 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp0, vi0);

                        ref byte ri1 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 1) + i);
                        var vi1 = Unsafe.ReadUnaligned<Vector<int>>(ref ri1);
                        var vp1 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp1, vi1);

                        ref byte ri2 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 2) + i);
                        var vi2 = Unsafe.ReadUnaligned<Vector<int>>(ref ri2);
                        var vp2 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp2, vi2);

                        ref byte ri3 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 3) + i);
                        var vi3 = Unsafe.ReadUnaligned<Vector<int>>(ref ri3);
                        var vp3 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp3, vi3);

                        ref byte ri4 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 4) + i);
                        var vi4 = Unsafe.ReadUnaligned<Vector<int>>(ref ri4);
                        var vp4 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp4, vi4);

                        ref byte ri5 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 5) + i);
                        var vi5 = Unsafe.ReadUnaligned<Vector<int>>(ref ri5);
                        var vp5 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp5, vi5);

                        ref byte ri6 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 6) + i);
                        var vi6 = Unsafe.ReadUnaligned<Vector<int>>(ref ri6);
                        var vp6 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp6, vi6);

                        ref byte ri7 = ref Unsafe.Add(ref r0, (Vector<int>.Count * sizeof(int) * 7) + i);
                        var vi7 = Unsafe.ReadUnaligned<Vector<int>>(ref ri7);
                        var vp7 = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp7, vi7);
                    }

                    /* Second upper bound for the vectorized path.
                     * Each iteration processes 16 bytes on SSE, 32 bytes on AVX2
                     * and 64 on AVX512 registers. When this point is reached,
                     * it means that there are at most 127 bytes remaining on SSE,
                     * or 255 on AVX2, or 511 on AVX512 systems.*/
                    var end32 = length - i - (Vector<int>.Count * sizeof(int));

                    for (; i <= end32; i += Vector<int>.Count * sizeof(int))
                    {
                        ref byte ri = ref Unsafe.Add(ref r0, i);
                        var vi = Unsafe.ReadUnaligned<Vector<int>>(ref ri);
                        var vp = Vector.Multiply(vh, v397);
                        vh = Vector.Xor(vp, vi);
                    }

                    /* Combine the partial hash values in each position.
                     * The loop below is automatically unrolled by the JIT. */
                    for (var j = 0; j < Vector<int>.Count; j++)
                    {
                        hash = unchecked((hash * 397) ^ vh[j]);
                    }
                }

                /* At this point, regardless of whether or not the previous
                 * branch was taken, there are at most 15 unprocessed bytes
                 * on SSE systems, 31 on AVX2 systems and 63 on AVX512 systems. */
            }
            else
            {
                // Process groups of 64 bytes at a time
                var end64 = length - (8 * sizeof(ulong));

                for (; i <= end64; i += 8 * sizeof(ulong))
                {
                    ref byte ri0 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 0) + i);
                    var value0 = Unsafe.ReadUnaligned<ulong>(ref ri0);
                    hash = unchecked((hash * 397) ^ (int)value0 ^ (int)(value0 >> 32));

                    ref byte ri1 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 1) + i);
                    var value1 = Unsafe.ReadUnaligned<ulong>(ref ri1);
                    hash = unchecked((hash * 397) ^ (int)value1 ^ (int)(value1 >> 32));

                    ref byte ri2 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 2) + i);
                    var value2 = Unsafe.ReadUnaligned<ulong>(ref ri2);
                    hash = unchecked((hash * 397) ^ (int)value2 ^ (int)(value2 >> 32));

                    ref byte ri3 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 3) + i);
                    var value3 = Unsafe.ReadUnaligned<ulong>(ref ri3);
                    hash = unchecked((hash * 397) ^ (int)value3 ^ (int)(value3 >> 32));

                    ref byte ri4 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 4) + i);
                    var value4 = Unsafe.ReadUnaligned<ulong>(ref ri4);
                    hash = unchecked((hash * 397) ^ (int)value4 ^ (int)(value4 >> 32));

                    ref byte ri5 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 5) + i);
                    var value5 = Unsafe.ReadUnaligned<ulong>(ref ri5);
                    hash = unchecked((hash * 397) ^ (int)value5 ^ (int)(value5 >> 32));

                    ref byte ri6 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 6) + i);
                    var value6 = Unsafe.ReadUnaligned<ulong>(ref ri6);
                    hash = unchecked((hash * 397) ^ (int)value6 ^ (int)(value6 >> 32));

                    ref byte ri7 = ref Unsafe.Add(ref r0, (sizeof(ulong) * 7) + i);
                    var value7 = Unsafe.ReadUnaligned<ulong>(ref ri7);
                    hash = unchecked((hash * 397) ^ (int)value7 ^ (int)(value7 >> 32));
                }

                /* At this point, there are up to 63 bytes left.
                 * If there are at least 32, unroll that iteration with
                 * the same procedure as before, but as uint values. */
                if (length - i >= 8 * sizeof(uint))
                {
                    ref byte ri0 = ref Unsafe.Add(ref r0, (sizeof(uint) * 0) + i);
                    var value0 = Unsafe.ReadUnaligned<uint>(ref ri0);
                    hash = unchecked((hash * 397) ^ (int)value0);

                    ref byte ri1 = ref Unsafe.Add(ref r0, (sizeof(uint) * 1) + i);
                    var value1 = Unsafe.ReadUnaligned<uint>(ref ri1);
                    hash = unchecked((hash * 397) ^ (int)value1);

                    ref byte ri2 = ref Unsafe.Add(ref r0, (sizeof(uint) * 2) + i);
                    var value2 = Unsafe.ReadUnaligned<uint>(ref ri2);
                    hash = unchecked((hash * 397) ^ (int)value2);

                    ref byte ri3 = ref Unsafe.Add(ref r0, (sizeof(uint) * 3) + i);
                    var value3 = Unsafe.ReadUnaligned<uint>(ref ri3);
                    hash = unchecked((hash * 397) ^ (int)value3);

                    ref byte ri4 = ref Unsafe.Add(ref r0, (sizeof(uint) * 4) + i);
                    var value4 = Unsafe.ReadUnaligned<uint>(ref ri4);
                    hash = unchecked((hash * 397) ^ (int)value4);

                    ref byte ri5 = ref Unsafe.Add(ref r0, (sizeof(uint) * 5) + i);
                    var value5 = Unsafe.ReadUnaligned<uint>(ref ri5);
                    hash = unchecked((hash * 397) ^ (int)value5);

                    ref byte ri6 = ref Unsafe.Add(ref r0, (sizeof(uint) * 6) + i);
                    var value6 = Unsafe.ReadUnaligned<uint>(ref ri6);
                    hash = unchecked((hash * 397) ^ (int)value6);

                    ref byte ri7 = ref Unsafe.Add(ref r0, (sizeof(uint) * 7) + i);
                    var value7 = Unsafe.ReadUnaligned<uint>(ref ri7);
                    hash = unchecked((hash * 397) ^ (int)value7);
                }

                // The non-SIMD path leaves up to 31 unprocessed bytes
            }

            /* At this point there might be up to 31 bytes left on both AVX2 systems,
             * and on systems with no hardware accelerated SIMD registers.
             * That number would go up to 63 on AVX512 systems, in which case it is
             * still useful to perform this last loop unrolling.
             * The only case where this branch is never taken is on SSE systems,
             * but since those are not so common anyway the code is left here for simplicity.
             * What follows is the same procedure as before, but with ushort values,
             * so that if there are at least 16 bytes available, those
             * will all be processed in a single unrolled iteration. */
            if (length - i >= 8 * sizeof(ushort))
            {
                ref byte ri0 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 0) + i);
                var value0 = Unsafe.ReadUnaligned<ushort>(ref ri0);
                hash = unchecked((hash * 397) ^ value0);

                ref byte ri1 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 1) + i);
                var value1 = Unsafe.ReadUnaligned<ushort>(ref ri1);
                hash = unchecked((hash * 397) ^ value1);

                ref byte ri2 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 2) + i);
                var value2 = Unsafe.ReadUnaligned<ushort>(ref ri2);
                hash = unchecked((hash * 397) ^ value2);

                ref byte ri3 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 3) + i);
                var value3 = Unsafe.ReadUnaligned<ushort>(ref ri3);
                hash = unchecked((hash * 397) ^ value3);

                ref byte ri4 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 4) + i);
                var value4 = Unsafe.ReadUnaligned<ushort>(ref ri4);
                hash = unchecked((hash * 397) ^ value4);

                ref byte ri5 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 5) + i);
                var value5 = Unsafe.ReadUnaligned<ushort>(ref ri5);
                hash = unchecked((hash * 397) ^ value5);

                ref byte ri6 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 6) + i);
                var value6 = Unsafe.ReadUnaligned<ushort>(ref ri6);
                hash = unchecked((hash * 397) ^ value6);

                ref byte ri7 = ref Unsafe.Add(ref r0, (sizeof(ushort) * 7) + i);
                var value7 = Unsafe.ReadUnaligned<ushort>(ref ri7);
                hash = unchecked((hash * 397) ^ value7);
            }

            // Handle the leftover items
            for (; i < length; i++)
            {
                hash = unchecked((hash * 397) ^ Unsafe.Add(ref r0, i));
            }

            return hash;
        }
    }
}
