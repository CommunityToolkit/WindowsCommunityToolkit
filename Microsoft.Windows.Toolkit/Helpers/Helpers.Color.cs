// *********************************************************
//  Copyright (c) Microsoft. All rights reserved.
//  This code is licensed under the MIT License (MIT).
//  THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
//  INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, 
//  FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. 
//  IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, 
//  DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT, 
//  TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH 
//  THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// *********************************************************
using System;
using System.Reflection;

using Windows.UI;

namespace Microsoft.Windows.Toolkit
{


    public static partial class Helpers
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
                throw new ArgumentException("Invalid color string.", nameof(colorString));
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
                        throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color format.", colorString));
                }
            }

            if (
                colorString.Length > 3 &&
                colorString[0] == 's' &&
                colorString[1] == 'c' &&
                colorString[2] == '#')
            {
                var values = colorString.Split(',');

                if (values.Length == 4)
                {
                    var scA = double.Parse(values[0].Substring(3));
                    var scR = double.Parse(values[1]);
                    var scG = double.Parse(values[2]);
                    var scB = double.Parse(values[3]);

                    return Color.FromArgb(
                        (byte)(scA * 255), 
                        (byte)(scR * 255), 
                        (byte)(scG * 255), 
                        (byte)(scB * 255));
                }

                if (values.Length == 3)
                {
                    var scR = double.Parse(values[0].Substring(3));
                    var scG = double.Parse(values[1]);
                    var scB = double.Parse(values[2]);

                    return Color.FromArgb(
                        255, 
                        (byte)(scR * 255), 
                        (byte)(scG * 255), 
                        (byte)(scB * 255));
                }

                throw new FormatException(string.Format("The {0} string passed in the c argument is not a recognized Color format (sc#[scA,]scR,scG,scB).", colorString));
            }

            var prop = typeof(Colors).GetTypeInfo().GetDeclaredProperty(colorString);
            return (Color)prop.GetValue(null);
        }
    }
}
