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

namespace Microsoft.Toolkit.Parsers.Core
{
    /// <summary>
    /// This class offers helpers for Parsing.
    /// </summary>
    public static class ParseHelpers
    {
        /// <summary>
        /// Determines if a string is blank or comprised entirely of whitespace characters.
        /// </summary>
        /// <returns>true if blank or white space</returns>
        public static bool IsBlankOrWhiteSpace(string str)
        {
            for (int i = 0; i < str.Length; i++)
            {
                if (!IsWhiteSpace(str[i]))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if a character is a whitespace character.
        /// </summary>
        /// <returns>true if is white space</returns>
        public static bool IsWhiteSpace(char c)
        {
            return c == ' ' || c == '\t' || c == '\r' || c == '\n';
        }
    }
}