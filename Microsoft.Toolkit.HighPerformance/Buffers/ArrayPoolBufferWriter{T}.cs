// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Buffers
{
    /// <summary>
    /// Represents a heap-based, array-backed output sink into which <typeparamref name="T"/> data can be written.
    /// </summary>
    /// <typeparam name="T">The type of items to write to the current instance.</typeparam>
    /// <remarks>
    /// This is a custom <see cref="IBufferWriter{T}"/> implementation that replicates the
    /// functionality and API surface of the array-based buffer writer available in
    /// .NET Standard 2.1, with the main difference being the fact that in this case
    /// the arrays in use are rented from the shared <see cref="ArrayPool{T}"/> instance,
    /// and that <see cref="ArrayPoolBufferWriter{T}"/> is also available on .NET Standard 2.0.
    /// </remarks>
    public sealed class ArrayPoolBufferWriter<T> : IBufferWriter<T>
    {
        /// <summary>
        /// The default buffer size to use to expand empty arrays.
        /// </summary>
        private const int DefaultInitialBufferSize = 256;

        /// <summary>
        /// The underlying <typeparamref name="T"/> array.
        /// </summary>
        private T[]? array;

#pragma warning disable IDE0032
        /// <summary>
        /// The starting offset within <see cref="array"/>.
        /// </summary>
        private int index;
#pragma warning restore IDE0032

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayPoolBufferWriter{T}"/> class.
        /// </summary>
        public ArrayPoolBufferWriter()
        {
            /* Since we're using pooled arrays, we allocate the buffer with the
             * default size immediately, we don't need to use lazy initialization
             * here to save unnecessary memory allocations. */
            this.array = ArrayPool<T>.Shared.Rent(DefaultInitialBufferSize);
            this.index = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ArrayPoolBufferWriter{T}"/> class.
        /// </summary>
        /// <param name="initialCapacity">The minimum capacity with which to initialize the underlying buffer.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="initialCapacity"/> is not positive (i.e. less than or equal to 0).
        /// </exception>
        public ArrayPoolBufferWriter(int initialCapacity)
        {
            if (initialCapacity <= 0)
            {
                ThrowArgumentOutOfRangeExceptionForInitialCapacity();
            }

            this.array = ArrayPool<T>.Shared.Rent(initialCapacity);
            this.index = 0;
        }

        /// <summary>
        /// Gets the data written to the underlying buffer so far, as a <see cref="ReadOnlyMemory{T}"/>.
        /// </summary>
        public ReadOnlyMemory<T> WrittenMemory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T[]? array = this.array;

                if (array is null)
                {
                    ThrowObjectDisposedException();
                }

                return array!.AsMemory(0, this.index);
            }
        }

        /// <summary>
        /// Gets the data written to the underlying buffer so far, as a <see cref="ReadOnlySpan{T}"/>.
        /// </summary>
        public ReadOnlySpan<T> WrittenSpan
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T[]? array = this.array;

                if (array is null)
                {
                    ThrowObjectDisposedException();
                }

                return array!.AsSpan(0, this.index);
            }
        }

        /// <summary>
        /// Gets the amount of data written to the underlying buffer so far.
        /// </summary>
        public int WrittenCount
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.index;
        }

        /// <summary>
        /// Gets the total amount of space within the underlying buffer.
        /// </summary>
        public int Capacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T[]? array = this.array;

                if (array is null)
                {
                    ThrowObjectDisposedException();
                }

                return array!.Length;
            }
        }

        /// <summary>
        /// Gets the amount of space available that can still be written into without forcing the underlying buffer to grow.
        /// </summary>
        public int FreeCapacity
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T[]? array = this.array;

                if (array is null)
                {
                    ThrowObjectDisposedException();
                }

                return array!.Length - this.index;
            }
        }

        /// <summary>
        /// Clears the data written to the underlying buffer.
        /// </summary>
        /// <remarks>
        /// You must clear the <see cref="ArrayPoolBufferWriter{T}"/> before trying to re-use it.
        /// </remarks>
        public void Clear()
        {
            T[]? array = this.array;

            if (array is null)
            {
                ThrowObjectDisposedException();
            }

            array.AsSpan(0, this.index).Clear();
            this.index = 0;
        }

        /// <inheritdoc/>
        public void Advance(int count)
        {
            if (count < 0)
            {
                ThrowArgumentOutOfRangeExceptionForNegativeCount();
            }

            T[]? array = this.array;

            if (array is null)
            {
                ThrowObjectDisposedException();
            }

            if (this.index > array!.Length - count)
            {
                ThrowArgumentExceptionForAdvancedTooFar();
            }

            this.index += count;
        }

        /// <inheritdoc/>
        public Memory<T> GetMemory(int sizeHint = 0)
        {
            CheckAndResizeBuffer(sizeHint);

            return this.array.AsMemory(this.index);
        }

        /// <inheritdoc/>
        public Span<T> GetSpan(int sizeHint = 0)
        {
            CheckAndResizeBuffer(sizeHint);

            return this.array.AsSpan(this.index);
        }

        /// <summary>
        /// Ensures that <see cref="array"/> has enough free space to contain a given number of new items.
        /// </summary>
        /// <param name="sizeHint">The minimum number of items to ensure space for in <see cref="array"/>.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private void CheckAndResizeBuffer(int sizeHint)
        {
            if (this.array is null)
            {
                ThrowObjectDisposedException();
            }

            if (sizeHint < 0)
            {
                ThrowArgumentOutOfRangeExceptionForNegativeSizeHint();
            }

            if (sizeHint == 0)
            {
                sizeHint = 1;
            }

            if (sizeHint > FreeCapacity)
            {
                int minimumSize = this.index + sizeHint;

                ArrayPool<T>.Shared.Resize(ref this.array, minimumSize);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the initial capacity is invalid.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1204", Justification = "Exception throwers at the end of class")]
        private static void ThrowArgumentOutOfRangeExceptionForInitialCapacity()
        {
            throw new ArgumentOutOfRangeException("initialCapacity", "The initial capacity must be a positive value");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the requested count is negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForNegativeCount()
        {
            throw new ArgumentOutOfRangeException("count", "The count can't be a negative value");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the size hint is negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentOutOfRangeExceptionForNegativeSizeHint()
        {
            throw new ArgumentOutOfRangeException("sizeHint", "The size hint can't be a negative value");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the requested count is negative.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowArgumentExceptionForAdvancedTooFar()
        {
            throw new ArgumentException("The buffer writer has advanced too far");
        }

        /// <summary>
        /// Throws an <see cref="ObjectDisposedException"/> when <see cref="array"/> is <see langword="null"/>.
        /// </summary>
        [MethodImpl(MethodImplOptions.NoInlining)]
        private static void ThrowObjectDisposedException()
        {
            throw new ObjectDisposedException("The current buffer has already been disposed");
        }
    }
}