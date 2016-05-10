using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    internal static class HttpRequest
    {
        internal static async Task<HttpRequestResult> DownloadAsync(HttpRequestSettings settings)
        {
            var result = new HttpRequestResult();

            //var filter = new HttpBaseProtocolFilter();
            //filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            //var httpClient = new HttpClient(filter);
            var httpClient = new HttpClient();

            AddRequestHeaders(httpClient, settings);

            HttpResponseMessage response = await httpClient.GetAsync(settings.RequestedUri);
            result.StatusCode = response.StatusCode;
            FixInvalidCharset(response);
            result.Result = await response.Content.ReadAsStringAsync();

            return result;
        }

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
                    if (!String.IsNullOrEmpty(settings.Headers[customHeaderName]))
                    {
                        httpClient.DefaultRequestHeaders.Add(customHeaderName, settings.Headers[customHeaderName]);
                    }
                }
            }
        }

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
