using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Runtime.CompilerServices;

namespace Microsoft.Toolkit.HighPerformance.Extensions
{
    /// <summary>
    /// Helpers for working with the <see cref="ReadOnlySpan{T}"/> type.
    /// </summary>
    public static partial class ReadOnlySpanExtensions
    {
        /// <summary>
        /// Tokenizes the values in the input <see cref="ReadOnlySpan{T}"/> instance using a specified separator.
        /// This extension should be used directly within a <see langword="foreach"/> loop:
        /// <code>
        /// ReadOnlySpan&lt;char&gt; text = "Hello, world!";
        ///
        /// foreach (var token in text.Tokenize(','))
        /// {
        ///     // Access the tokens here...
        /// }
        /// </code>
        /// The compiler will take care of properly setting up the <see langword="foreach"/> loop with the type returned from this method.
        /// </summary>
        /// <typeparam name="T">The type of items in the <see cref="ReadOnlySpan{T}"/> to tokenize.</typeparam>
        /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to tokenize.</param>
        /// <param name="separator">The separator <typeparamref name="T"/> item to use.</param>
        /// <returns>A wrapper type that will handle the tokenization for <paramref name="span"/>.</returns>
        /// <remarks>The returned <see cref="__Tokenizer{T}"/> value shouldn't be used directly: use this extension in a <see langword="foreach"/> loop.</remarks>
        [Pure]
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static __Tokenizer<T> Tokenize<T>(this ReadOnlySpan<T> span, T separator)
            where T : IEquatable<T>
        {
            return new __Tokenizer<T>(span, separator);
        }

        /// <summary>
        /// A <see langword="ref"/> <see langword="struct"/> that tokenizes a given <see cref="ReadOnlySpan{T}"/> instance.
        /// </summary>
        /// <typeparam name="T">The type of items to enumerate.</typeparam>
        [SuppressMessage("StyleCop.CSharp.NamingRules", "SA1300", Justification = "The type is not meant to be used directly by users")]
        [SuppressMessage("StyleCop.CSharp.OrderingRules", "SA1206", Justification = "The type is a ref struct")]
        public ref struct __Tokenizer<T>
            where T : IEquatable<T>
        {
            /// <summary>
            /// The source <see cref="ReadOnlySpan{T}"/> instance
            /// </summary>
            private readonly ReadOnlySpan<T> span;

            /// <summary>
            /// The separator <typeparamref name="T"/> item to use.
            /// </summary>
            private readonly T separator;

            /// <summary>
            /// Initializes a new instance of the <see cref="__Tokenizer{T}"/> struct.
            /// </summary>
            /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> to tokenize.</param>
            /// <param name="separator">The separator <typeparamref name="T"/> item to use.</param>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public __Tokenizer(ReadOnlySpan<T> span, T separator)
            {
                this.span = span;
                this.separator = separator;
            }

            /// <summary>
            /// Implements the duck-typed <see cref="IEnumerable{T}.GetEnumerator"/> method.
            /// </summary>
            /// <returns>An <see cref="Enumerator"/> instance targeting the current <see cref="ReadOnlySpan{T}"/> value.</returns>
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            public Enumerator GetEnumerator() => new Enumerator(this.span, this.separator);

            /// <summary>
            /// An enumerator for a source <see cref="ReadOnlySpan{T}"/> instance.
            /// </summary>
            public ref struct Enumerator
            {
                /// <summary>
                /// The source <see cref="ReadOnlySpan{T}"/> instance.
                /// </summary>
                private readonly ReadOnlySpan<T> span;

                /// <summary>
                /// The separator item to use.
                /// </summary>
                private readonly T separator;

                /// <summary>
                /// The current initial offset.
                /// </summary>
                private int start;

                /// <summary>
                /// The current final offset.
                /// </summary>
                private int end;

                /// <summary>
                /// Initializes a new instance of the <see cref="Enumerator"/> struct.
                /// </summary>
                /// <param name="span">The source <see cref="ReadOnlySpan{T}"/> instance.</param>
                /// <param name="separator">The separator item to use.</param>
                public Enumerator(ReadOnlySpan<T> span, T separator)
                {
                    this.span = span;
                    this.separator = separator;
                    this.start = 0;
                    this.end = -1;
                }

                /// <summary>
                /// Implements the duck-typed <see cref="System.Collections.IEnumerator.MoveNext"/> method.
                /// </summary>
                /// <returns><see langword="true"/> whether a new element is available, <see langword="false"/> otherwise</returns>
                [MethodImpl(MethodImplOptions.AggressiveInlining)]
                public bool MoveNext()
                {
                    int
                        newEnd = this.end + 1,
                        length = this.span.Length;

                    // Additional check if the separator is not the last character
                    if (newEnd <= length)
                    {
                        this.start = newEnd;

                        int index = this.span.Slice(newEnd).IndexOf(this.separator);

                        // Extract the current subsequence
                        if (index >= 0)
                        {
                            this.end = newEnd + index;

                            return true;
                        }

                        this.end = length;

                        return true;
                    }

                    return false;
                }

                /// <summary>
                /// Gets the duck-typed <see cref="IEnumerator{T}.Current"/> property.
                /// </summary>
                public ReadOnlySpan<T> Current
                {
                    [MethodImpl(MethodImplOptions.AggressiveInlining)]
                    get => this.span.Slice(this.start, this.end - this.start);
                }
            }
        }
    }
}
