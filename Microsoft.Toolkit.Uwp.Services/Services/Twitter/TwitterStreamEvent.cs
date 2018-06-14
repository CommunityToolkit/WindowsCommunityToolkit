// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter User type.
    /// </summary>
    public class TwitterStreamEvent : ITwitterResult
    {
        /// <summary>
        /// Gets or sets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        [JsonProperty(PropertyName = "event")]
        [JsonConverter(typeof(StringEnumConverter))]
        public TwitterStreamEventType EventType { get; set; }

        /// <summary>
        /// Gets or sets the source of the event. This is always the user who initiated the event.
        /// </summary>
        /// <value>The source.</value>
        public TwitterUser Source { get; set; }

        /// <summary>
        /// Gets or sets the target of the event. This is the user who was affected, or who owns the affected object.
        /// </summary>
        /// <value>The source.</value>
        public TwitterUser Target { get; set; }

        /// <summary>
        /// Gets or sets the target object.
        /// </summary>
        /// <value>The target object.</value>
        public Tweet TargetObject { get; set; }

        /// <summary>
        /// Gets or sets the creation date.
        /// </summary>
        /// <value>The creation date.</value>
        [JsonProperty(PropertyName = "created_at")]
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
    }
}
