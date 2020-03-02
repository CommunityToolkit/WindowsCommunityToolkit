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
        /// If both ImageWidth and ImageHeight are greater than 0, ImageStretch is set to Fill.
        /// </summary>
        public int ImageWidth { get; internal set; }

        /// <summary>
        /// Gets image height
        /// If value is greater than 0, ImageStretch is set to UniformToFill
        /// If both ImageWidth and ImageHeight are greater than 0, ImageStretch is set to Fill.
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
            protected override InlineParseResult<ImageInline> ParseInternal(LineBlock markdown, LineBlockPosition tripPos, MarkdownDocument document, IEnumerable<Type> ignoredParsers)
            {
                if (!tripPos.IsIn(markdown))
                {
                    throw new ArgumentOutOfRangeException(nameof(tripPos));
                }

                var line = markdown.SliceText(tripPos)[0];

                // Expect a '!' character.
                if (line[0] != '!')
                {
                    return null;
                }

                var firstPart = line.Slice(1);

                // Then a '[' character
                if (firstPart.Length == 0 || firstPart[0] != '[')
                {
                    return null;
                }

                var closing = firstPart.FindClosingBrace();

                if (closing == -1)
                {
                    return null;
                }

                firstPart = firstPart.Slice(0, closing + 1);

                // Extract the alt.
                string tooltip = firstPart.Slice(1, firstPart.Length - 2).ToString();

                var seccondPart = line.Slice(1 + firstPart.Length);

                if (seccondPart.Length == 0 || (seccondPart[0] != '[' && seccondPart[0] != '('))
                {
                    return null;
                }

                var seccodClosing = seccondPart.FindClosingBrace();
                if (seccodClosing == -1)
                {
                    return null;
                }

                seccondPart = seccondPart.Slice(0, seccodClosing + 1);

                string reference = string.Empty;
                string url = string.Empty;
                int imageWidth = 0;
                int imageHeight = 0;

                if (seccondPart[0] == '[')
                {
                    reference = seccondPart.Slice(1, seccondPart.Length - 2).ToString();
                }
                else
                {
                    var urlSpan = seccondPart.Slice(1, seccondPart.Length - 2);

                    var imageDimensionsPos = urlSpan.IndexOf(" =".AsSpan(), StringComparison.Ordinal);

                    url = imageDimensionsPos > 0
                        ? document.ResolveEscapeSequences(urlSpan.Slice(0, imageDimensionsPos), true, true)
                        : document.ResolveEscapeSequences(urlSpan, true, true);

                    if (imageDimensionsPos > 0)
                    {
                        var imageDimension = urlSpan.Slice(imageDimensionsPos + 2);

                        // trying to find 'x' which separates image width and height
                        var dimensionsSepatorPos = imageDimension.IndexOf("x".AsSpan(), StringComparison.Ordinal);

                        // didn't find separator, trying to parse value as imageWidth
                        if (dimensionsSepatorPos == -1)
                        {
                            int.TryParse(imageDimension.ToString(), out imageWidth);
                        }
                        else
                        {
                            // got width and height
                            var width = imageDimension.Slice(0, dimensionsSepatorPos);
                            var height = imageDimension.Slice(dimensionsSepatorPos + 1);
                            int.TryParse(width.ToString(), out imageWidth);
                            int.TryParse(height.ToString(), out imageHeight);
                        }
                    }
                }

                var completeInline = line.Slice(0, firstPart.Length + seccondPart.Length + 1);

                // We found something!
                var result = new ImageInline
                {
                    Tooltip = tooltip,
                    RenderUrl = url,
                    ReferenceId = reference,
                    Url = url,
                    Text = completeInline.ToString(),
                    ImageWidth = imageWidth,
                    ImageHeight = imageHeight,
                };
                return InlineParseResult.Create(result, tripPos, completeInline.Length);
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