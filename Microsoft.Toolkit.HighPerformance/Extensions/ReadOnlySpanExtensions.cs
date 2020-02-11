using System;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ReadOnlySpan{T}"/> type.
    /// </summary>
    public static partial class ReadOnlySpanExtensions
    {
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
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to enumerate</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="span"/></returns>
        /// <remarks>The returned <see cref="__ReadOnlySpanEnumerator{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ReadOnlySpanEnumerator<T> Enumerate<T>(ReadOnlySpan<T> span)
        {
            return new __ReadOnlySpanEnumerator<T>(span);
        }
    }
}
