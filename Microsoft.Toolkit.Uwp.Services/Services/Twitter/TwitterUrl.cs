using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    public class TwitterUrl
    {
        /// <summary>
        /// Gets or sets Url of the tweet.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets indices position of the tweet.
        /// </summary>
        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}