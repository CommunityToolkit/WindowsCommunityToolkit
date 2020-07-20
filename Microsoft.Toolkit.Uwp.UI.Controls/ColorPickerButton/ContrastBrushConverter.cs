// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Gets a color, either black or white, depending on the brightness of the supplied color.
    /// </summary>
    public class ContrastBrushConverter : IValueConverter
    {
        /// <summary>
        /// Gets or sets the alpha channel threshold below which a default color is used instead of black/white.
        /// </summary>
        public byte AlphaThreshold { get; set; } = 128;

        /// <inheritdoc/>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            Color comparisonColor;
            Color? defaultColor = null;

            // Get the changing color to compare against
            if (value is Color valueColor)
            {
                comparisonColor = valueColor;
            }
            else if (value is SolidColorBrush valueBrush)
            {
                comparisonColor = valueBrush.Color;
            }
            else
            {
                throw new ArgumentException("Invalid color value provided");
            }

            // Get the default color when transparency is high
            if (parameter is Color parameterColor)
            {
                defaultColor = parameterColor;
            }
            else if (parameter is SolidColorBrush parameterBrush)
            {
                defaultColor = parameterBrush.Color;
            }

            if (comparisonColor.A < AlphaThreshold &&
                defaultColor.HasValue)
            {
                // If the transparency is less than 50 %, just use the default brush
                // This can commonly be something like the TextControlForeground brush
                return new SolidColorBrush(defaultColor.Value);
            }
            else
            {
                // Chose a white/black brush based on contrast to the base color
                if (this.GetBrightness(comparisonColor) > 0.5)
                {
                    // Bright color, use a dark for contrast
                    return new SolidColorBrush(Colors.Black);
                }
                else
                {
                    // Dark color, use a light for contrast
                    return new SolidColorBrush(Colors.White);
                }
            }
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

        /// <summary>
        /// Gets the perceived brightness or intensity of the color.
        /// This value is normalized between zero (black) and one (white).
        /// </summary>
        /// <remarks>
        ///
        /// The base formula for luminance is from Rec. ITU-R BT.601-7 ():
        ///    https://www.itu.int/dms_pubrec/itu-r/rec/bt/R-REC-BT.601-7-201103-I!!PDF-E.pdf
        ///    Section 2.5.1 Construction of luminance (EY) and colour-difference (ER–EY) and (EB–EY) signals.
        ///
        /// This formula accounts for physiological aspects: the human eyeball is most sensitive to green light,
        /// less to red and least to blue.
        ///
        ///    Luminance = (0.299 * Red) + (0.587 * Green) + (0.114 * Blue)
        ///
        /// This formula is also recommended by the W3C Techniques For Accessibility Evaluation And Repair Tools
        ///    https://www.w3.org/TR/AERT/#color-contrast
        ///
        /// Contrary to the above formula, this is not called luminance and is called brightness instead.
        /// This value is not measurable and is subjective which better fits the definition of brightness:
        ///    - Luminance is the luminous intensity, projected on a given area and direction.
        ///      Luminance is an objectively measurable attribute. The unit is 'Candela per Square Meter' (cd/m2).
        ///    - Brightness is a subjective attribute of light. The monitor can be adjusted to a level of light
        ///      between very dim and very bright. Brightness is perceived and cannot be measured objectively.
        ///
        /// Other useful information can be found here:
        ///    http://www.nbdtech.com/Blog/archive/2008/04/27/Calculating-the-Perceived-Brightness-of-a-Color.aspx
        ///
        /// </remarks>
        private double GetBrightness(Color color)
        {
            return ((0.299 * color.R) + (0.587 * color.G) + (0.114 * color.B)) / 255;
        }
    }
}
