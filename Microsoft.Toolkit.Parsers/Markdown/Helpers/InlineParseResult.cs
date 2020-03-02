// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{
    /// <summary>
    /// Represents the result of parsing an inline element.
    /// </summary>
    public abstract class InlineParseResult
    {
        private protected InlineParseResult(MarkdownInline parsedElement, LineBlockPosition start, int length)
        {
            this.ParsedElement = parsedElement ?? throw new ArgumentNullException(nameof(parsedElement));
            this.Start = start;
            this.Length = length;
        }

        /// <summary>
        /// Gets the element that was parsed (can be <c>null</c>).
        /// </summary>
        public MarkdownInline ParsedElement { get; }

        /// <summary>
        /// Gets the start position.
        /// </summary>
        public LineBlockPosition Start { get; }

        /// <summary>
        /// Gets the number of Characters that were consumed.
        /// </summary>
        public int Length { get; }

        /// <summary>
        /// Instanciates an InlineParserResult.
        /// </summary>
        /// <typeparam name="T">The MarkdownInline type.</typeparam>
        /// <param name="markdownInline">The parsed inline.</param>
        /// <param name="start">The start of the inline.</param>
        /// <param name="length">The end of the inline.</param>
        /// <returns>The InlineParseResult.</returns>
        public static InlineParseResult<T> Create<T>(T markdownInline, LineBlockPosition start, int length)
            where T : MarkdownInline
            => new InlineParseResult<T>(markdownInline, start, length);
    }
}