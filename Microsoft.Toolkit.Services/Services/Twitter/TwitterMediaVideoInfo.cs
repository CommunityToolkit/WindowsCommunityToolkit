// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Video information
    /// </summary>
    public class TwitterMediaVideoInfo
    {
        /// <summary>
        /// Gets or sets video aspect ratio (width, height)
        /// </summary>
        [JsonProperty("aspect_ratio")]
        public int[] AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets duration of video in milliseconds
        /// </summary>
        [JsonProperty("duration_millis")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets video variants for different codecs, bitrates, etc.
        /// </summary>
        [JsonProperty("variants")]
        public TwitterMediaVideoVariants[] Variants { get; set; }
    }
}