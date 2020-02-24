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
            protected override InlineParseResult<CodeInline> ParseInternal(LineBlock markdown, int tripLine, int tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                // Check the first char.
                if (tripPos == maxEnd || markdown[tripPos] != '`')
                {
                    return null;
                }

                // There is an alternate syntax that starts and ends with two backticks.
                // e.g. ``sdf`sdf`` would be "sdf`sdf".
                int innerStart = tripPos + 1;
                int innerEnd, end;
                if (innerStart < maxEnd && markdown[innerStart] == '`')
                {
                    // Alternate double back-tick syntax.
                    innerStart++;

                    // Find the end of the span.
                    innerEnd = Common.IndexOf(markdown, "``", innerStart, maxEnd);
                    if (innerEnd == -1)
                    {
                        return null;
                    }

                    end = innerEnd + 2;
                }
                else
                {
                    // Standard single backtick syntax.

                    // Find the end of the span.
                    innerEnd = Common.IndexOf(markdown, '`', innerStart, maxEnd);
                    if (innerEnd == -1)
                    {
                        return null;
                    }

                    end = innerEnd + 1;
                }

                // The span must contain at least one character.
                if (innerStart == innerEnd)
                {
                    return null;
                }

                // We found something!
                var result = new CodeInline();
                result.Text = markdown.AsSpan(innerStart, innerEnd - innerStart).Trim(" \t\r\n".AsSpan()).ToString();
                return InlineParseResult.Create(result, tripPos, end);
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