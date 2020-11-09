// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Timeline item.
    /// </summary>
    public class Tweet : Toolkit.Parsers.SchemaBase, ITwitterResult
    {
        private string _text;

        /// <summary>
        /// Gets or sets time item was created.
        /// </summary>
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets item Id.
        /// </summary>
        [JsonPropertyName("id_str")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets text of the tweet (handles both 140 and 280 characters)
        /// </summary>
        [JsonPropertyName("text")]
        public string Text
        {
            get { return _text ?? FullText; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets text of the tweet (280 characters).
        /// </summary>
        [JsonPropertyName("full_text")]
        private string FullText { get; set; }

        /// <summary>
        /// Gets or sets display text range (indexes of tweet text without RT and leading user mentions)
        /// </summary>
        [JsonPropertyName("display_text_range")]
        public int[] DisplayTextRange { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether tweet is truncated
        /// (true when tweet is longer than 140 characters)
        /// This entity may be deprecated - it never seems to be set to true.
        /// </summary>
        [JsonPropertyName("truncated")]
        public bool IsTruncated { get; set; }

        /// <summary>
        /// Gets or sets attached content of the tweet
        /// </summary>
        [JsonPropertyName("entities")]
        public TwitterEntities Entities { get; set; }

        /// <summary>
        /// Gets or sets extended attached content of the tweet
        /// </summary>
        [JsonPropertyName("extended_entities")]
        public TwitterExtendedEntities ExtendedEntities { get; set; }

        /// <summary>
        /// Gets or sets tweet source (client or website used)
        /// </summary>
        [JsonPropertyName("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_screen_name
        /// </summary>
        [JsonPropertyName("in_reply_to_screen_name")]
        public string InReplyToScreenName { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_status_id_str
        /// </summary>
        [JsonPropertyName("in_reply_to_status_id_str")]
        public string InReplyToStatusId { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_user_id_str
        /// </summary>
        [JsonPropertyName("in_reply_to_user_id_str")]
        public string InReplyToUserId { get; set; }

        /// <summary>
        /// Gets or sets user who posted the status.
        /// </summary>
        [JsonPropertyName("user")]
        public TwitterUser User { get; set; }

        /// <summary>
        /// Gets or sets geo coordinates (latitude and longitude) returned by Twitter for some locations
        /// </summary>
        [JsonPropertyName("coordinates")]
        [JsonConverter(typeof(TwitterCoordinatesConverter))]
        public TwitterCoordinates Coordinates { get; set; }

        /// <summary>
        /// Gets or sets the Place object returned by Twitter for some locations
        /// </summary>
        [JsonPropertyName("place")]
        public TwitterPlace Place { get; set; }

        /// <summary>
        /// Gets or sets the Retweeted Tweet
        /// </summary>
        [JsonPropertyName("retweeted_status")]
        public Tweet RetweetedStatus { get; set; }

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
        /// Gets or sets quoted_status
        /// </summary>
        [JsonPropertyName("quoted_status")]
        public Tweet QuotedStatus { get; set; }

        /// <summary>
        /// Gets or sets quoted_status_id_str
        /// </summary>
        [JsonPropertyName("quoted_status_id_str")]
        public string QuotedStatusId { get; set; }

        /// <summary>
        /// Gets or sets quoted_status_permalink
        /// </summary>
        [JsonPropertyName("quoted_status_permalink")]
        public TwitterUrl QuotedStatusPermalink { get; set; }

        /// <summary>
        /// Gets or sets approximate count of tweets quoting tweet
        /// </summary>
        [JsonPropertyName("quote_count")]
        public int QuoteCount { get; set; }

        /// <summary>
        /// Gets or sets number of replies to tweet
        /// </summary>
        /// <remarks>
        /// Premium and Enterprise API access only
        /// </remarks>
        [JsonPropertyName("reply_count")]
        public int ReplyCount { get; set; }

        /// <summary>
        /// Gets or sets number of times tweet has been retweeted
        /// </summary>
        [JsonPropertyName("retweet_count")]
        public int RetweetCount { get; set; }

        /// <summary>
        /// Gets or sets number of times tweet has been liked
        /// </summary>
        [JsonPropertyName("favorite_count")]
        public int FavoriteCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not logged-in user has liked tweet
        /// </summary>
        [JsonPropertyName("favorited")]
        public bool Favorited { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not logged-in user has retweeted tweet
        /// </summary>
        [JsonPropertyName("retweeted")]
        public bool Retweeted { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether URL in tweet has been flagged for sensitive content
        /// </summary>
        [JsonPropertyName("possibly_sensitive")]
        public bool Sensitive { get; set; }

        /// <summary>
        /// Gets or sets stream filter of tweet
        /// </summary>
        [JsonPropertyName("filter_level")]
        public string FilterLevel { get; set; }

        /// <summary>
        /// Gets or sets BCP 47 language identifier of tweet content
        /// </summary>
        [JsonPropertyName("lang")]
        public string Language { get; set; }
    }
}