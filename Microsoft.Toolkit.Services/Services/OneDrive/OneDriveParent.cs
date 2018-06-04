// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Parent class use for the request
    /// </summary>
    public class OneDriveParent
    {
        /// <summary>
        /// Gets or sets parent path
        /// </summary>
        [JsonProperty("path")]
        public string Path { get; set; }
    }
}
