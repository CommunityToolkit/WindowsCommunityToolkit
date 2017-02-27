// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Globalization;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
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
        [JsonProperty(PropertyName = "id")]
        public decimal Id { get; set; }

        /// <summary>
        /// Gets or sets the sender id.
        /// </summary>
        /// <value>The sender id.</value>
        [JsonProperty(PropertyName = "sender_id")]
        public decimal SenderId { get; set; }

        /// <summary>
        /// Gets or sets the direct message text.
        /// </summary>
        /// <value>The direct message text.</value>
        [JsonProperty(PropertyName = "text")]
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the recipient id.
        /// </summary>
        /// <value>The recipient id.</value>
        [JsonProperty(PropertyName = "recipient_id")]
        public decimal RecipientId { get; set; }

        /// <summary>
        /// Gets or sets the created date.
        /// </summary>
        /// <value>The created date.</value>
        [JsonProperty(PropertyName = "created_at")]
        public string CreatedAt { get; set; }

        /// <summary>
        /// Gets or sets the name of the sender screen.
        /// </summary>
        /// <value>The name of the sender screen.</value>
        [JsonProperty(PropertyName = "sender_screen_name")]
        public string SenderScreenName { get; set; }

        /// <summary>
        /// Gets or sets the name of the recipient screen.
        /// </summary>
        /// <value>The name of the recipient screen.</value>
        [JsonProperty(PropertyName = "recipient_screen_name")]
        public string RecipientScreenName { get; set; }

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>The sender.</value>
        [JsonProperty(PropertyName = "sender")]
        public TwitterUser Sender { get; set; }

        /// <summary>
        /// Gets or sets the recipient.
        /// </summary>
        /// <value>The recipient.</value>
        [JsonProperty(PropertyName = "recipient")]
        public TwitterUser Recipient { get; set; }

        /// <summary>
        /// Gets or sets the entities.
        /// </summary>
        /// <value>The entities.</value>
        [JsonProperty(PropertyName = "entities")]
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
