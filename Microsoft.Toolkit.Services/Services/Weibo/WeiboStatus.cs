// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Text.Json.Serialization;
using Microsoft.Toolkit.Services.Services.Weibo;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo Timeline item.
    /// </summary>
    public class WeiboStatus : Toolkit.Parsers.SchemaBase
    {
        /// <summary>
        /// Gets or sets time item was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets the creation date
        /// </summary>
        public DateTime CreationDate
        {
            get
            {
                DateTime dt;
                if (!DateTime.TryParseExact(CreatedAt, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
                {
                    dt = DateTime.Today;
                }

                return dt;
            }
        }

        /// <summary>
        /// Gets or sets item Id.
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets text of the status (handles both 140 and 280 characters)
        /// </summary>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether status is truncated
        /// (true when Weibo status is longer than 140 characters)
        /// This entity may be deprecated - it never seems to be set to true.
        /// </summary>
        [JsonPropertyName("truncated")]
        public bool IsTruncated { get; set; }

        /// <summary>
        /// Gets or sets status source (client or website used)
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_screen_name
        /// </summary>
        [JsonPropertyName("in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_status_id
        /// </summary>
        [JsonPropertyName("in_reply_to_status_id")]
        public string InReplyToStatusId { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_user_id
        /// </summary>
        [JsonPropertyName("in_reply_to_user_id")]
        public string InReplyToUserId { get; set; }

        /// <summary>
        /// Gets or sets user who posted the status.
        /// </summary>
        [JsonPropertyName("user")]
        public WeiboUser User { get; set; }

        /// <summary>
        /// Gets or sets the Reposted Weibo status
        /// </summary>
        [JsonPropertyName("retweeted_status")]
        public WeiboStatus RepostedStatus { get; set; }

        /// <summary>
        /// Gets or sets the repost count
        /// </summary>
        [JsonPropertyName("reposts_count")]
        public int RepostCount { get; set; }

        /// <summary>
        /// Gets or sets the comment count
        /// </summary>
        [JsonPropertyName("comments_count")]
        public int CommentCount { get; set; }

        /// <summary>
        /// Gets or sets the url of the attached image in thumbnail size.
        /// </summary>
        [JsonPropertyName("thumbnail_pic")]
        public string ThumbnailImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the url of the attached image in medium size.
        /// </summary>
        [JsonPropertyName("bmiddle_pic")]
        public string MediumImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the url of the attached image in original size.
        /// </summary>
        [JsonPropertyName("original_pic")]
        public string OriginalImageUrl { get; set; }

        /// <summary>
        /// Gets or sets attached images array of the weibo.
        /// </summary>
        [JsonPropertyName("pic_urls")]
        public WeiboImage[] AttachedImages { get; set; }

        /// <summary>
        /// Gets or sets the geographic information.
        /// </summary>
        [JsonPropertyName("geo")]
        public WeiboGeoInfo GeographicInfo { get; set; }

        /// <summary>
        /// Gets a value indicating whether the weibo status includes attached images.
        /// </summary>
        public bool HasAttachedImage => AttachedImages != null && AttachedImages.Length > 0;
    }
}
