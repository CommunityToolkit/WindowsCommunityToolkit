// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Numerics;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace CommunityToolkit.WinUI.SampleApp.Common
{
    public class Vector3Converter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is string)
            {
                return value;
            }

            var thickness = (Vector3)value;

            return thickness.ToString().TrimStart('<').Replace(" ", string.Empty).TrimEnd('>');
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is string vectorString)
            {
                var vectorTokens = vectorString.Split(',')
                                                     .Where(tkn => !string.IsNullOrWhiteSpace(tkn))
                                                     .ToArray();
                switch (vectorTokens.Length)
                {
                    case 1:
                        var vectorValue = float.Parse(vectorString);
                        return new Vector3(vectorValue);
                    case 2:
                        var xValue = float.Parse(vectorTokens[0]);
                        var yValue = float.Parse(vectorTokens[1]);

                        return new Vector3(xValue, yValue, 0);
                    case 3:
                        return new Vector3(
                            float.Parse(vectorTokens[0]),
                            float.Parse(vectorTokens[1]),
                            float.Parse(vectorTokens[2]));
                    default:
                        return default(Vector3);
                }
            }

            return value.ToString();
        }
    }
}