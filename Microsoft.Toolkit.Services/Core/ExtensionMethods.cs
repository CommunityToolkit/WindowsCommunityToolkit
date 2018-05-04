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
using System.Reflection;
using Microsoft.Toolkit.Extensions;
using Microsoft.Toolkit.Services.Bing;

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts between country code and country name.
        /// </summary>
        /// <param name="value">BingCountry enumeration.</param>
        /// <returns>Returns country code.</returns>
        public static string GetStringValue(this BingCountry value)
        {
            return GetStringValue((Enum)value);
        }

        /// <summary>
        /// Converts between language code and language name.
        /// </summary>
        /// <param name="value">BingLanguage enumeration.</param>
        /// <returns>Returns language code.</returns>
        public static string GetStringValue(this BingLanguage value)
        {
            return GetStringValue((Enum)value);
        }

        /// <summary>
        /// Converts between enumeration value and string value.
        /// </summary>
        /// <param name="value">Enumeration.</param>
        /// <returns>Returns string value.</returns>
        private static string GetStringValue(Enum value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetRuntimeField(value.ToString());
            Parsers.Core.StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(Parsers.Core.StringValueAttribute), false) as Parsers.Core.StringValueAttribute[];
            if (attrs != null && attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }
}