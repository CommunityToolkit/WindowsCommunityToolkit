// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter Url
    /// </summary>
    public class TwitterUrl
    {
        /// <summary>
        /// Gets or sets Url of the tweet.
        /// </summary>
        [JsonProperty("url")]
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets indices position of the tweet.
        /// </summary>
        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}