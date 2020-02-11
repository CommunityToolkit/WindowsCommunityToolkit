using System;
using System.Diagnostics.CodeAnalysis;
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
        /// Returns a reference to the first element within a given <typeparamref name="T"/> array, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <returns>A reference to the first element within <paramref name="array"/>, or the location it would have used, if <paramref name="array"/> is empty.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to perform checks in case the returned value is dereferenced.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReference<T>(this T[] array)
        {
            var arrayData = Unsafe.As<RawArrayData>(array);
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);

            return ref r0;
        }

        /// <summary>
        /// Returns a reference to an element at a specified index within a given <typeparamref name="T"/> array, with no bounds checks.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="i">The index of the element to retrieve within <paramref name="array"/>.</param>
        /// <returns>A reference to the element within <paramref name="array"/> at the index specified by <paramref name="i"/>.</returns>
        /// <remarks>This method doesn't do any bounds checks, therefore it is responsibility of the caller to ensure the <paramref name="i"/> parameter is valid.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ref T DangerousGetReferenceAt<T>(this T[] array, int i)
        {
            var arrayData = Unsafe.As<RawArrayData>(array);
            ref T r0 = ref Unsafe.As<byte, T>(ref arrayData.Data);
            ref T ri = ref Unsafe.Add(ref r0, i);

            return ref ri;
        }

        // Description taken from CoreCLR: see https://source.dot.net/#System.Private.CoreLib/src/System/Runtime/CompilerServices/RuntimeHelpers.CoreCLR.cs,285.
        // CLR arrays are laid out in memory as follows (multidimensional array bounds are optional):
        // [ sync block || pMethodTable || num components || MD array bounds || array data .. ]
        //                 ^               ^                 ^                  ^ returned reference
        //                 |               |                 \-- ref Unsafe.As<RawArrayData>(array).Data
        //                 \-- array       \-- ref Unsafe.As<RawData>(array).Data
        // The BaseSize of an array includes all the fields before the array data,
        // including the sync block and method table. The reference to RawData.Data
        // points at the number of components, skipping over these two pointer-sized fields.
        private sealed class RawArrayData
        {
            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Definition from CoreCLR source")]
            public IntPtr Length;

            [SuppressMessage("StyleCop.CSharp.MaintainabilityRules", "SA1401", Justification = "Needs access to this field from parent class")]
            public byte Data;
        }

        /// <summary>
        /// Counts the number of occurrences of a given character into a target <typeparamref name="T"/> array instance.
        /// </summary>
        /// <typeparam name="T">The type of items in the input <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="value">The <typeparamref name="T"/> value to look for.</param>
        /// <returns>The number of occurrences of <paramref name="value"/> in <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static int Count<T>(this T[] array, T value)
            where T : struct, IEquatable<T>
        {
            return ReadOnlySpanExtensions.Count(array, value);
        }

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
        /// <param name="array">The source <typeparamref name="T"/> array to enumerate.</param>
        /// <returns>A wrapper type that will handle the value/index enumeration for <paramref name="array"/>.</returns>
        /// <remarks>The returned <see cref="ReadOnlySpanExtensions.__Enumerator{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanExtensions.__Enumerator<T> Enumerate<T>(this T[] array)
        {
            return new ReadOnlySpanExtensions.__Enumerator<T>(array);
        }

        /// <summary>
        /// Tokenizes the values in the input <typeparamref name="T"/> array instance using a specified separator.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// char[] text = "Hello, world!".ToCharArray();
        ///
        /// foreach (var token in text.Tokenize(','))
        /// {
        ///     // Access the tokens here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of items in the <typeparamref name="T"/> array to tokenize.</typeparam>
        /// <param name="array">The source <typeparamref name="T"/> array to tokenize.</param>
        /// <param name="separator">The separator <typeparamref name="T"/> item to use.</param>
        /// <returns>A wrapper type that will handle the tokenization for <paramref name="array"/>.</returns>
        /// <remarks>The returned <see cref="ReadOnlySpanExtensions.__Tokenizer{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ReadOnlySpanExtensions.__Tokenizer<T> Tokenize<T>(this T[] array, T separator)
            where T : IEquatable<T>
        {
            return new ReadOnlySpanExtensions.__Tokenizer<T>(array, separator);
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
