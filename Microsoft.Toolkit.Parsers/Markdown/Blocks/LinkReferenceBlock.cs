// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Inlines;

namespace Microsoft.Toolkit.Parsers.Markdown.Blocks
{
    /// <summary>
    /// Represents the target of a reference ([ref][]).
    /// </summary>
    public class LinkReferenceBlock : MarkdownBlock
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkReferenceBlock"/> class.
        /// </summary>
        public LinkReferenceBlock()
            : base(MarkdownBlockType.LinkReference)
        {
        }

        /// <summary>
        /// Gets or sets the reference ID.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the link URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a tooltip to display on hover.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return string.Format("[{0}]: {1} {2}", Id, Url, Tooltip);
        }

        public new class Parser : Parser<LinkReferenceBlock>
        {
            public override IEnumerable<Type> DefaultAfterParsers { get; } = new Type[] { typeof(CodeBlock.Parser) };

            protected override LinkReferenceBlock ParseInternal(string markdown, int startOfLine, int firstNonSpace, int realStartOfLine, int endOfFirstLine, int maxEnd, int quoteDepth, out int actualEnd, StringBuilder paragraphText, bool lineStartsNewParagraph, MarkdownDocument document)
            {

                actualEnd = startOfLine;

                // Expect a '[' character.
                if (startOfLine >= endOfFirstLine || markdown[startOfLine] != '[')
                {
                    return null;
                }

                // Find the ']' character
                int pos = startOfLine + 1;
                while (pos < endOfFirstLine)
                {
                    if (markdown[pos] == ']')
                    {
                        break;
                    }

                    pos++;
                }

                if (pos == endOfFirstLine)
                {
                    return null;
                }

                // Extract the ID.
                string id = markdown.Substring(startOfLine + 1, pos - (startOfLine + 1));

                // Expect the ':' character.
                pos++;
                if (pos == endOfFirstLine || markdown[pos] != ':')
                {
                    return null;
                }

                // Skip whitespace
                pos++;
                while (pos < endOfFirstLine && ParseHelpers.IsMarkdownWhiteSpace(markdown[pos]))
                {
                    pos++;
                }

                if (pos == endOfFirstLine)
                {
                    return null;
                }

                // Extract the URL.
                int urlStart = pos;
                while (pos < endOfFirstLine && !ParseHelpers.IsMarkdownWhiteSpace(markdown[pos]))
                {
                    pos++;
                }

                string url = TextRunInline.ResolveEscapeSequences(markdown, urlStart, pos);

                // Ignore leading '<' and trailing '>'.
                url = url.TrimStart('<').TrimEnd('>');

                // Skip whitespace.
                pos++;
                while (pos < endOfFirstLine && ParseHelpers.IsMarkdownWhiteSpace(markdown[pos]))
                {
                    pos++;
                }

                string tooltip = null;
                if (pos < endOfFirstLine)
                {
                    // Extract the tooltip.
                    char tooltipEndChar;
                    switch (markdown[pos])
                    {
                        case '(':
                            tooltipEndChar = ')';
                            break;

                        case '"':
                        case '\'':
                            tooltipEndChar = markdown[pos];
                            break;

                        default:
                            return null;
                    }

                    pos++;
                    int tooltipStart = pos;
                    while (pos < endOfFirstLine && markdown[pos] != tooltipEndChar)
                    {
                        pos++;
                    }

                    if (pos == endOfFirstLine)
                    {
                        return null;    // No end character.
                    }

                    tooltip = markdown.Substring(tooltipStart, pos - tooltipStart);

                    // Check there isn't any trailing text.
                    pos++;
                    while (pos < endOfFirstLine && ParseHelpers.IsMarkdownWhiteSpace(markdown[pos]))
                    {
                        pos++;
                    }

                    if (pos < endOfFirstLine)
                    {
                        return null;
                    }
                }

                // We found something!
                var result = new LinkReferenceBlock();
                result.Id = id;
                result.Url = url;
                result.Tooltip = tooltip;
                return result;
            }
        }
    }
}