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
    /// Represents a span containing bold italic text.
    /// </summary>
    public class BoldItalicTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="BoldItalicTextInline"/> class.
        /// </summary>
        public BoldItalicTextInline()
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
            protected override InlineParseResult<BoldTextInline> ParseInternal(LineBlock markdown, int tripLine, int tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                var line = markdown.Lines[tripLine];
                if (tripPos >= line.Length - 1)
                {
                    return null;
                }

                if (tripPos + 3 >= line.Length)
                {
                    return null;
                }

                // Check the start sequence.
                var startSequence = line.Slice(tripPos, 3);
                if (!startSequence.StartsWith("***".AsSpan()) && !startSequence.StartsWith("___".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.  The end sequence (either '***' or '___') must be the same
                // as the start sequence.
                var innerStart = tripPos + 3;
                var subBlock = markdown.SliceLines(tripLine).RemoveFromStart(innerStart);

                var endPosition = subBlock.IndexOf(startSequence, StringComparison.OrdinalIgnoreCase);

                if (endPosition.line == -1)
                {
                    return null;
                }

                var innerBlock = subBlock.SliceLines(0, endPosition.line + 1).RemoveFromEnd(subBlock.Lines[endPosition.line].Length - endPosition.posInLine);

                // The span must contain at least one character.
                if (innerBlock.LineCount > 0 && innerBlock.Lines[0].Length > 0)
                {
                    return null;
                }

                // The first character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(innerBlock.Lines[0][0]))
                {
                    return null;
                }

                // The last character inside the span must NOT be a space.
                if (innerBlock.Lines[innerBlock.LineCount].Length > 0 && ParseHelpers.IsMarkdownWhiteSpace(innerBlock.Lines[innerBlock.LineCount][innerBlock.Lines[innerBlock.LineCount].Length - 1]))
                {
                    return null;
                }

                // We found something!
                var bold = new BoldTextInline
                {
                    Inlines = new List<MarkdownInline>
                    {
                        new ItalicTextInline
                        {
                            Inlines = document.ParseInlineChildren(innerBlock, ignoredParsers),
                        },
                    },
                };
                return InlineParseResult.Create(bold, tripLine, tripPos, innerBlock.TextLength);
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

            return "***" + string.Join(string.Empty, Inlines) + "***";
        }
    }
}
