// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Graphics.Display;

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for <see cref="ScreenUnit" />.
    /// </summary>
    public static class ScreenUnitHelper
    {
        private const float PixelToCentimeterRatio = 37.79527559055f;
        private const float PixelToInchRatio = 96;
        private const float CentimeterToInchRatio = 2.54f;

        /// <summary>
        /// Convert a value from a screen unit to another one (ex: 1cm => 37.7953px)
        /// </summary>
        /// <param name="from">Start unit</param>
        /// <param name="to">End unit</param>
        /// <param name="value">The value to convert (using start unit)</param>
        /// <returns>Returns the result of the conversion</returns>
        public static float Convert(ScreenUnit from, ScreenUnit to, float value)
        {
            if (from == to)
            {
                return value;
            }

            switch (from)
            {
                case ScreenUnit.Pixel:
                    if (to == ScreenUnit.Centimeter)
                    {
                        return value / PixelToCentimeterRatio;
                    }

                    if (to == ScreenUnit.Inch)
                    {
                        return value / PixelToInchRatio;
                    }

                    if (to == ScreenUnit.EffectivePixel)
                    {
                        return value / (float)DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                case ScreenUnit.Centimeter:
                    if (to == ScreenUnit.Pixel)
                    {
                        return value * PixelToCentimeterRatio;
                    }

                    if (to == ScreenUnit.Inch)
                    {
                        return value / CentimeterToInchRatio;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                case ScreenUnit.Inch:
                    if (to == ScreenUnit.Pixel)
                    {
                        return value * PixelToInchRatio;
                    }

                    if (to == ScreenUnit.Centimeter)
                    {
                        return value * CentimeterToInchRatio;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                case ScreenUnit.EffectivePixel:
                    if (to == ScreenUnit.Pixel)
                    {
                        return value * (float)DisplayInformation.GetForCurrentView().RawPixelsPerViewPixel;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                default:
                    throw new ArgumentOutOfRangeException(nameof(from));
            }
        }
    }
}
