// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.ObjectModel;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class ColorPickerButton
    {
        /// <summary>
        /// Identifies the <see cref="CustomPaletteColors"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CustomPaletteColorsProperty =
            DependencyProperty.Register(nameof(CustomPaletteColors),
                                        typeof(ObservableCollection<Windows.UI.Color>),
                                        typeof(ColorPickerButton),
                                        new PropertyMetadata(Windows.UI.Color.FromArgb(0x00, 0x00, 0x00, 0x00)));

        /// <summary>
        /// Gets the list of custom palette colors.
        /// </summary>
        public ObservableCollection<Windows.UI.Color> CustomPaletteColors
        {
            get => (ObservableCollection<Windows.UI.Color>)base.GetValue(CustomPaletteColorsProperty);
        }

        /// <summary>
        /// Identifies the <see cref="CustomPaletteSectionCount"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CustomPaletteSectionCountProperty =
            DependencyProperty.Register(nameof(CustomPaletteSectionCount),
                                        typeof(int),
                                        typeof(ColorPickerButton),
                                        new PropertyMetadata(4));

        /// <summary>
        /// Gets or sets the number of colors in each section of the custom color palette.
        /// A section is the number of columns within an entire row in the palette.
        /// Within a standard palette, rows are shades and columns are unique colors.
        /// </summary>
        public int CustomPaletteSectionCount
        {
            get => (int)base.GetValue(CustomPaletteSectionCountProperty);
            set 
            {
                if (object.Equals(value, base.GetValue(CustomPaletteSectionCountProperty)) == false)
                {
                    base.SetValue(CustomPaletteSectionCountProperty, value);
                }
            }
        }

        /// <summary>
        /// Identifies the <see cref="CustomPalette"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty CustomPaletteProperty =
            DependencyProperty.Register(nameof(CustomPalette),
                                        typeof(IColorPalette),
                                        typeof(ColorPickerButton),
                                        new PropertyMetadata(DependencyProperty.UnsetValue));

        /// <summary>
        /// Gets or sets the custom color palette.
        /// This will automatically set <see cref="CustomPaletteColors"/> and <see cref="CustomPaletteSectionCount"/> 
        /// overwriting any existing values.
        /// </summary>
        public IColorPalette CustomPalette
        {
            get => (IColorPalette)base.GetValue(CustomPaletteProperty);
            set
            {
                if (object.Equals(value, base.GetValue(CustomPaletteProperty)) == false)
                {
                    base.SetValue(CustomPaletteProperty, value);
                }
            }
        }
    }
}
