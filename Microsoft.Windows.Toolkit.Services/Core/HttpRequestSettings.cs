using System;
using System.Net;

namespace Microsoft.Windows.Toolkit.Services.Core
{
    internal class HttpRequestSettings
    {
        public HttpRequestSettings()
        {
            this.Headers = new WebHeaderCollection();
        }

        public Uri RequestedUri { get; set; }

        public string UserAgent { get; set; }

        public WebHeaderCollection Headers { get; private set; }
    }
}
