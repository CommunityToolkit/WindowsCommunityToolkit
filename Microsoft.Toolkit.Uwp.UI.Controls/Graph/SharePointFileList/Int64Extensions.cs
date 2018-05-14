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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// All common long extensions should go here
    /// </summary>
    internal static class Int64Extensions
    {
        /// <summary>
        /// Translate numeric file size to string format.
        /// </summary>
        /// <param name="size">file size in bytes.</param>
        /// <returns>Returns file size string.</returns>
        public static string ToFileSizeString(this long size)
        {
            if (size < 1024)
            {
                return size.ToString("F0") + " bytes";
            }
            else if ((size >> 10) < 1024)
            {
                return (size / (float)1024).ToString("F1") + " KB";
            }
            else if ((size >> 20) < 1024)
            {
                return ((size >> 10) / (float)1024).ToString("F1") + " MB";
            }
            else if ((size >> 30) < 1024)
            {
                return ((size >> 20) / (float)1024).ToString("F1") + " GB";
            }
            else if ((size >> 40) < 1024)
            {
                return ((size >> 30) / (float)1024).ToString("F1") + " TB";
            }
            else if ((size >> 50) < 1024)
            {
                return ((size >> 40) / (float)1024).ToString("F1") + " PB";
            }
            else
            {
                return ((size >> 50) / (float)1024).ToString("F0") + " EB";
            }
        }
    }
}