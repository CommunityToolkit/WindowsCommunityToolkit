// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;
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

        /// <summary>
        /// Parse LinkReferenceBlock.
        /// </summary>
        public new class Parser : Parser<LinkReferenceBlock>
        {
            /// <inheritdoc/>
            protected override void ConfigureDefaults(DefaultParserConfiguration configuration)
            {
                base.ConfigureDefaults(configuration);
                configuration.After<CodeBlock.ParserIndented>();
            }

            /// <inheritdoc/>
            protected override BlockParseResult<LinkReferenceBlock> ParseInternal(in LineBlock markdown, int startLine, bool lineStartsNewParagraph, MarkdownDocument document)
            {
                var line = markdown[startLine];

                // Expect a '[' character.
                if (line.Length == 0 || line[0] != '[')
                {
                    return null;
                }

                // Find the ']' character
                var closingIndex = line.IndexOf(']');

                if (closingIndex == -1)
                {
                    return null;
                }

                // Extract the ID.
                var id = line.Slice(1, closingIndex - 1);

                // Expect the ':' character.
                var collonIndex = closingIndex + 1;
                if (collonIndex + 1 >= line.Length || line[collonIndex] != ':')
                {
                    return null;
                }

                // Skip whitespace
                var urlStart = line.Slice(collonIndex + 1).IndexOfNonWhiteSpace() + collonIndex + 1;
                if (urlStart == -1)
                {
                    return null;
                }

                // Extract the URL.
                var urlLength = line.Slice(urlStart).IndexOfNexWhiteSpace();
                if (urlLength == -1)
                {
                    urlLength = line.Slice(urlStart).Length;
                }

                var url = document.ResolveEscapeSequences(line.Slice(urlStart, urlLength), true, true);

                // Ignore leading '<' and trailing '>'.
                url = url.TrimStart('<').TrimEnd('>');

                // Skip whitespace.
                var pos = line.Slice(urlStart + urlLength).IndexOfNonWhiteSpace() + urlStart + urlLength;

                string tooltip = null;
                if (pos != -1)
                {
                    // Extract the tooltip.
                    char tooltipEndChar;
                    switch (line[pos])
                    {
                        case '(':
                            tooltipEndChar = ')';
                            break;

                        case '"':
                        case '\'':
                            tooltipEndChar = line[pos];
                            break;

                        default:
                            return null;
                    }

                    pos++;
                    int tooltipStart = pos;
                    var tooltipLength = line.Slice(tooltipStart).IndexOf(tooltipEndChar);

                    if (tooltipLength == -1)
                    {
                        return null;
                    }

                    tooltip = line.Slice(tooltipStart, pos - tooltipStart).ToString();

                    // Check there isn't any trailing text.
                    if (line.Length > pos && line.Slice(pos).IndexOfNonWhiteSpace() != -1)
                    {
                        return null;
                    }
                }

                // We found something!
                var result = new LinkReferenceBlock();
                result.Id = id.ToString();
                result.Url = url.ToString();
                result.Tooltip = tooltip;
                return BlockParseResult.Create(result, startLine, 1);
            }
        }
    }
}