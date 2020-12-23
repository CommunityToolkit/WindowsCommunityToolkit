// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A scale animation working on the composition or XAML layer.
    /// </summary>
    public class ScaleAnimation : Animation<Vector3?>, ITimeline
    {
        /// <summary>
        /// Gets or sets the target framework layer to animate.
        /// </summary>
        public FrameworkLayer Layer { get; set; }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            Vector3 to = To.Value;
            Vector3? from = From;
            TimeSpan? delay = Delay ?? delayHint;
            TimeSpan? duration = Duration ?? durationHint;
            EasingType easingType = EasingType ?? easingTypeHint ?? DefaultEasingType;
            EasingMode easingMode = EasingMode ?? easingModeHint ?? DefaultEasingMode;

            if (Layer == FrameworkLayer.Composition)
            {
                return builder.Scale(to, from, delay, duration, easingType, easingMode);
            }
            else
            {
                Vector2 to2 = new(to.X, to.Y);
                Vector2? from2 = from is null ? null : new(from.Value.X, from.Value.Y);

                return builder.Scale(to2, from2, delay, duration, easingType, easingMode, FrameworkLayer.Xaml);
            }
        }
    }
}
