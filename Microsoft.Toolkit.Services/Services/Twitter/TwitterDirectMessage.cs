// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter User type.
    /// </summary>
    public class TwitterDirectMessage : ITwitterResult
    {
        /// <summary>
        /// Gets or sets the direct message id.
        /// </summary>
        /// <value>The direct message id.</value>
        [JsonPropertyName("id")]
        public decimal Id { get; set; }

        /// <summary>
        /// Gets or sets the sender id.
        /// </summary>
        /// <value>The sender id.</value>
        [JsonPropertyName("sender_id")]
        public decimal SenderId { get; set; }

        /// <summary>
        /// Gets or sets the direct message text.
        /// </summary>
        /// <value>The direct message text.</value>
        [JsonPropertyName("text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the recipient id.
        /// </summary>
        /// <value>The recipient id.</value>
        [JsonPropertyName("recipient_id")]
        public decimal RecipientId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        [JsonPropertyName("created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender screen.
        /// </summary>
        /// <value>The name of the sender screen.</value>
        [JsonPropertyName("sender_screen_name")]
        public string SenderScreenName { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipient screen.
        /// </summary>
        /// <value>The name of the recipient screen.</value>
        [JsonPropertyName("recipient_screen_name")]
        public string RecipientScreenName { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        [JsonPropertyName("sender")]
        public TwitterUser Sender { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [JsonPropertyName("recipient")]
        public TwitterUser Recipient { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        [JsonPropertyName("entities")]
        public TwitterEntities Entities { get; set; }

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
    }
}
