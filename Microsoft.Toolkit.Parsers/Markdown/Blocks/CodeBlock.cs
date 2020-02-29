// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Text;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents a block of text that is displayed in a fixed-width font.  Inline elements and
    /// escape sequences are ignored inside the code block.
    /// </summary>
    public class CodeBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CodeBlock"/> class.
        /// </summary>
        public CodeBlock()
            : base(MarkdownBlockType.Code)
        {
        }

        /// <summary>
        /// Gets or sets the source code to display.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the Language specified in prefix, e.g. ```c# (Github Style Parsing).<para/>
        /// This does not guarantee that the Code Block has a language, or no language, some valid code might not have been prefixed, and this will still return null. <para/>
        /// To ensure all Code is Highlighted (If desired), you might have to determine the language from the provided string, such as looking for key words.
        /// </summary>
        public string CodeLanguage { get; set; }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return Text;
        }

        /// <summary>
        /// Parse Code Block.
        /// </summary>
        public class ParserIndented : Parser<CodeBlock>
        {
            /// <inheritdoc/>
            protected override BlockParseResult<CodeBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                if (!lineStartsNewParagraph)
                {
                    return null;
                }

                /*
                    This variant is every line starts with a tab character or at least 4 spaces
                */

                var codeBlock = markdown
                    .SliceLines(startLine)
                    .RemoveFromLine((line, index) =>
                    {
                        if (line.IsWhiteSpace())
                        {
                            return (0, line.Length, false, false);
                        }

                        int indentino = 0;
                        int charIndex = 0;
                        while ((line[charIndex] == ' ' || line[charIndex] == '\t') && indentino < 4 && charIndex < line.Length)
                        {
                            if (line[charIndex] == ' ')
                            {
                                indentino += 1;
                            }
                            else
                            {
                                indentino += 4;
                            }

                            charIndex++;
                        }

                        if (indentino >= 4)
                        {
                            return (charIndex, line.Length - charIndex, false, false);
                        }

                        return (0, 0, true, true);
                    })
                    .TrimEnd();

                if (codeBlock.LineCount == 0)
                {
                    // Not a valid code block.
                    return null;
                }

                // Blank lines should be trimmed from the start and end.
                var markdownBlock = new CodeBlock()
                {
                    Text = codeBlock.ToString(),
                    CodeLanguage = null,
                };
                return BlockParseResult.Create(markdownBlock, startLine, codeBlock.LineCount);
            }
        }

        public class ParserTicked : Parser<CodeBlock>
        {
            /// <inheritdoc/>
            protected override BlockParseResult<CodeBlock> ParseInternal(LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                if (!lineStartsNewParagraph)
                {
                    return null;
                }

                /*
                    This one the code block starts and ends with ```
                */

                var firstLine = markdown[startLine];

                if (firstLine.Length < 3
                    || firstLine[0] != '`'
                    || firstLine[1] != '`'
                    || firstLine[2] != '`')
                {
                    return null;
                }

                // Collects the Programming Language from the end of the starting ticks.
                var codeLanguage = firstLine.Slice(3).Trim();

                var code = markdown
                    .SliceLines(startLine + 1)
                    .RemoveFromLine((line, index) =>
                    {
                        if (line.Length >= 3
                            && line[0] == '`'
                            && line[1] == '`'
                            && line[2] == '`')
                        {
                            return (0, 0, true, true);
                        }

                        return (0, line.Length, false, false);
                    });

                // we consume every text in code and did not find a closing token.
                if (startLine + code.LineCount + 1 >= markdown.LineCount)
                {
                    // Not a valid code block.
                    return null;
                }

                // Blank lines should be trimmed from the start and end.
                var markdownBlock = new CodeBlock()
                {
                    Text = code.ToString(),
                    CodeLanguage = codeLanguage.IsWhiteSpace() ? null : codeLanguage.ToString(),
                };
                return BlockParseResult.Create(markdownBlock, startLine, code.LineCount + 2);
            }
        }
    }
}