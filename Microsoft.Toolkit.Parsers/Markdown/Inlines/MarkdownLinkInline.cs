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
        public class LinkParser : Parser<MarkdownLinkInline>
        {
            /// <summary>
            /// Gets or sets a value indicating whether a mail address is found in the link, will be appended with mailto protocall.
            /// </summary>
            public bool HandleEmails { get; set; }

            /// <inheritdoc/>
            public override ReadOnlySpan<char> TripChar => "[".AsSpan();

            /// <inheritdoc/>
            protected override InlineParseResult<MarkdownLinkInline> ParseInternal(in LineBlock markdown, in LineBlockPosition tripPos, MarkdownDocument document, HashSet<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line].Slice(tripPos.Column);

                // Expect a '[' character.
                if (line[0] != '[')
                {
                    return null;
                }

                // Find the ']' character, keeping in mind that [test [0-9]](http://www.test.com) is allowed.
                var firstPartLength = line.FindClosingBrace('[', ']') + 1;

                if (firstPartLength <= 0)
                {
                    return null;
                }

                var firstPart = line.Slice(0, firstPartLength);
                var text = line.Slice(1, firstPartLength - 2);
                line = line.Slice(firstPart.Length);

                if (line.Length == 0 || line[0] != '(')
                {
                    return null;
                }

                var seccondPartLength = line.FindClosingBrace('(', ')') + 1;

                if (seccondPartLength <= 0)
                {
                    return null;
                }

                var innerPart = line.Slice(1, seccondPartLength - 2).Trim();

                // If there is no text whatsoever, then this is not a valid link.
                if (innerPart.Length == 0)
                {
                    return null;
                }

                // Check if there is tooltip text.
                string url;
                string tooltip = null;
                bool lastUrlCharIsDoubleQuote = innerPart[innerPart.Length - 1] == '"';
                int tooltipStart = innerPart.IndexOf('\"');
                if (tooltipStart == 0)
                {
                    return null;
                }

                ReadOnlySpan<char> urlSpan;

                // we will at least found the last quote if `lastUrlCharIsDoubeQuote` is true
                if (lastUrlCharIsDoubleQuote && tooltipStart != innerPart.Length - 1)
                {
                    urlSpan = innerPart.Slice(0, tooltipStart);
                }
                else
                {
                    urlSpan = innerPart;
                }

                urlSpan = urlSpan.Trim();

                if (urlSpan.Length > 0 && urlSpan[0] == '<')
                {
                    var findClosingAngle = urlSpan.FindClosingBrace();
                    if (findClosingAngle == -1)
                    {
                        return null;
                    }

                    urlSpan = urlSpan.Slice(1, findClosingAngle - 1);
                }
                else if (urlSpan.IndexOfNexWhiteSpace() != -1)
                {
                    // url may not have whitespace
                    return null;
                }

                // Extract the URL (resolving any escape sequences).
                url = document.ResolveEscapeSequences(urlSpan, true, true).ToString();

                // we will at least found the last quote if `lastUrlCharIsDoubeQuote` is true
                if (lastUrlCharIsDoubleQuote && tooltipStart != innerPart.Length - 1)
                {
                    tooltip = innerPart.Slice(tooltipStart + 1, innerPart.Length - tooltipStart - 2).ToString();
                }

                if (this.HandleEmails && url.IsEmail())
                {
                    url = string.Format("mailto:{0}", url);
                }

                // We found a regular stand-alone link.
                var result = new MarkdownLinkInline
                {
                    Inlines = document.ParseInlineChildren(text, false, false, new HashSet<Type>(ignoredParsers.Concat(IgnoredSubParsers))),
                    Url = url,
                    Tooltip = tooltip,
                };
                return InlineParseResult.Create(result, tripPos, seccondPartLength + firstPartLength);
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
        /// Attempts to parse a markdown referebce link e.g. "[][reference]".
        /// </summary>
        public class ReferenceParser : Parser<MarkdownLinkInline>
        {
            /// <inheritdoc/>
            public override ReadOnlySpan<char> TripChar => "[".AsSpan();

            /// <inheritdoc/>
            protected override InlineParseResult<MarkdownLinkInline> ParseInternal(in LineBlock markdown, in LineBlockPosition tripPos, MarkdownDocument document, HashSet<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown[tripPos.Line].Slice(tripPos.Column);

                // Expect a '[' character.
                if (line[0] != '[')
                {
                    return null;
                }

                // Find the ']' character, keeping in mind that [test [0-9]](http://www.test.com) is allowed.
                var firstPartLength = line.FindClosingBrace('[', ']') + 1;

                if (firstPartLength <= 0)
                {
                    return null;
                }

                var firstPart = line.Slice(0, firstPartLength);
                var text = line.Slice(1, firstPartLength - 2);
                line = line.Slice(firstPart.Length);

                if (line.Length == 0 || line[0] != '[')
                {
                    return null;
                }

                // Find the ']' character.
                int referenceClose = line.IndexOf(']');
                if (referenceClose == -1)
                {
                    return null;
                }

                var referenceLength = referenceClose + 1;

                string reference;
                if (referenceLength == 2)
                {
                    reference = text.ToString();
                }
                else
                {
                    reference = line.Slice(1, referenceLength - 2).ToString();
                }

                // We found a reference-style link.
                var result = new MarkdownLinkInline
                {
                    Inlines = document.ParseInlineChildren(text, false, false, new HashSet<Type>(ignoredParsers.Concat(IgnoredSubParsers))),
                    ReferenceId = reference,
                };

                return InlineParseResult.Create(result, tripPos, referenceLength + firstPartLength);
            }

            /// <summary>
            /// This will ignore additional parsers when Parsing the text inside `[` and `]`.
            /// </summary>
            /// <typeparam name="T">The Type of Inline parser that should not used.</typeparam>
            public void IgnoreParserForBrackedText<T>()
                where T : Inlines.MarkdownInline.Parser, new()
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