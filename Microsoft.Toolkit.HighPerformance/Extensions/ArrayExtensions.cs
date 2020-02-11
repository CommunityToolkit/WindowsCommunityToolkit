using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="System.Array"/> type.
    /// </summary>
    public static class ArrayExtensions
    {
        /// <summary>
        /// Enumerates the items in the input <typeparamref name="T"/> array instance, as pairs of value/index values.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// string[] words = new[] { "Hello", ", ", "world", "!" };
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
        /// <param name="array">The source <typeparamref name="T"/> array to enumerate</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="array"/></returns>
        /// <remarks>The returned <see cref="ReadOnlySpanExtensions.__ReadOnlySpanEnumerator{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanExtensions.__ReadOnlySpanEnumerator<T> Enumerate<T>(this T[] array)
        {
            return new ReadOnlySpanExtensions.__ReadOnlySpanEnumerator<T>(array);
        }

        /// <summary>
        /// Gets a content hash from the input <typeparamref name="T"/> array instance using the Djb2 algorithm.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="span">The input <typeparamref name="T"/> array instance.</param>
        /// <returns>The Djb2 value for the input <typeparamref name="T"/> array instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDjb2HashCode<T>(this T[] span)
            where T : notnull
        {
            return ReadOnlySpanExtensions.GetDjb2HashCode<T>(span);
        }
    }
}
