// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
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
        [JsonProperty("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets item Id.
        /// </summary>
        [JsonProperty("id_str")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets text of the tweet (handles both 140 and 280 characters)
        /// </summary>
        [JsonProperty("text")]
        public string Text
        {
            get { return _text ?? FullText; }
            set { _text = value; }
        }

        /// <summary>
        /// Gets or sets text of the tweet (280 characters).
        /// </summary>
        [JsonProperty("full_text")]
        private string FullText { get; set; }

        /// <summary>
        /// Gets or sets display text range (indexes of tweet text without RT and leading user mentions)
        /// </summary>
        [JsonProperty("display_text_range")]
        public int[] DisplayTextRange { get; set; }

        /// <summary>
        /// Gets or sets truncated flag (true when tweet is longer than 140 characters)
        /// This entity may be deprecated - it never seems to be set to true.
        /// </summary>
        [JsonProperty("truncated")]
        public bool Truncated { get; set; }

        /// <summary>
        /// Gets or sets attached content of the tweet
        /// </summary>
        [JsonProperty("entities")]
        public TwitterEntities Entities { get; set; }

        /// <summary>
        /// Gets or sets tweet source (client or website used)
        /// </summary>
        [JsonProperty("source")]
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_screen_name
        /// </summary>
        [JsonProperty("in_reply_to_screen_name")]
        private string InReplyToScreenName { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_status_id_str
        /// </summary>
        [JsonProperty("in_reply_to_status_id_str")]
        private string InReplyToStatusId { get; set; }

        /// <summary>
        /// Gets or sets in_reply_to_user_id_str
        /// </summary>
        [JsonProperty("in_reply_to_user_id_str")]
        private string InReplyToUserId { get; set; }

        /// <summary>
        /// Gets or sets user who posted the status.
        /// </summary>
        [JsonProperty("user")]
        public TwitterUser User { get; set; }

        [JsonProperty("coordinates")]
        public TwitterCoordinates Coordinates { get; set; } 

        /// <summary>
        /// Gets or sets the Retweeted Tweet
        /// </summary>
        [JsonProperty("retweeted_status")]
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
        [JsonProperty("quoted_status")]
        private Tweet QuotedStatus { get; set; }

        /// <summary>
        /// Gets or sets quoted_status_id_str
        /// </summary>
        [JsonProperty("quoted_status_id_str")]
        private string QuotedStatusId { get; set; }

        /// <summary>
        /// Gets or sets quoted_status_permalink
        /// </summary>
        [JsonProperty("quoted_status_permalink")]
        private TwitterUrl QuotedStatusPermalink { get; set; }
    }
}