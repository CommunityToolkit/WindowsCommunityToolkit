using System;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Vision Service Exception for handling request errors
    /// </summary>
    public class VisionServiceException : Exception
    {
        /// <summary>
        /// Gets Error details
        /// </summary>
        public RequestExceptionDetails Details { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VisionServiceException"/> class.
        /// </summary>
        /// <param name="message">Exception message</param>
        /// <param name="details">Request Exception details</param>
        public VisionServiceException(string message, RequestExceptionDetails details)
            : base(message)
        {
            Details = details;
        }
    }
}
