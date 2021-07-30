// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Place
    /// </summary>
    public class TwitterPlace
    {
        /// <summary>
        /// Gets or sets the ID of the place
        /// </summary>
        [JsonPropertyName("id")]
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the URL of additional place metadata
        /// </summary>
        [JsonPropertyName("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the place type.
        /// </summary>
        [JsonPropertyName("place_type")]
        public string PlaceType { get; set; }

        /// <summary>
        /// Gets or sets the place name.
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the full, human-readable place name.
        /// </summary>
        [JsonPropertyName("full_name")]
        public string FullName { get; set; }

        /// <summary>
        /// Gets or sets the shortened country code (e.g. US) for the place.
        /// </summary>
        [JsonPropertyName("country_code")]
        public string CountryCode { get; set; }

        /// <summary>
        /// Gets or sets the name of the country for the place.
        /// </summary>
        [JsonPropertyName("country")]
        public string Country { get; set; }

        /// <summary>
        ///  Gets or sets the bounding box coordinates of a location.
        /// </summary>
        [JsonPropertyName("bounding_box")]
        public TwitterPlaceBoundingBox BoundingBox { get; set; }
    }
}
