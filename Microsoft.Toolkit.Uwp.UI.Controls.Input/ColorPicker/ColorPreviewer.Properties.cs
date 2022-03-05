// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Primitives
{
    /// <inheritdoc cref="ColorPreviewer"/>
    public partial class ColorPreviewer
    {
        /// <summary>
        /// Identifies the <see cref="HsvColor"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HsvColorProperty =
            DependencyProperty.Register(
                nameof(HsvColor),
                typeof(HsvColor),
                typeof(ColorPreviewer),
                new PropertyMetadata(
                    Colors.Transparent.ToHsv(),
                    (s, e) => (s as ColorPreviewer)?.OnDependencyPropertyChanged(s, e)));

        /// <summary>
        /// Gets or sets the HSV color represented by the color previewer.
        /// This is the preferred color property for accuracy.
        /// </summary>
        public HsvColor HsvColor
        {
            get => (HsvColor)this.GetValue(HsvColorProperty);
            set
            {
                if (object.Equals(value, this.GetValue(HsvColorProperty)) == false)
                {
                    this.SetValue(HsvColorProperty, value);
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="ShowAccentColors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ShowAccentColorsProperty =
            DependencyProperty.Register(
                nameof(ShowAccentColors),
                typeof(bool),
                typeof(ColorPreviewer),
                new PropertyMetadata(
                    true,
                    (s, e) => (s as ColorPreviewer)?.OnDependencyPropertyChanged(s, e)));

        /// <summary>
        /// Gets or sets a value indicating whether accent colors are shown along
        /// with the preview color.
        /// </summary>
        public bool ShowAccentColors
        {
            get => (bool)this.GetValue(ShowAccentColorsProperty);
            set
            {
                if (object.Equals(value, this.GetValue(ShowAccentColorsProperty)) == false)
                {
                    this.SetValue(ShowAccentColorsProperty, value);
                }
            }
        }
    }
}
