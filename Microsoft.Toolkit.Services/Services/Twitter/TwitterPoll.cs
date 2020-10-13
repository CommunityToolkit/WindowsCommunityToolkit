// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter poll object containing poll data.
    /// </summary>
    public class TwitterPoll
    {
        /// <summary>
        /// Gets or sets poll questions.
        /// </summary>
        [JsonPropertyName("options")]
        public TwitterPollOptions[] Options { get; set; }

        /// <summary>
        /// Gets or sets end timestamp as a string.
        /// </summary>
        [JsonPropertyName("end_datetime")]
        public string EndDateTime { get; set; }

        /// <summary>
        /// Gets or sets duration of the poll in minutes.
        /// </summary>
        [JsonPropertyName("duration_minutes")]
        public string DurationMinutes { get; set; }

        /// <summary>
        /// Gets end timestamp as a DateTime object.
        /// </summary>
        public DateTime PollEnd
        {
            get { return FormatDate(EndDateTime); }
        }

        private DateTime FormatDate(string input)
        {
            DateTime formattedDateTime;
            if (!DateTime.TryParseExact(input, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out formattedDateTime))
            {
                formattedDateTime = DateTime.Today;
            }

            return formattedDateTime;
        }
    }
}