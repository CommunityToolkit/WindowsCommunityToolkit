// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Xml.Linq;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a span that contains a reference for links to point to.
    /// </summary>
    public class LinkAnchorInline : MarkdownInline
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkAnchorInline"/> class.
        /// </summary>
        public LinkAnchorInline()
            : base(MarkdownInlineType.LinkReference)
        {
        }

        /// <summary>
        /// Gets or sets the Name of this Link Reference.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets the raw Link Reference.
        /// </summary>
        public string Raw { get; set; }

        /// <summary>
        /// Attempts to parse a comment span.
        /// </summary>
        public new class Parser : Parser<LinkAnchorInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "<";

            /// <inheritdoc/>
            protected override InlineParseResult<LinkAnchorInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                var line = markdown.SliceText(tripPos)[0];

                // Check the start sequence.
                if (line.StartsWith("<a".AsSpan()))
                {
                    return null;
                }

                // Find the end of the span.  The end sequence ('-->')
                var innerStart = 2;
                var innerEnd = markdown.IndexOf("</a>".AsSpan()).FromStart;
                int trueEnd = innerEnd + 4;
                if (innerEnd == -1)
                {
                    innerEnd = markdown.IndexOf("/>".AsSpan()).FromStart;
                    trueEnd = innerEnd + 2;
                    if (innerEnd == -1)
                    {
                        return null;
                    }
                }

                // This link Reference wasn't closed properly if the next link starts before a close.
                var nextLink = markdown.SliceText(innerStart).IndexOf("<a".AsSpan()).FromStart;
                if (nextLink > -1 && nextLink < innerEnd)
                {
                    return null;
                }

                var length = trueEnd;
                var contents = line.Slice(0, length).ToString();

                string link = null;

                try
                {
                    var xml = XElement.Parse(contents);
                    var attr = xml.Attribute("name");
                    if (attr != null)
                    {
                        link = attr.Value;
                    }
                }
                catch
                {
                    // Attempting to fetch link failed, ignore and continue.
                }

                // Remove whitespace if it exists.
                if (trueEnd + 1 <= markdown.TextLength && line[trueEnd] == ' ')
                {
                    trueEnd += 1;
                }

                // We found something!
                var result = new LinkAnchorInline
                {
                    Raw = contents,
                    Link = link,
                };
                return InlineParseResult.Create(result, tripPos, trueEnd - tripPos.FromStart);
            }
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return Raw;
        }
    }
}