// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Media
    /// </summary>
    public class TwitterMedia
    {
        /// <summary>
        /// Gets or sets MediaUrl.
        /// </summary>
        [JsonProperty("media_url")]
        public string MediaUrl { get; set; }

        /// <summary>
        /// Gets or sets Url.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }
    }
}