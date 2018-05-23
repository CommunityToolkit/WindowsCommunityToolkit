// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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