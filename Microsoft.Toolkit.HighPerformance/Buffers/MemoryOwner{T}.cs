using System;
using System.Buffers;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

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
        /// The underlying <typeparamref name="T"/> array.
        /// </summary>
        private T[]? array;

        /// <summary>
        /// The usable size within <see cref="array"/>.
        /// </summary>
        private readonly int size;

        /// <summary>
        /// Creates a new <see cref="MemoryOwner{T}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="size">The size of the new memory buffer to use.</param>
        /// <param name="clear">Indicates whether or not to clear the allocated memory area.</param>
        private MemoryOwner(int size, bool clear)
        {
            this.array = ArrayPool<T>.Shared.Rent(size);
            this.size = size;

            if (clear)
            {
                this.array.AsSpan(0, size).Clear();
            }
        }

        /// <summary>
        /// Releases the underlying buffer when the current instance is finalized.
        /// </summary>
        ~MemoryOwner() => this.Dispose();

        /// <summary>
        /// Creates a new <see cref="MemoryOwner{T}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="size">The size of the new memory buffer to use.</param>
        /// <remarks>This method is just a proxy for the <see langword="private"/> constructor, for clarity.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryOwner<T> Allocate(int size) => new MemoryOwner<T>(size, false);

        /// <summary>
        /// Creates a new <see cref="MemoryOwner{T}"/> instance with the specified parameters.
        /// </summary>
        /// <param name="size">The size of the new memory buffer to use.</param>
        /// <param name="clear">Indicates whether or not to clear the allocated memory area.</param>
        /// <remarks>This method is just a proxy for the <see langword="private"/> constructor, for clarity.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static MemoryOwner<T> Allocate(int size, bool clear) => new MemoryOwner<T>(size, clear);

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

                return new Memory<T>(array, 0, this.size);
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

                return new Span<T>(array, 0, this.size);
            }
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
