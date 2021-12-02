// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics.Contracts;

namespace CommunityToolkit.WinUI.UI.Animations
{
    /// <summary>
    /// An extension <see langword="class"/> for the <see cref="float"/> type
    /// </summary>
    internal static class FloatExtensions
    {
        /// <summary>
        /// Returns a <see cref="string"/> representation of a <see cref="float"/> that avoids scientific notation, which is not compatible with the composition expression animations API
        /// </summary>
        /// <param name="number">The input <see cref="float"/> to process</param>
        /// <returns>A <see cref="string"/> representation of <paramref name="number"/> that can be used in a expression animation</returns>
        [Pure]
        public static string ToCompositionString(this float number)
        {
            var defaultString = number.ToString(System.Globalization.CultureInfo.InvariantCulture);
            var eIndex = defaultString.IndexOf('E');

            // If the default string representation is not in scientific notation, we can use it
            if (eIndex == -1)
            {
                return defaultString;
            }

            // If the number uses scientific notation because it is too large, we can print it without the decimal places
            var exponent = int.Parse(defaultString.Substring(eIndex + 1));
            if (exponent >= 0)
            {
                return number.ToString($"F0", System.Globalization.CultureInfo.InvariantCulture);
            }

            // Otherwise, we need to print it with the right number of decimals
            var decimalPlaces = -exponent // The number of decimal places is the exponent of 10
                + eIndex // Plus each character in the mantissa
                + (number < 0 ?
                    -3 : // Minus the sign, dot and first number of the mantissa if negative
                    -2); // Minus the dot and first number of the mantissa otherwise

            return number.ToString($"F{decimalPlaces}", System.Globalization.CultureInfo.InvariantCulture);
        }
    }
}