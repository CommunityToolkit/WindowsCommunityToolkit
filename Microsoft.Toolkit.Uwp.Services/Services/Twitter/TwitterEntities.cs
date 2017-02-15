using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    public class TwitterEntities
    {
        /// <summary>
        /// Gets or sets Media of the tweet.
        /// </summary>
        [JsonProperty("media")]
        public TwitterMedia[] Media { get; set; }

        /// <summary>
        /// Gets or sets Urls of the tweet.
        /// </summary>
        [JsonProperty("urls")]
        public TwitterUrl[] Urls { get; set; }
    }
}