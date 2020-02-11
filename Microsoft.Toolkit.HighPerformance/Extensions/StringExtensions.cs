using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="string"/> type.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Enumerates the items in the input <see cref="string"/> instance, as pairs of value/index values.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// string text = "Hello, world!";
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
        /// <param name="text">The source <see cref="string"/> to enumerate</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="text"/></returns>
        /// <remarks>The returned <see cref="ReadOnlySpanExtensions.__ReadOnlySpanEnumerator{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanExtensions.__ReadOnlySpanEnumerator<char> Enumerate(this string text)
        {
            return new ReadOnlySpanExtensions.__ReadOnlySpanEnumerator<char>(text);
        }
    }
}
