using System.Net;

namespace Microsoft.Windows.Toolkit.Services.Core
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
