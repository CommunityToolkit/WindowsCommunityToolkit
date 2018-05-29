// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly types Facebook Album object. Partial for extending properties.
    /// </summary>
    public partial class FacebookAlbum
    {
        /// <summary>
        /// Gets a string description of the strongly typed properties in this model.
        /// </summary>
        public static string Fields => "id, name, description, cover_photo, picture";

        /// <summary>
        /// Gets or sets id property.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets name property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets description property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets cover_photo property.
        /// </summary>
        public FacebookPhoto Cover_Photo { get; set; }

        /// <summary>
        /// Gets or sets picture property.
        /// </summary>
        public FacebookPictureData Picture { get; set; }
    }
}
