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

using System.Text.RegularExpressions;

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// All common string extensions should go here
    /// </summary>
    public static class StringExtensions
    {
        internal const string DecimalRegex = "^-?[0-9]{1,28}([.,][0-9]{1,28})?$";
        internal const string NumberRegex = "^-?[0-9]{1,9}$";
        internal const string PhoneNumberRegex = @"^\s*\+?\s*([0-9][\s-]*){9,}$";
        internal const string CharactersRegex = "^[A-Za-z]+$";
        internal const string EmailRegex = "(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*|\"(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21\\x23-\\x5b\\x5d-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])*\")@(?:(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?|\\[(?:(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?)\\.){3}(?:25[0-5]|2[0-4][0-9]|[01]?[0-9][0-9]?|[a-z0-9-]*[a-z0-9]:(?:[\\x01-\\x08\\x0b\\x0c\\x0e-\\x1f\\x21-\\x5a\\x53-\\x7f]|\\\\[\\x01-\\x09\\x0b\\x0c\\x0e-\\x7f])+)\\])";

        /// <summary>
        /// Returns whether said string is a valid email or not.
        /// Uses general Email Regex (RFC 5322 Official Standard) from emailregex.com
        /// </summary>
        /// <returns><c>true</c> for valid email.<c>false</c> otherwise</returns>
        public static bool IsEmail(this string str)
        {
            return Regex.IsMatch(str, EmailRegex);
        }

        /// <summary>
        /// Returns whether said string is a valid decimal number or not.
        /// </summary>
        /// <returns><c>true</c> for valid decimal number.<c>false</c> otherwise</returns>
        public static bool IsDecimal(this string str)
        {
            return Regex.IsMatch(str, DecimalRegex);
        }

        /// <summary>
        /// Returns whether said string is a valid number or not.
        /// </summary>
        /// <returns><c>true</c> for valid number.<c>false</c> otherwise</returns>
        public static bool IsNumeric(this string str)
        {
            return Regex.IsMatch(str, NumberRegex);
        }

        /// <summary>
        /// Returns whether said string is a valid phonenumber or not.
        /// </summary>
        /// <returns><c>true</c> for valid phonenumber.<c>false</c> otherwise</returns>
        public static bool IsPhoneNumber(this string str)
        {
            return Regex.IsMatch(str, PhoneNumberRegex);
        }

        /// <summary>
        /// Returns whether said string is a valid Character or not.
        /// </summary>
        /// <returns><c>true</c> for valid Character.<c>false</c> otherwise</returns>
        public static bool IsCharacterString(this string str)
        {
            return Regex.IsMatch(str, CharactersRegex);
        }
    }
}
