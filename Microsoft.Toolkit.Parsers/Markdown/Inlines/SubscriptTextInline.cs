// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span containing subscript text.
    /// </summary>
    public class SubscriptTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SubscriptTextInline"/> class.
        /// </summary>
        public SubscriptTextInline()
            : base(MarkdownInlineType.Subscript)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Attempts to parse a subscript text span.
        /// </summary>
        public new class Parser : Parser<SubscriptTextInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "<";

            /// <inheritdoc/>
            protected override InlineParseResult<SubscriptTextInline> ParseInternal(string markdown, int minStart, int tripPos, int maxEnd, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                // Check the first character.
                // e.g. "<sub>……</sub>"
                if (maxEnd - tripPos < 5)
                {
                    return null;
                }

                if (!markdown.AsSpan(tripPos, 5).StartsWith("<sub>".AsSpan()))
                {
                    return null;
                }

                int innerStart = tripPos + 5;
                int innerEnd = Common.IndexOf(markdown, "</sub>", innerStart, maxEnd);

                // if don't have the end character or no character between start and end
                if (innerEnd == -1 || innerEnd == innerStart)
                {
                    return null;
                }

                // No match if the character after the caret is a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(markdown[innerStart]) || ParseHelpers.IsMarkdownWhiteSpace(markdown[innerEnd - 1]))
                {
                    return null;
                }

                // We found something!
                var result = new SubscriptTextInline();
                result.Inlines = document.ParseInlineChildren(markdown, innerStart, innerEnd, ignoredParsers);
                return InlineParseResult.Create(result, tripPos, innerEnd + 6);
            }
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Inlines == null)
            {
                return base.ToString();
            }

            return "<sub>" + string.Join(string.Empty, Inlines) + "</sub>";
        }
    }
}
