// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Extensions;
using Microsoft.Toolkit.HighPerformance.Memory.Internals;

namespace Microsoft.Toolkit.HighPerformance.Memory
{
    /// <summary>
    /// <see cref="Span2D{T}"/> represents a 2D region of arbitrary memory. Like the <see cref="Span{T}"/> type,
    /// it can point to either managed or native memory, or to memory allocated on the stack. It is type- and memory-safe.
    /// One key difference with <see cref="Span{T}"/> and arrays is that the underlying buffer for a <see cref="Span2D{T}"/>
    /// instance might not be contiguous in memory: this is supported to enable mapping arbitrary 2D regions even if they
    /// require padding between boundaries of sequential rows. All this logic is handled internally by the <see cref="Span2D{T}"/>
    /// type and it is transparent to the user, but note that working over discontiguous buffers has a performance impact.
    /// </summary>
    /// <typeparam name="T">The type of items in the current <see cref="Span2D{T}"/> instance.</typeparam>
    public readonly ref partial struct Span2D<T>
    {
        // Let's consider a representation of a discontiguous 2D memory
        // region within an existing array. The data is represented in
        // row-major order as usual, and the 'XX' grid cells represent
        // locations that are mapped by a given Span2D<T> instance:
        //
        //  reference__  _________width_________  ________...
        //             \/                       \/
        // | -- | -- | |- | -- | -- | -- | -- | -- | -- | -- |
        // | -- | -- | XX | XX | XX | XX | XX | XX | -- | -- |_
        // | -- | -- | XX | XX | XX | XX | XX | XX | -- | -- | |
        // | -- | -- | XX | XX | XX | XX | XX | XX | -- | -- | |_height
        // | -- | -- | XX | XX | XX | XX | XX | XX | -- | -- |_|
        // | -- | -- | -- | -- | -- | -- | -- | -- | -- | -- |
        // | -- | -- | -- | -- | -- | -- | -- | -- | -- | -- |
        // ...__pitch__/
        //
        // The pitch is used to calculate the offset between each
        // discontiguous row, so that any arbitrary memory locations
        // can be used to internally represent a 2D span. This gives
        // users much more flexibility when creating spans from data.
#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// The <see cref="Span{T}"/> instance pointing to the first item in the target memory area.
        /// </summary>
        /// <remarks>
        /// The <see cref="Span{T}.Length"/> field maps to the height of the 2D region.
        /// This is done to save 4 bytes in the layout of the <see cref="Span2D{T}"/> type.
        /// </remarks>
        private readonly Span<T> span;
#else
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
#endif

        /// <summary>
        /// The width of the specified 2D region.
        /// </summary>
        private readonly int width;

        /// <summary>
        /// The pitch of the specified 2D region.
        /// </summary>
        private readonly int pitch;

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct with the specified parameters.
        /// </summary>
        /// <param name="value">The reference to the first <typeparamref name="T"/> item to map.</param>
        /// <param name="height">The height of the 2D memory area to map.</param>
        /// <param name="width">The width of the 2D memory area to map.</param>
        /// <param name="pitch">The pitch of the 2D memory area to map (the distance between each row).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the parameters are negative.</exception>
        public Span2D(ref T value, int height, int width, int pitch)
        {
            if (width < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if (height < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

            if (pitch < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForPitch();
            }

            this.span = MemoryMarshal.CreateSpan(ref value, height);
            this.width = width;
            this.pitch = pitch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct with the specified parameters.
        /// </summary>
        /// <param name="pointer">The pointer to the first <typeparamref name="T"/> item to map.</param>
        /// <param name="height">The height of the 2D memory area to map.</param>
        /// <param name="width">The width of the 2D memory area to map.</param>
        /// <param name="pitch">The pitch of the 2D memory area to map (the distance between each row).</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the parameters are negative.</exception>
        public unsafe Span2D(void* pointer, int height, int width, int pitch)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                ThrowHelper.ThrowArgumentExceptionForManagedType();
            }

            if (width < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if (height < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

            if (pitch < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForPitch();
            }

            this.span = new Span<T>(pointer, height);
            this.width = width;
            this.pitch = pitch;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct with the specified parameters.
        /// </summary>
        /// <param name="instance">The target <see cref="object"/> instance.</param>
        /// <param name="offset">The initial offset within <see cref="instance"/>.</param>
        /// <param name="height">The height of the 2D memory area to map.</param>
        /// <param name="width">The width of the 2D memory area to map.</param>
        /// <param name="pitch">The pitch of the 2D memory area to map.</param>
        internal Span2D(object instance, IntPtr offset, int height, int width, int pitch)
        {
            this.instance = instance;
            this.offset = offset;
            this.height = height;
            this.width = width;
            this.pitch = pitch;
        }
#endif

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct.
        /// </summary>
        /// <param name="array">The target array to wrap.</param>
        /// <param name="width">The width of each row in the resulting 2D area.</param>
        /// <param name="height">The height of the resulting 2D area.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when either <paramref name="height"/> or <paramref name="width"/> are invalid.
        /// </exception>
        /// <remarks>The total area must match the lenght of <paramref name="array"/>.</remarks>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span2D(T[] array, int width, int height)
            : this(array, 0, width, height, 0)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct.
        /// </summary>
        /// <param name="array">The target array to wrap.</param>
        /// <param name="offset">The initial offset within <paramref name="array"/>.</param>
        /// <param name="width">The width of each row in the resulting 2D area.</param>
        /// <param name="height">The height of the resulting 2D area.</param>
        /// <param name="pitch">The pitch in the resulting 2D area.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when one of the input parameters is out of range.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the requested area is outside of bounds for <paramref name="array"/>.
        /// </exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span2D(T[] array, int offset, int width, int height, int pitch)
        {
            if (array.IsCovariant())
            {
                ThrowHelper.ThrowArrayTypeMismatchException();
            }

            if ((uint)offset > (uint)array.Length)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForOffset();
            }

            if (width < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if (height < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

            if (pitch < 0)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForPitch();
            }

            if (width == 0 || height == 0)
            {
                this = default;

                return;
            }

            int
                remaining = array.Length - offset,
                area = ((width + pitch) * (height - 1)) + width;

            if (area > remaining)
            {
                ThrowHelper.ThrowArgumentException();
            }

#if SPAN_RUNTIME_SUPPORT
            this.span = MemoryMarshal.CreateSpan(ref array.DangerousGetReferenceAt(offset), height);
#else
            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(offset));
            this.height = height;
#endif
            this.width = width;
            this.pitch = pitch;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct wrapping a 2D array.
        /// </summary>
        /// <param name="array">The given 2D array to wrap.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        public Span2D(T[,]? array)
        {
            if (array is null)
            {
                this = default;

                return;
            }

            if (array.IsCovariant())
            {
                ThrowHelper.ThrowArrayTypeMismatchException();
            }

#if SPAN_RUNTIME_SUPPORT
            this.span = MemoryMarshal.CreateSpan(ref array.DangerousGetReference(), array.GetLength(0));
#else
            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(0, 0));
            this.height = array.GetLength(0);
#endif
            this.width = array.GetLength(1);
            this.pitch = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct wrapping a 2D array.
        /// </summary>
        /// <param name="array">The given 2D array to wrap.</param>
        /// <param name="row">The target row to map within <paramref name="array"/>.</param>
        /// <param name="column">The target column to map within <paramref name="array"/>.</param>
        /// <param name="width">The width to map within <paramref name="array"/>.</param>
        /// <param name="height">The height to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when either <paramref name="height"/>, <paramref name="width"/> or <paramref name="height"/>
        /// are negative or not within the bounds that are valid for <paramref name="array"/>.
        /// </exception>
        public Span2D(T[,] array, int row, int column, int width, int height)
        {
            if (array.IsCovariant())
            {
                ThrowHelper.ThrowArrayTypeMismatchException();
            }

            int
                rows = array.GetLength(0),
                columns = array.GetLength(1);

            if ((uint)row >= (uint)rows)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForRow();
            }

            if ((uint)column >= (uint)columns)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForColumn();
            }

            if ((uint)width > (uint)(columns - column))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if ((uint)height > (uint)(rows - row))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

#if SPAN_RUNTIME_SUPPORT
            this.span = MemoryMarshal.CreateSpan(ref array.DangerousGetReferenceAt(row, column), height);
#else
            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(row, column));
            this.height = array.GetLength(0);
#endif
            this.width = width;
            this.pitch = columns - width;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct wrapping a layer in a 3D array.
        /// </summary>
        /// <param name="array">The given 3D array to wrap.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a parameter is invalid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span2D(T[,,] array, int depth)
        {
            if (array.IsCovariant())
            {
                ThrowHelper.ThrowArrayTypeMismatchException();
            }

            if ((uint)depth >= (uint)array.GetLength(0))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForDepth();
            }

#if SPAN_RUNTIME_SUPPORT
            this.span = MemoryMarshal.CreateSpan(ref array.DangerousGetReferenceAt(depth, 0, 0), array.GetLength(1));
#else
            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(depth, 0, 0));
            this.height = array.GetLength(1);
#endif
            this.width = array.GetLength(2);
            this.pitch = 0;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Span2D{T}"/> struct wrapping a layer in a 3D array.
        /// </summary>
        /// <param name="array">The given 3D array to wrap.</param>
        /// <param name="depth">The target layer to map within <paramref name="array"/>.</param>
        /// <param name="row">The target row to map within <paramref name="array"/>.</param>
        /// <param name="column">The target column to map within <paramref name="array"/>.</param>
        /// <param name="width">The width to map within <paramref name="array"/>.</param>
        /// <param name="height">The height to map within <paramref name="array"/>.</param>
        /// <exception cref="ArrayTypeMismatchException">
        /// Thrown when <paramref name="array"/> doesn't match <typeparamref name="T"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when a parameter is invalid.</exception>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span2D(T[,,] array, int depth, int row, int column, int width, int height)
        {
            if (array.IsCovariant())
            {
                ThrowHelper.ThrowArrayTypeMismatchException();
            }

            if ((uint)depth >= (uint)array.GetLength(0))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForDepth();
            }

            int
                rows = array.GetLength(1),
                columns = array.GetLength(2);

            if ((uint)row >= (uint)rows)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForRow();
            }

            if ((uint)column >= (uint)columns)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForColumn();
            }

