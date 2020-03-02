// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

[assembly: InternalsVisibleTo("UnitTests, PublicKey=002400000480000094000000060200000024000052534131000400000100010041753af735ae6140c9508567666c51c6ab929806adb0d210694b30ab142a060237bc741f9682e7d8d4310364b4bba4ee89cc9d3d5ce7e5583587e8ea44dca09977996582875e71fb54fa7b170798d853d5d8010b07219633bdb761d01ac924da44576d6180cdceae537973982bb461c541541d58417a3794e34f45e6f2d129e2")]

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents a list, with each list item proceeded by either a number or a bullet.
    /// </summary>
    public class ListBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ListBlock"/> class.
        /// </summary>
        public ListBlock()
            : base(MarkdownBlockType.List)
        {
        }

        /// <summary>
        /// Gets or sets the list items.
        /// </summary>
        public IList<ListItemBlock> Items { get; set; }

        /// <summary>
        /// Gets or sets the style of the list, either numbered or bulleted.
        /// </summary>
        public ListStyle Style { get; set; }

        /// <summary>
        /// Parsing helper method.
        /// </summary>
        /// <returns>Returns a ListItemPreamble.</returns>
        private static ListItemPreamble ParseItemPreamble(string markdown, int start, int maxEnd)
        {
            // There are two types of lists.
            // A numbered list starts with a number, then a period ('.'), then a space.
            // A bulleted list starts with a star ('*'), dash ('-') or plus ('+'), then a period, then a space.
            ListStyle style;
            if (markdown[start] == '*' || markdown[start] == '-' || markdown[start] == '+')
            {
                style = ListStyle.Bulleted;
                start++;
            }
            else if (markdown[start] >= '0' && markdown[start] <= '9')
            {
                style = ListStyle.Numbered;
                start++;

                // Skip any other digits.
                while (start < maxEnd)
                {
                    char c = markdown[start];
                    if (c < '0' || c > '9')
                    {
                        break;
                    }

                    start++;
                }

                // Next should be a period ('.').
                if (start == maxEnd || markdown[start] != '.')
                {
                    return null;
                }

                start++;
            }
            else
            {
                return null;
            }

            // Next should be a space.
            if (start == maxEnd || (markdown[start] != ' ' && markdown[start] != '\t'))
            {
                return null;
            }

            start++;

            // This is a valid list item.
            return new ListItemPreamble { Style = style, ContentStartPos = start };
        }

        /// <summary>
        /// Parsing helper method.
        /// </summary>
        /// <param name="listItem">The listItem to edit.</param>
        /// <param name="markdown">The markdown text.</param>
        /// <param name="start">The start position.</param>
        /// <param name="end">The end position.</param>
        /// <param name="newLine">True if the text is a new line. Prefixed whitespace is ignored then.</param>
        private static void AppendTextToListItem(ListItemBlock listItem, string markdown, int start, int end, bool newLine = false)
        {
            ListItemBuilder listItemBuilder = null;
            if (listItem.Blocks.Count > 0)
            {
                listItemBuilder = listItem.Blocks[listItem.Blocks.Count - 1] as ListItemBuilder;
            }

            if (listItemBuilder == null)
            {
                // Add a new block.
                listItemBuilder = new ListItemBuilder();
                listItem.Blocks.Add(listItemBuilder);
            }

            var builder = listItemBuilder.Builder;
            if (builder.Length >= 2 &&
                ParseHelpers.IsMarkdownWhiteSpace(builder[builder.Length - 2]) &&
                ParseHelpers.IsMarkdownWhiteSpace(builder[builder.Length - 1]))
            {
                builder.Length -= 2;
                builder.AppendLine();
            }
            else if (builder.Length > 0)
            {
                builder.Append(' ');
            }

            if (newLine)
            {
                start = Common.FindNextNoneWhiteSpace(markdown, start, end, true);
            }

            builder.Append(markdown, start, end - start);
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Items == null)
            {
                return base.ToString();
            }

            var result = new StringBuilder();
            for (int i = 0; i < Items.Count; i++)
            {
                if (result.Length > 0)
                {
                    result.AppendLine();
                }

                switch (Style)
                {
                    case ListStyle.Bulleted:
                        result.Append("* ");
                        break;

                    case ListStyle.Numbered:
                        result.Append(i + 1);
                        result.Append(".");
                        break;
                }

                result.Append(" ");
                result.Append(string.Join("\r\n", Items[i].Blocks));
            }

            return result.ToString();
        }

        /// <summary>
        /// Parse ListBlock.
        /// </summary>
        public new class Parser : Parser<ListBlock>
        {
            private (ListItemBlock item, int consumedLines, ListStyle style, int startNumber)? ParseItemBlock(LineBlock markdown, MarkdownDocument document)
            {
                if (markdown.LineCount == 0)
                {
                    return null;
                }

                var firstLine = markdown[0];

                var preInformation = GetListStyle(firstLine);
                if (preInformation is null)
                {
                    return null;
                }

                var (style, preLength, startNumber) = preInformation.Value;

                var indention = preLength;
                bool isFirstLine = true;
                bool isLazy = true;
                var listBlock = markdown.RemoveFromLine((line, lineIndex) =>
                {
                    if (isFirstLine)
                    {
                        isFirstLine = false;
                        return (indention, line.Length - indention, false, false);
                    }

                    if (line.IsWhiteSpace())
                    {
                        isLazy = false;
                        return (0, 0, false, false);
                    }

                    var firstNonWhitespace = line.IndexOfNonWhiteSpace();

                    var listInfo = GetListStyle(line.Slice(firstNonWhitespace));
                    if (listInfo.HasValue)
                    {

                        // a sub list must be indented more then 2 char more then the current indention
                        if (firstNonWhitespace >= indention)
                        {
                            // another list item will also disable lazy
                            isLazy = false;
                            return (indention, line.Length - indention, false, false);
                        }

                        // every other list will stop the current block
                        return (0, 0, true, true);
                    }

                    if (firstNonWhitespace < indention)
                    {
                        if (isLazy)
                        {
                            return (0, line.Length, false, false);
                        }

                        return (0, 0, true, true);
                    }

                    // if we are indented we will no longer support lazy
                    isLazy = false;
                    return (indention, line.Length - indention, false, false);
                });

                // remove empty lines at the end
                listBlock = listBlock.TrimEnd();

                var item = new ListItemBlock()
                {
                    Blocks = document.ParseBlocks(listBlock),
                };

                return (item, listBlock.LineCount, style, startNumber);
            }

            /// <inheritdoc/>
            protected override BlockParseResult<ListBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                var startBlock = markdown.SliceLines(startLine);
                var subBlock = startBlock;

                var itemList = new List<ListItemBlock>();

                ListStyle? listStyle = null;

                int startNumber = 0;

                int lastSkipedBlankLines = 0;
                while (true)
                {
                    var itemBlock = ParseItemBlock(subBlock, document);

                    if (itemBlock is null)
                    {
                        break;
                    }

                    if (!listStyle.HasValue)
                    {
                        listStyle = itemBlock.Value.style;
                        startNumber = itemBlock.Value.startNumber;
                    }

                    itemList.Add(itemBlock.Value.item);
                    subBlock = subBlock.SliceLines(itemBlock.Value.consumedLines);

                    lastSkipedBlankLines = 0;
                    for (int i = 0; i < subBlock.LineCount; i++)
                    {
                        if (!subBlock[i].IsWhiteSpace())
                        {
                            lastSkipedBlankLines = i;
                            subBlock = subBlock.SliceLines(i);
                            break;
                        }
                    }
                }

                if (itemList.Count == 0)
                {
                    return null;
                }

                var result = new ListBlock()
                {
                    Style = listStyle.Value,
                    Items = itemList,
                };

                return BlockParseResult.Create(result, startLine, startBlock.LineCount - subBlock.LineCount - lastSkipedBlankLines/*We skiped some white space after the block*/);
            }

            private static (ListStyle style, int length, int number)? GetListStyle(ReadOnlySpan<char> tocheck)
            {
                var nonWhiteSpace = tocheck.IndexOfNonWhiteSpace();

                // we may not be indented more then 3 spaces.
                if (nonWhiteSpace == -1 || nonWhiteSpace >= 4)
                {
                    return null;
                }

                var currentChar = tocheck[nonWhiteSpace];
                if (currentChar == '*' || currentChar == '+' || currentChar == '-')
                {
                    var nextCHar = tocheck.Slice(1 + nonWhiteSpace).IndexOfNonWhiteSpace() + 1;
                    if (nextCHar == 0 || (nextCHar > 1 && nextCHar <= 3))
                    {
                        return (ListStyle.Bulleted, nextCHar + nonWhiteSpace, default);
                    }

                    return null;
                }
                else if (char.IsDigit(currentChar))
                {
                    var numberOfDigits = 1;
                    for (int i = nonWhiteSpace + 1; i < tocheck.Length; i++)
                    {
                        if (!char.IsDigit(tocheck[i]))
                        {
                            break;
                        }

                        numberOfDigits++;
                    }

                    // May not have more then 9 digits according to CommonMark :/
                    if (numberOfDigits >= 10)
                    {
                        return null;
                    }

                    var afterNumber = tocheck.Slice(numberOfDigits + nonWhiteSpace);

                    // after the number there must be a ) or . e.g. `1)` / `1.`
                    if (afterNumber.Length == 0 || (afterNumber[0] != '.' && afterNumber[0] != ')'))
                    {
                        return null;
                    }

                    afterNumber = afterNumber.Slice(1);

                    var nextCHar = afterNumber.IndexOfNonWhiteSpace();
                    if (nextCHar == -1 || (nextCHar >= 1 && nextCHar <= 3))
                    {
                        return (ListStyle.Numbered, nextCHar + 1 + numberOfDigits + nonWhiteSpace, int.Parse(tocheck.Slice(nonWhiteSpace, numberOfDigits).ToString()));
                    }

                    return null;
                }
                else
                {
                    return null;
                }
            }
        }
    }
}