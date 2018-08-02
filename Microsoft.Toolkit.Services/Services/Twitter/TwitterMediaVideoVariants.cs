// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Video properties
    /// </summary>
    public class TwitterMediaVideoVariants
    {
        /// <summary>
        /// Gets or sets video bitrate in bits-per-second
        /// </summary>
        [JsonProperty("bitrate")]
        public int Bitrate { get; set; }

        /// <summary>
        /// Gets or sets the MIME type of the video
        /// </summary>
        [JsonProperty("content_type")]
        public string ContentType { get; set; }

        /// <summary>
        /// Gets or sets the direct URL for the video variant
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}