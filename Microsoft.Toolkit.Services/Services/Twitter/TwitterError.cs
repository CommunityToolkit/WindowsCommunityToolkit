// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text.Json.Serialization;

namespace Microsoft.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter error type
    /// </summary>
    public class TwitterError
    {
        /// <summary>
        /// Gets or sets error code
        /// </summary>
        [JsonPropertyName("code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets error message
        /// </summary>
        [JsonPropertyName("message")]
        public string Message { get; set; }
    }
}
