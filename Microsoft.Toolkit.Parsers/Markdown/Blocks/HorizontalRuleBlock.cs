// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

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

        /// <summary>
        /// Parse HorizontalRule.
        /// </summary>
        public new class Parser : Parser<HorizontalRuleBlock>
        {
            /// <inheritdoc/>
            protected override BlockParseResult<HorizontalRuleBlock> ParseInternal(in LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                var line = markdown[startLine];

                // A horizontal rule is a line with at least 3 stars, optionally separated by spaces
                // OR a line with at least 3 dashes, optionally separated by spaces
                // OR a line with at least 3 underscores, optionally separated by spaces.
                char hrChar = '\0';
                int hrCharCount = 0;
                int pos = 0;
                var nonSpaceINdex = line.IndexOfNonWhiteSpace();
                if (nonSpaceINdex == -1)
                {
                    return null;
                }

                var nonSpaceChar = line[nonSpaceINdex];
                if (nonSpaceChar != '*' && nonSpaceChar != '-' && nonSpaceChar != '_')
                {
                    return null;
                }

                while (pos < line.Length)
                {
                    char c = line[pos++];
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
                return hrCharCount >= 3 ? BlockParseResult.Create(new HorizontalRuleBlock(), startLine, 1) : null;
            }

            /// <inheritdoc/>
            protected override void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
                base.ConfigureDefaults(configuration);
                configuration.Before<ListBlock.Parser>();
            }
        }
    }
}