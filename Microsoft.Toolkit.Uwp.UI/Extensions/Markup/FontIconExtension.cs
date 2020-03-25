// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Text;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Custom <see cref="MarkupExtension"/> which can provide <see cref="FontIcon"/> values.
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(FontIcon))]
    public class FontIconExtension : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the <see cref="string"/> representing the icon to display.
        /// </summary>
        public string Glyph { get; set; }

        /// <summary>
        /// Gets or sets the size of the icon to display.
        /// </summary>
        public double FontSize { get; set; }

        /// <summary>
        /// Gets or sets the font family to use to display the icon. If <see langword="null"/>, "Segoe MDL2 Assets" will be used.
        /// </summary>
        public FontFamily FontFamily { get; set; }

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

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            var fontIcon = new FontIcon
            {
                Glyph = Glyph,
                FontFamily = FontFamily ?? new FontFamily("Segoe MDL2 Assets"),
                FontWeight = FontWeight,
                FontStyle = FontStyle,
                IsTextScaleFactorEnabled = IsTextScaleFactorEnabled,
                MirroredWhenRightToLeft = MirroredWhenRightToLeft
            };

            if (FontSize > 0)
            {
                fontIcon.FontSize = FontSize;
            }

            return fontIcon;
        }
    }
}
