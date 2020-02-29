// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

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
        /// Parses QuoteBlock.
        /// </summary>
        public new class Parser : Parser<QuoteBlock>
        {
            /// <inheritdoc/>
            protected override void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
                base.ConfigureDefaults(configuration);
                configuration.After<CodeBlock.ParserIndented>();
            }

            /// <inheritdoc/>
            protected override BlockParseResult<QuoteBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                if (markdown[startLine].Length == 0 || markdown[startLine][0] != '>')
                {
                    return null;
                }

                bool lastDidNotContainedQuoteCharacter = false;
                bool lastWasEmpty = false;
                var qutedBlock = markdown.SliceText(startLine).RemoveFromLine((line, lineIndex) =>
                {
                    int startOfText;
                    var nonSpace = line.IndexOfNonWhiteSpace();
                    if (nonSpace == -1)
                    {
                        if (!lastWasEmpty)
                        {
                            lastWasEmpty = true;
                            startOfText = 0;
                        }
                        else
                        {
                            return (0, 0, true, true);
                        }
                    }
                    else if (line[nonSpace] != '>')
                    {
                        if (lastWasEmpty || lastDidNotContainedQuoteCharacter)
                        {
                            return (0, 0, true, true);
                        }
                        else
                        {
                            lastDidNotContainedQuoteCharacter = true;
                            startOfText = nonSpace;
                        }
                    }
                    else
                    {
                        lastDidNotContainedQuoteCharacter = false;
                        lastWasEmpty = false;
                        startOfText = nonSpace + 1;
                    }

                    if (startOfText >= line.Length)
                    {
                        return (0, 0, false, false);
                    }

                    // ignore the first space aufter aqute character
                    if (line[startOfText] == ' ')
                    {
                        startOfText++;
                    }

                    if (startOfText >= line.Length)
                    {
                        return (0, 0, false, false);
                    }

                    return (startOfText, line.Length - startOfText, false, false);
                });

                if (lastWasEmpty)
                {
                    qutedBlock = qutedBlock.SliceText(0, qutedBlock.LineCount - 1);
                }

                var result = new QuoteBlock();

                if (qutedBlock.LineCount != 0)
                {
                    // Recursively call into the markdown block parser.
                    result.Blocks = document.ParseBlocks(qutedBlock);
                }
                else
                {
                    result.Blocks = Array.Empty<MarkdownBlock>();
                }

                return BlockParseResult.Create(result, startLine, qutedBlock.LineCount);
            }
        }
    }
}