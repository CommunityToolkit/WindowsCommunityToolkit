// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Media Info
    /// </summary>
    public class TwitterMediaAdditionalInfo
    {
        /// <summary>
        /// Gets or sets title of video
        /// </summary>
        [JsonPropertyName("title")]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description of video
        /// </summary>
        [JsonPropertyName("description")]
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether video is embeddable
        /// </summary>
        [JsonPropertyName("embeddable")]
        public bool Embeddable { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether "monetizable"
        /// </summary>
        [JsonPropertyName("monetizable")]
        public bool Monetizable { get; set; }
    }
}