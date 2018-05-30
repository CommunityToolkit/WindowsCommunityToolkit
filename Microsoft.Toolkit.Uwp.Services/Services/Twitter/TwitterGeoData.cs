// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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