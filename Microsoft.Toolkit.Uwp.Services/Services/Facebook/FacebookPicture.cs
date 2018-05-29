// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly typed object for presenting picture data returned from service provider.
    /// </summary>
    public class FacebookPicture
    {
        /// <summary>
        /// Gets or sets a value indicating whether the picture is a silhouette or not.
        /// </summary>
        public bool Is_Silhouette { get; set; }

        /// <summary>
        /// Gets or sets an url to the picture.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the ID of the picture.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the url of the page with the picture.
        /// </summary>
        public string Link { get; set; }
    }
}
