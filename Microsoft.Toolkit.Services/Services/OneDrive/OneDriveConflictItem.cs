// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    /// Class Item
    /// </summary>
    public class OneDriveConflictItem
    {
        /// <summary>
        /// Gets or sets the conflict resolution behavior for actions that create a new item
        /// </summary>
        [JsonProperty("@name.conflictBehavior")]
        public string ConflictBehavior { get; set; }
    }
}
