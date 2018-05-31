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
        /// This array always exists, even if it's empty (no hashtags).
        /// </summary>
        [JsonProperty("hashtags")]
        public TwitterHashtag[] Hashtags { get; set; }

        /// <summary>
        /// Gets or sets Media array of the tweet.
        /// This array will not exist if no media is attached (GIF, photo, or video).
        /// </summary>
        [JsonProperty("media")]
        public TwitterMedia[] Media { get; set; }

        /// <summary>
        /// Gets or sets Urls array of the tweet.
        /// This array will always exist, even if it's empty (no URLs).
        /// </summary>
        [JsonProperty("urls")]
        public TwitterUrl[] Urls { get; set; }
    }
}