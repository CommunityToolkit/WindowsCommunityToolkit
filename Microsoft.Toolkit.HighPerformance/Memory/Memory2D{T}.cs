// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Buffers;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

namespace Microsoft.Toolkit.HighPerformance.Memory
{
    /// <summary>
    /// <see cref="Memory2D{T}"/> represents a 2D region of arbitrary memory. It is to <see cref="Span2D{T}"/>
    /// what <see cref="Memory{T}"/> is to <see cref="Span{T}"/>. For further details on how the internal layout
    /// is structured, see the docs for <see cref="Span2D{T}"/>. The <see cref="Memory2D{T}"/> type can wrap arrays
    /// of any rank, provided that a valid series of parameters for the target memory area(s) are provided.
    /// </summary>
    /// <typeparam name="T">The type of items in the current <see cref="Span2D{T}"/> instance.</typeparam>
    public readonly struct Memory2D<T> : IEquatable<Memory2D<T>>
    {
        /// <summary>
        /// The target <see cref="object"/> instance, if present.
        /// </summary>
        private readonly object? instance;

        /// <summary>
        /// The initial offset within <see cref="instance"/>.
        /// </summary>
        private readonly IntPtr offset;

        /// <summary>
        /// The height of the specified 2D region.
        /// </summary>
        private readonly int height;

        /// <summary>
        /// The width of the specified 2D region.
        /// </summary>
        private readonly int width;

        /// <summary>
        /// The pitch of the specified 2D region.
        /// </summary>
        private readonly int pitch;

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory2D{T}"/> struct.
        /// </summary>
        /// <param name="array">The target array to wrap.</param>
        /// <param name="offset">The initial offset within <paramref name="array"/>.</param>
        /// <param name="width">The width of each row in the resulting 2D area.</param>
        /// <param name="height">The height of the resulting 2D area.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when either <paramref name="offset"/>, <paramref name="height"/> or <paramref name="width"/> are invalid.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory2D(T[] array, int offset, int width, int height)
        {
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                ThrowArrayTypeMismatchException();
            }

            if ((uint)offset >= (uint)array.Length)
            {
                throw new Exception();
            }

            int remaining = array.Length - offset;

            if (((uint)width * (uint)height) > (uint)remaining)
            {
                throw new Exception();
            }

            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(offset));
            this.height = height;
            this.width = width;
            this.pitch = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory2D{T}"/> struct.
        /// </summary>
        /// <param name="array">The target array to wrap.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory2D(T[,]? array)
        {
            if (array is null)
            {
                this = default;

                return;
            }

            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                ThrowArrayTypeMismatchException();
            }

            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReference());
            this.height = array.GetLength(0);
            this.width = array.GetLength(1);
            this.pitch = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory2D{T}"/> struct wrapping a 2D array.
        /// </summary>
        /// <param name="array">The given 2D array to wrap.</param>
        /// <param name="row">The target row to map within <paramref name="array"/>.</param>
        /// <param name="column">The target column to map within <paramref name="array"/>.</param>
        /// <param name="width">The width to map within <paramref name="array"/>.</param>
        /// <param name="height">The height to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when either <paramref name="height"/>, <paramref name="width"/> or <paramref name="height"/>
        /// are negative or not within the bounds that are valid for <paramref name="array"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory2D(T[,] array, int row, int column, int width, int height)
        {
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                ThrowArrayTypeMismatchException();
            }

            int
                rows = array.GetLength(0),
                columns = array.GetLength(1);

            if ((uint)row >= (uint)rows ||
                (uint)column >= (uint)columns ||
                width > (columns - column) ||
                height > (rows - row))
            {
                throw new Exception();
            }

            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(row, column));
            this.height = height;
            this.width = width;
            this.pitch = row + (array.GetLength(1) - column);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Memory2D{T}"/> struct wrapping a layer in a 3D array.
        /// </summary>
        /// <param name="array">The given 3D array to wrap.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">Thrown when either <paramref name="depth"/> is invalid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Memory2D(T[,,] array, int depth)
        {
            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                ThrowArrayTypeMismatchException();
            }

            if ((uint)depth >= (uint)array.GetLength(0))
            {
                throw new Exception();
            }

            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(depth, 0, 0));
            this.height = array.GetLength(1);
            this.width = array.GetLength(2);
            this.pitch = 0;
        }

        /// <summary>
        /// Defines an implicit conversion of an array to a <see cref="Memory2D{T}"/>
        /// </summary>
        public static implicit operator Memory2D<T>(T[,]? array) => new Memory2D<T>(array);

        /// <summary>
        /// Gets an empty <see cref="Memory2D{T}"/> instance.
        /// </summary>
        public static Memory2D<T> Empty => default;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="Memory2D{T}"/> instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (this.height | this.width) == 0;
        }

        /// <summary>
        /// Gets the length of the current <see cref="Memory2D{T}"/> instance.
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.height * this.width;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Microsoft.Toolkit.HighPerformance.Memory.Memory2D<{typeof(T)}>[{this.height}, {this.width}]";
        }

        /// <summary>
        /// Slices the current instance with the specified parameters.
        /// </summary>
        /// <param name="row">The target row to map within the current instance.</param>
        /// <param name="column">The target column to map within the current instance.</param>
        /// <param name="width">The width to map within the current instance.</param>
        /// <param name="height">The height to map within the current instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when either <paramref name="height"/>, <paramref name="width"/> or <paramref name="height"/>
        /// are negative or not within the bounds that are valid for the current instance.
        /// </exception>
        /// <returns>A new <see cref="Memory2D{T}"/> instance representing a slice of the current one.</returns>
        [Pure]
        public Memory2D<T> Slice(int row, int column, int width, int height)
        {
            throw new NotImplementedException("TODO");
        }

        /// <summary>
        /// Gets a <see cref="Span2D{T}"/> instance from the current memory.
        /// </summary>
        public Span2D<T> Span
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if (!(this.instance is null))
                {
                    ref T r0 = ref this.instance.DangerousGetObjectDataReferenceAt<T>(this.offset);

                    return new Span2D<T>(ref r0, this.height, this.width, this.pitch);
                }

                return default;
            }
        }

        /// <summary>
        /// Copies the contents of this <see cref="Memory2D{T}"/> into a destination <see cref="Memory{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="Memory{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination" /> is shorter than the source <see cref="Memory2D{T}"/> instance.
        /// </exception>
        public void CopyTo(Memory<T> destination) => Span.CopyTo(destination.Span);

        /// <summary>
        /// Attempts to copy the current <see cref="Memory2D{T}"/> instance to a destination <see cref="Memory{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Memory{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operaation was successful.</returns>
        public bool TryCopyTo(Memory<T> destination) => Span.TryCopyTo(destination.Span);

        /// <summary>
        /// Creates a handle for the memory.
        /// The GC will not move the memory until the returned <see cref="MemoryHandle"/>
        /// is disposed, enabling taking and using the memory's address.
        /// </summary>
        /// <exception cref="ArgumentException">
        /// An instance with nonprimitive (non-blittable) members cannot be pinned.
        /// </exception>
        /// <returns>A <see cref="MemoryHandle"/> instance wrapping the pinned handle.</returns>
        public unsafe MemoryHandle Pin()
        {
            if (!(this.instance is null))
            {
                GCHandle handle = GCHandle.Alloc(this.instance, GCHandleType.Pinned);

                void* pointer = Unsafe.AsPointer(ref this.instance.DangerousGetObjectDataReferenceAt<T>(this.offset));

                return new MemoryHandle(pointer, handle);
            }

            return default;
        }

        /// <summary>
        /// Copies the contents of the current <see cref="Memory2D{T}"/> instance into a new 2D array.
        /// </summary>
        /// <returns>A 2D array containing the data in the current <see cref="Memory2D{T}"/> instance.</returns>
        [Pure]
        public T[,] ToArray() => Span.ToArray();

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override bool Equals(object? obj)
        {
            return obj is Memory2D<T> memory && Equals(memory);
        }

        /// <inheritdoc/>
        public bool Equals(Memory2D<T> other)
        {
            return
                this.instance == other.instance &&
                this.offset == other.offset &&
                this.height == other.height &&
                this.width == other.width &&
                this.pitch == other.pitch;
        }

        /// <inheritdoc/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public override int GetHashCode()
        {
            if (!(this.instance is null))
            {
                return HashCode.Combine(
                    RuntimeHelpers.GetHashCode(this.instance),
                    this.offset,
                    this.height,
                    this.width,
                    this.pitch);
            }

            return 0;
        }

        /// <summary>
        /// Throws an <see cref="ArrayTypeMismatchException"/> when using an array of an invalid type.
        /// </summary>
        private static void ThrowArrayTypeMismatchException()
        {
            throw new ArrayTypeMismatchException("The given array doesn't match the specified Span2D<T> type");
        }
    }
}
