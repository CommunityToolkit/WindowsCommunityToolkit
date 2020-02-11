using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Enumerables;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="Span{T}"/> type.
    /// </summary>
    public static class SpanExtensions
    {
        /// <summary>
        /// Returns a reference to the first element within a given <see cref="Span{T}"/>, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="Span{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="Span{T}"/> instance.</param>
        /// <returns>A reference to the first element within <paramref name="span"/>.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReference<T>(this Span<T> span)
        {
            return ref MemoryMarshal.GetReference(span);
        }

        /// <summary>
        /// Returns a reference to an element at a specified index within a given <see cref="Span{T}"/>, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <see cref="Span{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="Span{T}"/> instance.</param>
        /// <param name="i">The index of the element to retrieve within <paramref name="span"/>.</param>
        /// <returns>A reference to the element within <paramref name="span"/> at the index specified by <paramref name="i"/>.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReferenceAt<T>(this Span<T> span, int i)
        {
            ref T r0 = ref MemoryMarshal.GetReference(span);
            ref T ri = ref Unsafe.Add(ref r0, i);

            return ref ri;
        }

        /// <summary>
        /// Counts the number of occurrences of a given character into a target <see cref="Span{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="Span{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="Span{T}"/> instance to read.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="span"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this Span<T> span, T value)
            where T : struct, IEquatable<T>
        {
            return ReadOnlySpanExtensions.Count(span, value);
        }

        /// <summary>
        /// Enumerates the items in the input <see cref="Span{T}"/> instance, as pairs of value/index values.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// Span&lt;string&gt; words = new[] { "Hello", ", ", "world", "!" };
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
        /// <param name="span">The source <see cref="Span{T}"/> to enumerate.</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="span"/>.</returns>
        /// <remarks>The returned <see cref="ReadOnlySpanEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanEnumerable<T> Enumerate<T>(this Span<T> span)
        {
            return new ReadOnlySpanEnumerable<T>(span);
        }

        /// <summary>
        /// Tokenizes the values in the input <see cref="Span{T}"/> instance using a specified separator.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// Span&lt;char&gt; text = "Hello, world!".ToCharArray();
        ///
        /// foreach (var token in text.Tokenize(','))
        /// {
        ///     // Access the tokens here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of items in the <see cref="Span{T}"/> to tokenize.</typeparam>
        /// <param name="span">The source <see cref="Span{T}"/> to tokenize.</param>
        /// <param name="separator">The separator <typeparamref name="T"/> item to use.</param>
        /// <returns>A wrapper type that will handle the tokenization for <paramref name="span"/>.</returns>
        /// <remarks>The returned <see cref="ReadOnlySpanTokenizer{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanTokenizer<T> Tokenize<T>(this Span<T> span, T separator)
            where T : IEquatable<T>
        {
            return new ReadOnlySpanTokenizer<T>(span, separator);
        }

        /// <summary>
        /// Gets a content hash from the input <see cref="Span{T}"/> instance using the Djb2 algorithm.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <see cref="Span{T}"/> instance.</typeparam>
        /// <param name="span">The input <see cref="Span{T}"/> instance.</param>
        /// <returns>The Djb2 value for the input <see cref="Span{T}"/> instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDjb2HashCode<T>(this Span<T> span)
            where T : notnull
        {
            return ReadOnlySpanExtensions.GetDjb2HashCode<T>(span);
        }
    }
}
