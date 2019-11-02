// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span containing strikethrough text.
    /// </summary>
    public class StrikethroughTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StrikethroughTextInline"/> class.
        /// </summary>
        public StrikethroughTextInline()
            : base(MarkdownInlineType.Strikethrough)
        {
        }

        /// <summary>
        /// Gets or sets The contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Attempts to parse a strikethrough text span.
        /// </summary>
        public new class Parser : Parser<StrikethroughTextInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "~";

            /// <inheritdoc/>
            protected override InlineParseResult<StrikethroughTextInline> ParseInternal(string markdown, int minStart, int tripPos, int maxEnd, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                // Check the start sequence.
                if (tripPos >= maxEnd - 1 || markdown.Substring(tripPos, 2) != "~~")
                {
                    return null;
                }

                // Find the end of the span.
                var innerStart = tripPos + 2;
                int innerEnd = Common.IndexOf(markdown, "~~", innerStart, maxEnd);
                if (innerEnd == -1)
                {
                    return null;
                }

                // The span must contain at least one character.
                if (innerStart == innerEnd)
                {
                    return null;
                }

                // The first character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(markdown[innerStart]))
                {
                    return null;
                }

                // The last character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(markdown[innerEnd - 1]))
                {
                    return null;
                }

                // We found something!
                var result = new StrikethroughTextInline();
                result.Inlines = document.ParseInlineChildren(markdown, innerStart, innerEnd, ignoredParsers);
                return InlineParseResult.Create(result, tripPos, innerEnd + 2);
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

            return "~~" + string.Join(string.Empty, Inlines) + "~~";
        }
    }
}