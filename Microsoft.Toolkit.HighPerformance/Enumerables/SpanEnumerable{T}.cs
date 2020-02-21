// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that enumerates the items in a given <see cref="Span{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300", Justification = "The type is not meant to be used directly by users")]
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    public readonly ref struct SpanEnumerable<T>
    {
        /// <summary>
        /// The source <see cref="Span{T}"/> instance
        /// </summary>
        private readonly Span<T> span;

        /// <summary>
        /// Initializes a new instance of the <see cref="SpanEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="span">The source <see cref="Span{T}"/> to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public SpanEnumerable(Span<T> span)
        {
            this.span = span;
        }

        /// <summary>
        /// Implements the duck-typed <see cref="IEnumerable{T}.GetEnumerator"/> method.
        /// </summary>
        /// <returns>An <see cref="Enumerator"/> instance targeting the current <see cref="Span{T}"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this.span);

        /// <summary>
        /// An enumerator for a source <see cref="Span{T}"/> instance.
        /// </summary>
        public ref struct Enumerator
        {
            /// <summary>
            /// The source <see cref="Span{T}"/> instance.
            /// </summary>
            private readonly Span<T> span;

            /// <summary>
            /// The current index within <see cref="span"/>.
            /// </summary>
            private int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="span">The source <see cref="Span{T}"/> instance.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(Span<T> span)
            {
                this.span = span;
                this.index = -1;
            }

            /// <summary>
            /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
            /// </summary>
            /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public bool MoveNext()
            {
                int newIndex = this.index + 1;

                if (newIndex < this.span.Length)
                {
                    this.index = newIndex;

                    return true;
                }

                return false;
            }

            /// <summary>
            /// Gets the duck-typed <see cref="IEnumerator{T}.Current"/> property.
            /// </summary>
            [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008", Justification = "ValueTuple<T1,T2> return type")]
            public Item Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
#if NETSTANDARD2_1
                    ref T r0 = ref MemoryMarshal.GetReference(span);
                    ref T ri = ref Unsafe.Add(ref r0, index);

                    /* On .NET Standard 2.1 we can save 4 bytes by piggybacking
                     * the current index in the length of the wrapped span.
                     * We're going to use the first item as the target reference,
                     * and the length as a host for the current original offset.
                     * This is not possible on .NET Standard 2.1 as we lack
                     * the API to create spans from arbitrary references. */
                    return new Item(MemoryMarshal.CreateSpan(ref ri, index));
#else
                    return new Item(span, index);
#endif
                }
            }
        }

        /// <summary>
        /// An item from a source <see cref="Span{T}"/> instance.
        /// </summary>
        public readonly ref struct Item
        {
            /// <summary>
            /// The source <see cref="Span{T}"/> instance.
            /// </summary>
            private readonly Span<T> span;

#if NETSTANDARD2_1
            /// <summary>
            /// Initializes a new instance of the <see cref="Item"/> struct.
            /// </summary>
            /// <param name="span">The source <see cref="Span{T}"/> instance.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Item(Span<T> span)
            {
                this.span = span;
            }
#else
            /// <summary>
            /// The current index within <see cref="span"/>.
            /// </summary>
            private readonly int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="span">The source <see cref="Span{T}"/> instance.</param>
            /// <param name="index">The current index within <paramref name="span"/>.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Item(Span<T> span, int index)
            {
                this.span = span;
                this.index = index;
            }
#endif
            /// <summary>
            /// Gets the reference to the current value.
            /// </summary>
            public ref T Value
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
#if NETSTANDARD2_1
                    return ref MemoryMarshal.GetReference(span);
#else
                    ref T r0 = ref MemoryMarshal.GetReference(span);
                    ref T ri = ref Unsafe.Add(ref r0, index);

                    return ref ri;
#endif
                }
            }

            /// <summary>
            /// Gets the current index.
            /// </summary>
            public int Index
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
#if NETSTANDARD2_1
                    return span.Length;
#else
                    return index;
#endif
                }
            }
        }
    }
}
