using System;
using Windows.UI;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Gets a color, either black or white, depending on the brightness of the supplied color.
    /// </summary>
    public class ContrastColorConverter : IValueConverter
    {
        /// <inheritdoc/>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            Color comparisonColor;

            // Get the changing color to compare against
            try
            {
                comparisonColor = (Color)value;
            }
            catch
            {
                throw new ArgumentException("Invalid color value provided");
            }

            if (this.GetBrightness(comparisonColor) > 0.5)
            {
                // Bright color, use a dark for contrast
                return Colors.Black;
            }
            else
            {
                // Dark color, use a light for contrast
                return Colors.White;
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
