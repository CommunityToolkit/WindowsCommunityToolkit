// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace CommunityToolkit.WinUI.UI.Animations
{
    /// <summary>
    /// An <see langword="enum"/> that indicates the framework layer to target in a specific animation.
    /// </summary>
    public enum FrameworkLayer
    {
        /// <summary>
        /// Indicates the <see cref="Microsoft.UI.Composition"/> APIs.
        /// </summary>
        Composition,

        /// <summary>
        /// Indicates the <see cref="Microsoft.UI.Xaml.Media.Animation"/> APIs.
        /// </summary>
        Xaml
    }
}