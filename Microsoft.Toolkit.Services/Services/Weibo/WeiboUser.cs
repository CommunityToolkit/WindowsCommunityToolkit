// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo User type.
    /// </summary>
    public class WeiboUser
    {
        /// <summary>
        /// Gets or sets user Id.
        /// </summary>
        [JsonProperty("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user  name.
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets user screen name.
        /// </summary>
        [JsonProperty("screen_name")]
        public string ScreenName { get; set; }

        /// <summary>
        /// Gets or sets profile location.
        /// </summary>
        [JsonProperty("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets profile url.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets profile description.
        /// </summary>
        [JsonProperty("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets profile image url.
        /// </summary>
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        /// <summary>
        /// Gets or sets high resolution profile image url.
        /// </summary>
        [JsonProperty("avatar_large")]
        public string HighResolutionProfileImageUrl { get; set; }

        /// <summary>
        /// Gets or sets followers count.
        /// </summary>
        [JsonProperty("followers_count")]
        public int FollowersCount { get; set; }

        /// <summary>
        /// Gets or sets count of accounts user is following.
        /// </summary>
        [JsonProperty("friends_count")]
        public int FriendsCount { get; set; }

        /// <summary>
        /// Gets or sets total count of statuses user has liked.
        /// </summary>
        [JsonProperty("favourites_count")]
        public int FavoritesCount { get; set; }

        /// <summary>
        /// Gets or sets total count of Weibo statuses (including reposted statuses) posted by user.
        /// </summary>
        [JsonProperty("statuses_count")]
        public int StatusesCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is verified (blue checkmark).
        /// </summary>
        [JsonProperty("verified")]
        public bool Verified { get; set; }
    }
}