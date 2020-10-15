// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
#if SPAN_RUNTIME_SUPPORT
using System.Runtime.InteropServices;
#endif
using Microsoft.Toolkit.HighPerformance.Extensions;
#if !SPAN_RUNTIME_SUPPORT
using RuntimeHelpers = Microsoft.Toolkit.HighPerformance.Helpers.Internals.RuntimeHelpers;
#endif

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that iterates items from arbitrary memory locations.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct RefEnumerable<T>
    {
#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// The <see cref="Span{T}"/> instance pointing to the first item in the target memory area.
        /// </summary>
        /// <remarks>The <see cref="Span{T}.Length"/> field maps to the total available length.</remarks>
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
        /// The total available length for the sequence.
        /// </summary>
        private readonly int length;
#endif

        /// <summary>
        /// The distance between items in the sequence to enumerate.
        /// </summary>
        /// <remarks>The distance refers to <typeparamref name="T"/> items, not byte offset.</remarks>
        private readonly int step;

        /// <summary>
        /// The current position in the sequence.
        /// </summary>
        private int position;

#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// Initializes a new instance of the <see cref="RefEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="reference">A reference to the first item of the sequence.</param>
        /// <param name="length">The number of items in the sequence.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RefEnumerable(ref T reference, int length, int step)
        {
            this.span = MemoryMarshal.CreateSpan(ref reference, length * step);
            this.step = step;
            this.position = -step;
        }
#else
        /// <summary>
        /// Initializes a new instance of the <see cref="RefEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="instance">The target <see cref="object"/> instance.</param>
        /// <param name="offset">The initial offset within <see paramref="instance"/>.</param>
        /// <param name="length">The number of items in the sequence.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal RefEnumerable(object? instance, IntPtr offset, int length, int step)
        {
            this.instance = instance;
            this.offset = offset;
            this.length = length * step;
            this.step = step;
            this.position = -step;
        }
#endif

        /// <inheritdoc cref="System.Collections.IEnumerable.GetEnumerator"/>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly RefEnumerable<T> GetEnumerator() => this;

        /// <inheritdoc cref="System.Collections.IEnumerator.MoveNext"/>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool MoveNext()
        {
            int position = this.position + this.step;

            if (
#if SPAN_RUNTIME_SUPPORT
                position < this.span.Length
#else
                position < this.length
#endif
            )
            {
                this.position = position;

                return true;
            }

            return false;
        }

        /// <inheritdoc cref="System.Collections.Generic.IEnumerator{T}.Current"/>
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if SPAN_RUNTIME_SUPPORT
                return ref this.span.DangerousGetReferenceAt(this.position);
#else
                unsafe
                {
                    ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
                    ref T ri = ref Unsafe.Add(ref r0, (IntPtr)(void*)(uint)this.position);

                    return ref ri;
                }
#endif
            }
        }

        /// <summary>
        /// Clears the contents of the current <see cref="RefEnumerable{T}"/> instance.
        /// </summary>
        public readonly unsafe void Clear()
        {
#if SPAN_RUNTIME_SUPPORT
            // Fast path for contiguous items
            if (this.step == 1)
            {
                this.span.Clear();

                return;
            }

            ref T r0 = ref this.span.DangerousGetReference();
            int length = this.span.Length;
#else
            ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
            int length = this.length;
#endif

            for (int i = 0; i < length; i += this.step)
            {
                Unsafe.Add(ref r0, (IntPtr)(void*)(uint)i) = default!;
            }
        }

        /// <summary>
        /// Copies the contents of this <see cref="RefEnumerable{T}"/> into a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination" /> is shorter than the source <see cref="RefEnumerable{T}"/> instance.
        /// </exception>
        public readonly unsafe void CopyTo(Span<T> destination)
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
            int length = this.length;
#endif
            if ((uint)destination.Length < (uint)(length / this.step))
            {
                ThrowArgumentExceptionForDestinationTooShort();
            }

            ref T destinationRef = ref destination.DangerousGetReference();

            for (int i = 0; i < length; i += this.step)
            {
                Unsafe.Add(ref destinationRef, (IntPtr)(void*)(uint)i) = Unsafe.Add(ref sourceRef, (IntPtr)(void*)(uint)i);
            }
        }

        /// <summary>
        /// Attempts to copy the current <see cref="RefEnumerable{T}"/> instance to a destination <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Span{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public readonly bool TryCopyTo(Span<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            int length = this.span.Length;
#else
            int length = this.length;
#endif

            if (destination.Length >= length / this.step)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Fills the elements of this <see cref="RefEnumerable{T}"/> with a specified value.
        /// </summary>
        /// <param name="value">The value to assign to each element of the <see cref="RefEnumerable{T}"/> instance.</param>
        /// <remarks>
        /// This method will always return the whole sequence from the start, ignoring the
        /// current position in case the sequence has already been enumerated in part.
        /// </remarks>
        public readonly unsafe void Fill(T value)
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.step == 1)
            {
                this.span.Fill(value);

                return;
            }

            ref T r0 = ref this.span.DangerousGetReference();
            int length = this.span.Length;
#else
            ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
            int length = this.length;
#endif

            for (int i = 0; i < length; i += this.step)
            {
                Unsafe.Add(ref r0, (IntPtr)(void*)(uint)i) = value;
            }
        }

        /// <summary>
        /// Returns a <typeparamref name="T"/> array with the values in the target row.
        /// </summary>
        /// <returns>A <typeparamref name="T"/> array with the values in the target row.</returns>
        /// <remarks>
        /// This method will allocate a new <typeparamref name="T"/> array, so only
        /// use it if you really need to copy the target items in a new memory location.
        /// Additionally, this method will always return the whole sequence from the start,
        /// ignoring the current position in case the sequence has already been enumerated in part.
        /// </remarks>
        [Pure]
        public readonly unsafe T[] ToArray()
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.step == 1)
            {
                return this.span.ToArray();
            }

            int length = this.span.Length;
#else
            int length = this.length;
#endif

            // Empty array if no data is mapped
            if (length == 0)
            {
                return Array.Empty<T>();
            }

#if SPAN_RUNTIME_SUPPORT
            ref T sourceRef = ref this.span.DangerousGetReference();
#else
            ref T sourceRef = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.instance, this.offset);
#endif

            T[] array = new T[length / this.step];
            ref T destinationRef = ref array.DangerousGetReference();

            for (int i = 0, j = 0; i < length; i += this.step, j++)
            {
                Unsafe.Add(ref destinationRef, (IntPtr)(void*)(uint)j) = Unsafe.Add(ref sourceRef, (IntPtr)(void*)(uint)i);
            }

            return array;
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
