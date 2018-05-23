// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// Strong type for sharing data to LinkedIn.
    /// </summary>
    public partial class LinkedInShareRequest
    {
        /// <summary>
        /// Gets or sets comment property.
        /// </summary>
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets visibility property.
        /// </summary>
        public LinkedInVisibility Visibility { get; set; }

        /// <summary>
        /// Gets or sets content property.
        /// </summary>
        public LinkedInContent Content { get; set; }
    }
}
