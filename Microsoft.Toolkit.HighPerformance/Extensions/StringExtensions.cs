using System.Diagnostics.CodeAnalysis;
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
        /// Returns a reference to the first element within a given <see cref="string"/>, with no bounds checks.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance.</param>
        /// <returns>A reference to the first element within <paramref name="text"/>, or the location it would have used, if <paramref name="text"/> is empty.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly char DangerousGetReference(this string text)
        {
            var stringData = Unsafe.As<RawStringData>(text);

            return ref stringData.Data;
        }

        /// <summary>
        /// Returns a reference to an element at a specified index within a given <see cref="string"/>, with no bounds checks.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance.</param>
        /// <param name="i">The index of the element to retrieve within <paramref name="text"/>.</param>
        /// <returns>A reference to the element within <paramref name="text"/> at the index specified by <paramref name="i"/>.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref readonly char DangerousGetReferenceAt(this string text, int i)
        {
            var stringData = Unsafe.As<RawStringData>(text);
            ref var ri = ref Unsafe.Add(ref stringData.Data, i);

            return ref ri;
        }

        // Description adapted from CoreCLR: see https://source.dot.net/#System.Private.CoreLib/src/System/Runtime/CompilerServices/RuntimeHelpers.CoreCLR.cs,285.
        // CLR strings are laid out in memory as follows:
        // [ sync block || pMethodTable || length || string data .. ]
        //                 ^                         ^
        //                 |                         \-- ref Unsafe.As<RawStringData>(text).Data
        //                 \-- string
        // The reference to RawStringData.Data points to the first character in the
        // string, skipping over the sync block, method table and string length.
        private sealed class RawStringData
        {
            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Definition from CoreCLR source")]
            public int Length;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Needs access to this field from parent class")]
            public char Data;
        }

        /// <summary>
        /// Counts the number of occurrences of a given character into a target <see cref="string"/> instance.
        /// </summary>
        /// <param name="text">The input <see cref="string"/> instance to read.</param>
        /// <param name="c">The character to look for.</param>
        /// <returns>The number of occurrences of <paramref name="c"/> in <paramref name="text"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count(this string text, char c)
        {
            return ReadOnlySpanExtensions.Count(text, c);
        }

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
        /// <param name="text">The source <see cref="string"/> to enumerate</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="text"/></returns>
        /// <remarks>The returned <see cref="ReadOnlySpanExtensions.__ReadOnlySpanEnumerator{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanExtensions.__ReadOnlySpanEnumerator<char> Enumerate(this string text)
        {
            return new ReadOnlySpanExtensions.__ReadOnlySpanEnumerator<char>(text);
        }

        /// <summary>
        /// Gets a content hash from the input <see cref="string"/> instance using the Djb2 algorithm.
        /// </summary>
        /// <param name="text">The source <see cref="string"/> to enumerate.</param>
        /// <returns>The Djb2 value for the input <see cref="string"/> instance.</returns>
        /// <remarks>The Djb2 hash is fully deterministic and with no random components.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int GetDjb2HashCode(this string text)
        {
            return ReadOnlySpanExtensions.GetDjb2HashCode<char>(text);
        }
    }
}
