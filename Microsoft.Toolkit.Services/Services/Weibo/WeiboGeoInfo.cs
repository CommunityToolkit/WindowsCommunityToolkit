// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Services.Weibo
{
    /// <summary>
    /// Geographic information for a Weibo status
    /// </summary>
    public class WeiboGeoInfo
    {
        /// <summary>
        /// Gets the numeric latitude (null if the value could not be converted)
        /// </summary>
        [JsonProperty("latitude")]
        public double Latitude { get; internal set; }

        /// <summary>
        /// Gets the numeric longitude (null if the value could not be converted)
        /// </summary>
        [JsonProperty("longitude")]
        public double Longitude { get; internal set; }

        /// <summary>
        /// Gets the city
        /// </summary>
        [JsonProperty("city_name")]
        public double City { get; internal set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"({Latitude}, {Longitude})";
        }
    }
}
