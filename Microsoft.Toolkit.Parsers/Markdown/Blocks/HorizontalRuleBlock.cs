// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using Microsoft.Toolkit.Parsers.Core;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents a horizontal line.
    /// </summary>
    public class HorizontalRuleBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HorizontalRuleBlock"/> class.
        /// </summary>
        public HorizontalRuleBlock()
            : base(MarkdownBlockType.HorizontalRule)
        {
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return "---";
        }

        public new class Factory : Factory<HorizontalRuleBlock>
        {
            protected override HorizontalRuleBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // A horizontal rule is a line with at least 3 stars, optionally separated by spaces
                // OR a line with at least 3 dashes, optionally separated by spaces
                // OR a line with at least 3 underscores, optionally separated by spaces.
                char hrChar = '\0';
                int hrCharCount = 0;
                int pos = startOfLine;
                actualEnd = startOfLine;
                var nonSpaceChar = markdown[firstNonSpace];
                if (nonSpaceChar != '*' && nonSpaceChar != '-' && nonSpaceChar != '_')
                {
                    actualEnd = startOfLine;
                    return null;
                }
                while (pos < endOfFirstLine)
                {
                    char c = markdown[pos++];
                    if (c == '*' || c == '-' || c == '_')
                    {
                        // All of the non-whitespace characters on the line must match.
                        if (hrCharCount > 0 && c != hrChar)
                        {
                            return null;
                        }

                        hrChar = c;
                        hrCharCount++;
                    }
                    else if (c == '\n')
                    {
                        break;
                    }
                    else if (!ParseHelpers.IsMarkdownWhiteSpace(c))
                    {
                        return null;
                    }
                }

                // Hopefully there were at least 3 stars/dashes/underscores.
                return hrCharCount >= 3 ? new HorizontalRuleBlock() : null;
            }
        }
    }
}