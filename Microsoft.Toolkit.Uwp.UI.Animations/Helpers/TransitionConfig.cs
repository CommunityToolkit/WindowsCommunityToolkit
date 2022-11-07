// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using Microsoft.Toolkit.Uwp.UI.Animations.Helpers;
using Windows.Foundation;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Configuration used for the transition between UI elements.
    /// </summary>
    public class TransitionConfig
    {
        /// <summary>
        /// Gets or sets an id to indicate the target UI elements.
        /// </summary>
        public string? Id { get; set; }

        /// <summary>
        /// Gets or sets the scale strategy of the transition.
        /// The default value is <see cref="ScaleMode.None"/>.
        /// </summary>
        public ScaleMode ScaleMode { get; set; } = ScaleMode.None;

        /// <summary>
        /// Gets or sets the custom scale calculator.
        /// Only works when <see cref="ScaleMode"/> is <see cref="ScaleMode.Custom"/>.
        /// If this value is not set, the scale strategy will fall back to <see cref="ScaleMode.None"/>.
        /// </summary>
        public IScalingCalculator? CustomScalingCalculator { get; set; } = null;

        /// <summary>
        /// Gets or sets a value indicating whether clip animations are enabled for the target UI elements.
        /// </summary>
        public bool EnableClipAnimation { get; set; }

        /// <summary>
        /// Gets or sets the center point used to calculate the element's translation or scale when animating.
        /// Value is normalized with respect to the size of the animated element.
        /// For example, a value of (0.0, 0.5) means that this point is at the leftmost point of the element horizontally and the center of the element vertically.
        /// The default value is (0, 0).
        /// </summary>
        public Point NormalizedCenterPoint { get; set; } = default;

        /// <summary>
        /// Gets or sets the easing function type for the transition.
        /// If this value is not set, it will fall back to the value in <see cref="TransitionHelper.DefaultEasingType"/>.
        /// </summary>
        public EasingType? EasingType { get; set; } = null;

        /// <summary>
        /// Gets or sets the easing function mode for the transition.
        /// If this value is not set, it will fall back to the value in <see cref="TransitionHelper.DefaultEasingMode"/>.
        /// </summary>
        public EasingMode? EasingMode { get; set; } = null;

        /// <summary>
        /// Gets or sets the key point of opacity transition.
        /// The time the keyframe of opacity from 0 to 1 or from 1 to 0 should occur at, expressed as a percentage of the animation duration. The allowed values are from (0, 0) to (1, 1).
        /// <see cref="OpacityTransitionProgressKey"/>.X will be used in the animation of the normal direction.
        /// <see cref="OpacityTransitionProgressKey"/>.Y will be used in the animation of the reverse direction.
        /// If this value is not set, it will fall back to the value in <see cref="TransitionHelper.DefaultOpacityTransitionProgressKey"/>.
        /// </summary>
        public Point? OpacityTransitionProgressKey { get; set; } = null;
    }
}
