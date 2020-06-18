// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Memory
{
    /// <inheritdoc cref="Span2D{T}"/>
    public readonly ref partial struct Span2D<T>
    {
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
