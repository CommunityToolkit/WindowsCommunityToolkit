using System;
using Windows.Web.Http;

namespace Microsoft.Toolkit.Uwp
{
    /// <summary>
    /// HttpHelperResponse instance to hold data from Http Response.
    /// </summary>
    public class HttpHelperResponse : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="HttpHelperResponse"/> class.
        /// Default constructor.
        /// </summary>
        public HttpHelperResponse()
        {
            StatusCode = HttpStatusCode.Ok;
            Result = null;
        }

        /// <summary>
        /// Gets or sets holds request StatusCode.
        /// </summary>
        public HttpStatusCode StatusCode { get; set; }

        /// <summary>
        /// Gets or sets holds request Result.
        /// </summary>
        public IHttpContent Result { get; set; }

        /// <summary>
        /// Gets a value indicating whether holds request Success boolean.
        /// </summary>
        public bool Success => StatusCode == HttpStatusCode.Ok;

        /// <summary>
        /// Dispose underlying content
        /// </summary>
        public void Dispose()
        {
            Result.Dispose();
        }
    }
}
