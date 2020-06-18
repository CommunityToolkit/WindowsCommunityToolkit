// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Microsoft.Toolkit.HighPerformance.Extensions;

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
    public readonly ref struct Span2D<T>
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
        /// <exception cref="ArgumentException">
        /// Thrown when either <paramref name="height"/>, <paramref name="width"/> or <paramref name="pitch"/> are negative.
        /// </exception>
        public Span2D(ref T value, int height, int width, int pitch)
        {
            if ((height | width | pitch) < 0)
            {
                ThrowHelper.ThrowArgumentException();
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
        /// <exception cref="ArgumentException">
        /// Thrown when <typeparamref name="T"/> is a reference type or contains references. Also thrown
        /// when either <paramref name="height"/>, <paramref name="width"/> or <paramref name="pitch"/> are negative.
        /// </exception>
        public unsafe Span2D(void* pointer, int height, int width, int pitch)
        {
            if (RuntimeHelpers.IsReferenceOrContainsReferences<T>())
            {
                ThrowHelper.ThrowArgumentExceptionForManagedType();
            }

            if ((height | width | pitch) < 0)
            {
                ThrowHelper.ThrowArgumentException();
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
            this.span = MemoryMarshal.CreateSpan(ref array[0, 0], array.GetLength(0));
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
        /// <exception cref="ArgumentException">
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

            if (width > (columns - column))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForWidth();
            }

            if (height > (rows - row))
            {
                ThrowHelper.ThrowArgumentOutOfRangeExceptionForHeight();
            }

#if SPAN_RUNTIME_SUPPORT
            this.span = MemoryMarshal.CreateSpan(ref array[row, column], height);
#else
            this.instance = array;
            this.offset = array.DangerousGetObjectDataByteOffset(ref array.DangerousGetReferenceAt(row, column));
            this.height = array.GetLength(0);
#endif
            this.width = width;
            this.pitch = row + (array.GetLength(1) - column);
        }

        /// <summary>
        /// Gets an empty <see cref="Span2D{T}"/> instance.
        /// </summary>
        public static Span<T> Empty => default;

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
#if SPAN_RUNTIME_SUPPORT
            if (this.pitch == 0)
            {
                // If the pitch is 0, it means all the target area is contiguous
                // in memory with no padding between row boundaries. In this case
                // we can just create a Span<T> over the area and use it to clear it.
                MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Size).Clear();
            }
            else
            {
                ref T r0 = ref MemoryMarshal.GetReference(this.span);
                IntPtr step = (IntPtr)(this.width + this.pitch);

                // Clear each row individually, as they're not contiguous
                for (int i = 0; i < this.span.Length; i++)
                {
                    MemoryMarshal.CreateSpan(ref r0, this.width).Clear();

                    r0 = ref Unsafe.Add(ref r0, step);
                }
            }
#else
            // Fallback to the enumerator to traverse the span
            foreach (ref T item in this)
            {
                item = default!;
            }
#endif
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
#if SPAN_RUNTIME_SUPPORT
            if (this.pitch == 0)
            {
                // If the pitch is 0, we can copy in a single pass
                MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Size).CopyTo(destination);
            }
            else
            {
                if (Size > destination.Length)
                {
                    ThrowHelper.ThrowArgumentExceptionForDestinationTooShort();
                }

                ref T sourceRef = ref MemoryMarshal.GetReference(this.span);
                IntPtr step = (IntPtr)(this.width + this.pitch);
                int offset = 0;

                // Copy each row individually
                for (int i = 0; i < this.span.Length; i++)
                {
                    MemoryMarshal.CreateSpan(ref sourceRef, this.width).CopyTo(destination.Slice(offset));

                    sourceRef = ref Unsafe.Add(ref sourceRef, step);
                    offset += this.width;
                }
            }
#else
            // Similar to the previous case
            ref T destinationRef = ref MemoryMarshal.GetReference(destination);
            IntPtr offset = default;

            foreach (T item in this)
            {
                Unsafe.Add(ref destinationRef, offset) = item;

                offset += 1;
            }
