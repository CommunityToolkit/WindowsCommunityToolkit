// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents a type of hyperlink where the text can be different from the target URL.
    /// </summary>
    public class MarkdownLinkInline : MarkdownInline, IInlineContainer, ILinkElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MarkdownLinkInline"/> class.
        /// </summary>
        public MarkdownLinkInline()
            : base(MarkdownInlineType.MarkdownLink)
        {
        }

        /// <summary>
        /// Gets or sets the contents of the inline.
        /// </summary>
        public IList<MarkdownInline> Inlines { get; set; }

        /// <summary>
        /// Gets or sets the link URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a tooltip to display on hover.
        /// </summary>
        public string Tooltip { get; set; }

        /// <summary>
        /// Gets or sets the ID of a reference, if this is a reference-style link.
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Attempts to parse a markdown link e.g. "[](http://www.reddit.com)".
        /// </summary>
        public new class Parser : Parser<MarkdownLinkInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "[";

            /// <inheritdoc/>
            protected override InlineParseResult<MarkdownLinkInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line];


                // Expect a '[' character.
                if (line[tripPos.Column] != '[')
                {
                    return null;
                }

                // Find the ']' character, keeping in mind that [test [0-9]](http://www.test.com) is allowed.
                int linkTextOpen = tripPos.Column + 1;
                int pos = linkTextOpen;
                int linkTextClose;
                int openSquareBracketCount = 0;
                while (true)
                {
                    linkTextClose = line.Slice(pos).IndexOfAny(new char[] { '[', ']' }) + pos;
                    if (linkTextClose == -1)
                    {
                        return null;
                    }

                    if (line[linkTextClose] == '[')
                    {
                        openSquareBracketCount++;
                    }
                    else if (openSquareBracketCount > 0)
                    {
                        openSquareBracketCount--;
                    }
                    else
                    {
                        break;
                    }

                    pos = linkTextClose + 1;
                }

                // Skip whitespace.
                pos = linkTextClose + 1;
                while (pos < line.Length && ParseHelpers.IsMarkdownWhiteSpace(line[pos]))
                {
                    pos++;
                }

                if (pos == line.Length)
                {
                    return null;
                }

                // Expect the '(' character or the '[' character.
                int linkOpen = pos;
                if (line[pos] == '(')
                {
                    // Skip whitespace.
                    linkOpen++;
                    while (linkOpen < line.Length && ParseHelpers.IsMarkdownWhiteSpace(line[linkOpen]))
                    {
                        linkOpen++;
                    }

                    // Find the ')' character.
                    pos = linkOpen;
                    int linkClose = -1;
                    var openParenthesis = 0;
                    while (pos < line.Length)
                    {
                        if (line[pos] == ')')
                        {
                            if (openParenthesis == 0)
                            {
                                linkClose = pos;
                                break;
                            }
                            else
                            {
                                openParenthesis--;
                            }
                        }

                        if (line[pos] == '(')
                        {
                            openParenthesis++;
                        }

                        pos++;
                    }

                    if (pos >= line.Length)
                    {
                        return null;
                    }

                    int end = linkClose + 1;

                    // Skip whitespace backwards.
                    while (linkClose > linkOpen && ParseHelpers.IsMarkdownWhiteSpace(line[linkClose - 1]))
                    {
                        linkClose--;
                    }

                    // If there is no text whatsoever, then this is not a valid link.
                    if (linkOpen == linkClose)
                    {
                        return null;
                    }

                    // Check if there is tooltip text.
                    string url;
                    string tooltip = null;
                    bool lastUrlCharIsDoubleQuote = line[linkClose - 1] == '"';
                    int tooltipStart = line.Slice(linkOpen, linkClose - 1).IndexOf(" \"".AsSpan()) + linkOpen;
                    if (tooltipStart == linkOpen || line[linkOpen] == '\"')
                    {
                        return null;
                    }

                    if (lastUrlCharIsDoubleQuote && tooltipStart != -1)
                    {
                        // Extract the URL (resolving any escape sequences).
                        url = TextRunInline.ResolveEscapeSequences(line.Slice(linkOpen, tooltipStart)).TrimEnd(' ', '\t', '\r', '\n');
                        tooltip = line.Slice(tooltipStart + 2, (linkClose - 1) - (tooltipStart + 2)).ToString();
                    }
                    else
                    {
                        // Extract the URL (resolving any escape sequences).
                        url = TextRunInline.ResolveEscapeSequences(line.Slice(linkOpen, linkClose));
                    }

                    // Check the URL is okay.
                    if (!Common.IsUrlValid(ref url))
                    {
                        if (!url.IsEmail())
                        {
                            return null;
                        }
                        else
                        {
                            tooltip = url = string.Format("mailto:{0}", url);
                        }
                    }

                    // We found a regular stand-alone link.
                    var result = new MarkdownLinkInline
                    {
                        Inlines = document.ParseInlineChildren(new LineBlock(line.Slice(linkTextOpen, linkTextClose)), ignoredParsers.Concat(IgnoredSubParsers)),
                        Url = url,
                        Tooltip = tooltip
                    };
                    return InlineParseResult.Create(result, tripPos, end);
                }
                else if (line[pos] == '[')
                {
                    // Find the ']' character.
                    int linkClose = line.Slice(pos + 1, line.Length).IndexOf("]".AsSpan()) + pos + 1;
                    if (linkClose == -1)
                    {
                        return null;
                    }

                    // We found a reference-style link.
                    var result = new MarkdownLinkInline
                    {
                        Inlines = document.ParseInlineChildren(new LineBlock(line.Slice(linkTextOpen, linkTextClose)), ignoredParsers.Concat(IgnoredSubParsers)),
                        ReferenceId = linkClose - (linkOpen + 1) > 0
                            ? line.Slice(linkOpen + 1, linkClose - (linkOpen + 1)).ToString()
                            : line.Slice(linkTextOpen, linkTextClose - linkTextOpen).ToString(),
                    };

                    return InlineParseResult.Create(result, tripPos, linkClose + 1);
                }

                return null;
            }

            /// <summary>
            /// This will ignore additional parsers when Parsing the text inside `[` and `]`.
            /// </summary>
            /// <typeparam name="T">The Type of Inline parser that should not used.</typeparam>
            public void IgnoreParserForBrackedText<T>()
                where T : Inlines.MarkdownInline.Parser
            {
                IgnoredSubParsers.Add(typeof(T));
            }

            private static readonly HashSet<Type> IgnoredSubParsers = new HashSet<Type>() { typeof(MarkdownLinkInline.Parser), typeof(HyperlinkInline.AngleBracketLinkParser), typeof(HyperlinkInline.EmailAddressParser), typeof(HyperlinkInline.PartialLinkParser), typeof(HyperlinkInline.ReditLinkParser), typeof(HyperlinkInline.UrlParser) };
        }

        /// <summary>
        /// If this is a reference-style link, attempts to converts it to a regular link.
        /// </summary>
        /// <param name="document"> The document containing the list of references. </param>
        internal void ResolveReference(MarkdownDocument document)
        {
            if (document == null)
            {
                throw new ArgumentNullException("document");
            }

            if (ReferenceId == null)
            {
                return;
            }

            // Look up the reference ID.
            var reference = document.LookUpReference(ReferenceId);
            if (reference == null)
            {
                return;
            }

            var url = reference.Url;

            // The reference was found. Check the URL is valid.
            if (!Common.IsUrlValid(ref url))
            {
                return;
            }

            // Everything is cool when you're part of a team.
            Url = url;
            Tooltip = reference.Tooltip;
            ReferenceId = null;
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (Inlines == null || Url == null)
            {
                return base.ToString();
            }

            if (ReferenceId != null)
            {
                return string.Format("[{0}][{1}]", string.Join(string.Empty, Inlines), ReferenceId);
            }

            return string.Format("[{0}]({1})", string.Join(string.Empty, Inlines), Url);
        }
    }
}