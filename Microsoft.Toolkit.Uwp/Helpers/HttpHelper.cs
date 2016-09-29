using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Windows.Web.Http;
using Windows.Web.Http.Filters;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// This class provides static helper methods for streams.
    /// </summary>
    public class HttpHelper
    {
        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static HttpHelper _instance;

        /// <summary>
        /// Private instance field.
        /// </summary>
        private HttpClient httpClient = null;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static HttpHelper Instance => _instance ?? (_instance = new HttpHelper());

        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelper"/> class.
        /// </summary>
        protected HttpHelper()
        {
            var filter = new HttpBaseProtocolFilter();
            filter.CacheControl.ReadBehavior = HttpCacheReadBehavior.MostRecent;

            httpClient = new HttpClient(filter);
        }

        /// <summary>
        /// Process Http Request using instance of HttpClient.
        /// </summary>
        /// <param name="request">instance of <see cref="HttpHelperRequest"/></param>
        /// <returns>Instane of <see cref="HttpHelperResponse"/></returns>
        public async Task<HttpHelperResponse> SendRequestAsync(HttpHelperRequest request)
        {
            var httpRequestMessage = request.ToHttpRequestMessage();
            using (var response = await httpClient.SendRequestAsync(httpRequestMessage))
            {
                FixInvalidCharset(response);

                return new HttpHelperResponse()
                {
                    StatusCode = response.StatusCode,
                    Result = response.Content
                };
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
