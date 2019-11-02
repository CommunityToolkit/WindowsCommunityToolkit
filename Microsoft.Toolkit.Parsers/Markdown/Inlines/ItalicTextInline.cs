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
    /// Represents a span containing italic text.
    /// </summary>
    public class ItalicTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ItalicTextInline"/> class.
        /// </summary>
        public ItalicTextInline()
            : base(MarkdownInlineType.Italic)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Attempts to parse a italic text span.
        /// </summary>
        public new class Parser : Parser<ItalicTextInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "*_";

            /// <inheritdoc/>
            protected override InlineParseResult<ItalicTextInline> ParseInternal(string markdown, int minStart, int tripPos, int maxEnd, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                // Check the first char.
                char startChar = markdown[tripPos];
                if (tripPos == maxEnd || (startChar != '*' && startChar != '_'))
                {
                    return null;
                }

                // Find the end of the span.  The end character (either '*' or '_') must be the same as
                // the start character.
                var innerStart = tripPos + 1;
                int innerEnd = Common.IndexOf(markdown, startChar, tripPos + 1, maxEnd);
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
                var result = new ItalicTextInline();
                result.Inlines = document.ParseInlineChildren(markdown, innerStart, innerEnd, ignoredParsers);
                return InlineParseResult.Create(result, tripPos, innerEnd + 1);
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

            return "*" + string.Join(string.Empty, Inlines) + "*";
        }
    }
}