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
            protected override InlineParseResult<ItalicTextInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line];

                // Check the start sequence.
                var startSequence = line.Slice(tripPos.Column, 2);
                if (!startSequence.StartsWith("*".AsSpan()) && !startSequence.StartsWith("_".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.  The end sequence (either '**' or '__') must be the same
                // as the start sequence.
                var subBlock = markdown.Slice(tripPos).Slice(1);
                var endPosition = subBlock.IndexOf(startSequence, StringComparison.OrdinalIgnoreCase);
                if (endPosition == LineBlockPosition.NotFound)
                {
                    return null;
                }

                var innerBlock = subBlock.Slice(endPosition);

                // The span must contain at least one character.
                if (innerBlock.TextLength == 0)
                {
                    return null;
                }

                // The first character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(innerBlock[0][0]))
                {
                    return null;
                }

                // The last character inside the span must NOT be a space.
                if (ParseHelpers.IsMarkdownWhiteSpace(innerBlock[innerBlock.LineCount - 1][innerBlock[innerBlock.LineCount - 1].Length - 1]))
                {
                    return null;
                }

                // We found something!
                var result = new ItalicTextInline();
                result.Inlines = document.ParseInlineChildren(innerBlock, ignoredParsers);
                return InlineParseResult.Create(result, tripPos, endPosition.FromStart + 1);
            }
        }



        /// <summary>
        /// Attempts to parse a bold text span.
        /// </summary>
        public class ParserAsterix : InlineSourundParser<ItalicTextInline>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParserAsterix"/> class.
            /// </summary>
            public ParserAsterix()
                : base("*")
            {
            }

            /// <inheritdoc/>
            protected override ItalicTextInline MakeInline(List<MarkdownInline> inlines) => new ItalicTextInline
            {
                Inlines = inlines,
            };
        }

        /// <summary>
        /// Attempts to parse a bold text span.
        /// </summary>
        public class ParserUnderscore : InlineSourundParser<ItalicTextInline>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParserUnderscore"/> class.
            /// </summary>
            public ParserUnderscore()
                : base("_")
            {
            }

            /// <inheritdoc/>
            protected override ItalicTextInline MakeInline(List<MarkdownInline> inlines) => new ItalicTextInline
            {
                Inlines = inlines,
            };
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