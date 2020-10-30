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
using Microsoft.Toolkit.HighPerformance.Helpers.Internals;
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
        internal readonly Span<T> Span;
#else
        /// <summary>
        /// The target <see cref="object"/> instance, if present.
        /// </summary>
        internal readonly object? Instance;

        /// <summary>
        /// The initial offset within <see cref="Instance"/>.
        /// </summary>
        internal readonly IntPtr Offset;

        /// <summary>
        /// The total available length for the sequence.
        /// </summary>
        internal readonly int Length;
#endif

        /// <summary>
        /// The distance between items in the sequence to enumerate.
        /// </summary>
        /// <remarks>The distance refers to <typeparamref name="T"/> items, not byte offset.</remarks>
        internal readonly int Step;

        /// <summary>
        /// The current position in the sequence.
        /// </summary>
        internal int Position;

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
            Span = MemoryMarshal.CreateSpan(ref reference, length);
            Step = step;

            this.Position = -1;
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
            Instance = instance;
            Offset = offset;
            Length = length;
            Step = step;

            this.Position = -1;
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
#if SPAN_RUNTIME_SUPPORT
            return ++this.Position < this.Span.Length;
#else
            return ++this.Position < this.Length;
#endif
        }

        /// <inheritdoc cref="System.Collections.Generic.IEnumerator{T}.Current"/>
        public readonly ref T Current
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get
            {
#if SPAN_RUNTIME_SUPPORT
                ref T r0 = ref this.Span.DangerousGetReference();
#else
                ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.Instance, this.Offset);
#endif

                // Here we just offset by shifting down as if we were traversing a 2D array with a
                // a single column, with the width of each row represented by the step, the height
                // represented by the current position, and with only the first element of each row
                // being inspected. We can perform all the indexing operations in this type as nint,
                // as the maximum offset is guaranteed never to exceed the maximum value, since on
                // 32 bit architectures it's not possible to allocate that much memory anyway.
                nint offset = (nint)(uint)this.Position * (nint)(uint)this.Step;
                ref T ri = ref Unsafe.Add(ref r0, offset);

                return ref ri;
            }
        }

        /// <summary>
        /// Clears the contents of the current <see cref="RefEnumerable{T}"/> instance.
        /// </summary>
        public readonly void Clear()
        {
#if SPAN_RUNTIME_SUPPORT
            // Fast path for contiguous items
            if (this.Step == 1)
            {
                this.Span.Clear();

                return;
            }

            ref T r0 = ref this.Span.DangerousGetReference();
            int length = this.Span.Length;
#else
            ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.Instance, this.Offset);
            int length = this.Length;
#endif

            RefEnumerableHelper.Clear(ref r0, (nint)(uint)length, (nint)(uint)this.Step);
        }

        /// <summary>
        /// Copies the contents of this <see cref="RefEnumerable{T}"/> into a destination <see cref="Span{T}"/> instance.
        /// </summary>
        /// <param name="destination">The destination <see cref="Span{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="destination"/> is shorter than the source <see cref="RefEnumerable{T}"/> instance.
        /// </exception>
        public readonly void CopyTo(Span<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.Step == 1)
            {
                this.Span.CopyTo(destination);

                return;
            }

            ref T sourceRef = ref this.Span.DangerousGetReference();
            int length = this.Span.Length;
#else
            ref T sourceRef = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.Instance, this.Offset);
            int length = this.Length;
#endif
            if ((uint)destination.Length < (uint)length)
            {
                ThrowArgumentExceptionForDestinationTooShort();
            }

            ref T destinationRef = ref destination.DangerousGetReference();

            RefEnumerableHelper.CopyTo(ref sourceRef, ref destinationRef, (nint)(uint)length, (nint)(uint)this.Step);
        }

        /// <summary>
        /// Attempts to copy the current <see cref="RefEnumerable{T}"/> instance to a destination <see cref="Span{T}"/>.
        /// </summary>
        /// <param name="destination">The target <see cref="Span{T}"/> of the copy operation.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public readonly bool TryCopyTo(Span<T> destination)
        {
#if SPAN_RUNTIME_SUPPORT
            int length = this.Span.Length;
#else
            int length = this.Length;
#endif

            if (destination.Length >= length)
            {
                CopyTo(destination);

                return true;
            }

            return false;
        }

        /// <summary>
        /// Copies the contents of a source <see cref="ReadOnlySpan{T}"/> into the current <see cref="RefEnumerable{T}"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <exception cref="ArgumentException">
        /// Thrown when the current <see cref="RefEnumerable{T}"/> is shorter than the source <see cref="ReadOnlySpan{T}"/> instance.
        /// </exception>
        internal readonly void CopyFrom(ReadOnlySpan<T> source)
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.Step == 1)
            {
                source.CopyTo(this.Span);

                return;
            }

            ref T destinationRef = ref this.Span.DangerousGetReference();
            int destinationLength = this.Span.Length;
#else
            ref T destinationRef = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.Instance, this.Offset);
            int destinationLength = this.Length;
#endif
            ref T sourceRef = ref source.DangerousGetReference();
            int sourceLength = source.Length;

            if ((uint)destinationLength < (uint)sourceLength)
            {
                ThrowArgumentExceptionForDestinationTooShort();
            }

            RefEnumerableHelper.CopyFrom(ref sourceRef, ref destinationRef, (nint)(uint)sourceLength, (nint)(uint)this.Step);
        }

        /// <summary>
        /// Attempts to copy the source <see cref="ReadOnlySpan{T}"/> into the current <see cref="RefEnumerable{T}"/> instance.
        /// </summary>
        /// <param name="source">The source <see cref="ReadOnlySpan{T}"/> instance.</param>
        /// <returns>Whether or not the operation was successful.</returns>
        public readonly bool TryCopyFrom(ReadOnlySpan<T> source)
        {
#if SPAN_RUNTIME_SUPPORT
            int length = this.Span.Length;
#else
            int length = this.Length;
#endif

            if (length >= source.Length)
            {
                CopyFrom(source);

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
        public readonly void Fill(T value)
        {
#if SPAN_RUNTIME_SUPPORT
            if (this.Step == 1)
            {
                this.Span.Fill(value);

                return;
            }

            ref T r0 = ref this.Span.DangerousGetReference();
            int length = this.Span.Length;
#else
            ref T r0 = ref RuntimeHelpers.GetObjectDataAtOffsetOrPointerReference<T>(this.Instance, this.Offset);
            int length = this.Length;
#endif

            RefEnumerableHelper.Fill(ref r0, (nint)(uint)length, (nint)(uint)this.Step, value);
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
        public readonly T[] ToArray()
        {
#if SPAN_RUNTIME_SUPPORT
            int length = this.Span.Length;
#else
            int length = this.Length;
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
        /// Throws an <see cref="ArgumentException"/> when the target span is too short.
        /// </summary>
        private static void ThrowArgumentExceptionForDestinationTooShort()
        {
            throw new ArgumentException("The target span is too short to copy all the current items to");
        }
    }
}
