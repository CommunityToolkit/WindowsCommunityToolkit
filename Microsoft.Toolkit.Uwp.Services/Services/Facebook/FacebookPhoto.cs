// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly types Facebook Photo object. Partial for extending properties.
    /// </summary>
    public partial class FacebookPhoto
    {
        /// <summary>
        /// Gets a string description of the strongly typed properties in this model.
        /// </summary>
        public static string Fields => "id, album, link, created_time, name, images, picture";

        /// <summary>
        /// Gets or sets id property.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets album property.
        /// </summary>
        public FacebookAlbum Album { get; set; }

        /// <summary>
        /// Gets or sets a link to the entity instance.
        /// </summary>
        public string Link { get; set; }

        /// <summary>
        /// Gets or sets time the entity instance was created.
        /// </summary>
        public DateTime Created_Time { get; set; }

        /// <summary>
        /// Gets or sets name property.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets images property.
        /// </summary>
        public List<FacebookPlatformImageSource> Images { get; set; }

        /// <summary>
        /// Gets or sets picture property.
        /// </summary>
        public string Picture { get; set; }
    }
}
