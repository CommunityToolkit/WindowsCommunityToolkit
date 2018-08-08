// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo User type.
    /// </summary>
    public class WeiboUser
    {
        /// <summary>
        /// Gets or sets profile image url.
        /// </summary>
        [JsonProperty("profile_image_url")]
        public string ProfileImageUrl { get; set; }
    }
}