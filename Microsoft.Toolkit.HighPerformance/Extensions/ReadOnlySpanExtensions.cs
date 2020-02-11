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
        public static ref readonly T DangerousGetReferenceAt<T>(this ReadOnlySpan<T> span, int i)
        {
            ref T r0 = ref MemoryMarshal.GetReference(span);
            ref T ri = ref Unsafe.Add(ref r0, i);

            return ref ri;
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
            var hash = 5381;

            foreach (var item in span)
            {
                hash = unchecked((hash << 5) + hash + item.GetHashCode());
            }

            return hash;
        }
    }
}
