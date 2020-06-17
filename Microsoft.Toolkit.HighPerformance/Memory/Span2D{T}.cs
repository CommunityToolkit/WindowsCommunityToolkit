// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if SPAN_RUNTIME_SUPPORT

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

        /// <summary>
        /// The <see cref="Span{T}"/> instance pointing to the first item in the target memory area.
        /// </summary>
        /// <remarks>
        /// The <see cref="Span{T}.Length"/> field maps to the height of the 2D region.
        /// This is done to save 4 bytes in the layout of the <see cref="Span2D{T}"/> type.
        /// </remarks>
        private readonly Span<T> span;

        /// <summary>
        /// The width of the specified 2D region.
        /// </summary>
        private readonly int width;

        /// <summary>
        /// The pitch of the specified 2D region.
        /// </summary>
        private readonly int pitch;

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
                ThrowArgumentExceptionForNegativeSize();
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
                ThrowArgumentExceptionForManagedType();
            }

            if ((height | width | pitch) < 0)
            {
                ThrowArgumentExceptionForNegativeSize();
            }

            this.span = new Span<T>(pointer, height);
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

            if (!typeof(T).IsValueType && array.GetType() != typeof(T[]))
            {
                ThrowArrayTypeMismatchException();
            }

            this.span = MemoryMarshal.CreateSpan(ref array[0, 0], array.GetLength(0));
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
                ThrowArgumentExceptionForNegativeOrInvalidParameter();
            }

            this.span = MemoryMarshal.CreateSpan(ref array[row, column], height);
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
            get => (this.span.Length | this.width) == 0;
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
                if ((uint)i >= (uint)this.span.Length ||
                    (uint)j >= (uint)this.width)
                {
                    ThrowIndexOutOfRangeException();
                }

                return ref Unsafe.Add(
                    ref MemoryMarshal.GetReference(this.span),
                    (i * (this.width + this.pitch)) + j);
            }
        }

        /// <summary>
        /// Gets the length of the current <see cref="Span2D{T}"/> instance.
        /// </summary>
        public int Length
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => this.span.Length * this.width;
        }

        /// <summary>
        /// Clears the contents of the current <see cref="Span2D{T}"/> instance.
        /// </summary>
        public void Clear()
        {
            if (this.pitch == 0)
            {
                // If the pitch is 0, it means all the target area is contiguous
                // in memory with no padding between row boundaries. In this case
                // we can just create a Span<T> over the area and use it to clear it.
                MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Length).Clear();
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
            if (this.pitch == 0)
            {
                // If the pitch is 0, we can copy in a single pass
                MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Length).CopyTo(destination);
            }
            else
            {
                if (Length > destination.Length)
                {
                    ThrowArgumentExceptionForDestinationTooShort();
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

#pragma warning disable CS0809 // Obsolete member overrides non-obsolete member
        /// <inheritdoc cref="Span{T}.Equals(object)"/>
        [EditorBrowsable(EditorBrowsableState.Never)]
        [Obsolete("Equals() on Span will always throw an exception. Use == instead.")]
        public override bool Equals(object obj)
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

        /// <summary>
        /// Fills the elements of this span with a specified value.
        /// </summary>
        /// <param name="value">The value to assign to each element of the <see cref="Span2D{T}"/> instance.</param>
        public void Fill(T value)
        {
            if (this.pitch == 0)
            {
                MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Length).Fill(value);
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

            if (this.Length != 0)
            {
                r0 = ref MemoryMarshal.GetReference(this.span);
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
            return ref MemoryMarshal.GetReference(this.span);
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
                left.span == right.span &&
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
        /// Copies the contents of the current <see cref="Span2D{T}"/> instance into a new 2D array.
        /// </summary>
        /// <returns>A 2D array containing the data in the current <see cref="Span2D{T}"/> instance.</returns>
        [Pure]
        public T[,] ToArray()
        {
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

            return array;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Microsoft.Toolkit.HighPerformance.Memory.Span2D<{typeof(T)}>[{this.span.Length}, {this.width}]";
        }

        /// <summary>
        /// Attempts to copy the current <see cref="Span2D{T}"/> instance to a destination <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Span{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operaation was successful.</returns>
        public bool TryCopyTo(Span<T> destination)
        {
            if (destination.Length >= Length)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Tries to get a <see cref="Span{T}"/> instance, if the underlying buffer is contiguous.
        /// </summary>
        /// <param name="span">The resulting <see cref="Span{T}"/>, in case of success.</param>
        /// <returns>Whether or not <paramref name="span"/> was correctly assigned.</returns>
        public bool TryAsSpan(out Span<T> span)
        {
            if (this.pitch == 0)
            {
                // We can only create a Span<T> if the buffer is contiguous
                span = MemoryMarshal.CreateSpan(ref MemoryMarshal.GetReference(this.span), Length);

                return true;
            }

            span = default;

            return false;
        }

        /// <summary>
        /// Provides an enumerator for the elements of a <see cref="Span2D{T}"/> instance.
        /// </summary>
        public ref struct Enumerator
        {
            /// <summary>
            /// The <see cref="Span{T}"/> instance pointing to the first item in the target memory area.
            /// </summary>
            /// <remarks>Just like in <see cref="Span2D{T}"/>, the length is the height of the 2D region.</remarks>
            private readonly Span<T> span;

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
                this.span = span.span;
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
                if (this.y < (this.span.Length - 1))
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
                    return ref Unsafe.Add(
                        ref MemoryMarshal.GetReference(this.span),
                        (this.y * (this.width + this.pitch)) + this.x);
                }
            }
        }

        /// <summary>
        /// Throws an <see cref="IndexOutOfRangeException"/> when the a given coordinate is invalid.
        /// </summary>
        /// <remarks>
        /// Throwing <see cref="IndexOutOfRangeException"/> is technically discouraged in the docs, but
        /// we're doing that here for consistency with the official <see cref="Span{T}"/> type from the BCL.
        /// </remarks>
        private static void ThrowIndexOutOfRangeException()
        {
            throw new IndexOutOfRangeException();
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when using the <see langword="void"/>* constructor with a managed type.
        /// </summary>
        private static void ThrowArgumentExceptionForManagedType()
        {
            throw new ArgumentException("Can't create a Span2D<T> from a pointer when T is a managed type");
        }

        /// <summary>
        /// Throws an <see cref="ArrayTypeMismatchException"/> when using an array of an invalid type.
        /// </summary>
        private static void ThrowArrayTypeMismatchException()
        {
            throw new ArrayTypeMismatchException("The given array doesn't match the specified Span2D<T> type");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a constructor parameter is negative.
        /// </summary>
        private static void ThrowArgumentExceptionForNegativeSize()
        {
            throw new ArgumentException("The size parameters must be positive values");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when a constructor parameter is negative or invalid.
        /// </summary>
        private static void ThrowArgumentExceptionForNegativeOrInvalidParameter()
        {
            throw new ArgumentException("The given parameters must be non negative and within valid range for the array");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentException"/> when the target span is too short.
        /// </summary>
        private static void ThrowArgumentExceptionForDestinationTooShort()
        {
            throw new ArgumentException("The target span is too short to copy all the current items to");
        }
    }
}

#endif
