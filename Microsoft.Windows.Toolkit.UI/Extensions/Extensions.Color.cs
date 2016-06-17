using System.Globalization;

using Windows.UI;

namespace Microsoft.Windows.Toolkit.UI
{
    public partial class Extensions
    {
        /// <summary>
        /// Converts a <see cref="Color"/> value to a string representation of the value in hex.
        /// </summary>
        /// <param name="color">The <see cref="Color"/> to convert.</param>
        /// <returns>Returns a string representing the hex value.</returns>
        public static string ToHex(this Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Converts an ARGB or RGB hex color value to a <see cref="Color"/>.
        /// </summary>
        /// <param name="hexValue">
        /// The hex color value represented as a string.
        /// </param>
        /// <returns>
        /// Returns the <see cref="Color"/> representation of the hex color value string.
        /// </returns>
        public static Color ToColor(this string hexValue)
        {
            var val = hexValue.ToUpper();

            var color = Colors.Transparent;

            switch (val.Length)
            {
                // RGB
                case 7:
                    color = new Color
                                {
                                    A = 255,
                                    R = byte.Parse(val.Substring(1, 2), NumberStyles.AllowHexSpecifier),
                                    G = byte.Parse(val.Substring(3, 2), NumberStyles.AllowHexSpecifier),
                                    B = byte.Parse(val.Substring(5, 2), NumberStyles.AllowHexSpecifier)
                                };
                    break;

                // ARGB
                case 9:
                    color = new Color
                                {
                                    A = byte.Parse(val.Substring(1, 2), NumberStyles.AllowHexSpecifier),
                                    R = byte.Parse(val.Substring(3, 2), NumberStyles.AllowHexSpecifier),
                                    G = byte.Parse(val.Substring(5, 2), NumberStyles.AllowHexSpecifier),
                                    B = byte.Parse(val.Substring(7, 2), NumberStyles.AllowHexSpecifier)
                                };
                    break;
            }

            return color;
        }
    }
}