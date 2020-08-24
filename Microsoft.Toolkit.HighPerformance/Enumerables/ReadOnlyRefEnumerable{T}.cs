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
    /// A <see langword="ref"/> <see langword="struct"/> that iterates readonly items from arbitrary memory locations.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public ref struct ReadOnlyRefEnumerable<T>
    {
#if SPAN_RUNTIME_SUPPORT
        /// <summary>
        /// The <see cref="ReadOnlySpan{T}"/> instance pointing to the first item in the target memory area.
        /// </summary>
        /// <remarks>The <see cref="ReadOnlySpan{T}.Length"/> field maps to the total available length.</remarks>
        private readonly ReadOnlySpan<T> span;
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
        /// Initializes a new instance of the <see cref="ReadOnlyRefEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="reference">A reference to the first item of the sequence.</param>
        /// <param name="length">The number of items in the sequence.</param>
        /// <param name="step">The distance between items in the sequence to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        internal ReadOnlyRefEnumerable(in T reference, int length, int step)
        {
            this.span = MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(reference), length * step);
            this.step = step;
            this.position = -step;
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
            this.length = length * step;
            this.step = step;
            this.position = -step;
        }
#endif

        /// <inheritdoc cref="System.Collections.IEnumerable.GetEnumerator"/>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public readonly ReadOnlyRefEnumerable<T> GetEnumerator() => this;

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
        public readonly ref readonly T Current
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

        /// <inheritdoc cref="RefEnumerable{T}.ToArray"/>
        [Pure]
        public readonly unsafe T[] ToArray()
        {
#if SPAN_RUNTIME_SUPPORT
            // Fast path for contiguous items
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
    }
}
