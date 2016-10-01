using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.CognitiveServices
{
    /// <summary>
    /// Represets the result of Vision Service tag image request
    /// </summary>
    public class ImageTags
    {
        /// <summary>
        /// Gets or sets image tags
        /// </summary>
        [JsonProperty("tags")]
        public Tag[] Tags { get; set; }

        /// <summary>
        /// Gets or sets request id
        /// </summary>
        [JsonProperty("requestId")]
        public string RequestId { get; set; }

        /// <summary>
        /// Gets or sets request meta data
        /// </summary>
        [JsonProperty("metadata")]
        public Metadata Metadata { get; set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return VisionServiceJsonHelper.JsonSerialize(this);
        }
    }

    /// <summary>
    /// Image tag request meta data
    /// </summary>
    public class Metadata
    {
        /// <summary>
        /// Gets or sets image width
        /// </summary>
        [JsonProperty("width")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets image height
        /// </summary>
        [JsonProperty("height")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets image format
        /// </summary>
        [JsonProperty("format")]
        public string Format { get; set; }
    }

    /// <summary>
    /// Represents image tag
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// Gets or sets tag name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets tag confidence
        /// </summary>
        [JsonProperty("confidence")]
        public float Confidence { get; set; }
    }
}
