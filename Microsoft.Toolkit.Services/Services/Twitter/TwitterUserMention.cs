// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter user object containing user mention indices.
    /// </summary>
    public class TwitterUserMention : TwitterUser
    {
        /// <summary>
        /// Gets or sets the start and end position of the user mention
        /// </summary>
        [JsonProperty("indices")]
        public int[] Indices { get; set; }
    }
}
