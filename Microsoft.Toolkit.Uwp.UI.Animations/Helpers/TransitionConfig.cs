// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Handler used to calculate the change in the scaling of an element when it is in transition.
    /// </summary>
    /// <param name="source">The source element.</param>
    /// <param name="target">The target element.</param>
    /// <returns>A value tuple whose first value represents the horizontal scaling change and whose second value represents the vertical scaling change.</returns>
    public delegate (double, double) ScaleHandler(UIElement source, UIElement target);

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
        /// Gets or sets the custom scale handler.
        /// Only works when <see cref="ScaleMode"/> is <see cref="ScaleMode.Custom"/>.
        /// If this value is not set, the scale strategy will fall back to <see cref="ScaleMode.None"/>.
        /// </summary>
        public ScaleHandler? CustomScaleHandler { get; set; } = null;

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
    }
}
