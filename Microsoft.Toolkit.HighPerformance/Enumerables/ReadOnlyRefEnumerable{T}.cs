// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
#if SPAN_RUNTIME_SUPPORT
using System.Runtime.InteropServices;
#endif
using Microsoft.Toolkit.HighPerformance.Helpers.Internals;
using Microsoft.Toolkit.HighPerformance.Memory.Internals;

#if !SPAN_RUNTIME_SUPPORT
using RuntimeHelpers = Microsoft.Toolkit.HighPerformance.Helpers.Internals.RuntimeHelpers;
#endif

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that iterates readonly items from arbitrary memory locations.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    public readonly ref struct ReadOnlyRefEnumerable<T>
    {
#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// The <see cref="ReadOnlySpan{T}"/> instance pointing to the first item in the target memory area.
        /// </summary>
        /// <remarks>The <see cref="ReadOnlySpan{T}.Length"/> field maps to the total available length.</remarks>
        private readonly ReadOnlySpan<T> span;

        /// <summary>
        /// Gets the total available length for the sequence.
        /// </summary>
        public int Length => span.Length;
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
        /// Gets the total available length for the sequence.
        /// </summary>
        public int Length { get; }
#endif

        /// <summary>
        /// The distance between items in the sequence to enumerate.
        /// </summary>
        /// <remarks>The distance refers to <typeparamref name="T"/> items, not byte offset.</remarks>
        private readonly int step;

        /// <summary>
        /// Gets the element at the specified zero-based index.
        /// </summary>
        /// <returns>A reference to the element at the specified index.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown when <paramref name="index"/> is invalid.
        /// </exception>
        public ref readonly T this[int index]
        {
            get
            {
                if ((uint)index >= (uint)Length)
                {
                    ThrowHelper.ThrowIndexOutOfRangeException();
                }

                return ref DangerousGetReferenceAt(index);
            }
        }

#if NETSTANDARD2_1_OR_GREATER
        /// <summary>
        /// Gets the element at the specified zero-based index.
        /// </summary>
        /// <returns>A reference to the element at the specified index.</returns>
        /// <exception cref="IndexOutOfRangeException">
        /// Thrown when <paramref name="index"/> is invalid.
        /// </exception>
        public ref readonly T this[Index index]
        {
            get => ref this[index.GetOffset(Length)];
        }
#endif

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRefEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="span">The <see cref="ReadOnlySpan{T}"/> instance pointing to the first item in the target memory area.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        private ReadOnlyRefEnumerable(ReadOnlySpan<T> span, int step)
        {
            this.span = span;
            this.step = step;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRefEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="reference">A reference to the first item of the sequence.</param>
        /// <param name="length">The number of items in the sequence.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyRefEnumerable(in T reference, int length, int step)
        {
            this.span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(reference), length);
            this.step = step;
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ReadOnlyRefEnumerable{T}"/> struct with the specified parameters.
        /// </summary>
        /// <param name="value">The reference to the first <typeparamref name="T"/> item to map.</param>
        /// <param name="length">The number of items in the sequence.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        /// <returns>A <see cref="ReadOnlyRefEnumerable{T}"/> instance with the specified parameters.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when one of the parameters are negative.</exception>
        [Pure]
        public static ReadOnlyRefEnumerable<T> DangerousCreate(in T value, int length, int step)
        {
            if (length < 0)
            {
                ThrowArgumentOutOfRangeExceptionForLength();
            }

            if (step < 0)
            {
                ThrowArgumentOutOfRangeExceptionForStep();
            }

            OverflowHelper.EnsureIsInNativeIntRange(length, 1, step);

            return new ReadOnlyRefEnumerable<T>(in value, length, step);
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlyRefEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="instance">The target <see cref="object"/> instance.</param>
        /// <param name="offset">The initial offset within <see paramref="instance"/>.</param>
        /// <param name="length">The number of items in the sequence.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyRefEnumerable(object? instance, IntPtr offset, int length, int step)
        {
            this.instance = instance;
            this.offset = offset;
            Length = length;
            this.step = step;
        }
#endif

        /// <summary>
        /// Returns a reference to the first element within the current instance, with no bounds check.
        /// </summary>
        /// <returns>A reference to the first element within the current instance.</returns>
        internal ref readonly T DangerousGetReference()
        {
#if SPAN_RUNTIME_SUPPORT
            return ref MemoryMarshal.GetReference(this.span);
#else
            return ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
#endif
        }

        /// <summary>
        /// Returns a reference to a specified element within the current instance, with no bounds check.
        /// </summary>
        /// <returns>A reference to the element at the specified index.</returns>
        internal ref readonly T DangerousGetReferenceAt(int index)
        {
#if SPAN_RUNTIME_SUPPORT
            ref T r0 = ref MemoryMarshal.GetReference(this.span);
#else
            ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
#endif
            // Here we just offset by shifting down as if we were traversing a 2D array with a
            // a single column, with the width of each row represented by the step, the height
            // represented by the current position, and with only the first element of each row
            // being inspected. We can perform all the indexing operations in this type as nint,
            // as the maximum offset is guaranteed never to exceed the maximum value, since on
            // 32 bit architectures it's not possible to allocate that much memory anyway.
            nint offset = (nint)(uint)index * (nint)(uint)this.step;
            ref T ri = ref Unsafe.Add(ref r0, offset);
            return ref ri;
        }

        /// <inheritdoc cref="System.Collections.IEnumerable.GetEnumerator"/>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator()
        {
            return new Enumerator(this);
        }

        /// <summary>
        /// Copies the contents of this <see cref="ReadOnlyRefEnumerable{T}"/> into a destination <see cref="RefEnumerable{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="RefEnumerable{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination"/> is shorter than the source <see cref="ReadOnlyRefEnumerable{T}"/> instance.
        /// </exception>
        public void CopyTo(RefEnumerable<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.step == 1)
            {
                destination.CopyFrom(this.span);

                return;
            }

            if (destination.Step == 1)
            {
                CopyTo(destination.Span);

                return;
            }

            ref T sourceRef = ref this.span.DangerousGetReference();
            ref T destinationRef = ref destination.Span.DangerousGetReference();
            int
                sourceLength = this.span.Length,
                destinationLength = destination.Span.Length;
#else
            ref T sourceRef = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
            ref T destinationRef = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(destination.Instance, destination.Offset);
            int
                sourceLength = Length,
                destinationLength = destination.Length;
#endif

            if ((uint)destinationLength < (uint)sourceLength)
            {
                ThrowArgumentExceptionForDestinationTooShort();
            }

            RefEnumerableHelper.CopyTo(ref sourceRef, ref destinationRef, (nint)(uint)sourceLength, (nint)(uint)this.step, (nint)(uint)destination.Step);
        }

        /// <summary>
        /// Attempts to copy the current <see cref="ReadOnlyRefEnumerable{T}"/> instance to a destination <see cref="RefEnumerable{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="RefEnumerable{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool TryCopyTo(RefEnumerable<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            int
                sourceLength = this.span.Length,
                destinationLength = destination.Span.Length;
#else
            int
                sourceLength = Length,
                destinationLength = destination.Length;
#endif

            if (destinationLength >= sourceLength)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the contents of this <see cref="RefEnumerable{T}"/> into a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination"/> is shorter than the source <see cref="RefEnumerable{T}"/> instance.
        /// </exception>
        public void CopyTo(Span<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.step == 1)
            {
                this.span.CopyTo(destination);

                return;
            }

            ref T sourceRef = ref this.span.DangerousGetReference();
            int length = this.span.Length;
#else
            ref T sourceRef = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
            int length = Length;
#endif
            if ((uint)destination.Length < (uint)length)
            {
                ThrowArgumentExceptionForDestinationTooShort();
            }

            ref T destinationRef = ref destination.DangerousGetReference();

            RefEnumerableHelper.CopyTo(ref sourceRef, ref destinationRef, (nint)(uint)length, (nint)(uint)this.step);
        }

        /// <summary>
        /// Attempts to copy the current <see cref="RefEnumerable{T}"/> instance to a destination <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Span{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public bool TryCopyTo(Span<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            int length = this.span.Length;
#else
            int length = Length;
#endif

            if (destination.Length >= length)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <inheritdoc cref="RefEnumerable{T}.ToArray"/>
        [Pure]
        public T[] ToArray()
        {
#if SPAN_RUNTIME_SUPPORT
            int length = this.span.Length;
#else
            int length = Length;
#endif

            // Empty array if no data is mapped
            if (length == 0)
            {
                return Array.Empty<T>();
            }

            T[] array = new T[length];

            CopyTo(array);

            return array;
        }

        /// <summary>
        /// Implicitly converts a <see cref="RefEnumerable{T}"/> instance into a <see cref="ReadOnlyRefEnumerable{T}"/> one.
        /// </summary>
        /// <param name="enumerable">The input <see cref="RefEnumerable{T}"/> instance.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator ReadOnlyRefEnumerable<T>(RefEnumerable<T> enumerable)
        {
#if SPAN_RUNTIME_SUPPORT
            return new ReadOnlyRefEnumerable<T>(enumerable.Span, enumerable.Step);
#else
            return new ReadOnlyRefEnumerable<T>(enumerable.Instance, enumerable.Offset, enumerable.Length, enumerable.Step);
#endif
        }

        /// <summary>
        /// A custom enumerator type to traverse items within a <see cref="ReadOnlyRefEnumerable{T}"/> instance.
        /// </summary>
        public ref struct Enumerator
        {
            /// <summary>
            /// The <see cref="ReadOnlyRefEnumerable{T}"/> used by this enumerator.
            /// </summary>
            private readonly ReadOnlyRefEnumerable<T> enumerable;

            /// <summary>
            /// The current position in the sequence.
            /// </summary>
            private int position;

#if SPAN_RUNTIME_SUPPORT
            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="span">The <see cref="ReadOnlySpan{T}"/> instance with the info on the items to traverse.</param>
            /// <param name="step">The distance between items in the sequence to enumerate.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(ReadOnlySpan<T> span, int step)
                : this(new ReadOnlyRefEnumerable<T>(span, step))
            {
            }
#else
            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="instance">The target <see cref="object"/> instance.</param>
            /// <param name="offset">The initial offset within <see paramref="instance"/>.</param>
            /// <param name="length">The number of items in the sequence.</param>
            /// <param name="step">The distance between items in the sequence to enumerate.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            internal Enumerator(object? instance, IntPtr offset, int length, int step)
                : this(new ReadOnlyRefEnumerable<T>(instance, offset, length, step))
            {
            }
#endif

            internal Enumerator(ReadOnlyRefEnumerable<T> enumerable)
            {
                this.enumerable = enumerable;
                this.position = -1;
            }

            /// <inheritdoc cref="System.Collections.IEnumerator.MoveNext"/>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
#if SPAN_RUNTIME_SUPPORT
                return ++this.position < this.enumerable.span.Length;
#else
                return ++this.position < this.enumerable.Length;
#endif
            }

            /// <inheritdoc cref="System.Collections.Generic.IEnumerator{T}.Current"/>
            public readonly ref readonly T Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get => ref this.enumerable.DangerousGetReferenceAt(this.position);
            }
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the "length" parameter is invalid.
        /// </summary>
        private static void ThrowArgumentOutOfRangeExceptionForLength()
        {
            throw new ArgumentOutOfRangeException("length");
        }

        /// <summary>
        /// Throws an <see cref="ArgumentOutOfRangeException"/> when the "step" parameter is invalid.
        /// </summary>
        private static void ThrowArgumentOutOfRangeExceptionForStep()
        {
            throw new ArgumentOutOfRangeException("step");
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