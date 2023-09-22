// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Creates an accent color for a base color value.
    /// </summary>
    public class AccentColorConverter : IValueConverter
    {
        /// <summary>
        /// The amount to change the Value channel for each accent color step.
        /// </summary>
        public const double ValueDelta = 0.1;

        /// <summary>
        /// This does not account for perceptual differences and also does not match with
        /// system accent color calculation.
        /// </summary>
        /// <remarks>
        /// Use the HSV representation as it's more perceptual.
        /// In most cases only the value is changed by a fixed percentage so the algorithm is reproducible.
        /// </remarks>
        /// <param name="hsvColor">The base color to calculate the accent from.</param>
        /// <param name="accentStep">The number of accent color steps to move.</param>
        /// <returns>The new accent color.</returns>
        public static HsvColor GetAccent(HsvColor hsvColor, int accentStep)
        {
            if (accentStep != 0)
            {
                double colorValue = hsvColor.V;
                colorValue += accentStep * AccentColorConverter.ValueDelta;
                colorValue = Math.Round(colorValue, 2);

                return new HsvColor()
                {
                    A = Math.Clamp(hsvColor.A, 0.0, 1.0),
                    H = Math.Clamp(hsvColor.H, 0.0, 360.0),
                    S = Math.Clamp(hsvColor.S, 0.0, 1.0),
                    V = Math.Clamp(colorValue, 0.0, 1.0),
                };
            }
            else
            {
                return hsvColor;
            }
        }

        /// <inheritdoc/>
        public object Convert(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            int accentStep;
            Color? rgbColor = null;
            HsvColor? hsvColor = null;

            // Get the current color in HSV
            if (value is Color valueColor)
            {
                rgbColor = valueColor;
            }
            else if (value is HsvColor valueHsvColor)
            {
                hsvColor = valueHsvColor;
            }
            else if (value is SolidColorBrush valueBrush)
            {
                rgbColor = valueBrush.Color;
            }
            else
            {
                // Invalid color value provided
                return DependencyProperty.UnsetValue;
            }

            // Get the value component delta
            try
            {
                accentStep = int.Parse(parameter?.ToString(), CultureInfo.InvariantCulture);
            }
            catch
            {
                // Invalid parameter provided, unable to convert to integer
                return DependencyProperty.UnsetValue;
            }

            if (hsvColor == null &&
                rgbColor != null)
            {
                hsvColor = rgbColor.Value.ToHsv();
            }

            if (hsvColor != null)
            {
                var hsv = AccentColorConverter.GetAccent(hsvColor.Value, accentStep);

                return Uwp.Helpers.ColorHelper.FromHsv(hsv.H, hsv.S, hsv.V, hsv.A);
            }
            else
            {
                return DependencyProperty.UnsetValue;
            }
        }

        /// <inheritdoc/>
        public object ConvertBack(
            object value,
            Type targetType,
            object parameter,
            string language)
        {
            return DependencyProperty.UnsetValue;
        }
    }
}
