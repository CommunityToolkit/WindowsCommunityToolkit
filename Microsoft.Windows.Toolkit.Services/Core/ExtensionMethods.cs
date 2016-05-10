// ***********************************************************************
// <copyright file="ExtensionMethods.cs" company="Microsoft">
//     Copyright (c) 2015 Microsoft. All rights reserved.
// </copyright>
// <summary></summary>
// ***********************************************************************

namespace Microsoft.Windows.Toolkit.Services.Core
{
    using System;
    using System.Net;
    using System.Text.RegularExpressions;

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

    }

}
