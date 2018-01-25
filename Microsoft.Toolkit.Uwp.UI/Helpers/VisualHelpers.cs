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
using Windows.Foundation.Metadata;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Helpers
{
    /// <summary>
    /// Helpers for Handling UWP Visuals.
    /// </summary>
    public static class VisualHelpers
    {
        /// <summary>
        /// Gets a value indicating whether Acrylic is Supported.
        /// </summary>
        public static bool SupportsFluentAcrylic => ApiInformation.IsTypePresent("Windows.UI.Xaml.Media.AcrylicBrush");

        /// <summary>
        /// Converts a Hex Color Codes to Color.
        /// </summary>
        /// <param name="hex">Hex Representation</param>
        /// <returns>Color</returns>
        public static Color ColorFromHex(string hex)
        {
            hex = hex.Replace("#", string.Empty);

            byte a = 255;
            int index = 0;

            if (hex.Length == 8)
            {
                a = (byte)Convert.ToUInt32(hex.Substring(index, 2), 16);
                index += 2;
            }

            byte r = (byte)Convert.ToUInt32(hex.Substring(index, 2), 16);
            index += 2;
            byte g = (byte)Convert.ToUInt32(hex.Substring(index, 2), 16);
            index += 2;
            byte b = (byte)Convert.ToUInt32(hex.Substring(index, 2), 16);

            return Color.FromArgb(a, r, g, b);
        }
    }
}