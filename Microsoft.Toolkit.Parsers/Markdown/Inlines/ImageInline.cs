// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Parsers.Core;
using Microsoft.Toolkit.Parsers.Markdown.Helpers;

namespace Microsoft.Toolkit.Parsers.Markdown.Inlines
{
    /// <summary>
    /// Represents an embedded image.
    /// </summary>
    public class ImageInline : MarkdownInline, IInlineLeaf
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
        /// Gets or sets the image Render URL.
        /// </summary>
        public string RenderUrl { get; set; }

        /// <summary>
        /// Gets or sets a text to display on hover.
        /// </summary>
        public string Tooltip { get; set; }

        /// <inheritdoc/>
        public string Text { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the ID of a reference, if this is a reference-style link.
        /// </summary>
        public string ReferenceId { get; set; }

        /// <summary>
        /// Gets image width
        /// If value is greater than 0, ImageStretch is set to UniformToFill
        /// If both ImageWidth and ImageHeight are greater than 0, ImageStretch is set to Fill
        /// </summary>
        public int ImageWidth { get; internal set; }

        /// <summary>
        /// Gets image height
        /// If value is greater than 0, ImageStretch is set to UniformToFill
        /// If both ImageWidth and ImageHeight are greater than 0, ImageStretch is set to Fill
        /// </summary>
        public int ImageHeight { get; internal set; }

        /// <summary>
        /// Attempts to parse an image e.g. "![Toolkit logo](https://raw.githubusercontent.com/windows-toolkit/WindowsCommunityToolkit/master/Microsoft.Toolkit.Uwp.SampleApp/Assets/ToolkitLogo.png)".
        /// </summary>
        public new class Parser : Parser<ImageInline>
        {
            /// <inheritdoc/>
            public override IEnumerable<char> TripChar => "!";

            /// <inheritdoc/>
            protected override InlineParseResult<ImageInline> ParseInternal(string markdown, int minStart, int tripPos, int maxEnd, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                // Expect a '!' character.
                if (tripPos >= maxEnd || markdown[tripPos] != '!')
                {
                    return null;
                }

                int pos = tripPos + 1;

                // Then a '[' character
                if (pos >= maxEnd || markdown[pos] != '[')
                {
                    return null;
                }

                pos++;

                // Find the ']' character
                while (pos < maxEnd)
                {
                    if (markdown[pos] == ']')
                    {
                        break;
                    }

                    pos++;
                }

                if (pos == maxEnd)
                {
                    return null;
                }

                // Extract the alt.
                string tooltip = markdown.Substring(tripPos + 2, pos - (tripPos + 2));

                // Expect the '(' character.
                pos++;

                string reference = string.Empty;
                string url = string.Empty;
                int imageWidth = 0;
                int imageHeight = 0;

                if (pos < maxEnd && markdown[pos] == '[')
                {
                    var refstart = pos;

                    // Find the reference ']' character
                    while (pos < maxEnd)
                    {
                        if (markdown[pos] == ']')
                        {
                            break;
                        }

                        pos++;
                    }

                    reference = markdown.Substring(refstart + 1, pos - refstart - 1);
                }
                else if (pos < maxEnd && markdown[pos] == '(')
                {
                    while (pos < maxEnd && ParseHelpers.IsMarkdownWhiteSpace(markdown[pos]))
                    {
                        pos++;
                    }

                    // Extract the URL.
                    int urlStart = pos;
                    while (pos < maxEnd && markdown[pos] != ')')
                    {
                        pos++;
                    }

                    var imageDimensionsPos = markdown.IndexOf(" =", urlStart, pos - urlStart, StringComparison.Ordinal);

                    url = imageDimensionsPos > 0
                        ? TextRunInline.ResolveEscapeSequences(markdown, urlStart + 1, imageDimensionsPos)
                        : TextRunInline.ResolveEscapeSequences(markdown, urlStart + 1, pos);

                    if (imageDimensionsPos > 0)
                    {
                        // trying to find 'x' which separates image width and height
                        var dimensionsSepatorPos = markdown.IndexOf("x", imageDimensionsPos + 2, pos - imageDimensionsPos - 1, StringComparison.Ordinal);

                        // didn't find separator, trying to parse value as imageWidth
                        if (dimensionsSepatorPos == -1)
                        {
                            var imageWidthStr = markdown.Substring(imageDimensionsPos + 2, pos - imageDimensionsPos - 2);

                            int.TryParse(imageWidthStr, out imageWidth);
                        }
                        else
                        {
                            var span = markdown.AsSpan(imageDimensionsPos + 2, pos - imageDimensionsPos - 2);
                            var index = span.IndexOf('x');

                            // got width and height
                            if (index >= 0 && span.Slice(index + 1).IndexOf('x') != 1)
                            {
                                int.TryParse(span.Slice(0, index).ToString(), out imageWidth);
                                int.TryParse(span.Slice(index + 1).ToString(), out imageHeight);
                            }
                        }
                    }
                }

                if (pos == maxEnd)
                {
                    return null;
                }

                // We found something!
                var result = new ImageInline
                {
                    Tooltip = tooltip,
                    RenderUrl = url,
                    ReferenceId = reference,
                    Url = url,
                    Text = markdown.Substring(tripPos, pos + 1 - tripPos),
                    ImageWidth = imageWidth,
                    ImageHeight = imageHeight
                };
                return InlineParseResult.Create(result, tripPos, pos + 1);
            }
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
            RenderUrl = url;
            ReferenceId = null;
        }

        /// <summary>
        /// Converts the object into it's textual representation.
        /// </summary>
        /// <returns> The textual representation of this object. </returns>
        public override string ToString()
        {
            if (ImageWidth > 0 && ImageHeight > 0)
            {
                return string.Format("![{0}]: {1} (Width: {2}, Height: {3})", Tooltip, Url, ImageWidth, ImageHeight);
            }

            if (ImageWidth > 0)
            {
                return string.Format("![{0}]: {1} (Width: {2})", Tooltip, Url, ImageWidth);
            }

            if (ImageHeight > 0)
            {
                return string.Format("![{0}]: {1} (Height: {2})", Tooltip, Url, ImageHeight);
            }

            return string.Format("![{0}]: {1}", Tooltip, Url);
        }
    }
}