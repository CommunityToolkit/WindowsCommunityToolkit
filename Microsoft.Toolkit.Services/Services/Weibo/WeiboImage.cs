// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo image
    /// </summary>
    public class WeiboImage
    {
        /// <summary>
        /// Gets or sets the url of the attached image in thumbnail size.
        /// </summary>
        [JsonProperty("thumbnail_pic")]
        public string ThumbnailImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the url of the attached image in medium size.
        /// </summary>
        [JsonProperty("bmiddle_pic")]
        public string MediumImageUrl { get; set; }

        /// <summary>
        /// Gets or sets the url of the attached image in original size.
        /// </summary>
        [JsonProperty("original_pic")]
        public string OriginalImageUrl { get; set; }
    }
}
