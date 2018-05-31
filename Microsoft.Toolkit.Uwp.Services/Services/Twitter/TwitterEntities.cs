// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Entities containing Twitter entities object tweet
    /// </summary>
    public class TwitterEntities
    {
        /// <summary>
        /// Gets or sets Hashtags array of the tweet.
        /// </summary>
        [JsonProperty("Hashtags")]
        public TwitterHashtag[] Hashtags { get; set; }

        /// <summary>
        /// Gets or sets Media array of the tweet.
        /// </summary>
        [JsonProperty("media")]
        public TwitterMedia[] Media { get; set; }

        /// <summary>
        /// Gets or sets Urls array of the tweet.
        /// </summary>
        [JsonProperty("urls")]
        public TwitterUrl[] Urls { get; set; }
    }
}