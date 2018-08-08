// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo error type
    /// </summary>
    public class WeiboError
    {
        /// <summary>
        /// Gets or sets error code
        /// </summary>
        [JsonProperty("error_code")]
        public int Code { get; set; }

        /// <summary>
        /// Gets or sets error message
        /// </summary>
        [JsonProperty("error")]
        public string Message { get; set; }
    }
}