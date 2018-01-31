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

        /// <summary>
        /// Gets image width
        /// If value is greater than 0, ImageStretch is set to UniformToFill
        /// If both ImageWidth and ImageHeight are greater than 0, ImageStretch is set to Fill
        /// </summary>
        public int ImageWidth { get; private set; }

        /// <summary>
        /// Gets image height
        /// If value is greater than 0, ImageStretch is set to UniformToFill
        /// If both ImageWidth and ImageHeight are greater than 0, ImageStretch is set to Fill
        /// </summary>
        public int ImageHeight { get; private set; }

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

            var imageDimensionsPos = markdown.IndexOf(" =", urlStart, pos - urlStart, StringComparison.Ordinal);

            var url = imageDimensionsPos > 0
                ? TextRunInline.ResolveEscapeSequences(markdown, urlStart, imageDimensionsPos)
                : TextRunInline.ResolveEscapeSequences(markdown, urlStart, pos);

            int imageWidth = 0;
            int imageHeight = 0;

            if (imageDimensionsPos > 0)
            {
                // trying to find 'x' which separates image width and height
                var dimensionsSepatorPos = markdown.IndexOf("x", imageDimensionsPos, pos - imageDimensionsPos - 2, StringComparison.Ordinal);

                // didn't find separator, trying to parse value as imageWidth
                if (dimensionsSepatorPos == -1)
                {
                    var imageWidthStr = markdown.Substring(imageDimensionsPos + 2, pos - imageDimensionsPos - 2);

                    int.TryParse(imageWidthStr, out imageWidth);
                }
                else
                {
                    var dimensions = markdown.Substring(imageDimensionsPos + 2, pos - imageDimensionsPos - 2).Split('x');

                    // got width and height
                    if (dimensions.Length == 2)
                    {
                        int.TryParse(dimensions[0], out imageWidth);
                        int.TryParse(dimensions[1], out imageHeight);
                    }
                }
            }

            // We found something!
            var result = new ImageInline
            {
                Tooltip = tooltip,
                Url = url,
                Text = markdown.Substring(start, pos + 1 - start),
                ImageWidth = imageWidth,
                ImageHeight = imageHeight
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
