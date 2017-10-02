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
    /// Represents an embedded image.
    /// </summary>
    internal class ImageInline : MarkdownInline, IInlineLeaf
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ImageInline"/> class.
        /// </summary>
        public ImageInline()
            : base(MarkdownInlineType.Image)
        {
        }

        /// <summary>
        /// Gets or sets the image URL.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets a text to display on hover.
        /// </summary>
        public string Tooltip { get; set; }

        public string Text { get; set; } = string.Empty;

        internal static void AddTripChars(List<Common.InlineTripCharHelper> tripCharHelpers)
        {
            tripCharHelpers.Add(new Common.InlineTripCharHelper() { FirstChar = '!', Method = Common.InlineParseMethod.Image });
        }

        /// <summary>
        /// Attempts to parse an image e.g. "![Toolkit logo](https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/master/Microsoft.Toolkit.Uwp.SampleApp/Assets/ToolkitLogo.png)".
        /// </summary>
        /// <param name="markdown"> The markdown text. </param>
        /// <param name="start"> The location to start parsing. </param>
        /// <param name="end"> The location to stop parsing. </param>
        /// <returns> A parsed markdown image, or <c>null</c> if this is not a markdown image. </returns>
        internal static Common.InlineParseResult Parse(string markdown, int start, int end)
        {
            // Expect a '!' character.
            if (start >= end || markdown[start] != '!')
            {
                return null;
            }

            int pos = start + 1;

            // Then a '[' character
            if (pos >= end || markdown[pos] != '[')
            {
                return null;
            }

            pos++;

            // Find the ']' character
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

            // Extract the alt.
            string tooltip = markdown.Substring(start + 2, pos - (start + 2));

            // Expect the '(' character.
            pos++;
            if (pos == end || markdown[pos] != '(')
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
            while (pos < end && markdown[pos] != ')')
            {
                pos++;
            }

            string url = TextRunInline.ResolveEscapeSequences(markdown, urlStart, pos);

            // We found something!
            var result = new ImageInline
            {
                Tooltip = tooltip,
                Url = url,
                Text = markdown.Substring(start, pos + 1 - start)
            };
            return new Common.InlineParseResult(result, start, pos + 1);
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            return string.Format("[{0}]: {1}", Tooltip, Url);
        }
    }
}
