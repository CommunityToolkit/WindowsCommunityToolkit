// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{
    /// <summary>
    /// Represents the result of parsing an inline element.
    /// </summary>
    public abstract class InlineParseResult
    {
        private protected InlineParseResult(MarkdownInline parsedElement, int start, int end)
        {
            ParsedElement = parsedElement;
            Start = start;
            End = end;
        }

        /// <summary>
        /// Gets the element that was parsed (can be <c>null</c>).
        /// </summary>
        public MarkdownInline ParsedElement { get; }

        /// <summary>
        /// Gets the position of the first character in the parsed element.
        /// </summary>
        public int Start { get; }

        /// <summary>
        /// Gets the position of the character after the last character in the parsed element.
        /// </summary>
        public int End { get; }

        public static InlineParseResult<T> Create<T>(T parsedElement, int start, int end)
            where T : MarkdownInline
            => new InlineParseResult<T>(parsedElement, start, end);
    }

    /// <summary>
    /// Represents the result of parsing an inline element.
    /// </summary>
    public class InlineParseResult<T> : InlineParseResult
        where T : MarkdownInline
    {

        internal InlineParseResult(T parsedElement, int start, int end) : base(parsedElement, start, end)
        {
        }

        /// <summary>
        /// Gets the element that was parsed (can be <c>null</c>).
        /// </summary>
        public new T ParsedElement => (T)base.ParsedElement;

    }
}