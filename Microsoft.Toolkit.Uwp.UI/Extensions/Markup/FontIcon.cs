// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Custom <see cref="MarkupExtension"/> which can provide <see cref="Windows.UI.Xaml.Controls.FontIcon"/> values.
    /// </summary>
    [Bindable]
    [MarkupExtensionReturnType(ReturnType = typeof(Windows.UI.Xaml.Controls.FontIcon))]
    public class FontIcon : MarkupExtension
    {
        /// <summary>
        /// Gets or sets the <see cref="string"/> representing the icon to display.
        /// </summary>
        public string Glyph { get; set; }

        /// <summary>
        /// Gets or sets the font family to use to display the icon. If <see langword="null"/>, "Segoe MDL2 Assets" will be used.
        /// </summary>
        public FontFamily FontFamily { get; set; }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            return new Windows.UI.Xaml.Controls.FontIcon
            {
                Glyph = Glyph,
                FontFamily = FontFamily ?? new FontFamily("Segoe MDL2 Assets")
            };
        }
    }
}
