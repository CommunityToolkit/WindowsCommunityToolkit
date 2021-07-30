// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The type of caching to be applied to <see cref="ImageEx"/>.
    /// Default is <see cref="Custom"/>
    /// </summary>
    public enum ImageExCachingStrategy
    {
        /// <summary>
        /// Caching is handled by <see cref="ImageEx"/>'s custom caching system.
        /// </summary>
        Custom,

        /// <summary>
        /// Caching is handled internally by UWP.
        /// </summary>
        Internal
    }
}
