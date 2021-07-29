// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Media
    /// </summary>
    public class TwitterMedia
    {
        /// <summary>
        /// Gets or sets ID as string.
        /// </summary>
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets indices array.
        /// </summary>
        [JsonPropertyName("indices")]
        public int[] Indices { get; set; }

        /// <summary>
        /// Gets or sets MediaUrl (direct link to image).
        /// </summary>
        [JsonPropertyName("media_url")]
        public string MediaUrl { get; set; }

        /// <summary>
        /// Gets or sets HTTPS MediaUrl.
        /// </summary>
        [JsonPropertyName("media_url_https")]
        public string MediaUrlHttps { get; set; }

        /// <summary>
        /// Gets or sets t.co shortened tweet Url.
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets DisplayUrl (pics.twitter.com Url).
        /// </summary>
        [JsonPropertyName("display_url")]
        public string DisplayUrl { get; set; }

        /// <summary>
        /// Gets or sets DisplayUrl (pics.twitter.com Url).
        /// </summary>
        [JsonPropertyName("expanded_url")]
        public string ExpandedUrl { get; set; }

        /// <summary>
        /// Gets or sets MediaType - photo, animated_gif, or video
        /// </summary>
        [JsonPropertyName("type")]
        public string MediaType { get; set; }

        /// <summary>
        /// Gets or sets size array
        /// </summary>
        [JsonPropertyName("sizes")]
        public TwitterMediaSizes Sizes { get; set; }

        /// <summary>
        /// Gets or sets the SourceId - tweet ID of media's original tweet
        /// </summary>
        [JsonPropertyName("source_status_id_str")]
        public string SourceIdStr { get; set; }

        /// <summary>
        /// Gets or sets metadata for video attached to tweet
        /// </summary>
        [JsonPropertyName("video_info")]
        public TwitterMediaVideoInfo VideoInfo { get; set; }

        /// <summary>
        /// Gets or sets extended metadata for video attached to tweet.
        /// </summary>
        [JsonPropertyName("additional_media_info")]
        public TwitterMediaAdditionalInfo AdditionalMediaInfo { get; set; }
    }
}