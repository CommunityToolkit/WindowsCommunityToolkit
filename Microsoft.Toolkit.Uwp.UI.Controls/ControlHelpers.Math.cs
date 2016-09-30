﻿// ******************************************************************
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

namespace Microsoft.Toolkit.Uwp
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
    }
}