            if ((uint)width > (uint)(columns - column))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if ((uint)height > (uint)(rows - row))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

#if SPAN_RUNTIME_SUPPORT
            this.span = MemoryMarshal.CreateSpan(ref array.DangerousGetReferenceAt(depth, row, column), height);
#else
            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(depth, row, column));
            this.height = height;
#endif
            this.width = width;
            this.pitch = columns - width;
        }

        /// <summary>
        /// Gets an empty <see cref="Span2D{T}"/> instance.
        /// </summary>
        public static Span2D<T> Empty => default;

        /// <summary>
        /// Gets a value indicating whether the current <see cref="Span2D{T}"/> instance is empty.
        /// </summary>
        public bool IsEmpty
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => (Height | Width) == 0;
        }

        /// <summary>
        /// Gets the length of the current <see cref="Span2D{T}"/> instance.
        /// </summary>
        public int Size
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => Height * Width;
        }

        /// <summary>
        /// Gets the height of the underlying 2D memory area.
        /// </summary>
        public int Height
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if SPAN_RUNTIME_SUPPORT
                return this.span.Length;
#else
                return this.height;
#endif
            }
        }

        /// <summary>
        /// Gets the width of the underlying 2D memory area.
        /// </summary>
        public int Width
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.width;
        }

        /// <summary>
        /// Gets the element at the specified zero-based indices.
        /// </summary>
        /// <param name="i">The target row to get the element from.</param>
        /// <param name="j">The target column to get the element from.</param>
        /// <returns>A reference to the element at the specified indices.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown when either <paramref name="i"/> or <paramref name="j"/> are invalid.
        /// </exception>
        public ref T this[int i, int j]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
                if ((uint)i >= (uint)Height ||
                    (uint)j >= (uint)Width)
                {
                    ThrowHelper.ThrowIndexOutOfRangeException();
                }

