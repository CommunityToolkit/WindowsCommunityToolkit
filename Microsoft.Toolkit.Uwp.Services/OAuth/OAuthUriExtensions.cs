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
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Windows.Foundation;

namespace Microsoft.Toolkit.Uwp.Services.OAuth
{
    /// <summary>
    /// OAuth Uri extensions.
    /// </summary>
    internal static class OAuthUriExtensions
    {
        /// <summary>
        /// Get query parameters from Uri.
        /// </summary>
        /// <param name="uri">Uri to process.</param>
        /// <returns>Dictionary of query parameters.</returns>
        public static IDictionary<string, string> GetQueryParams(this Uri uri)
        {
            return new WwwFormUrlDecoder(uri.Query).ToDictionary(decoderEntry => decoderEntry.Name, decoderEntry => decoderEntry.Value);
        }

        /// <summary>
        /// Get absolute Uri.
        /// </summary>
        /// <param name="uri">Uri to process.</param>
        /// <returns>Uri without query string.</returns>
        public static string AbsoluteWithoutQuery(this Uri uri)
        {
            if (string.IsNullOrEmpty(uri.Query))
            {
                return uri.AbsoluteUri;
            }

            return uri.AbsoluteUri.Replace(uri.Query, string.Empty);
        }

        /// <summary>
        /// Normalize the Uri into string.
        /// </summary>
        /// <param name="uri">Uri to process.</param>
        /// <returns>Normalized string.</returns>
        public static string Normalize(this Uri uri)
        {
            var result = new StringBuilder(string.Format(CultureInfo.InvariantCulture, "{0}://{1}", uri.Scheme, uri.Host));
            if (!((uri.Scheme == "http" && uri.Port == 80) || (uri.Scheme == "https" && uri.Port == 443)))
            {
                result.Append(string.Concat(":", uri.Port));
            }

            result.Append(uri.AbsolutePath);

            return result.ToString();
        }
    }
}
