// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Enumerables
{
    /// <summary>
    /// A <see langword="ref"/> <see langword="struct"/> that enumerates the items in a given <see cref="ReadOnlySpan{T}"/> instance.
    /// </summary>
    /// <typeparam name="T">The type of items to enumerate.</typeparam>
    [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
    [EditorBrowsable(EditorBrowsableState.Never)]
    public readonly ref struct ReadOnlySpanEnumerable<T>
    {
        /// <summary>
        /// The source <see cref="ReadOnlySpan{T}"/> instance
        /// </summary>
        private readonly ReadOnlySpan<T> span;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReadOnlySpanEnumerable{T}"/> struct.
        /// </summary>
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to enumerate.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public ReadOnlySpanEnumerable(ReadOnlySpan<T> span)
        {
            this.span = span;
        }

        /// <summary>
        /// Implements the duck-typed <see cref="IEnumerable{T}.GetEnumerator"/> method.
        /// </summary>
        /// <returns>An <see cref="Enumerator"/> instance targeting the current <see cref="ReadOnlySpan{T}"/> value.</returns>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Enumerator GetEnumerator() => new Enumerator(this.span);

        /// <summary>
        /// An enumerator for a source <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        [EditorBrowsable(EditorBrowsableState.Never)]
        public ref struct Enumerator
        {
            /// <summary>
            /// The source <see cref="ReadOnlySpan{T}"/> instance.
            /// </summary>
            private readonly ReadOnlySpan<T> span;

            /// <summary>
            /// The current index within <see cref="span"/>.
            /// </summary>
            private int index;

            /// <summary>
            /// Initializes a new instance of the <see cref="Enumerator"/> struct.
            /// </summary>
            /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> instance.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator(ReadOnlySpan<T> span)
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
            public (int Index, T Value) Current
            {
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                get
                {
                    int currentIndex = this.index;
                    T value = Unsafe.Add(ref MemoryMarshal.GetReference(this.span), currentIndex);

                    return (currentIndex, value);
                }
            }
        }
    }
}
