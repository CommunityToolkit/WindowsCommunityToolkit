// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Newtonsoft.Json;

namespace Microsoft.Toolkit.Services.OneDrive
{
    /// <summary>
    ///  RootParentReference class use for the request
    /// </summary>
    public class OneDriveParentReference
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OneDriveParentReference"/> class.
        /// </summary>
        public OneDriveParentReference()
        {
            Parent = new OneDriveParent();
        }

        /// <summary>
        /// Gets or sets the reference to the parent's item
        /// </summary>
        [JsonProperty("parentReference")]
        public OneDriveParent Parent { get; set; }

        /// <summary>
        /// Gets or sets the item's name
        /// </summary>
        [JsonProperty("name")]
        public string Name { get; set; }
    }
}
