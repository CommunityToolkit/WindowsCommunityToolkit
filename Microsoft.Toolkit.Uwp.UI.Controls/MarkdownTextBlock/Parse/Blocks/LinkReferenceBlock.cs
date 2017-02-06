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

using Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Helpers;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Markdown.Parse
{
    /// <summary>
    /// Represents the target of a reference ([ref][]).
    /// </summary>
    internal class LinkReferenceBlock : MarkdownBlock
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
        /// Attempts to parse a reference e.g. "[example]: http://www.reddit.com 'title'".
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location to start parsing. </param>
        /// <param name="end"> The location to stop parsing. </param>
        /// <returns> A parsed markdown link, or <c>null</c> if this is not a markdown link. </returns>
        internal static LinkReferenceBlock Parse(string markdown, int start, int end)
        {
            // Expect a '[' character.
            if (start >= end || markdown[start] != '[')
            {
                return null;
            }

            // Find the ']' character
            int pos = start + 1;
            while (pos < end)
            {
                if (markdown[pos] == ']')
                {
                    break;
                }

                pos++;
            }

            if (pos == end)
            {
                return null;
            }

            // Extract the ID.
            string id = markdown.Substring(start + 1, pos - (start + 1));

            // Expect the ':' character.
            pos++;
            if (pos == end || markdown[pos] != ':')
            {
                return null;
            }

            // Skip whitespace
            pos++;
            while (pos < end && Common.IsWhiteSpace(markdown[pos]))
            {
                pos++;
            }

            if (pos == end)
            {
                return null;
            }

            // Extract the URL.
            int urlStart = pos;
            while (pos < end && !Common.IsWhiteSpace(markdown[pos]))
            {
                pos++;
            }

            string url = TextRunInline.ResolveEscapeSequences(markdown, urlStart, pos);

            // Ignore leading '<' and trailing '>'.
            url = url.TrimStart('<').TrimEnd('>');

            // Skip whitespace.
            pos++;
            while (pos < end && Common.IsWhiteSpace(markdown[pos]))
            {
                pos++;
            }

            string tooltip = null;
            if (pos < end)
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
                while (pos < end && markdown[pos] != tooltipEndChar)
                {
                    pos++;
                }

                if (pos == end)
                {
                    return null;    // No end character.
                }

                tooltip = markdown.Substring(tooltipStart, pos - tooltipStart);

                // Check there isn't any trailing text.
                pos++;
                while (pos < end && Common.IsWhiteSpace(markdown[pos]))
                {
                    pos++;
                }

                if (pos < end)
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

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return string.Format("[{0}]: {1} {2}", Id, Url, Tooltip);
        }
    }
}
