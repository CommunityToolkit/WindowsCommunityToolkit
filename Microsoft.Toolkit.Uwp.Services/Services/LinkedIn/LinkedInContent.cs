// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Strong type representation of Content.
    /// </summary>
    public class LinkedInContent
    {
        /// <summary>
        /// Gets or sets title property.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets description property.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets submitted url property.
        /// </summary>
        public string SubmittedUrl { get; set; }

        /// <summary>
        /// Gets or sets submitted image url property.
        /// </summary>
        public string SubmittedImageUrl { get; set; }
    }
}
