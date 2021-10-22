// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Indicates the target property of the UI element to be animated.
    /// </summary>
    public enum AnimationTarget
    {
        /// <summary>
        /// The translation property of a UI element.
        /// </summary>
        Translation,

        /// <summary>
        /// The scale property of a UI element.
        /// </summary>
        Scale,

        /// <summary>
        /// The horizontal scale property of a UI element.
        /// </summary>
        ScaleX,

        /// <summary>
        /// The vertical scale property of a UI element.
        /// </summary>
        ScaleY,

        /// <summary>
        /// The opacity property of a UI element.
        /// </summary>
        Opacity
    }
}
