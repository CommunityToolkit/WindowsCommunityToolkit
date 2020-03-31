// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that iterates a column in a given 2D <typeparamref name="T"/> array instance.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly ref struct Array2DColumnEnumerable<T>
    {
        /// <summary>
        /// The source 2D <typeparamref name="T"/> array instance.
        /// </summary>
        private readonly T[,] array;

        /// <summary>
        /// The target column to iterate within <see cref="array"/>.
        /// </summary>
        private readonly int column;

        /// <summary>
        /// Initializes a new instance of the <see cref="Array2DColumnEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="array">The source 2D <typeparamref name="T"/> array instance.</param>
        /// <param name="column">The target column to iterate within <paramref name="array"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Array2DColumnEnumerable(T[,] array, int column)
        {
            this.array = array;
            this.column = column;
        }

        /// <summary>
        /// Implements the duck-typed <see cref="IEnumerable{T}.GetEnumerator"/> method.
        /// </summary>
        /// <returns>An <see cref="Enumerator"/> instance targeting the current 2D <typeparamref name="T"/> array instance.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this.array, this.column);

        /// <summary>
        /// Returns a <typeparamref name="T"/> array with the values in the target column.
        /// </summary>
        /// <returns>A <typeparamref name="T"/> array with the values in the target column.</returns>
        /// <remarks>
        /// This method will allocate a new <typeparamref name="T"/> array, so only
        /// use it if you really need to copy the target items in a new memory location.
        /// </remarks>
        [Pure]
        public T[] ToArray()
        {
            if ((uint)column >= (uint)this.array.GetLength(1))
            {
                ThrowArgumentOutOfRangeExceptionForInvalidColumn();
            }

            int height = this.array.GetLength(0);

            T[] array = new T[height];

            for (int i = 0; i < height; i++)
            {
                array.DangerousGetReferenceAt(i) = this.array.DangerousGetReferenceAt(i, this.column);
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
            /// The target column to iterate within <see cref="array"/>.
            /// </summary>
            private readonly int column;

            /// <summary>
            /// The height of a column in <see cref="array"/>.
            /// </summary>
            private readonly int height;

            /// <summary>
            /// The current row.
            /// </summary>
            private int row;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="array">The source 2D array instance.</param>
            /// <param name="column">The target column to iterate within <paramref name="array"/>.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(T[,] array, int column)
            {
                if ((uint)column >= (uint)array.GetLength(1))
                {
                    ThrowArgumentOutOfRangeExceptionForInvalidColumn();
                }

                this.array = array;
                this.column = column;
                this.height = array.GetLength(0);
                this.row = -1;
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
            /// </summary>
            /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int row = this.row + 1;

                if (row < this.height)
                {
                    this.row = row;

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
                get => ref this.array.DangerousGetReferenceAt(this.row, this.column);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the <see cref="column"/> is invalid.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForInvalidColumn()
        {
            throw new ArgumentOutOfRangeException(nameof(column), "The target column parameter was not valid");
        }
    }
}
