// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Markup;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Custom <see cref="MarkupExtension"/> which can provide symbol-baased <see cref="FontIcon"/> values.
    /// </summary>
    [MarkupExtensionReturnType(ReturnType = typeof(FontIcon))]
    public class SymbolIconExtension : TextIconExtension<Symbol>
    {
        [ThreadStatic]
        private static FontFamily segoeMDL2AssetsFontFamily;

        /// <summary>
        /// Gets the reusable "Segoe MDL2 Assets" <see cref="FontFamily"/> instance.
        /// </summary>
        private static FontFamily SegoeMDL2AssetsFontFamily
        {
            get => segoeMDL2AssetsFontFamily ??= new FontFamily("Segoe MDL2 Assets");
        }

        /// <inheritdoc/>
        protected override object ProvideValue()
        {
            var fontIcon = new FontIcon
            {
                Glyph = unchecked((char)Glyph).ToString(),
                FontFamily = SegoeMDL2AssetsFontFamily,
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
