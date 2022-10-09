// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Indicates the strategy when the scale property of a UI element is animated.
    /// </summary>
    public enum ScaleMode
    {
        /// <summary>
        /// Do not make any changes to the scale attribute of the UI element.
        /// </summary>
        None,

        /// <summary>
        /// Apply the scaling changes to the horizontal and vertical directions of the UI element.
        /// </summary>
        Scale,

        /// <summary>
        /// Apply the scaling changes to the horizontal and vertical directions of the UI element,
        /// but the value is calculated based on the change in the horizontal direction.
        /// </summary>
        ScaleX,

        /// <summary>
        /// Apply scaling changes to the horizontal and vertical directions of the UI element,
        /// but the value is calculated based on the change in the vertical direction.
        /// </summary>
        ScaleY,

        /// <summary>
        /// Apply the scaling changes calculated by using custom scale handler.
        /// </summary>
        Custom,
    }
}
