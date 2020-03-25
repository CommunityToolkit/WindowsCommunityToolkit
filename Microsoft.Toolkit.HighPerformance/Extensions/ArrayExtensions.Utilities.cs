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
    /// Helpers for working with the <see cref="Array"/> type.
    /// </summary>
    public static partial class ArrayExtensions
    {
        /// <summary>
        /// Returns a <see cref="Span{T}"/> over a row in a given 2D <typeparamref name="T"/> array instance.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="row">The target row to retrieve (0-based index).</param>
        /// <returns>A <see cref="Span{T}"/> with the items from the target row within <paramref name="array"/>.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Span<T> GetRow<T>(this T[,] array, int row)
        {
            if ((uint)row >= (uint)array.GetLength(0))
            {
                throw new ArgumentOutOfRangeException(nameof(row));
            }

            ref T r0 = ref array.DangerousGetReferenceAt(row, 0);

            return MemoryMarshal.CreateSpan(ref r0, array.GetLength(1));
        }

        /// <summary>
        /// Returns an enumerable that returns the items from a given column in a given 2D <typeparamref name="T"/> array instance.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// int[,] matrix =
        /// {
        ///     { 1, 2, 3 },
        ///     { 4, 5, 6 },
        ///     { 7, 8, 9 }
        /// };
        ///
        /// foreach (ref int number in matrix.GetColumn(1))
        /// {
        ///     // Access the current number by reference here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of elements in the input 2D <typeparamref name="T"/> array instance.</typeparam>
        /// <param name="array">The input <typeparamref name="T"/> array instance.</param>
        /// <param name="column">The target column to retrieve (0-based index).</param>
        /// <returns>A wrapper type that will handle the column enumeration for <paramref name="array"/>.</returns>
        /// <remarks>The returned <see cref="Array2DColumnEnumerable{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static Array2DColumnEnumerable<T> GetColumn<T>(this T[,] array, int column)
        {
            return new Array2DColumnEnumerable<T>(array, column);
        }
    }
}
