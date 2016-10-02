using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// represents Describe Image request result
    /// </summary>
    public class ImageDescription
    {
        /// <summary>
        /// Gets or sets Image Description
        /// </summary>
        [JsonProperty("description")]
        public Description Description { get; set; }

        /// <summary>
        /// Gets or sets Request id
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets Image Metadata
        /// </summary>
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }
    }
}
