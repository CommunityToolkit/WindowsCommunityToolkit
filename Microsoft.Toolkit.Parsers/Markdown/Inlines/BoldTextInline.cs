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
    /// Represents a span that contains bold text.
    /// </summary>
    public class BoldTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoldTextInline"/> class.
        /// </summary>
        public BoldTextInline()
            : base(MarkdownInlineType.Bold)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Attempts to parse a bold text span.
        /// </summary>
        public new class Parser : Parser<BoldTextInline>
        {
            /// <inheritdoc/>
            protected override InlineParseResult<BoldTextInline> ParseInternal(string markdown, int minStart, int tripPos, int maxEnd, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (tripPos >= maxEnd - 1)
                {
                    return null;
                }

                // Check the start sequence.
                string startSequence = markdown.Substring(tripPos, 2);
                if (startSequence != "**" && startSequence != "__")
                {
                    return null;
                }

                // Find the end of the span.  The end sequence (either '**' or '__') must be the same
                // as the start sequence.
                var innerStart = tripPos + 2;
                int innerEnd = Common.IndexOf(markdown, startSequence, innerStart, maxEnd);
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
                var result = new BoldTextInline();
                result.Inlines = document.ParseInlineChildren(markdown, innerStart, innerEnd, ignoredParsers);
                return InlineParseResult.Create(result, tripPos, innerEnd + 2);
            }

            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "*_";
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

            return "**" + string.Join(string.Empty, Inlines) + "**";
        }
    }
}
