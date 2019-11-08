// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Internal class used to provide helpers for controls
    /// </summary>
    internal static partial class ControlHelpers
    {
        /// <summary>
        /// Gets the positive modulo of an integer
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="module">Module to use</param>
        /// <returns>Positive modulo</returns>
        public static int Mod(this int value, int module)
        {
            int result = value % module;
            return result >= 0 ? result : (result + module) % module;
        }

        /// <summary>
        /// Gets modulo of value + 1
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="module">Module to use</param>
        /// <returns>Modulo of value + 1</returns>
        public static int IncMod(this int value, int module)
        {
            return (value + 1).Mod(module);
        }

        /// <summary>
        /// Gets modulo of value - 1
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="module">Module to use</param>
        /// <returns>Modulo of value - 1</returns>
        public static int DecMod(this int value, int module)
        {
            return (value - 1).Mod(module);
        }

        /// <summary>
        /// Gets the positive modulo of a double
        /// </summary>
        /// <param name="value">Value to use</param>
        /// <param name="module">Module to use</param>
        /// <returns>Positive modulo</returns>
        public static double Mod(this double value, double module)
        {
            double res = value % module;
            return res >= 0 ? res : (res + module) % module;
        }

        internal static Thickness NewThickness(double uniformLength)
        {
            Thickness t = default(Thickness);
            t.Left = uniformLength;
            t.Top = uniformLength;
            t.Right = uniformLength;
            t.Bottom = uniformLength;
            return t;
        }

        internal static Thickness NewThickness(double left, double top, double right, double bottom)
        {
            Thickness t = default(Thickness);
            t.Left = left;
            t.Top = top;
            t.Right = right;
            t.Bottom = bottom;
            return t;
        }

        internal static GridLength NewGridLength(double pixels)
        {
            GridLength g = default(GridLength);
            g.Value = pixels;
            g.GridUnitType = GridUnitType.Pixel;
            return g;
        }

        internal static GridLength NewGridLength(double value, GridUnitType gridUnitType)
        {
            GridLength g = default(GridLength);
            g.Value = value;
            g.GridUnitType = gridUnitType;
            return g;
        }

        internal static GridLength GridLengthAuto { get; } = new GridLength
        {
            GridUnitType = GridUnitType.Auto
        };

        internal static bool IsStar(this GridLength g)
        {
            return g.GridUnitType == GridUnitType.Star;
        }

        internal static CornerRadius NewCornerRadius(double uniformRadius)
        {
            CornerRadius c = default(CornerRadius);
            c.TopLeft = uniformRadius;
            c.TopRight = uniformRadius;
            c.BottomRight = uniformRadius;
            c.BottomLeft = uniformRadius;
            return c;
        }
    }
}