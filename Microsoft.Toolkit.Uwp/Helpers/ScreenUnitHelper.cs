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

namespace Microsoft.Toolkit.Uwp.Helpers
{
    /// <summary>
    /// This class provides static helper methods for <see cref="ScreenUnit" />.
    /// </summary>
    public static class ScreenUnitHelper
    {
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
                        return value / 37.79527559055f;
                    }

                    if (to == ScreenUnit.Inch)
                    {
                        return value / 96;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                case ScreenUnit.Centimeter:
                    if (to == ScreenUnit.Pixel)
                    {
                        return value * 37.79527559055f;
                    }

                    if (to == ScreenUnit.Inch)
                    {
                        return value / 2.54f;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                case ScreenUnit.Inch:
                    if (to == ScreenUnit.Pixel)
                    {
                        return value * 96;
                    }

                    if (to == ScreenUnit.Centimeter)
                    {
                        return value * 2.54f;
                    }

                    throw new ArgumentOutOfRangeException(nameof(to));

                default:
                    throw new ArgumentOutOfRangeException(nameof(from));
            }
        }
    }
}
