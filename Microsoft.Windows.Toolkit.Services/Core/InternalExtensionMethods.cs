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

namespace Microsoft.Windows.Toolkit.Services.Core
{
    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    internal static class InternalExtensionMethods
    {
        private static Regex RemoveCommentsRegex = new Regex("<!--.*?-->", RegexOptions.Singleline);
        private static Regex RemoveScriptsRegex = new Regex(@"(?s)<script.*?(/>|</script>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
        private static Regex RemoveStylesRegex = new Regex(@"(?s)<style.*?(/>|</style>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="value">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this string value, int length)
        {
            return Truncate(value, length, false);
        }

        /// <summary>
        /// Truncates the specified string to the specified length.
        /// </summary>
        /// <param name="value">The string to be truncated.</param>
        /// <param name="length">The maximum length.</param>
        /// <param name="ellipsis">if set to <c>true</c> add a text ellipsis.</param>
        /// <returns>Truncated string.</returns>
        public static string Truncate(this string value, int length, bool ellipsis)
        {
            if (!string.IsNullOrEmpty(value))
            {
                value = value.Trim();
                if (value.Length > length)
                {
                    if (ellipsis)
                    {
                        return value.Substring(0, length) + "...";
                    }
                    else
                    {
                        return value.Substring(0, length);
                    }
                }
            }

            return value ?? string.Empty;
        }

        public static string FixHtml(this string html)
        {
            // Remove comments
            var withoutComments = RemoveCommentsRegex.Replace(html, string.Empty);

            // Remove scripts
            var withoutScripts = RemoveScriptsRegex.Replace(withoutComments, string.Empty);

            // Remove styles
            var withoutStyles = RemoveStylesRegex.Replace(withoutScripts, string.Empty);

            return withoutStyles;
        }
    }
}
