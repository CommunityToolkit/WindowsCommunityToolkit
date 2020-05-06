// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="IBufferWriter{T}"/> type.
    /// </summary>
    public static class IBufferWriterExtensions
    {
        /// <summary>
        /// Writes a <see cref="byte"/> into a target <see cref="IBufferWriter{T}"/> instance.
        /// </summary>
        /// <param name="writer">The target <see cref="IBufferWriter{T}"/> instance to write to.</param>
        /// <param name="value">The input value to write to <paramref name="writer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this IBufferWriter<byte> writer, byte value)
        {
            Span<byte> span = writer.GetSpan(1);

            span[0] = value;

            writer.Advance(1);
        }

        /// <summary>
        /// Writes a value of a specified type into a target <see cref="IBufferWriter{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="writer">The target <see cref="IBufferWriter{T}"/> instance to write to.</param>
        /// <param name="value">The input value to write to <paramref name="writer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IBufferWriter<byte> writer, in T value)
            where T : unmanaged
        {
            int length = Unsafe.SizeOf<T>();
            Span<byte> span = writer.GetSpan(length);

            MemoryMarshal.Write(span, ref Unsafe.AsRef(value));

            writer.Advance(length);
        }

        /// <summary>
        /// Writes a value of a specified type into a target <see cref="IBufferWriter{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="writer">The target <see cref="IBufferWriter{T}"/> instance to write to.</param>
        /// <param name="value">The input value to write to <paramref name="writer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IBufferWriter<T> writer, T value)
        {
            Span<T> span = writer.GetSpan(1);

            span[0] = value;

            writer.Advance(1);
        }

        /// <summary>
        /// Writes a series of bytes of a specified type into a target <see cref="IBufferWriter{T}"/> instance.
        /// </summary>
        /// <param name="writer">The target <see cref="IBufferWriter{T}"/> instance to write to.</param>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> to write to <paramref name="writer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write(this IBufferWriter<byte> writer, ReadOnlySpan<byte> span)
        {
            Span<byte> destination = writer.GetSpan(span.Length);

            span.CopyTo(destination);

            writer.Advance(span.Length);
        }

        /// <summary>
        /// Writes a series of items of a specified type into a target <see cref="IBufferWriter{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="writer">The target <see cref="IBufferWriter{T}"/> instance to write to.</param>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> to write to <paramref name="writer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IBufferWriter<byte> writer, ReadOnlySpan<T> span)
            where T : unmanaged
        {
            ReadOnlySpan<byte> source = MemoryMarshal.AsBytes(span);
            Span<byte> destination = writer.GetSpan(source.Length);

            source.CopyTo(destination);

            writer.Advance(source.Length);
        }

        /// <summary>
        /// Writes a series of items of a specified type into a target <see cref="IBufferWriter{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of value to write.</typeparam>
        /// <param name="writer">The target <see cref="IBufferWriter{T}"/> instance to write to.</param>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> to write to <paramref name="writer"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Write<T>(this IBufferWriter<T> writer, ReadOnlySpan<T> span)
            where T : unmanaged
        {
            Span<T> destination = writer.GetSpan(span.Length);

            span.CopyTo(destination);

            writer.Advance(span.Length);
        }
    }
}