#endif
        }

        /// <summary>
        /// Copies the contents of this <see cref="Span2D{T}"/> into a destination <see cref="Span2D{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span2D{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination" /> is shorter than the source <see cref="Span2D{T}"/> instance.
        /// </exception>
        public void CopyTo(Span2D<T> destination)
        {
            throw new NotImplementedException("TODO");
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
#if SPAN_RUNTIME_SUPPORT
            if (this.pitch == 0)
            {
                MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Size).Fill(value);
            }
            else
            {
                ref T sourceRef = ref MemoryMarshal.GetReference(this.span);
                IntPtr step = (IntPtr)(this.width + this.pitch);

                // Fill each row individually
                for (int i = 0; i < this.span.Length; i++)
                {
                    MemoryMarshal.CreateSpan(ref sourceRef, this.width).Fill(value);

                    sourceRef = ref Unsafe.Add(ref sourceRef, step);
                }
            }
#else
            // Use the enumerator again
            foreach (ref T item in this)
            {
                item = value;
            }
#endif
        }

        /// <summary>
        /// Returns an enumerator for the current <see cref="Span2D{T}"/> instance.
        /// </summary>
        /// <returns>
        /// An enumerator that can be used to traverse the items in the current <see cref="Span2D{T}"/> instance
        /// </returns>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this);

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
            throw new NotImplementedException("TODO");
        }

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
#if SPAN_RUNTIME_SUPPORT
            T[,] array = new T[this.span.Length, this.width];

            if (this.pitch == 0)
            {
                CopyTo(array.AsSpan());
            }
            else
            {
                ref T sourceRef = ref MemoryMarshal.GetReference(this.span);
                IntPtr step = (IntPtr)(this.width + this.pitch);
                int offset = 0;

                // Copy each row individually
                for (int i = 0; i < this.span.Length; i++)
                {
                    MemoryMarshal.CreateSpan(ref sourceRef, this.width).CopyTo(array.AsSpan().Slice(offset));

                    sourceRef = ref Unsafe.Add(ref sourceRef, step);
                    offset += this.width;
                }
            }
#else
            T[,] array = new T[this.height, this.width];

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
#if SPAN_RUNTIME_SUPPORT
            int height = this.span.Length;
#else
            int height = this.height;
#endif

            return $"Microsoft.Toolkit.HighPerformance.Memory.Span2D<{typeof(T)}>[{height}, {this.width}]";
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

        /// <summary>
        /// Provides an enumerator for the elements of a <see cref="Span2D{T}"/> instance.
        /// </summary>
        public ref struct Enumerator
        {
#if SPAN_RUNTIME_SUPPORT
            /// <summary>
            /// The <see cref="Span{T}"/> instance pointing to the first item in the target memory area.
            /// </summary>
            /// <remarks>Just like in <see cref="Span2D{T}"/>, the length is the height of the 2D region.</remarks>
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

            /// <summary>
            /// The current horizontal offset.
            /// </summary>
            private int x;

            /// <summary>
            /// The current vertical offset.
            /// </summary>
            private int y;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="span">The target <see cref="Span2D{T}"/> instance to enumerate.</param>
            internal Enumerator(Span2D<T> span)
            {
#if SPAN_RUNTIME_SUPPORT
                this.span = span.span;
#else
                this.instance = span.instance;
                this.offset = span.offset;
                this.height = span.height;
#endif
                this.width = span.width;
                this.pitch = span.pitch;
                this.x = -1;
                this.y = 0;
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
            /// </summary>
            /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int x = this.x + 1;

                // Horizontal move, within range
                if (x < this.width)
                {
                    this.x = x;

                    return true;
                }

                // We reached the end of a row and there is at least
                // another row available: wrap to a new line and continue.
                if (
#if SPAN_RUNTIME_SUPPORT
                    this.y < (this.span.Length - 1)
#else
                    this.y < this.height - 1
#endif
                )
                {
                    this.x = 0;
                    this.y++;

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Gets the duck-typed <see cref="System.Collections.Generic.IEnumerator{T}.Current"/> property.
            /// </summary>
            public ref T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
#if SPAN_RUNTIME_SUPPORT
                    ref T r0 = ref MemoryMarshal.GetReference(this.span);
#else
                    ref T r0 = ref this.instance!.DangerousGetObjectDataReferenceAt<T>(this.offset);
#endif
                    int index = (this.y * (this.width + this.pitch)) + this.x;

                    return ref Unsafe.Add(ref r0, index);
                }
            }
        }
    }
}
