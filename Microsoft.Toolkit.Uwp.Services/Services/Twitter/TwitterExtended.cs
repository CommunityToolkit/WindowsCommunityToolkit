// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter User type.
    /// </summary>
    public class TwitterExtended
    {
        /// <summary>
        /// Gets or sets the text of the tweet (280 characters).
        /// </summary>
        [JsonProperty("full_text")]
        public string FullText { get; set; }
    }
}