// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter errors type.
    /// </summary>
    public class TwitterErrors
    {
        /// <summary>
        /// Gets or sets the list of errors
        /// </summary>
        [JsonPropertyName("errors")]
        public TwitterError[] Errors { get; set; }
    }
}