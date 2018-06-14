// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Entities containing Media and Urls of a tweet
    /// </summary>
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