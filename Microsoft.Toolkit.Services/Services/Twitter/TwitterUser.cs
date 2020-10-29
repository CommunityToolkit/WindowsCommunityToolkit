// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter User type.
    /// </summary>
    public class TwitterUser
    {
        /// <summary>
        /// Gets or sets user Id.
        /// </summary>
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets user name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets user screen name.
        /// </summary>
        [JsonPropertyName("screen_name")]
        public string ScreenName { get; set; }

        /// <summary>
        /// Gets or sets profile location.
        /// </summary>
        [JsonPropertyName("location")]
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets profile url.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets profile description.
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether protected status of user.
        /// </summary>
        [JsonPropertyName("protected")]
        public bool Protected { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether account is verified (blue check mark).
        /// </summary>
        [JsonPropertyName("verified")]
        public bool Verified { get; set; }

        /// <summary>
        /// Gets or sets followers count.
        /// </summary>
        [JsonPropertyName("followers_count")]
        public int FollowersCount { get; set; }

        /// <summary>
        /// Gets or sets count of accounts user is following.
        /// </summary>
        [JsonPropertyName("friends_count")]
        public int FriendsCount { get; set; }

        /// <summary>
        /// Gets or sets count of public lists user is a member of.
        /// </summary>
        [JsonPropertyName("listed_count")]
        public int ListedCount { get; set; }

        /// <summary>
        /// Gets or sets total count of tweets user has liked.
        /// </summary>
        [JsonPropertyName("favourites_count")]
        public int FavoritesCount { get; set; }

        /// <summary>
        /// Gets or sets total count of tweets (including retweets) posted by user.
        /// </summary>
        [JsonPropertyName("statuses_count")]
        public int StatusesCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether geotagging is enabled.
        /// This determines whether or not to geotag the user's posts.
        /// </summary>
        [JsonPropertyName("geo_enabled")]
        public bool GeoEnabled { get; set; }

        /// <summary>
        /// Gets or sets BCP 47 language code according to user's account settings.
        /// </summary>
        [JsonPropertyName("lang")]
        public string Lang { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether contributor mode is enabled.
        /// </summary>
        [JsonPropertyName("contributors_enabled")]
        public bool ContributorsEnabled { get; set; }

        /// <summary>
        /// Gets or sets profile background color (web hex value).
        /// </summary>
        [JsonPropertyName("profile_background_color")]
        public string ProfileBackgroundColor { get; set; }

        /// <summary>
        /// Gets or sets profile background image url.
        /// </summary>
        [JsonPropertyName("profile_background_image_url")]
        public string ProfileBackgroundImageUrl { get; set; }

        /// <summary>
        /// Gets or sets profile background image url using https.
        /// </summary>
        [JsonPropertyName("profile_background_image_url_https")]
        public string ProfileBackgroundImageUrlHttps { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether profile background image is tiled.
        /// </summary>
        [JsonPropertyName("profile_background_tile")]
        public bool ProfileBackgroundTile { get; set; }

        /// <summary>
        /// Gets or sets profile banner url.
        /// </summary>
        [JsonPropertyName("profile_banner_url")]
        public string ProfileBannerUrl { get; set; }

        /// <summary>
        /// Gets or sets profile image url.
        /// </summary>
        [JsonPropertyName("profile_image_url")]
        public string ProfileImageUrl { get; set; }

        /// <summary>
        /// Gets or sets profile image url using https.
        /// </summary>
        [JsonPropertyName("profile_image_url_https")]
        public string ProfileImageUrlHttps { get; set; }

        /// <summary>
        /// Gets or sets profile link color (web hex value).
        /// </summary>
        [JsonPropertyName("profile_link_color")]
        public string ProfileLinkColor { get; set; }

        /// <summary>
        /// Gets or sets profile sidebar border color (web hex value).
        /// </summary>
        [JsonPropertyName("profile_sidebar_border_color")]
        public string ProfileSidebarBorderColor { get; set; }

        /// <summary>
        /// Gets or sets profile sidebar fill color (web hex value).
        /// </summary>
        [JsonPropertyName("profile_sidebar_fill_color")]
        public string ProfileSidebarFillColor { get; set; }

        /// <summary>
        /// Gets or sets profile text color (web hex value).
        /// </summary>
        [JsonPropertyName("profile_text_color")]
        public string ProfileTextColor { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the user has selected to use their uploaded background image in their profile.
        /// </summary>
        [JsonPropertyName("profile_use_background_image")]
        public bool ProfileUseBackgroundImage { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not user is using the default profile theme and background.
        /// </summary>
        [JsonPropertyName("default_profile")]
        public bool DefaultProfile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the user is using the default profile image.
        /// </summary>
        [JsonPropertyName("default_profile_image")]
        public bool DefaultProfileImage { get; set; }

        /// <summary>
        /// Gets or sets "withheld in" countries.
        /// </summary>
        [JsonPropertyName("withheld_in_countries")]
        public string[] WithheldInCountries { get; set; }

        /// <summary>
        /// Gets or sets withheld scope (status or profile).
        /// </summary>
        [JsonPropertyName("withheld_scope")]
        public string WithheldScope { get; set; }
    }
}
