// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A base model representing an animation that can be used in XAML.
    /// </summary>
    public abstract class Animation
    {
        /// <summary>
        /// Gets or sets the optional initial delay for the animation.
        /// </summary>
        public TimeSpan? Delay { get; set; }

        /// <summary>
        /// Gets or sets the animation duration.
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets the optional easing function type for the animation.
        /// </summary>
        public EasingType EasingType { get; set; } = DefaultEasingType;

        /// <summary>
        /// Gets or sets the optional easing function mode for the animation.
        /// </summary>
        public EasingMode EasingMode { get; set; } = DefaultEasingMode;
    }
}
