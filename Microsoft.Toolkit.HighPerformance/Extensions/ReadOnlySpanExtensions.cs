// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Enumerables;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ReadOnlySpan{T}"/> type.
    /// </summary>
    public static partial class ReadOnlySpanExtensions
    {
        /// <summary>
        /// Returns a reference to the first element within a given <see cref="ReadOnlySpan{T}"/>, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <returns>A reference to the first element within <paramref name="span"/>.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReference<T>(this ReadOnlySpan<T> span)
        {
            return ref MemoryMarshal.GetReference(span);
        }

        /// <summary>
        /// Returns a reference to an element at a specified index within a given <see cref="ReadOnlySpan{T}"/>, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <param name="i">The index of the element to retrieve within <paramref name="span"/>.</param>
        /// <returns>A reference to the element within <paramref name="span"/> at the index specified by <paramref name="i"/>.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, int i)
        {
            ref T r0 = ref MemoryMarshal.GetReference(span);
            ref T ri = ref Unsafe.Add(ref r0, i);

            return ref ri;
        }

        /// <summary>
        /// Casts a <see cref="ReadOnlySpan{T}"/> of one primitive type <typeparamref name="T"/> to <see cref="ReadOnlySpan{T}"/> of bytes.
        /// That type may not contain pointers or references. This is checked at runtime in order to preserve type safety.
        /// </summary>
        /// <typeparam name="T">The type if items in the source <see cref="ReadOnlySpan{T}"/></typeparam>
        /// <param name="span">The source slice, of type <typeparamref name="T"/>.</param>
        /// <returns>A <see cref="ReadOnlySpan{T}"/> of bytes.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="T"/> contains pointers.
        /// </exception>
        /// <exception cref="OverflowException">
        /// Thrown if the <see cref="ReadOnlySpan{T}.Length"/> property of the new <see cref="ReadOnlySpan{T}"/> would exceed <see cref="int.MaxValue"/>.
        /// </exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<byte> AsBytes<T>(this ReadOnlySpan<T> span)
            where T : unmanaged
        {
            return MemoryMarshal.AsBytes(span);
        }

        /// <summary>
        /// Casts a <see cref="ReadOnlySpan{T}"/> of one primitive type <typeparamref name="TFrom"/> to another primitive type <typeparamref name="TTo"/>.
        /// These types may not contain pointers or references. This is checked at runtime in order to preserve type safety.
        /// </summary>
        /// <typeparam name="TFrom">The type of items in the source <see cref="ReadOnlySpan{T}"/>.</typeparam>
        /// <typeparam name="TTo">The type of items in the destination <see cref="ReadOnlySpan{T}"/>.</typeparam>
        /// <param name="span">The source slice, of type <typeparamref name="TFrom"/>.</param>
        /// <returns>A <see cref="ReadOnlySpan{T}"/> of type <typeparamref name="TTo"/></returns>
        /// <remarks>
        /// Supported only for platforms that support misaligned memory access or when the memory block is aligned by other means.
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="TFrom"/> or <typeparamref name="TTo"/> contains pointers.
        /// </exception>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpan<TTo> Cast<TFrom, TTo>(this ReadOnlySpan<TFrom> span)
            where TFrom : struct
            where TTo : struct
        {
            return MemoryMarshal.Cast<TFrom, TTo>(span);
        }

        /// <summary>
        /// Enumerates the items in the input <see cref="ReadOnlySpan{T}"/> instance, as pairs of value/index values.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// ReadOnlySpan&lt;string&gt; words = new[] { "Hello", ", ", "world", "!" };
        ///
        /// foreach (var item in words.Enumerate())
        /// {
        ///     // Access the index and value of each item here...
        ///     int index = item.Index;
        ///     string value = item.Value;
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of items to enumerate.</typeparam>
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to enumerate.</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="span"/>.</returns>
        /// <remarks>The returned <see cref="ReadOnlySpanEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanEnumerable<T> Enumerate<T>(this ReadOnlySpan<T> span)
        {
            return new ReadOnlySpanEnumerable<T>(span);
        }

        /// <summary>
        /// Tokenizes the values in the input <see cref="ReadOnlySpan{T}"/> instance using a specified separator.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// ReadOnlySpan&lt;char&gt; text = "Hello, world!";
        ///
        /// foreach (var token in text.Tokenize(','))
        /// {
        ///     // Access the tokens here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of items in the <see cref="ReadOnlySpan{T}"/> to tokenize.</typeparam>
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to tokenize.</param>
        /// <param name="separator">The separator <typeparamref name="T"/> item to use.</param>
        /// <returns>A wrapper type that will handle the tokenization for <paramref name="span"/>.</returns>
        /// <remarks>The returned <see cref="ReadOnlySpanTokenizer{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTokenizer<T> Tokenize<T>(this ReadOnlySpan<T> span, T separator)
            where T : IEquatable<T>
        {
            return new ReadOnlySpanTokenizer<T>(span, separator);
        }

        /// <summary>
        /// Gets a content hash from the input <see cref="ReadOnlySpan{T}"/> instance using the Djb2 algorithm.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="ReadOnlySpan{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <returns>The Djb2 value for the input <see cref="ReadOnlySpan{T}"/> instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        public static int GetDjb2HashCode<T>(this ReadOnlySpan<T> span)
            where T : notnull
        {
            ref T r0 = ref MemoryMarshal.GetReference(span);
            int
                hash = 5381,
                length = span.Length,
                i = 0,
                end8 = length - 8;

            // Main loop with 8 unrolled iterations
            for (; i <= end8; i += 8)
            {
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 0).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 1).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 2).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 3).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 4).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 5).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 6).GetHashCode());
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i + 7).GetHashCode());
            }

            // Handle the leftover items
            for (; i < length; i++)
            {
                hash = unchecked((hash << 5) + hash + Unsafe.Add(ref r0, i).GetHashCode());
            }

            return hash;
        }
    }
}
