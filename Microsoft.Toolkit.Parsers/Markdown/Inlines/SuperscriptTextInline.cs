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
    /// Represents a span containing superscript text.
    /// </summary>
    public class SuperscriptTextInline : MarkdownInline, IInlineContainer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SuperscriptTextInline"/> class.
        /// </summary>
        public SuperscriptTextInline()
            : base(MarkdownInlineType.Superscript)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Returns the chars that if found means we might have a match.
        /// </summary>

        /// <summary>
        /// Attempts to parse a superscript text span.
        /// </summary>
        public class ParserTags : InlineSourundParser<SuperscriptTextInline>
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="ParserTags"/> class.
            /// </summary>
            public ParserTags()
                : base("<sup>", "</sup>")
            {
            }

            /// <inheritdoc/>
            protected override SuperscriptTextInline MakeInline(List<MarkdownInline> inlines) => new SuperscriptTextInline
            {
                Inlines = inlines,
            };
        }

        /// <summary>
        /// Attempts to parse a superscript text span.
        /// </summary>
        public class ParserTop : Parser<SuperscriptTextInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "^";

            /// <inheritdoc/>
            protected override InlineParseResult<SuperscriptTextInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line];

                // Check the first character.
                if (line[tripPos.Column] != '^')
                {
                    return null;
                }


                line = line.Slice(tripPos.Column + 1);

                if (line.Length == 0)
                {
                    return null;
                }

                ReadOnlySpan<char> txt;

                // +1 for the ^
                int additionalCharacter = 1;

                // The content might be enclosed in parentheses.
                if (line[0] == '(')
                {
                    var closing = line.FindClosingBrace('(', ')');
                    if (closing == -1)
                    {
                        return null;
                    }

                    txt = line.Slice(1, closing - 1);

                    // for the parentises
                    additionalCharacter += 2;
                }
                else
                {
                    var end = line.IndexOfNexWhiteSpace();
                    if (end == -1)
                    {
                        end = line.Length;
                    }
                    else if (end == 0)
                    {
                        // No match if the character after the caret is a space.
                        return null;
                    }

                    txt = line.Slice(0, end);
                }

                // We found something!
                var result = new SuperscriptTextInline
                {
                    Inlines = document.ParseInlineChildren(txt, ignoredParsers),
                };
                return InlineParseResult.Create(result, tripPos, txt.Length + additionalCharacter);
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

            return "^(" + string.Join(string.Empty, Inlines) + ")";
        }
    }
}