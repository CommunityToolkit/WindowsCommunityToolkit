// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.Uwp.UI.Helpers.Internals
{
    /// <summary>
    /// A simple buffer writer implementation using pooled arrays.
    /// </summary>
    /// <typeparam name="T">The type of items to store in the list.</typeparam>
    /// <remarks>
    /// This type is a <see langword="ref"/> <see langword="struct"/> to avoid the object allocation and to
    /// enable the pattern-based <see cref="IDisposable"/> support. We aren't worried with consumers not
    /// using this type correctly since it's private and only accessible within the parent type.
    /// </remarks>
    internal struct ArrayPoolBufferWriter<T> : IDisposable
    {
        /// <summary>
        /// The default buffer size to use to expand empty arrays.
        /// </summary>
        private const int DefaultInitialBufferSize = 128;

        /// <summary>
        /// The underlying <typeparamref name="T"/> array.
        /// </summary>
        private T[] array;

        /// <summary>
        /// The starting offset within <see cref="array"/>.
        /// </summary>
        private int index;

        /// <summary>
        /// Creates a new instance of the <see cref="ArrayPoolBufferWriter{T}"/> struct.
        /// </summary>
        /// <returns>A new <see cref="ArrayPoolBufferWriter{T}"/> instance.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static ArrayPoolBufferWriter<T> Create()
        {
            return new() { array = ArrayPool<T>.Shared.Rent(DefaultInitialBufferSize) };
        }

        /// <summary>
        /// Gets the total number of items stored in the current instance.
        /// </summary>
        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.array.Length;
        }

        /// <summary>
        /// Gets the item at the specified offset into the current buffer in use.
        /// </summary>
        /// <param name="index">The index of the element to retrieve.</param>
        /// <returns>The item at the specified offset into the current buffer in use.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="index"/> is out of range.</exception>
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)index >= this.index)
                {
                    static void Throw() => throw new ArgumentOutOfRangeException(nameof(index));

                    Throw();
                }

                return this.array[index];
            }
        }

        /// <summary>
        /// Adds a new item to the current collection.
        /// </summary>
        /// <param name="item">The item to add.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Add(T item)
        {
            if (this.index == this.array.Length)
            {
                ResizeBuffer();
            }

            this.array[this.index++] = item;
        }

        /// <summary>
        /// Resets the underlying array and the stored items.
        /// </summary>
        public void Reset()
        {
            Array.Clear(this.array, 0, this.index);

            this.index = 0;
        }

        /// <summary>
        /// Resizes <see cref="array"/> when there is no space left for new items.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private void ResizeBuffer()
        {
            T[] rent = ArrayPool<T>.Shared.Rent(this.index << 2);

            Array.Copy(this.array, 0, rent, 0, this.index);
            Array.Clear(this.array, 0, this.index);

            ArrayPool<T>.Shared.Return(this.array);

            this.array = rent;
        }

        /// <inheritdoc cref="IDisposable.Dispose"/>
        public void Dispose()
        {
            Array.Clear(this.array, 0, this.index);

            ArrayPool<T>.Shared.Return(this.array);
        }
    }
}