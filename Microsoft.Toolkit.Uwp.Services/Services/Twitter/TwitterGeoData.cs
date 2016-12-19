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

using System.Globalization;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// A class to contain the latitude and longitude of a tweet.
    /// </summary>
    public class TwitterGeoData
    {
        private const int LatitudeIndex = 0;
        private const int LongitudeIndex = 1;
        private const string PointType = "Point";

        /// <summary>
        /// Gets or sets the type of data
        /// </summary>
        [JsonProperty("type")]
        public string DataType { get; set; }

        /// <summary>
        /// Gets the latitude and longitude in a coordinate format.
        /// </summary>
        public string DisplayCoordinates
        {
            get
            {
                string result = null;

                if (Coordinates != null)
                {
                    result = $"({Coordinates[LatitudeIndex]}, {Coordinates[LongitudeIndex]})";
                }

                return result;
            }
        }

        /// <summary>
        /// Gets or sets the coordinates of the geographic data
        /// </summary>
        [JsonProperty("coordinates")]
        public string[] Coordinates { get; set; }

        /// <summary>
        /// Gets the numeric latitude (null if the value could not be converted)
        /// </summary>
        public double? Latitude
        {
            get
            {
                return ParseCoordinate(LatitudeIndex);
            }
        }

        /// <summary>
        /// Gets the numeric longitude (null if the value could not be converted)
        /// </summary>
        public double? Longitude
        {
            get
            {
                return ParseCoordinate(LongitudeIndex);
            }
        }

        private double? ParseCoordinate(int index)
        {
            double? result = null;
            double parsed;

            if (DataType == PointType
            && Coordinates != null
            && !string.IsNullOrEmpty(Coordinates[index])
            && double.TryParse(Coordinates[index], NumberStyles.AllowDecimalPoint | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out parsed))
            {
                result = parsed;
            }

            return result;
        }
    }
}