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
using Microsoft.Toolkit.Uwp.Services.Bing;

namespace Microsoft.Toolkit.Uwp.Services.Core
{
    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    [Obsolete("This class is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Parsers and Microsoft.Toolkit.Services.")]
    public static class ExtensionMethods
    {
        /// <summary>
        /// Converts object into string.
        /// </summary>
        /// <param name="value">Object value.</param>
        /// <returns>Returns string value.</returns>
        [Obsolete("This method is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Parsers.")]
        public static string ToSafeString(this object value)
        {
            return Parsers.Core.ExtensionMethods.ToSafeString(value);
        }

        /// <summary>
        /// Decode HTML string.
        /// </summary>
        /// <param name="htmlText">HTML string.</param>
        /// <returns>Returns decoded HTML string.</returns>
        [Obsolete("This method is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Parsers.")]
        public static string DecodeHtml(this string htmlText)
        {
            return Parsers.Core.ExtensionMethods.DecodeHtml(htmlText);
        }

        /// <summary>
        /// Converts between country code and country name.
        /// </summary>
        /// <param name="value">BingCountry enumeration.</param>
        /// <returns>Returns country code.</returns>
        [Obsolete("This method is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.")]
        public static string GetStringValue(this BingCountry value)
        {
            return Toolkit.Services.Core.ExtensionMethods.GetStringValue((Toolkit.Services.Bing.BingCountry)value);
        }

        /// <summary>
        /// Converts between language code and language name.
        /// </summary>
        /// <param name="value">BingLanguage enumeration.</param>
        /// <returns>Returns language code.</returns>
        [Obsolete("This method is being deprecated. Please use the .NET Standard Library counterpart found in Microsoft.Toolkit.Services.")]
        public static string GetStringValue(this BingLanguage value)
        {
            return Toolkit.Services.Core.ExtensionMethods.GetStringValue((Toolkit.Services.Bing.BingLanguage)value);
        }
    }
}