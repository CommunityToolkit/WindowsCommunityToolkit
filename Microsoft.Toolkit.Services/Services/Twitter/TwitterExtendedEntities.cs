// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Extended Entities containing an array of TwitterMedia.
    /// </summary>
    public class TwitterExtendedEntities
    {
        /// <summary>
        /// Gets or sets Media of the tweet.
        /// </summary>
        [JsonPropertyName("media")]
        public TwitterMedia[] Media { get; set; }
    }
}
