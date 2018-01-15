// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Helpers;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse
{
    /// <summary>
    /// Represents a type of hyperlink where the text can be different from the target URL.
    /// </summary>
    internal class MarkdownLinkInline : MarkdownInline, IInlineContainer, ILinkElement
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
        /// Returns the chars that if found means we might have a match.
        /// </summary>
        internal static void AddTripChars(List<Common.InlineTripCharHelper> tripCharHelpers)
        {
            tripCharHelpers.Add(new Common.InlineTripCharHelper() { FirstChar = '[', Method = Common.InlineParseMethod.MarkdownLink });
        }

        /// <summary>
        /// Attempts to parse a markdown link e.g. "[](http://www.reddit.com)".
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location to start parsing. </param>
        /// <param name="maxEnd"> The location to stop parsing. </param>
        /// <returns> A parsed markdown link, or <c>null</c> if this is not a markdown link. </returns>
        internal static Common.InlineParseResult Parse(string markdown, int start, int maxEnd)
        {
            // Expect a '[' character.
            if (start == maxEnd || markdown[start] != '[')
            {
                return null;
            }

            // Find the ']' character, keeping in mind that [test [0-9]](http://www.test.com) is allowed.
            int linkTextOpen = start + 1;
            int pos = linkTextOpen;
            int linkTextClose;
            int openSquareBracketCount = 0;
            while (true)
            {
                linkTextClose = markdown.IndexOfAny(new char[] { '[', ']' }, pos, maxEnd - pos);
                if (linkTextClose == -1)
                {
                    return null;
                }

                if (markdown[linkTextClose] == '[')
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
            while (pos < maxEnd && Common.IsWhiteSpace(markdown[pos]))
            {
                pos++;
            }

            if (pos == maxEnd)
            {
                return null;
            }

            // Expect the '(' character or the '[' character.
            int linkOpen = pos;
            if (markdown[pos] == '(')
            {
                // Skip whitespace.
                linkOpen++;
                while (linkOpen < maxEnd && Common.IsWhiteSpace(markdown[linkOpen]))
                {
                    linkOpen++;
                }

                // Find the ')' character.
                pos = linkOpen;
                int linkClose = -1;
                var openParenthesis = 0;
                while (pos < maxEnd)
                {
                    if (markdown[pos] == ')')
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

                    if (markdown[pos] == '(')
                    {
                        openParenthesis++;
                    }

                    pos++;
                }

                if (pos >= maxEnd)
                {
                    return null;
                }

                int end = linkClose + 1;

                // Skip whitespace backwards.
                while (linkClose > linkOpen && Common.IsWhiteSpace(markdown[linkClose - 1]))
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
                bool lastUrlCharIsDoubleQuote = markdown[linkClose - 1] == '"';
                int tooltipStart = Common.IndexOf(markdown, " \"", linkOpen, linkClose - 1);
                if (tooltipStart == linkOpen)
                {
                    return null;
                }

                if (lastUrlCharIsDoubleQuote && tooltipStart != -1)
                {
                    // Extract the URL (resolving any escape sequences).
                    url = TextRunInline.ResolveEscapeSequences(markdown, linkOpen, tooltipStart).TrimEnd(' ', '\t', '\r', '\n');
                    tooltip = markdown.Substring(tooltipStart + 2, (linkClose - 1) - (tooltipStart + 2));
                }
                else
                {
                    // Extract the URL (resolving any escape sequences).
                    url = TextRunInline.ResolveEscapeSequences(markdown, linkOpen, linkClose);
                }

                // Check the URL is okay.
                if (!IsUrlValid(url))
                {
                    return null;
                }

                // We found a regular stand-alone link.
                var result = new MarkdownLinkInline();
                result.Inlines = Common.ParseInlineChildren(markdown, linkTextOpen, linkTextClose, ignoreLinks: true);
                result.Url = url.Replace(" ", "%20");
                result.Tooltip = tooltip;
                return new Common.InlineParseResult(result, start, end);
            }
            else if (markdown[pos] == '[')
            {
                // Find the ']' character.
                int linkClose = Common.IndexOf(markdown, ']', pos + 1, maxEnd);
                if (linkClose == -1)
                {
                    return null;
                }

                // We found a reference-style link.
                var result = new MarkdownLinkInline();
                result.Inlines = Common.ParseInlineChildren(markdown, linkTextOpen, linkTextClose, ignoreLinks: true);
                result.ReferenceId = markdown.Substring(linkOpen + 1, linkClose - (linkOpen + 1));
                if (result.ReferenceId == string.Empty)
                {
                    result.ReferenceId = markdown.Substring(linkTextOpen, linkTextClose - linkTextOpen);
                }

                return new Common.InlineParseResult(result, start, linkClose + 1);
            }

            return null;
        }

        /// <summary>
        /// If this is a reference-style link, attempts to converts it to a regular link.
        /// </summary>
        /// <param name="document"> The document containing the list of references. </param>
        public void ResolveReference(MarkdownDocument document)
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

            // The reference was found. Check the URL is valid.
            if (!IsUrlValid(reference.Url))
            {
                return;
            }

            // Everything is cool when you're part of a team.
            Url = reference.Url;
            Tooltip = reference.Tooltip;
            ReferenceId = null;
        }

        /// <summary>
        /// Checks if the given URL is allowed in a markdown link.
        /// </summary>
        /// <param name="url"> The URL to check. </param>
        /// <returns> <c>true</c> if the URL is valid; <c>false</c> otherwise. </returns>
        private static bool IsUrlValid(string url)
        {
            // URLs can be relative.
            if (url.StartsWith("/") || url.StartsWith("#"))
            {
                return true;
            }

            // Check the scheme is allowed.
            bool schemeIsAllowed = false;
            foreach (var scheme in HyperlinkInline.KnownSchemes)
            {
                if (url.StartsWith(scheme))
                {
                    schemeIsAllowed = true;
                    break;
                }
            }

            return schemeIsAllowed;
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
