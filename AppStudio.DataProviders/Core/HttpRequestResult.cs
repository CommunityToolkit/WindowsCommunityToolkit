using System.Net;

namespace AppStudio.DataProviders.Core
{
    internal class HttpRequestResult
    {
        public HttpRequestResult()
        {
            this.StatusCode = HttpStatusCode.OK;
            this.Result = string.Empty;
        }

        public HttpStatusCode StatusCode { get; set; }

        public string Result { get; set; }

        public bool Success { get { return (this.StatusCode == HttpStatusCode.OK && !string.IsNullOrEmpty(this.Result)); } }
    }
}
