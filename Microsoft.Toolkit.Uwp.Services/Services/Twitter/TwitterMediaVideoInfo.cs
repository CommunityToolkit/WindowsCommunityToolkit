// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    public class TwitterMediaVideoInfo
    {
        /// <summary>
        /// Gets or sets the aspect ratio of the video (width, height).
        /// </summary>
        [JsonProperty("aspect_ratio")]
        public int[] AspectRatio { get; set; }

        /// <summary>
        /// Gets or sets the video length in milliseconds.
        /// </summary>
        [JsonProperty("duration_millis")]
        public int Duration { get; set; }

        /// <summary>
        /// Gets or sets array of video variants (bitrate, etc.)
        /// </summary>
        [JsonProperty("variants")]
        public TwitterMediaVideoVariants[] Variants { get; set; }
    }
}