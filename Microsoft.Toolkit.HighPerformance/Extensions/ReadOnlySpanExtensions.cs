using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ReadOnlySpan{T}"/> type.
    /// </summary>
    public static partial class ReadOnlySpanExtensions
    {
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
        public static ref readonly T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, int i)
        {
            ref T r0 = ref MemoryMarshal.GetReference(span);
            ref T ri = ref Unsafe.Add(ref r0, i);

            return ref ri;
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
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to enumerate</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="span"/></returns>
        /// <remarks>The returned <see cref="__ReadOnlySpanEnumerator{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __ReadOnlySpanEnumerator<T> Enumerate<T>(this ReadOnlySpan<T> span)
        {
            return new __ReadOnlySpanEnumerator<T>(span);
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
            int hash = 5381;

            foreach (var item in span)
            {
                hash = unchecked((hash << 5) + hash + item.GetHashCode());
            }

            return hash;
        }
    }
}
