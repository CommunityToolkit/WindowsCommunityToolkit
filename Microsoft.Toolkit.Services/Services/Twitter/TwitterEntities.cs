// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Entities containing Twitter entities object tweet
    /// </summary>
    public class TwitterEntities
    {
        /// <summary>
        /// Gets or sets Hashtags array of the tweet.
        /// This array will be empty if no Hashtags are present.
        /// </summary>
        [JsonProperty("Hashtags")]
        public TwitterHashtag[] Hashtags { get; set; }

        /// <summary>
        /// Gets or sets Symbols array of the tweet.
        /// This array will be empty if no Symbols are present.
        /// </summary>
        [JsonProperty("Symbols")]
        public TwitterSymbol[] Symbols { get; set; }

        /// <summary>
        /// Gets or sets Media array of the tweet.
        /// This array will not exist if no media is present.
        /// </summary>
        [JsonProperty("media")]
        public TwitterMedia[] Media { get; set; }

        /// <summary>
        /// Gets or sets Urls array of the tweet.
        /// This array will be empty if no Urls are present.
        /// </summary>
        [JsonProperty("urls")]
        public TwitterUrl[] Urls { get; set; }

        /// <summary>
        /// Gets or sets array of usernames mentioned in the tweet.
        /// This array will be empty if no usernames are mentioned.
        /// </summary>
        [JsonProperty("user_mentions")]
        public TwitterUserMention[] UserMentions { get; set; }

        /// <summary>
        /// Gets or sets the poll in a tweet.
        /// This array will not exist if no poll is present.
        /// This array will always have one poll.
        /// </summary>
        [JsonProperty("polls")]
        public TwitterPoll Poll { get; set; }
    }
}