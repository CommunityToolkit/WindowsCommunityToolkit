// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Text;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// An abstract <see cref="MarkupExtension"/> which to produce text-based icons.
    /// </summary>
    /// <typeparam name="T">The type representing the glyph for the current icon.</typeparam>
    public abstract class TextIconExtension<T> : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the <typeparamref name="T"/> value representing the icon to display.
        /// </summary>
        public T Glyph { get; set; }

        /// <summary>
        /// Gets or sets the size of the icon to display.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the thickness of the icon glyph.
        /// </summary>
        public FontWeight FontWeight { get; set; } = FontWeights.Normal;

        /// <summary>
        /// Gets or sets the font style for the icon glyph.
        /// </summary>
        public FontStyle FontStyle { get; set; } = FontStyle.Normal;

        /// <summary>
        /// Gets or sets a value indicating whether automatic text enlargement, to reflect the system text size setting, is enabled.
        /// </summary>
        public bool IsTextScaleFactorEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the icon is mirrored when the flow direction is right to left.
        /// </summary>
        public bool MirroredWhenRightToLeft { get; set; }
    }
}
