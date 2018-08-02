// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter poll options object containing poll questions.
    /// </summary>
    public class TwitterPollOptions
    {
        /// <summary>
        /// Gets or sets int value of the poll position.
        /// </summary>
        [JsonProperty("position")]
        public int Position { get; set; }

        /// <summary>
        /// Gets or sets text of the poll question.
        /// </summary>
        [JsonProperty("text")]
        public string Text { get; set; }
    }
}