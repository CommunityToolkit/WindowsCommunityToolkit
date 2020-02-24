// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents a heading.
    /// </summary>
    public class HeaderBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HeaderBlock"/> class.
        /// </summary>
        public HeaderBlock()
            : base(MarkdownBlockType.Header)
        {
        }

        private int _headerLevel;

        /// <summary>
        /// Gets or sets the header level (1-6).  1 is the most important header, 6 is the least important.
        /// </summary>
        public int HeaderLevel
        {
            get
            {
                return _headerLevel;
            }

            set
            {
                if (value < 1 || value > 6)
                {
                    throw new ArgumentOutOfRangeException("HeaderLevel", "The header level must be between 1 and 6 (inclusive).");
                }

                _headerLevel = value;
            }
        }

        /// <summary>
        /// Gets or sets the contents of the block.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

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

            return string.Join(string.Empty, Inlines);
        }

        /// <summary>
        /// Parses Header with Hash.
        /// </summary>
        public class HashParser : Parser<HeaderBlock>
        {
            /// <inheritdoc/>
            protected override BlockParseResult<HeaderBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // This type of header starts with one or more '#' characters, followed by the header
                // text, optionally followed by any number of hash characters.
                if (markdown[firstNonSpace] != '#' || firstNonSpace != startOfLine)
                {
                    return null;
                }

                var result = new HeaderBlock();

                // Figure out how many consecutive hash characters there are.
                int pos = startOfLine;
                while (pos < endOfFirstLine && markdown[pos] == '#' && pos - startOfLine < 6)
                {
                    pos++;
                }

                result.HeaderLevel = pos - startOfLine;

                // space between hash an start of header text is ignored.
                while (pos < endOfFirstLine && markdown[pos] == ' ')
                {
                    pos++;
                }

                if (result.HeaderLevel == 0)
                {
                    return null;
                }

                var endOfHeader = endOfFirstLine;

                // Ignore any hashes at the end of the line.
                while (pos < endOfHeader && markdown[endOfHeader - 1] == '#')
                {
                    endOfHeader--;
                }

                // Ignore any space at the end of the line.
                while (pos < endOfHeader && markdown[endOfHeader - 1] == ' ')
                {
                    endOfHeader--;
                }

                // Parse the inline content.
                result.Inlines = document.ParseInlineChildren(markdown, pos, endOfHeader, Array.Empty<Type>());

                return BlockParseResult.Create(result, startOfLine, endOfFirstLine);
            }
        }

        /// <summary>
        /// Parse Underline.
        /// </summary>
        public class UnderlineParser : Parser<HeaderBlock>
        {
            /// <inheritdoc/>
            protected override void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
                base.ConfigureDefaults(configuration);
                configuration.Before<HorizontalRuleBlock.Parser>();
            }

            /// <inheritdoc/>
            protected override BlockParseResult<HeaderBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // Check the second line is valid.
                if (endOfFirstLine <= startOfLine)
                {
                    return null;
                }

                // This type of header starts with some text on the first line, followed by one or more
                // underline characters ('=' or '-') on the second line.
                // The second line can have whitespace after the underline characters, but not before
                // or between each character.
                var nonSpaceChar = markdown[firstNonSpace];

                if ((nonSpaceChar != '-' && nonSpaceChar != '=') || firstNonSpace != startOfLine || maxStart >= startOfLine)
                {
                    return null;
                }

                Common.FindPreviousSingleNewLine(markdown, startOfLine - 1, maxStart, out var startOfHeader);
                var endOfHeader = startOfLine - startOfHeader;
                bool headerHasOnlyWhiteSpace = true;
                for (int i = startOfHeader; i < endOfHeader; i++)
                {
                    if (!char.IsWhiteSpace(markdown[i]))
                    {
                        headerHasOnlyWhiteSpace = false;
                        break;
                    }
                }

                if (headerHasOnlyWhiteSpace)
                {
                    // header must contain at least one non whitespace character
                    return null;
                }

                // Figure out what the underline character is ('=' or '-').
                char underlineChar = markdown[startOfLine];
                if (underlineChar != '=' && underlineChar != '-')
                {
                    return null;
                }

                // Read past consecutive underline characters.
                int pos = startOfLine + 1;
                for (; pos < endOfFirstLine; pos++)
                {
                    char c = markdown[pos];
                    if (c != underlineChar)
                    {
                        break;
                    }

                    pos++;
                }

                // The rest of the line must be whitespace.
                for (; pos < endOfFirstLine; pos++)
                {
                    char c = markdown[pos];
                    if (c != ' ' && c != '\t')
                    {
                        return null;
                    }

                    pos++;
                }

                var result = new HeaderBlock();
                result.HeaderLevel = underlineChar == '=' ? 1 : 2;

                // Parse the inline content.
                result.Inlines = document.ParseInlineChildren(markdown, startOfHeader, endOfHeader, Array.Empty<Type>());

                // We're going to have to remove the header text from the pending
                // paragraph by prematurely ending the current paragraph.
                // We already made sure that there is a paragraph in progress.
                return BlockParseResult.Create(result, startOfHeader, endOfFirstLine);
            }
        }
    }
}