#if SPAN_RUNTIME_SUPPORT
                ref T r0 = ref MemoryMarshal.GetReference(this.span);
#else
                ref T r0 = ref this.instance!.DangerousGetObjectDataReferenceAt<T>(this.offset);
#endif
                int index = (i * (this.width + this.pitch)) + j;

                return ref Unsafe.Add(ref r0, index);
            }
        }

        /// <summary>
        /// Clears the contents of the current <see cref="Span2D{T}"/> instance.
        /// </summary>
        public void Clear()
        {
            if (TryGetSpan(out Span<T> span))
            {
                span.Clear();
            }
            else
            {
#if SPAN_RUNTIME_SUPPORT
                // Clear one row at a time
                for (int i = 0; i < Height; i++)
                {
                    GetRowSpan(i).Clear();
                }
#else
                // Fallback to the enumerator
                foreach (ref T item in this)
                {
                    item = default!;
                }
#endif
            }
        }

        /// <summary>
        /// Copies the contents of this <see cref="Span2D{T}"/> into a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination" /> is shorter than the source <see cref="Span2D{T}"/> instance.
        /// </exception>
        public void CopyTo(Span<T> destination)
        {
            if (TryGetSpan(out Span<T> span))
            {
                span.CopyTo(destination);
            }
            else
            {
                if (Size > destination.Length)
                {
                    ThrowHelper.ThrowArgumentExceptionForDestinationTooShort();
                }

#if SPAN_RUNTIME_SUPPORT
                // Copy each row individually
                for (int i = 0, j = 0; i < Height; i++, j += Width)
                {
                    GetRowSpan(i).CopyTo(destination.Slice(j));
                }
#else
                ref T destinationRef = ref MemoryMarshal.GetReference(destination);
                IntPtr offset = default;

                // Fallback to the enumerator again
                foreach (T item in this)
                {
                    Unsafe.Add(ref destinationRef, offset) = item;

                    offset += 1;
                }
#endif
            }
        }

        /// <summary>
        /// Copies the contents of this <see cref="Span2D{T}"/> into a destination <see cref="Span2D{T}"/> instance.
        /// For this API to succeed, the target <see cref="Span2D{T}"/> has to have the same shape as the current one.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span2D{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination" /> is shorter than the source <see cref="Span2D{T}"/> instance.
        /// </exception>
        public void CopyTo(Span2D<T> destination)
        {
            if (destination.Height != Height ||
                destination.width != Width)
            {
                ThrowHelper.ThrowArgumentException();
            }

            if (destination.TryGetSpan(out Span<T> span))
            {
                CopyTo(span);
            }
            else
            {
#if SPAN_RUNTIME_SUPPORT
                // Copy each row individually
                for (int i = 0; i < Height; i++)
                {
                    GetRowSpan(i).CopyTo(destination.GetRowSpan(i));
                }
#else
                Enumerator destinationEnumerator = destination.GetEnumerator();

                // Fallback path with two enumerators
                foreach (T item in this)
                {
                    _ = destinationEnumerator.MoveNext();

                    destinationEnumerator.Current = item;
                }
#endif
            }
        }

        /// <summary>
        /// Attempts to copy the current <see cref="Span2D{T}"/> instance to a destination <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Span{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool TryCopyTo(Span<T> destination)
        {
            if (destination.Length >= Size)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to copy the current <see cref="Span2D{T}"/> instance to a destination <see cref="Span2D{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Span2D{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool TryCopyTo(Span2D<T> destination)
        {
            if (destination.Height == Height &&
                destination.Width == Width)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Fills the elements of this span with a specified value.
        /// </summary>
        /// <param name="value">The value to assign to each element of the <see cref="Span2D{T}"/> instance.</param>
        public void Fill(T value)
        {
            if (TryGetSpan(out Span<T> span))
            {
                span.Fill(value);
            }
            else
            {
#if SPAN_RUNTIME_SUPPORT
                // Fill one row at a time
                for (int i = 0; i < Height; i++)
                {
                    GetRowSpan(i).Fill(value);
                }
#else
                // Fill using the enumerator as above
                foreach (ref T item in this)
                {
                    item = value;
                }
#endif
            }
        }

        /// <summary>
        /// Returns a reference to the 0th element of the <see cref="Span2D{T}"/> instance. If the current
        /// instance is empty, returns a <see langword="null"/> reference. It can be used for pinning
        /// and is required to support the use of span within a fixed statement.
        /// </summary>
        /// <returns>A reference to the 0th element, or a <see langword="null"/> reference.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        [EditorBrowsable(EditorBrowsableState.Never)]
        public unsafe ref T GetPinnableReference()
        {
            ref T r0 = ref Unsafe.AsRef<T>(null);

            if (Size != 0)
            {
                r0 = ref this.DangerousGetReference();
            }

            return ref r0;
        }

        /// <summary>
        /// Returns a reference to the first element within the current instance, with no bounds check.
        /// </summary>
        /// <returns>A reference to the first element within the current instance.</returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ref T DangerousGetReference()
        {
#if SPAN_RUNTIME_SUPPORT
            return ref MemoryMarshal.GetReference(this.span);
#else
            return ref this.instance!.DangerousGetObjectDataReferenceAt<T>(this.offset);
#endif
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
        /// <returns>A new <see cref="Span2D{T}"/> instance representing a slice of the current one.</returns>
        [Pure]
        public Span2D<T> Slice(int row, int column, int width, int height)
        {
            if ((uint)row >= Height)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForRow();
            }

            if ((uint)column >= this.width)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForColumn();
            }

            if ((uint)width > (this.width - column))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if ((uint)height > (Height - row))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

            int shift = ((this.width + this.pitch) * row) + column;

#if SPAN_RUNTIME_SUPPORT
            ref T r0 = ref Unsafe.Add(ref MemoryMarshal.GetReference(this.span), shift);
            int pitch = this.pitch + column;

            return new Span2D<T>(ref r0, height, width, pitch);
#else
            unsafe
            {
                IntPtr offset = (IntPtr)((byte*)this.offset + shift);

                return new Span2D<T>(this.instance!, offset, height, width, this.pitch);
            }
#endif
        }

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Gets a <see cref="Span{T}"/> for a specified row.
        /// </summary>
        /// <param name="row">The index of the target row to retrieve.</param>
        /// <exception cref="ArgumentOutOfRangeException">Throw when <paramref name="row"/> is out of range.</exception>
        /// <returns>The resulting row <see cref="Span{T}"/>.</returns>
        [Pure]
        public Span<T> GetRowSpan(int row)
        {
            if ((uint)row >= (uint)Height)
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForRow();
            }

            int offset = (this.width + this.pitch) * row;
            ref T r0 = ref MemoryMarshal.GetReference(this.span);
            ref T r1 = ref Unsafe.Add(ref r0, offset);

            return MemoryMarshal.CreateSpan(ref r1, this.width);
        }
