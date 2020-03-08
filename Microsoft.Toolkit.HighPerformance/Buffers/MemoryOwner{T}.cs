// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

#nullable enable

namespace Microsoft.Toolkit.HighPerformance.Buffers
{
    /// <summary>
    /// An <see cref="IMemoryOwner{T}"/> implementation with an embedded size and a fast <see cref="Span{T}"/> accessor.
    /// </summary>
    /// <typeparam name="T">The type of items to store in the current instance.</typeparam>
    public sealed class MemoryOwner<T> : IMemoryOwner<T>
    {
        /// <summary>
        /// The usable size within <see cref="array"/>.
        /// </summary>
#pragma warning disable IDE0032
        private readonly int size;
#pragma warning restore IDE0032

        /// <summary>
        /// The underlying <typeparamref name="T"/> array.
        /// </summary>
        private T[]? array;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemoryOwner{T}"/> class with the specified parameters.
        /// </summary>
        /// <param name="size">The size of the new memory buffer to use.</param>
        /// <param name="clear">Indicates whether or not to clear the allocated memory area.</param>
        public MemoryOwner(int size, bool clear)
        {
            this.size = size;
            this.array = ArrayPool<T>.Shared.Rent(size);

            if (clear)
            {
                this.array.AsSpan(0, size).Clear();
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="MemoryOwner{T}"/> class.
        /// </summary>
        ~MemoryOwner() => this.Dispose();

        /// <summary>
        /// Gets an empty <see cref="MemoryOwner{T}"/> instance.
        /// </summary>
        [Pure]
        public static MemoryOwner<T> Empty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => new MemoryOwner<T>(0, false);
        }

        /// <summary>
        /// Creates a new <see cref="MemoryOwner{T}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="size">The size of the new memory buffer to use.</param>
        /// <returns>A <see cref="MemoryOwner{T}"/> instance of the requested size.</returns>
        /// <remarks>This method is just a proxy for the <see langword="private"/> constructor, for clarity.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryOwner<T> Allocate(int size) => new MemoryOwner<T>(size, false);

        /// <summary>
        /// Creates a new <see cref="MemoryOwner{T}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="size">The size of the new memory buffer to use.</param>
        /// <param name="clear">Indicates whether or not to clear the allocated memory area.</param>
        /// <returns>A <see cref="MemoryOwner{T}"/> instance of the requested size.</returns>
        /// <remarks>This method is just a proxy for the <see langword="private"/> constructor, for clarity.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryOwner<T> Allocate(int size, bool clear) => new MemoryOwner<T>(size, clear);

        /// <summary>
        /// Gets the number of items in the current instance
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.size;
        }

        /// <inheritdoc/>
        public Memory<T> Memory
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T[]? array = this.array;

                if (array is null)
                {
                    ThrowObjectDisposedException();
                }

                return new Memory<T>(array!, 0, this.size);
            }
        }

        /// <summary>
        /// Gets a <see cref="Span{T}"/> wrapping the memory belonging to the current instance.
        /// </summary>
        public Span<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                T[]? array = this.array;

                if (array is null)
                {
                    ThrowObjectDisposedException();
                }

                return new Span<T>(array!, 0, this.size);
            }
        }

        /// <summary>
        /// Returns a reference to the first element within the current instance, with no bounds check.
        /// </summary>
        /// <returns>A reference to the first element within the current instance.</returns>
        /// <remarks>
        /// This method does not perform bounds checks on the underlying buffer, but does check whether
        /// the buffer itself has been disposed or not. This check should not be removed, and it's also
        /// the reason why the method to get a reference at a specified offset is not present.
        /// .</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T DangerousGetReference()
        {
            T[]? array = this.array;

            if (array is null)
            {
                ThrowObjectDisposedException();
            }

            return ref array!.DangerousGetReference();
        }

        /// <inheritdoc/>
        public void Dispose()
        {
            T[]? array = this.array;

            if (array is null)
            {
                return;
            }

            this.array = null;

            ArrayPool<T>.Shared.Return(array);
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
