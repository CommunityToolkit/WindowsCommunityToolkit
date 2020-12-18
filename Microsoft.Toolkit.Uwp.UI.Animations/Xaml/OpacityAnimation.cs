// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A set of animations that can be grouped together.
    /// </summary>
    public class OpacityAnimation : TypedAnimation<double>, ITimeline
    {
        /// <summary>
        /// Gets or sets the target framework layer to animate.
        /// </summary>
        public FrameworkLayer Layer { get; set; }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            return builder.Opacity(From, To, Delay ?? delayHint, Duration ?? durationHint.GetValueOrDefault(), EasingType, EasingMode, Layer);
        }
    }
}
