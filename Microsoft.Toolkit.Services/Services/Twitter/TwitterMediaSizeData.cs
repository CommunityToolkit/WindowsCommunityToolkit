// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Entities containing size details for each size of an image.
    /// </summary>
    public class TwitterMediaSizeData
    {
        /// <summary>
        /// Gets or sets width integer.
        /// </summary>
        [JsonProperty("w")]
        public int Width { get; set; }

        /// <summary>
        /// Gets or sets height integer.
        /// </summary>
        [JsonProperty("h")]
        public int Height { get; set; }

        /// <summary>
        /// Gets or sets resize string.
        /// </summary>
        [JsonProperty("resize")]
        public string Resize { get; set; }
    }
}
