using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Vision request exception details
    /// </summary>
    public class RequestExceptionDetails
    {
        /// <summary>
        /// Gets or sets request exception code
        /// </summary>
        [JsonProperty("code")]
        public string Code { get; set; }

        /// <summary>
        /// Gets or sets request exception id
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets request exception message
        /// </summary>
        [JsonProperty("message")]
        public string Message { get; set; }
    }
}
