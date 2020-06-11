// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that iterates a column in a given 2D <typeparamref name="T"/> array instance.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
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

            ref T r0 = ref array.DangerousGetReference();
            int i = 0;

            // Leverage the enumerator to traverse the column
            foreach (T item in this)
            {
                Unsafe.Add(ref r0, i++) = item;
            }

            return array;
        }

        /// <summary>
        /// An enumerator for a source 2D array instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref struct Enumerator
        {
#if SPAN_RUNTIME_SUPPORT
            /// <summary>
            /// The <see cref="Span{T}"/> instance mapping the target 2D array.
            /// </summary>
            /// <remarks>
            /// In runtimes where we have support for the <see cref="Span{T}"/> type, we can
            /// create one from the input 2D array and use that to traverse the target column.
            /// This reduces the number of operations to perform for the offsetting to the right
            /// column element (we simply need to add <see cref="width"/> to the offset at each
            /// iteration to move down by one row), and allows us to use the fast <see cref="Span{T}"/>
            /// accessor instead of the slower indexer for 2D arrays, as we can then access each
            /// individual item linearly, since we know the absolute offset from the base location.
            /// </remarks>
            private readonly Span<T> span;

            /// <summary>
            /// The width of the target 2D array.
            /// </summary>
            private readonly int width;

            /// <summary>
            /// The current absolute offset within <see cref="span"/>.
            /// </summary>
            private int offset;
#else
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
#endif

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

#if SPAN_RUNTIME_SUPPORT
                this.span = array.AsSpan();
                this.width = array.GetLength(1);
                this.offset = column - this.width;
#else
                this.array = array;
                this.column = column;
                this.height = array.GetLength(0);
                this.row = -1;
#endif
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
            /// </summary>
            /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
#if SPAN_RUNTIME_SUPPORT
                int offset = this.offset + this.width;

                if ((uint)offset < (uint)this.span.Length)
                {
                    this.offset = offset;

                    return true;
                }
#else
                int row = this.row + 1;

                if (row < this.height)
                {
                    this.row = row;

                    return true;
                }
#endif
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
#if SPAN_RUNTIME_SUPPORT
                    return ref this.span.DangerousGetReferenceAt(this.offset);
#else
                    return ref this.array[this.row, this.column];
#endif
                }
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