#endif

        /// <summary>
        /// Tries to get a <see cref="Span{T}"/> instance, if the underlying buffer is contiguous.
        /// </summary>
        /// <param name="span">The resulting <see cref="Span{T}"/>, in case of success.</param>
        /// <returns>Whether or not <paramref name="span"/> was correctly assigned.</returns>
        public bool TryGetSpan(out Span<T> span)
        {
            if (this.pitch == 0)
            {
#if SPAN_RUNTIME_SUPPORT
                // We can only create a Span<T> if the buffer is contiguous
                span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Size);

                return true;
#else
                // An empty Span2D<T> is still valid
                if (this.instance is null)
                {
                    span = default;

                    return true;
                }

                // Without Span<T> runtime support, we can only get a Span<T> from a T[] instance
                if (this.instance.GetType() == typeof(T[]))
                {
                    span = Unsafe.As<T[]>(this.instance).AsSpan((int)this.offset, Size);

                    return true;
                }
#endif
            }

            span = default;

            return false;
        }

        /// <summary>
        /// Copies the contents of the current <see cref="Span2D{T}"/> instance into a new 2D array.
        /// </summary>
        /// <returns>A 2D array containing the data in the current <see cref="Span2D{T}"/> instance.</returns>
        [Pure]
        public T[,] ToArray()
        {
            T[,] array = new T[Height, this.width];

#if SPAN_RUNTIME_SUPPORT
            CopyTo(array.AsSpan());
#else
            // Skip the initialization if the array is empty
            if (Size > 0)
            {
                ref T r0 = ref array.DangerousGetReference();
                IntPtr offset = default;

                // Fallback once again on the enumerator to copy the items
                foreach (T item in this)
                {
                    Unsafe.Add(ref r0, offset) = item;

                    offset += 1;
                }
            }
#endif

            return array;
        }

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        /// <inheritdoc cref="Span{T}.Equals(object)"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Equals() on Span will always throw an exception. Use == instead.")]
        public override bool Equals(object? obj)
        {
            throw new NotSupportedException("Microsoft.Toolkit.HighPerformance.Span2D<T>.Equals(object) is not supported");
        }

        /// <inheritdoc cref="Span{T}.GetHashCode()"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("GetHashCode() on Span will always throw an exception.")]
        public override int GetHashCode()
        {
            throw new NotSupportedException("Microsoft.Toolkit.HighPerformance.Span2D<T>.GetHashCode() is not supported");
        }
