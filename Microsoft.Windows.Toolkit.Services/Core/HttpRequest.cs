// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************
using System;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Microsoft.Toolkit.Uwp.Services.Core
{
    /// <summary>
    /// Core HttpRequest class.
    /// </summary>
    internal static class HttpRequest
    {
        /// <summary>
        /// Downloads data with specified settings.
        /// </summary>
        /// <param name="settings">HttpRequestSettings instance.</param>
        /// <returns>Returns HttpRequestResult instance.</returns>
        internal static async Task<HttpRequestResult> DownloadAsync(HttpRequestSettings settings)
        {
            var result = new HttpRequestResult();

            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            var httpClient = new HttpClient(filter);

            AddRequestHeaders(httpClient, settings);

            HttpResponseMessage response = await httpClient.GetAsync(settings.RequestedUri);
            result.StatusCode = response.StatusCode;
            FixInvalidCharset(response);
            result.Result = await response.Content.ReadAsStringAsync();

            return result;
        }

        /// <summary>
        /// Add default request headers to HttpClient object.
        /// </summary>
        /// <param name="httpClient">HttpClient instance.</param>
        /// <param name="settings">HttpRequestSettings instance.</param>
        private static void AddRequestHeaders(HttpClient httpClient, HttpRequestSettings settings)
        {
            if (!string.IsNullOrEmpty(settings.UserAgent))
            {
                httpClient.DefaultRequestHeaders.UserAgent.ParseAdd(settings.UserAgent);
            }

            if (settings.Headers != null)
            {
                foreach (var customHeaderName in settings.Headers.AllKeys)
                {
                    if (!string.IsNullOrEmpty(settings.Headers[customHeaderName]))
                    {
                        httpClient.DefaultRequestHeaders.Add(customHeaderName, settings.Headers[customHeaderName]);
                    }
                }
            }
        }

        /// <summary>
        /// Fix invalid charset returned by some web sites.
        /// </summary>
        /// <param name="response">HttpResponseMessage instance.</param>
        private static void FixInvalidCharset(HttpResponseMessage response)
        {
            if (response != null && response.Content != null && response.Content.Headers != null
                && response.Content.Headers.ContentType != null && response.Content.Headers.ContentType.CharSet != null)
            {
                // Fix invalid charset returned by some web sites.
                string charset = response.Content.Headers.ContentType.CharSet;
                if (charset.Contains("\""))
                {
                    response.Content.Headers.ContentType.CharSet = charset.Replace("\"", string.Empty);
                }
            }
        }
    }
}
