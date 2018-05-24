// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// A class for Twitter status other than the pictures
    /// NOTE: Can be extended to handle the other pieces of the Twitter Status REST API.
    /// https://dev.twitter.com/rest/reference/post/statuses/update
    /// Validation COULD be added to the Lat/Long, but since Twitter ignores these if they are invalid then no big deal.
    /// </summary>
    public class TwitterStatus
    {
        /// <summary>
        /// Gets or sets a value indicating whether the explicit latitude and longitude of the "tweet" message is displayed.
        /// NOTE: Whether or not to put a pin on the exact coordinates a Tweet has been sent from.
        /// </summary>
        public bool DisplayCoordinates { get; set; }

        /// <summary>
        /// Gets or sets the ID of the original tweet.
        /// </summary>
        public string InReplyToStatusId { get; set; }

        /// <summary>
        /// Gets or sets the latitude of the "tweet" message.
        /// NOTE: This parameter will be ignored unless it is inside the range -90.0 to +90.0 (North is positive) inclusive.
        /// It will also be ignored if there isn’t a corresponding long parameter.
        /// </summary>
        public double? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the "tweet" message.
        /// NOTE: The valid ranges for longitude is -180.0 to +180.0 (East is positive) inclusive.
        /// This parameter will be ignored if outside that range, if it is not a number, if geo_enabled is disabled,
        /// or if there not a corresponding lat parameter.
        /// </summary>
        public double? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the text of the Tweet message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the text of the Tweet message.
        /// </summary>
        public string PlaceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Tweet contains sensitive content (such as nudity, etc.).
        /// </summary>
        public bool PossiblySensitive { get; set; }

        /// <summary>
        /// Gets the request parameters
        /// </summary>
        public string RequestParameters
        {
            get
            {
                string result = $"status={Uri.EscapeDataString(Message)}";

                if (Latitude.HasValue && Longitude.HasValue)
                {
                    result = $"{result}&lat={Latitude.Value.ToString(CultureInfo.InvariantCulture)}&long={Longitude.Value.ToString(CultureInfo.InvariantCulture)}";
                    result = AddRequestParameter(result, "display_coordinates", DisplayCoordinates);
                }

                result = AddRequestParameter(result, "in_reply_to_status_id", InReplyToStatusId);
                result = AddRequestParameter(result, "place_id", PlaceId);
                result = AddRequestParameter(result, "possibly_sensitive", PossiblySensitive);
                result = AddRequestParameter(result, "trim_user", TrimUser);

                return result;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the Tweet returned in a timeline will include a user object including only the status authors numerical ID.
        /// </summary>
        public bool TrimUser { get; set; }

        private string AddRequestParameter(string request, string parameterName, bool value)
        {
            var result = request;

            if (value)
            {
                result = $"{result}&{parameterName}=true";
            }

            return result;
        }

        private string AddRequestParameter(string request, string parameterName, string value)
        {
            var result = request;

            if (!string.IsNullOrEmpty(value))
            {
                result = $"{result}&{parameterName}={Uri.EscapeDataString(value)}";
            }

            return result;
        }
    }
}