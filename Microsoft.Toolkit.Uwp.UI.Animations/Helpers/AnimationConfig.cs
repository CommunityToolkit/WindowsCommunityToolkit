// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        /// Gets or sets the strategy for animating the opacity of a UI element.
        /// The default value is <see cref="OpacityAnimationMode.Faster"/>.
        /// </summary>
        public OpacityAnimationMode OpacityMode { get; set; } = OpacityAnimationMode.Faster;
    }
}
