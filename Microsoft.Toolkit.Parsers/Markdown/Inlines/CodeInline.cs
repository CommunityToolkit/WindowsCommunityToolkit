// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span containing code, or other text that is to be displayed using a
    /// fixed-width font.
    /// </summary>
    public class CodeInline : MarkdownInline, IInlineLeaf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeInline"/> class.
        /// </summary>
        public CodeInline()
            : base(MarkdownInlineType.Code)
        {
        }

        /// <summary>
        /// Gets or sets the text to display as code.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Attempts to parse a code text span.
        /// </summary>
        public new class Parser : Parser<CodeInline>
        {
            /// <inheritdoc/>
            protected override InlineParseResult<CodeInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line];

                // Check the first char.
                if (line[tripPos.Column] != '`')
                {
                    return null;
                }

                var subBlock = markdown.SliceText(tripPos).SliceText(1);
                if (subBlock.LineCount == 0 || subBlock[0].Length == 0)
                {
                    return null;
                }

                int length;
                int innerLength;

                // There is an alternate syntax that starts and ends with two backticks.
                // e.g. ``sdf`sdf`` would be "sdf`sdf".
                if (line[tripPos.Column + 1] == '`')
                {
                    // Alternate double back-tick syntax.
                    subBlock = subBlock.SliceText(1);

                    // Find the end of the span.
                    var innerEnd = subBlock.IndexOf("``".AsSpan());
                    if (innerEnd == LineBlockPosition.NotFound)
                    {
                        return null;
                    }

                    // +2 for the first two ticks and +2 for the last two.
                    length = innerEnd.FromStart + 2 + 2;
                    innerLength = innerEnd.FromStart;
                }
                else
                {
                    // Standard single backtick syntax.

                    // Find the end of the span.
                    var innerEnd = subBlock.IndexOf("`".AsSpan());
                    if (innerEnd == LineBlockPosition.NotFound)
                    {
                        return null;
                    }

                    // +1 for the first tick and +1 for the last.
                    length = innerEnd.FromStart + 1 + 1;
                    innerLength = innerEnd.FromStart;
                }

                // The span must contain at least one character.
                if (length == 0)
                {
                    return null;
                }

                // We found something!
                var code = subBlock.SliceText(0, innerLength);

                if (code[0][0] == ' ')
                {
                    code = code.SliceText(1);
                }

                if (code[code.LineCount - 1][code[code.LineCount - 1].Length - 1] == ' ')
                {
                    code = code.SliceText(0, code.TextLength - 1);
                }

                var result = new CodeInline
                {
                    Text = code.ToStringBuilder().Replace("\r\n", " ").Replace('\n', ' ').Replace('\r', ' ').ToString(),
                };
                return InlineParseResult.Create(result, tripPos, length);
            }

            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "`";
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Text == null)
            {
                return base.ToString();
            }

            return "`" + Text + "`";
        }
    }
}