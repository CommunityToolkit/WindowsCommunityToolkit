// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  Class OneDriveItemConflictBehavior which define
    /// </summary>
    public class OneDriveItemConflictBehavior
    {
        /// <summary>
        /// Gets or sets the item's name
        /// </summary>
        [JsonProperty("item")]
        public OneDriveConflictItem Item { get; set; }
    }
}
