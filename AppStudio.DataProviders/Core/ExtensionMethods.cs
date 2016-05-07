// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace AppStudio.DataProviders.Core
{
    using System;
    using System.Net;
    using System.Reflection;
    using System.Text.RegularExpressions;
    using AppStudio.DataProviders.Bing;

    /// <summary>
    /// This class offers general purpose methods.
    /// </summary>
    public static class ExtensionMethods
    {
        private static Regex RemoveHtmlTagsRegex = new Regex(@"(?></?\w+)(?>(?:[^>'""]+|'[^']*'|""[^""]*"")*)>");

        public static string ToSafeString(this object value)
        {
            if (value == null)
            {
                return null;
            }

            return value.ToString();
        }


        public static string DecodeHtml(this string htmlText)
        {
            if (htmlText == null)
            {
                return null;
            }

            var ret = InternalExtensionMethods.FixHtml(htmlText);

            //Remove html tags
            ret = RemoveHtmlTagsRegex.Replace(ret, string.Empty);

            return WebUtility.HtmlDecode(ret);
        }

        public static string GetStringValue(this BingCountry value)
        {
            string output = null;
            Type type = value.GetType();

            FieldInfo fi = type.GetRuntimeField(value.ToString());
            StringValueAttribute[] attrs = fi.GetCustomAttributes(typeof(StringValueAttribute), false) as StringValueAttribute[];
            if (attrs.Length > 0)
            {
                output = attrs[0].Value;
            }

            return output;
        }
    }

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
        public static string Truncate(this String value, int length)
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
        public static string Truncate(this String value, int length, bool ellipsis)
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
