// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Microsoft.Toolkit.Services.OAuth
{
    /// <summary>
    /// OAuth Encoder.
    /// </summary>
    internal static class OAuthEncoder
    {
        /// <summary>
        /// Url encode input string.
        /// </summary>
        /// <param name="value">Input string.</param>
        /// <returns>Encoded string.</returns>
        public static string UrlEncode(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return string.Empty;
            }

            var result = Uri.EscapeDataString(value);

            // UrlEncode escapes with lowercase characters (e.g. %2f) but oAuth needs %2F
            result = Regex.Replace(result, "(%[0-9a-f][0-9a-f])", c => c.Value.ToUpper());

            // these characters are not escaped by UrlEncode() but needed to be escaped
            result = result
                        .Replace("(", "%28")
                        .Replace(")", "%29")
                        .Replace("$", "%24")
                        .Replace("!", "%21")
                        .Replace("*", "%2A")
                        .Replace("'", "%27");

            // these characters are escaped by UrlEncode() but will fail if unescaped!
            result = result.Replace("%7E", "~");

            return result;
        }

        /// <summary>
        /// Encode list of parameters.
        /// </summary>
        /// <param name="parameters">List of parameters.</param>
        /// <returns>Encoded string of parameters.</returns>
        public static string UrlEncode(IEnumerable<OAuthParameter> parameters)
        {
            string rawUrl = string.Join("&", parameters.OrderBy(p => p.Key).Select(p => p.ToString()).ToArray());
            return UrlEncode(rawUrl);
        }

    }
}
