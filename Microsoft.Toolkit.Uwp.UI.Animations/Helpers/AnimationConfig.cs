// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Numerics;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Configuration used for UI element animation.
    /// </summary>
    public class AnimationConfig
    {
        /// <summary>
        /// Gets or sets id of a UI element.
        /// </summary>
        public string Id { get; set; }

        /// <summary>
        /// Gets or sets the scale strategy of a UI element.
        /// The default value is <see cref="ScaleMode.None"/>.
        /// </summary>
        public ScaleMode ScaleMode { get; set; } = ScaleMode.None;

        /// <summary>
        /// Gets or sets the center point used to calculate the element's translation or scale when animating.
        /// Value is normalized with respect to the size of the animated element.
        /// For example, a value of (0.0, 0.5) means that this point is at the leftmost point of the element horizontally and the center of the element vertically.
        /// The default value is <see cref="Vector2.Zero"/>.
        /// </summary>
        public Vector2 NormalizedCenterPoint { get; set; } = Vector2.Zero;

        /// <summary>
        /// Gets or sets the strategy for animating the opacity of a UI element.
        /// The default value is <see cref="OpacityAnimationMode.Faster"/>.
        /// </summary>
        public OpacityAnimationMode OpacityMode { get; set; } = OpacityAnimationMode.Faster;
    }
}
