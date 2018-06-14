// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for colors.
    /// </summary>
    public static class ColorHelper
    {
        /// <summary>
        /// Returns a color based on XAML color string.
        /// </summary>
        /// <param name="colorString">The color string. Any format used in XAML should work.</param>
        /// <returns>Parsed color</returns>
        public static Color ToColor(this string colorString)
        {
            if (string.IsNullOrEmpty(colorString))
            {
                throw new ArgumentException(nameof(colorString));
            }

            if (colorString[0] == '#')
            {
                switch (colorString.Length)
                {
                    case 9:
                        {
                            var cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                            var a = (byte)(cuint >> 24);
                            var r = (byte)((cuint >> 16) & 0xff);
                            var g = (byte)((cuint >> 8) & 0xff);
                            var b = (byte)(cuint & 0xff);

                            return Color.FromArgb(a, r, g, b);
                        }

                    case 7:
                        {
                            var cuint = Convert.ToUInt32(colorString.Substring(1), 16);
                            var r = (byte)((cuint >> 16) & 0xff);
                            var g = (byte)((cuint >> 8) & 0xff);
                            var b = (byte)(cuint & 0xff);

                            return Color.FromArgb(255, r, g, b);
                        }

                    case 5:
                        {
                            var cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                            var a = (byte)(cuint >> 12);
                            var r = (byte)((cuint >> 8) & 0xf);
                            var g = (byte)((cuint >> 4) & 0xf);
                            var b = (byte)(cuint & 0xf);
                            a = (byte)(a << 4 | a);
                            r = (byte)(r << 4 | r);
                            g = (byte)(g << 4 | g);
                            b = (byte)(b << 4 | b);

                            return Color.FromArgb(a, r, g, b);
                        }

                    case 4:
                        {
                            var cuint = Convert.ToUInt16(colorString.Substring(1), 16);
                            var r = (byte)((cuint >> 8) & 0xf);
                            var g = (byte)((cuint >> 4) & 0xf);
                            var b = (byte)(cuint & 0xf);
                            r = (byte)(r << 4 | r);
                            g = (byte)(g << 4 | g);
                            b = (byte)(b << 4 | b);

                            return Color.FromArgb(255, r, g, b);
                        }

                    default:
                        throw new FormatException(string.Format("The {0} string passed in the colorString argument is not a recognized Color format.", colorString));
                }
            }

            if (colorString.Length > 3 && colorString[0] == 's' && colorString[1] == 'c' && colorString[2] == '#')
            {
                var values = colorString.Split(',');

                if (values.Length == 4)
                {
                    var scA = double.Parse(values[0].Substring(3));
                    var scR = double.Parse(values[1]);
                    var scG = double.Parse(values[2]);
                    var scB = double.Parse(values[3]);

                    return Color.FromArgb((byte)(scA * 255), (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
                }

                if (values.Length == 3)
                {
                    var scR = double.Parse(values[0].Substring(3));
                    var scG = double.Parse(values[1]);
                    var scB = double.Parse(values[2]);

                    return Color.FromArgb(255, (byte)(scR * 255), (byte)(scG * 255), (byte)(scB * 255));
                }

                throw new FormatException(string.Format("The {0} string passed in the colorString argument is not a recognized Color format (sc#[scA,]scR,scG,scB).", colorString));
            }

            var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(colorString);

            if (prop != null)
            {
                return (Color)prop.GetValue(null);
            }

            throw new FormatException(string.Format("The {0} string passed in the colorString argument is not a recognized Color.", colorString));
        }

        /// <summary>
        /// Converts a Color value to a string representation of the value in hexadecimal.
        /// </summary>
        /// <param name="color">The Color to convert.</param>
        /// <returns>Returns a string representing the hex value.</returns>
        public static string ToHex(this Color color)
        {
            return $"#{color.A:X2}{color.R:X2}{color.G:X2}{color.B:X2}";
        }

        /// <summary>
        /// Returns the color value as a premultiplied Int32 - 4 byte ARGB structure.
        /// </summary>
        /// <param name="color">the Color to convert</param>
        /// <returns>Returns a int representing the color.</returns>
        public static int ToInt(this Color color)
        {
            var a = color.A + 1;
            var col = (color.A << 24) | ((byte)((color.R * a) >> 8) << 16) | ((byte)((color.G * a) >> 8) << 8) | (byte)((color.B * a) >> 8);
            return col;
        }

        /// <summary>
        /// Converts an RGBA Color the HSL representation.
        /// </summary>
        /// <param name="color">The Color to convert.</param>
        /// <returns>HslColor.</returns>
        public static HslColor ToHsl(this Color color)
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * color.R;
            var g = toDouble * color.G;
            var b = toDouble * color.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

            if (chroma == 0)
            {
                h1 = 0;
            }
            else if (max == r)
            {
                // The % operator doesn't do proper modulo on negative
                // numbers, so we'll add 6 before using it
                h1 = (((g - b) / chroma) + 6) % 6;
            }
            else if (max == g)
            {
                h1 = 2 + ((b - r) / chroma);
            }
            else
            {
                h1 = 4 + ((r - g) / chroma);
            }

            double lightness = 0.5 * (max + min);
            double saturation = chroma == 0 ? 0 : chroma / (1 - Math.Abs((2 * lightness) - 1));
            HslColor ret;
            ret.H = 60 * h1;
            ret.S = saturation;
            ret.L = lightness;
            ret.A = toDouble * color.A;
            return ret;
        }

        /// <summary>
        /// Converts an RGBA Color the HSV representation.
        /// </summary>
        /// <param name="color">Color to convert.</param>
        /// <returns>HsvColor</returns>
        public static HsvColor ToHsv(this Color color)
        {
            const double toDouble = 1.0 / 255;
            var r = toDouble * color.R;
            var g = toDouble * color.G;
            var b = toDouble * color.B;
            var max = Math.Max(Math.Max(r, g), b);
            var min = Math.Min(Math.Min(r, g), b);
            var chroma = max - min;
            double h1;

            if (chroma == 0)
            {
                h1 = 0;
            }
            else if (max == r)
            {
                // The % operator doesn't do proper modulo on negative
                // numbers, so we'll add 6 before using it
                h1 = (((g - b) / chroma) + 6) % 6;
            }
            else if (max == g)
            {
                h1 = 2 + ((b - r) / chroma);
            }
            else
            {
                h1 = 4 + ((r - g) / chroma);
            }

            double saturation = chroma == 0 ? 0 : chroma / max;
            HsvColor ret;
            ret.H = 60 * h1;
            ret.S = saturation;
            ret.V = max;
            ret.A = toDouble * color.A;
            return ret;
        }

        /// <summary>
        /// Returns a Color struct based on HSL model.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="lightness">0..1 range lightness</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns>A Color object</returns>
        public static Color FromHsl(double hue, double saturation, double lightness, double alpha = 1.0)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue));
            }

            double chroma = (1 - Math.Abs((2 * lightness) - 1)) * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
            double m = lightness - (0.5 * chroma);
            double r1, g1, b1;

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)(255 * (r1 + m));
            byte g = (byte)(255 * (g1 + m));
            byte b = (byte)(255 * (b1 + m));
            byte a = (byte)(255 * alpha);

            return Color.FromArgb(a, r, g, b);
        }

        /// <summary>
        /// Returns a Color struct based on HSV model.
        /// </summary>
        /// <param name="hue">0..360 range hue</param>
        /// <param name="saturation">0..1 range saturation</param>
        /// <param name="value">0..1 range value</param>
        /// <param name="alpha">0..1 alpha</param>
        /// <returns>A Color object</returns>
        public static Color FromHsv(double hue, double saturation, double value, double alpha = 1.0)
        {
            if (hue < 0 || hue > 360)
            {
                throw new ArgumentOutOfRangeException(nameof(hue));
            }

            double chroma = value * saturation;
            double h1 = hue / 60;
            double x = chroma * (1 - Math.Abs((h1 % 2) - 1));
            double m = value - chroma;
            double r1, g1, b1;

            if (h1 < 1)
            {
                r1 = chroma;
                g1 = x;
                b1 = 0;
            }
            else if (h1 < 2)
            {
                r1 = x;
                g1 = chroma;
                b1 = 0;
            }
            else if (h1 < 3)
            {
                r1 = 0;
                g1 = chroma;
                b1 = x;
            }
            else if (h1 < 4)
            {
                r1 = 0;
                g1 = x;
                b1 = chroma;
            }
            else if (h1 < 5)
            {
                r1 = x;
                g1 = 0;
                b1 = chroma;
            }
            else
            {
                r1 = chroma;
                g1 = 0;
                b1 = x;
            }

            byte r = (byte)(255 * (r1 + m));
            byte g = (byte)(255 * (g1 + m));
            byte b = (byte)(255 * (b1 + m));
            byte a = (byte)(255 * alpha);

            return Color.FromArgb(a, r, g, b);
        }
    }
}
