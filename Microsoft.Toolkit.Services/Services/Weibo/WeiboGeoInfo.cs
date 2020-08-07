// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Services.Weibo
{
    /// <summary>
    /// Geographic information for a Weibo status
    /// </summary>
    public class WeiboGeoInfo
    {
        /// <summary>
        /// Gets the type of geographic information
        /// </summary>
        [JsonPropertyName("type")]
        public string Type { get; internal set; }

        /// <summary>
        /// Gets the coordinates
        /// </summary>
        [JsonPropertyName("coordinates")]
        public double[] Coordinates { get; internal set; }

        /// <inheritdoc/>
        public override string ToString()
        {
            if (Coordinates.Length > 1)
            {
                return $"({Coordinates[0]}, {Coordinates[1]})";
            }

            return "(0, 0)";
        }
    }
}
