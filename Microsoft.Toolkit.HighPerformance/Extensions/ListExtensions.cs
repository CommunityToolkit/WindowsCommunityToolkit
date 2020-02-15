// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Enumerables;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="List{T}"/> type.
    /// </summary>
    public static class ListExtensions
    {
        /// <summary>
        /// Returns a reference to the first element within a given <see cref="List{T}"/>, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="List{T}"/> instance.</typeparam>
        /// <param name="list">The input <see cref="List{T}"/> instance.</param>
        /// <returns>A reference to the first element within <paramref name="list"/>, or the location it would have used, if <paramref name="list"/> is empty.</returns>
        /// <remarks>
        /// This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.
        /// Furthermore, <paramref name="list"/> should not be modified while using the returned reference, as its underlying update might be swapped out when doing so.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReference<T>(this List<T> list)
        {
            var listData = Unsafe.As<RawListData<T>>(list);
            ref T r0 = ref listData._items.DangerousGetReference();

            return ref r0;
        }

        /// <summary>
        /// Returns a reference to an element at a specified index within a given <see cref="List{T}"/>, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="List{T}"/> instance.</typeparam>
        /// <param name="list">The input <see cref="List{T}"/> instance.</param>
        /// <param name="i">The index of the element to retrieve within <paramref name="list"/>.</param>
        /// <returns>A reference to the element within <paramref name="list"/> at the index specified by <paramref name="i"/>.</returns>
        /// <remarks>
        /// This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.
        /// Furthermore, <paramref name="list"/> should not be modified while using the returned reference, as its underlying update might be swapped out when doing so.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReferenceAt<T>(this List<T> list, int i)
        {
            var listData = Unsafe.As<RawListData<T>>(list);
            ref T ri = ref listData._items.DangerousGetReferenceAt(i);

            return ref ri;
        }

        /// <summary>
        /// Returns the underlying <typeparamref name="T"/> array being used by a given <see cref="List{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="List{T}"/> instance.</typeparam>
        /// <param name="list">The input <see cref="List{T}"/> instance.</param>
        /// <returns>The underlying <typeparamref name="T"/> array currently in use by <paramref name="list"/>.</returns>
        /// <remarks>
        /// The input <see cref="List{T}"/> should not be modified while using the returned array, as doing so might cause <paramref name="list"/> to swap it out.
        /// The length of the returned <typeparamref name="T"/> arrat might be greater than the number of items in the list.
        /// Use the <see cref="List{T}.Count"/> property to determine how many items in the array are currently in use.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static T[] DangerousGetUnderlyingArray<T>(this List<T> list)
        {
            var listData = Unsafe.As<RawListData<T>>(list);

            return listData._items;
        }

        /// <summary>
        /// Returns a <see cref="Span{T}"/> with the items currently stored in a given <see cref="List{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="List{T}"/> instance.</typeparam>
        /// <param name="list">The input <see cref="List{T}"/> instance.</param>
        /// <returns>A <see cref="Span{T}"/> with the items currently in use by <paramref name="list"/>.</returns>
        /// <remarks>The input <see cref="List{T}"/> should not be modified while using the returned <see cref="Span{T}"/>.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> DangerousAsSpan<T>(this List<T> list)
        {
            var listData = Unsafe.As<RawListData<T>>(list);
            var span = listData._items.AsSpan();

            return span;
        }

        // List<T> structure taken from CoreCLR: https://source.dot.net/#System.Private.CoreLib/List.cs,cf7f4095e4de7646
        private sealed class RawListData<T>
        {
#pragma warning disable CS0649 // Unassigned fields
#pragma warning disable SA1401 // Fields should be private
#pragma warning disable CS8618 // Uninitialized non-nullable field
            public T[] _items;
#pragma warning restore CS8618
            public int _size;
            public int _version;
#pragma warning restore CS0649
#pragma warning restore SA1401
        }

        /// <summary>
        /// Counts the number of occurrences of a given value into a target <see cref="List{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="List{T}"/> instance.</typeparam>
        /// <param name="list">The input <see cref="List{T}"/> instance.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="list"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this List<T> list, T value)
            where T : IEquatable<T>
        {
            var span = new ReadOnlySpan<T>(list.DangerousGetUnderlyingArray(), 0, list.Count);

            return span.Count(value);
        }

        /// <summary>
        /// Enumerates the items in the input <see cref="List{T}"/> instance, as pairs of value/index values.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// List&lt;string> words = new List&lt;string> { "Hello", ", ", "world", "!" };
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
        /// <param name="list">The source <see cref="List{T}"/> to enumerate.</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="list"/>.</returns>
        /// <remarks>
        /// The returned <see cref="ReadOnlySpanEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.
        /// The input <see cref="List{T}"/> should not be modified while using the returned array, as doing so might cause <paramref name="list"/> to swap it out.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanEnumerable<T> Enumerate<T>(this List<T> list)
        {
            var span = new ReadOnlySpan<T>(list.DangerousGetUnderlyingArray(), 0, list.Count);

            return span.Enumerate();
        }

        /// <summary>
        /// Tokenizes the values in the input <see cref="List{T}"/> instance using a specified separator.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// List&lt;char> text = "Hello, world!".ToList();
        ///
        /// foreach (var token in text.Tokenize(','))
        /// {
        ///     // Access the tokens here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of items in the <see cref="List{T}"/> to tokenize.</typeparam>
        /// <param name="list">The source <see cref="List{T}"/> to tokenize.</param>
        /// <param name="separator">The separator <typeparamref name="T"/> item to use.</param>
        /// <returns>A wrapper type that will handle the tokenization for <paramref name="list"/>.</returns>
        /// <remarks>
        /// The returned <see cref="SpanTokenizer{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.
        /// The input <see cref="List{T}"/> should not be modified while using the returned array, as doing so might cause <paramref name="list"/> to swap it out.
        /// </remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static SpanTokenizer<T> Tokenize<T>(this List<T> list, T separator)
            where T : IEquatable<T>
        {
            var span = list.DangerousGetUnderlyingArray().AsSpan(0, list.Count);

            return span.Tokenize(separator);
        }

        /// <summary>
        /// Gets a content hash from the input <see cref="List{T}"/> instance using the Djb2 algorithm.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="List{T}"/> instance.</typeparam>
        /// <param name="list">The input<see cref="List{T}"/> instance.</param>
        /// <returns>The Djb2 value for the input <see cref="List{T}"/> instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDjb2HashCode<T>(this List<T> list)
            where T : notnull
        {
            var span = new ReadOnlySpan<T>(list.DangerousGetUnderlyingArray(), 0, list.Count);

            return span.GetDjb2HashCode();
        }
    }
}
