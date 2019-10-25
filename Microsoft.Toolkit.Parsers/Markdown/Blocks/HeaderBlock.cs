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

        public class HashFactory : Factory<HeaderBlock>
        {
            protected override HeaderBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // This type of header starts with one or more '#' characters, followed by the header
                // text, optionally followed by any number of hash characters.
                actualEnd = startOfLine;
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
                if (result.HeaderLevel == 0)
                {
                    return null;
                }

                // Ignore any hashes at the end of the line.
                while (pos < endOfFirstLine && markdown[endOfFirstLine - 1] == '#')
                {
                    endOfFirstLine--;
                }

                // Parse the inline content.
                result.Inlines = Common.ParseInlineChildren(markdown, pos, endOfFirstLine);
                return result;
            }
        }

        public class UnderlineFactory : Factory<HeaderBlock>
        {

            public override IEnumerable<Type> DefaultBeforeFactorys { get; } = new Type[] { typeof(HorizontalRuleBlock) };
            protected override HeaderBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                // This type of header starts with some text on the first line, followed by one or more
                // underline characters ('=' or '-') on the second line.
                // The second line can have whitespace after the underline characters, but not before
                // or between each character.

                var nonSpaceChar = markdown[firstNonSpace];
                actualEnd = startOfLine;
                if ((nonSpaceChar != '-' && nonSpaceChar != '=') || firstNonSpace != startOfLine || paragraphText.Length == 0)
                {
                    return null;
                }

                Common.FindPreviousSingleNewLine(paragraphText.ToString(), paragraphText.Length - 1, 0, out var startOfHeader);

                // Check the second line is valid.
                if (endOfFirstLine <= startOfLine)
                {
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
                result.Inlines = Common.ParseInlineChildren(paragraphText.ToString(), startOfHeader, paragraphText.Length - startOfHeader);

                // We're going to have to remove the header text from the pending
                // paragraph by prematurely ending the current paragraph.
                // We already made sure that there is a paragraph in progress.
                paragraphText.Length -= (paragraphText.Length - startOfHeader);

                return result;
            }
        }
    }
}