// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents a block that is displayed using a quote style.  Quotes are used to indicate
    /// that the text originated elsewhere (e.g. a previous comment).
    /// </summary>
    public class QuoteBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuoteBlock"/> class.
        /// </summary>
        public QuoteBlock()
            : base(MarkdownBlockType.Quote)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the block.
        /// </summary>
        public IList<MarkdownBlock> Blocks { get; set; }

        /// <summary>
        /// Parses QuoteBlock
        /// </summary>
        public new class Parser : Parser<QuoteBlock>
        {
            /// <inheritdoc/>
            protected override void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
                base.ConfigureDefaults(configuration);
                configuration.After<CodeBlock.Parser>();
            }

            /// <inheritdoc/>
            protected override QuoteBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                var nonSpace = firstNonSpace;
                actualEnd = startOfLine;
                if (markdown[nonSpace] != '>')
                {
                    return null;
                }

                var lines = new List<ReadOnlyMemory<char>>();
                var temporaryLines = new List<ReadOnlyMemory<char>>();
                var newLine = startOfLine;
                int endOfLine;
                bool lastWasEmpty = false;
                bool lastDidNotContainedQuoteCharacter = false;
                while (true)
                {
                    endOfLine = Helpers.Common.FindNextSingleNewLine(markdown, newLine, maxEnd, out var nextLine);
                    if (newLine == nextLine)
                    {
                        break;
                    }

                    nonSpace = Helpers.Common.FindNextNoneWhiteSpace(markdown, newLine, endOfLine, false);

                    if (nonSpace == -1)
                    {
                        if (!lastWasEmpty)
                        {
                            lastWasEmpty = true;
                        }
                    }
                    else if (markdown[nonSpace] != '>')
                    {
                        if (lastWasEmpty || lastDidNotContainedQuoteCharacter)
                        {
                            break;
                        }
                        else
                        {
                            lastDidNotContainedQuoteCharacter = true;
                        }
                    }
                    else
                    {
                        lastDidNotContainedQuoteCharacter = false;
                        lastWasEmpty = false;
                        lines.AddRange(temporaryLines);
                        temporaryLines.Clear();
                    }

                    var actualStart = nonSpace == -1 ? newLine : nonSpace;
                    if (markdown[actualStart] == '>')
                    {
                        actualStart += 1;
                    }

                    if (markdown.Length > actualStart && markdown[actualStart] == ' ')
                    {
                        actualStart++;
                    }

                    if (markdown.Length > actualStart)
                    {
                        int length;
                        if (endOfLine < markdown.Length)
                        {
                            length = endOfLine - actualStart + 1;
                        }
                        else
                        {
                            length = endOfLine - actualStart;
                        }

                        var memory = markdown.AsMemory(actualStart, length);
                        if (lastWasEmpty)
                        {
                            temporaryLines.Add(memory);
                        }
                        else
                        {
                            actualEnd = endOfLine;
                            lines.Add(memory);
                        }
                    }

                    newLine = nextLine;
                }

                if (lines.Last().Length == 0)
                {
                    lines.RemoveAt(lines.Count - 1);
                }

                var filteredString = new StringFilter(lines);

                var result = new QuoteBlock();

                // Recursively call into the markdown block parser.
                result.Blocks = document.Parse(filteredString.ToString(), 0, filteredString.Length, out _);

                return result;
            }
        }
    }
}