// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Indicates the transition strategy between controls.
    /// </summary>
    public enum TransitionMode
    {
        /// <summary>
        /// The default transition strategy.
        /// </summary>
        Normal,

        /// <summary>
        /// The transition strategy for image or other UI elements that require smoother transitions.
        /// </summary>
        Image,
    }
}
