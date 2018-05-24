// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Strongly types Facebook PlatformImageSource object. Partial for extending properties.
    /// </summary>
    public partial class FacebookPlatformImageSource
    {
        /// <summary>
        /// Gets a string description of the strongly typed properties in this model.
        /// </summary>
        public static string Fields => "height, source, width";

        /// <summary>
        /// Gets or sets height property.
        /// </summary>
        public string Height { get; set; }

        /// <summary>
        /// Gets or sets source property.
        /// </summary>
        public string Source { get; set; }

        /// <summary>
        /// Gets or sets width property.
        /// </summary>
        public string Width { get; set; }
    }
}
