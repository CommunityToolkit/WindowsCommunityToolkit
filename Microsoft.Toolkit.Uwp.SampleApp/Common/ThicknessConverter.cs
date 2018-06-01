// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.Common
{
    public class ThicknessConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return value;
            }

            var thickness = (Thickness)value;

            return thickness.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string thicknessString)
            {
                var thicknessTokens = thicknessString.Split(',')
                                                     .Where(tkn => !string.IsNullOrWhiteSpace(tkn))
                                                     .ToArray();
                switch (thicknessTokens.Length)
                {
                    case 1:
                        var thicknessValue = double.Parse(thicknessString);
                        return new Thickness
                        {
                            Left = thicknessValue,
                            Top = thicknessValue,
                            Right = thicknessValue,
                            Bottom = thicknessValue
                        };
                    case 2:
                        var thicknessHorizontalValue = double.Parse(thicknessTokens[0]);
                        var thicknessVerticalValue = double.Parse(thicknessTokens[1]);

                        return new Thickness
                        {
                            Left = thicknessHorizontalValue,
                            Top = thicknessVerticalValue,
                            Right = thicknessHorizontalValue,
                            Bottom = thicknessVerticalValue
                        };
                    case 4:
                        return new Thickness
                        {
                            Left = double.Parse(thicknessTokens[0]),
                            Top = double.Parse(thicknessTokens[1]),
                            Right = double.Parse(thicknessTokens[2]),
                            Bottom = double.Parse(thicknessTokens[3])
                        };
                    default:
                        return default(Thickness);
                }
            }

            return value.ToString();
        }
    }
}