#pragma warning restore CS0809

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Microsoft.Toolkit.HighPerformance.Memory.Span2D<{typeof(T)}>[{Height}, {this.width}]";
        }

        /// <summary>
        /// Checks whether two <see cref="Span2D{T}"/> instances are equal.
        /// </summary>
        /// <param name="left">The first <see cref="Span2D{T}"/> instance to compare.</param>
        /// <param name="right">The second <see cref="Span2D{T}"/> instance to compare.</param>
        /// <returns>Whether or not <paramref name="left"/> and <paramref name="right"/> are equal.</returns>
        public static bool operator ==(Span2D<T> left, Span2D<T> right)
        {
            return
#if SPAN_RUNTIME_SUPPORT
                left.span == right.span &&
#else
                ReferenceEquals(left.instance, right.instance) &&
                left.offset == right.offset &&
                left.height == right.height &&
#endif
                left.width == right.width &&
                left.pitch == right.pitch;
        }

        /// <summary>
        /// Checks whether two <see cref="Span2D{T}"/> instances are not equal.
        /// </summary>
        /// <param name="left">The first <see cref="Span2D{T}"/> instance to compare.</param>
        /// <param name="right">The second <see cref="Span2D{T}"/> instance to compare.</param>
        /// <returns>Whether or not <paramref name="left"/> and <paramref name="right"/> are not equal.</returns>
        public static bool operator !=(Span2D<T> left, Span2D<T> right)
        {
            return !(left == right);
        }

        /// <summary>
        /// Implicily converts a given 2D array into a <see cref="Span2D{T}"/> instance.
        /// </summary>
        /// <param name="array">The input 2D array to convert.</param>
        public static implicit operator Span2D<T>(T[,]? array) => new Span2D<T>(array);
    }
}
