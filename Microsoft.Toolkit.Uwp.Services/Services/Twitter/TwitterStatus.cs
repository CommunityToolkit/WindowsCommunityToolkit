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
        /// Gets or sets the latitude of the "tweet" message.
        /// NOTE: This parameter will be ignored unless it is inside the range -90.0 to +90.0 (North is positive) inclusive.
        /// It will also be ignored if there isn’t a corresponding long parameter.
        /// </summary>
        public decimal? Latitude { get; set; }

        /// <summary>
        /// Gets or sets the longitude of the "tweet" message.
        /// NOTE: The valid ranges for longitude is -180.0 to +180.0 (East is positive) inclusive.
        /// This parameter will be ignored if outside that range, if it is not a number, if geo_enabled is disabled,
        /// or if there not a corresponding lat parameter.
        /// </summary>
        public decimal? Longitude { get; set; }

        /// <summary>
        /// Gets or sets the text of the "tweet" message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets a the Request parameters
        /// </summary>
        public string RequestParameters
        {
            get
            {
                string result = $"status={Uri.EscapeDataString(Message)}";

                if (Latitude.HasValue && Longitude.HasValue)
                {
                    result = $"{result}&lat={Latitude.Value}&long={Longitude.Value}";
                }

                if (DisplayCoordinates)
                {
                    result = $"{result}&display_coordinates=true";
                }

                return result;
            }
        }
    }
}