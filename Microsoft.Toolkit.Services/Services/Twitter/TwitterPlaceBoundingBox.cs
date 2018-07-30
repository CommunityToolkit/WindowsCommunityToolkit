// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Globalization;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Place Bounding Box
    /// </summary>
    public class TwitterPlaceBoundingBox
    {
        /// <summary>
        /// Gets or sets the bounding box coordinates of the tweet's geolocation data.
        /// </summary>
        [JsonProperty("coordinates")]
        public List<List<float[]>> Coordinates { get; set; }

        /// <summary>
        /// Gets or sets the coordinate type. Polygon for a bounding box, Pointn for an exact coordinate.
        /// </summary>
        [JsonProperty("type")]
        public string Type { get; set; }

        /// <summary>
        /// Gets the coordinates array of the tweet's geolocation data
        /// </summary>
        public List<float[]> CoordinatesArray
        {
            get
            {
                List<float[]> result = null;

                if (Coordinates != null)
                {
                    result = Coordinates[0];
                }

                return result;
            }
        }
    }
}
