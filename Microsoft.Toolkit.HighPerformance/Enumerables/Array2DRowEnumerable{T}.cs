// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !SPAN_RUNTIME_SUPPORT

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that iterates a row in a given 2D <typeparamref name="T"/> array instance.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly ref struct Array2DRowEnumerable<T>
    {
        /// <summary>
        /// The source 2D <typeparamref name="T"/> array instance.
        /// </summary>
        private readonly T[,] array;

        /// <summary>
        /// The target row to iterate within <see cref="array"/>.
        /// </summary>
        private readonly int row;

        /// <summary>
        /// Initializes a new instance of the <see cref="Array2DRowEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="array">The source 2D <typeparamref name="T"/> array instance.</param>
        /// <param name="row">The target row to iterate within <paramref name="array"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array2DRowEnumerable(T[,] array, int row)
        {
            this.array = array;
            this.row = row;
        }

        /// <summary>
        /// Implements the duck-typed <see cref="IEnumerable{T}.GetEnumerator"/> method.
        /// </summary>
        /// <returns>An <see cref="Enumerator"/> instance targeting the current 2D <typeparamref name="T"/> array instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this.array, this.row);

        /// <summary>
        /// Returns a <typeparamref name="T"/> array with the values in the target row.
        /// </summary>
        /// <returns>A <typeparamref name="T"/> array with the values in the target row.</returns>
        /// <remarks>
        /// This method will allocate a new <typeparamref name="T"/> array, so only
        /// use it if you really need to copy the target items in a new memory location.
        /// </remarks>
        [Pure]
        public T[] ToArray()
        {
            if ((uint)row >= (uint)this.array.GetLength(0))
            {
                ThrowArgumentOutOfRangeExceptionForInvalidRow();
            }

            int width = this.array.GetLength(1);

            T[] array = new T[width];

            for (int i = 0; i < width; i++)
            {
                array.DangerousGetReferenceAt(i) = this.array.DangerousGetReferenceAt(this.row, i);
            }

            return array;
        }

        /// <summary>
        /// An enumerator for a source 2D array instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref struct Enumerator
        {
            /// <summary>
            /// The source 2D array instance.
            /// </summary>
            private readonly T[,] array;

            /// <summary>
            /// The target row to iterate within <see cref="array"/>.
            /// </summary>
            private readonly int row;

            /// <summary>
            /// The width of a row in <see cref="array"/>.
            /// </summary>
            private readonly int width;

            /// <summary>
            /// The current column.
            /// </summary>
            private int column;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="array">The source 2D array instance.</param>
            /// <param name="row">The target row to iterate within <paramref name="array"/>.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(T[,] array, int row)
            {
                if ((uint)row >= (uint)array.GetLength(0))
                {
                    ThrowArgumentOutOfRangeExceptionForInvalidRow();
                }

                this.array = array;
                this.row = row;
                this.width = array.GetLength(1);
                this.column = -1;
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
            /// </summary>
            /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int column = this.column + 1;

                if (column < this.width)
                {
                    this.column = column;

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Gets the duck-typed <see cref="IEnumerator{T}.Current"/> property.
            /// </summary>
            public ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    // This type is never used on .NET Core runtimes, where
                    // the fast indexer is available. Therefore, we can just
                    // use the built-in indexer for 2D arrays to access the value.
                    return ref this.array[this.row, this.column];
                }
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the <see cref="row"/> is invalid.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForInvalidRow()
        {
            throw new ArgumentOutOfRangeException(nameof(row), "The target row parameter was not valid");
        }
    }
}

#endif
