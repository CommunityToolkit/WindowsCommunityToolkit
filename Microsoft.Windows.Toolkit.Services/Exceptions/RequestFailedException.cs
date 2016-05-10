using System;
using System.Net;

namespace Microsoft.Windows.Toolkit.Services.Exceptions
{
    public class RequestFailedException : Exception
    {
        public RequestFailedException()
        {
        }

        public RequestFailedException(string message)
            : base(message)
        {
        }

        [CLSCompliant(false)]
        public RequestFailedException(HttpStatusCode statusCode, string reason)
            : base(string.Format("Request failed with status code {0} and reason '{1}'", (int)statusCode, reason))
        {
        }

        public RequestFailedException(string message, Exception innerException)
            : base(message, innerException)
        {
        }
    }
}
