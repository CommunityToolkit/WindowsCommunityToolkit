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
using System.Globalization;
using System.Net;
using System.Text.RegularExpressions;

namespace Microsoft.Toolkit.Extensions
{
    /// <summary>
    /// All common long extensions should go here
    /// </summary>
    public static class Int64Extensions
    {
        /// <summary>
        /// Translate numeric file size to string format.
        /// </summary>
        /// <param name="size">file size in bytes.</param>
        /// <returns>Returns file size string.</returns>
        public static string ToFileSizeString(this long size)
        {
            if (size == 1)
            {
                return "1 byte";
            }
            else if (size < 1024 * 2)
            {
                return $"{size} bytes";
            }
            else if (size < 1024 * 1024)
            {
                return $"{size}KB";
            }
            else
            {
                return $"{size}MB";
            }
        }
    }
}