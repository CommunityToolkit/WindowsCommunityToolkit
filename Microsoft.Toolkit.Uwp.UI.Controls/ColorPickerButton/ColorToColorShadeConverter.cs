// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Creates an accent color shade from a color value.
    /// Only +/- 3 shades from the given color are supported.
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1025:Code should not contain multiple whitespace in a row", Justification = "Whitespace is used to align code in columns for readability.")]
    public class ColorToColorShadeConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            int shade;
            HsvColor hsvColor;

            // Get the value component delta
            try
            {
                shade = System.Convert.ToInt32(parameter?.ToString());
            }
            catch
            {
                throw new ArgumentException("Invalid parameter provided, unable to convert to double");
            }

            // Get the current color in HSV
            try
            {
                hsvColor = ((Color)value).ToHsv();
            }
            catch
            {
                throw new ArgumentException("Invalid color value provided, unable to convert to HsvColor");
            }

            double colorHue        = hsvColor.H;
            double colorSaturation = hsvColor.S;
            double colorValue      = hsvColor.V;
            double colorAlpha      = hsvColor.A;

            // Use the HSV representation as it's more perceptual
            switch (shade)
            {
                case -3:
                    {
                        colorHue        *= 1.0;
                        colorSaturation *= 1.10;
                        colorValue      *= 0.40;
                        break;
                    }

                case -2:
                    {
                        colorHue        *= 1.0;
                        colorSaturation *= 1.05;
                        colorValue      *= 0.50;
                        break;
                    }

                case -1:
                    {
                        colorHue        *= 1.0;
                        colorSaturation *= 1.0;
                        colorValue      *= 0.75;
                        break;
                    }

                case 0:
                    {
                        // No change
                        break;
                    }

                case 1:
                    {
                        colorHue        *= 1.00;
                        colorSaturation *= 1.00;
                        colorValue      *= 1.05;
                        break;
                    }

                case 2:
                    {
                        colorHue        *= 1.00;
                        colorSaturation *= 0.75;
                        colorValue      *= 1.05;
                        break;
                    }

                case 3:
                    {
                        colorHue        *= 1.00;
                        colorSaturation *= 0.65;
                        colorValue      *= 1.05;
                        break;
                    }
            }

            return Uwp.Helpers.ColorHelper.FromHsv(
                Math.Clamp(colorHue,        0.0, 360.0),
                Math.Clamp(colorSaturation, 0.0, 1.0),
                Math.Clamp(colorValue,      0.0, 1.0),
                Math.Clamp(colorAlpha,      0.0, 1.0));
        }

        /// <inheritdoc/>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
