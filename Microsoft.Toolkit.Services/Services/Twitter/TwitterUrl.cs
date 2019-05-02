// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Url
    /// </summary>
    public class TwitterUrl
    {
        /// <summary>
        /// Gets or sets DisplayUrl of the Url.
        /// </summary>
        [JsonProperty("display_url")]
        public string DisplayUrl { get; set; }

        /// <summary>
        /// Gets or sets ExpandedUrl of the Url.
        /// </summary>
        [JsonProperty("expanded_url")]
        public string ExpandedUrl { get; set; }

        /// <summary>
        /// Gets or sets indices position of the tweet.
        /// </summary>
        [JsonProperty("indices")]
        public int[] Indices { get; set; }

        /// <summary>
        /// Gets or sets unwound Url metadata position of the tweet.
        /// </summary>
        [JsonProperty("unwound")]
        public TwitterUrlUnwound Unwound { get; set; }

        /// <summary>
        /// Gets or sets t.co Url of the tweet.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}