using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ReadOnlySpan{T}"/> type.
    /// </summary>
    public static partial class ReadOnlySpanExtensions
    {
        /// <summary>
        /// A <see langword="ref"/> <see langword="struct"/> that enumerates the items in a given <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items to enumerate.</typeparam>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300", Justification = "The type is not meant to be used directly by users")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
        public readonly ref struct __ReadOnlySpanEnumerator<T>
        {
            /// <summary>
            /// The source <see cref="ReadOnlySpan{T}"/> instance
            /// </summary>
            private readonly ReadOnlySpan<T> span;

            /// <summary>
            /// Initializes a new instance of the <see cref="__ReadOnlySpanEnumerator{T}"/> struct.
            /// </summary>
            /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to enumerate.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __ReadOnlySpanEnumerator(ReadOnlySpan<T> span)
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
            public ref struct Enumerator
            {
                /// <summary>
                /// The source <see cref="ReadOnlySpan{T}"/> instance.
                /// </summary>
                private readonly ReadOnlySpan<T> Span;

                /// <summary>
                /// The current index within <see cref="Span"/>.
                /// </summary>
                private int _Index;

                /// <summary>
                /// Initializes a new instance of the <see cref="Enumerator"/> struct.
                /// </summary>
                /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> instance.</param>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public Enumerator(ReadOnlySpan<T> span)
                {
                    Span = span;
                    _Index = -1;
                }

                /// <summary>
                /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
                /// </summary>
                /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext()
                {
                    int index = _Index + 1;

                    if (index < Span.Length)
                    {
                        _Index = index;

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
                        int index = _Index;
                        T value = Unsafe.Add(ref MemoryMarshal.GetReference(Span), index);

                        return (index, value);
                    }
                }
            }
        }
    }
}
