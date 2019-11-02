// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Helpers
{

    /// <summary>
    /// Represents the result of parsing an inline element.
    /// </summary>
    /// <typeparam name="T">The type of the parsed inline.</typeparam>
    public class InlineParseResult<T> : InlineParseResult
        where T : MarkdownInline
    {
        internal InlineParseResult(T parsedElement, int start, int end)
            : base(parsedElement, start, end)
        {
        }

        /// <summary>
        /// Gets the element that was parsed (can be <c>null</c>).
        /// </summary>
        public new T ParsedElement => (T)base.ParsedElement;
    }
}