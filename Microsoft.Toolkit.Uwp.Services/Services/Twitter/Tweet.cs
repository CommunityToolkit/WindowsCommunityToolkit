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
        /// Gets or sets the geographic data (latitude and longitude)
        /// </summary>
        [JsonProperty("geo")]
        public TwitterGeoData GeoData { get; set; }

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
        /// Gets or sets the extended mode.
        /// </summary>
        [JsonProperty("extended_tweet")]
        public TwitterExtended Extended { get; set; }

        /// <summary>
        /// Gets or sets user who posted the status.
        /// </summary>
        [JsonProperty("user")]
        public TwitterUser User { get; set; }

        /// <summary>
        /// Gets or sets attached content of the tweet
        /// </summary>
        [JsonProperty("entities")]
        public TwitterEntities Entities { get; set; }

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
        /// Gets or sets text of the tweet (280 characters).
        /// </summary>
        [JsonProperty("full_text")]
        private string FullText { get; set; }
    }
}