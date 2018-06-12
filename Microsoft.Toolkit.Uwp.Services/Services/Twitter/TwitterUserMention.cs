// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
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

        ///// <summary>
        ///// Gets or sets the ID of the user mentioned as a string
        ///// </summary>
        //[JsonProperty("id_str")]
        //public string Id { get; set; }

        ///// <summary>
        ///// Gets or sets the display name of the user mentioned
        ///// </summary>
        //[JsonProperty("name")]
        //public string Name { get; set; }

        ///// <summary>
        ///// Gets or sets the handle of the user mentioned
        ///// </summary>
        //[JsonProperty("screen_name")]
        //public string ScreenName { get; set; }
    }
}
