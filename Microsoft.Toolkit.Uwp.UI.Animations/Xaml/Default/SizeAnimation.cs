// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A size animation working on the composition or XAML layer.
    /// </summary>
    public class SizeAnimation : Animation<string, Vector3>
    {
        /// <summary>
        /// Gets or sets the target framework layer to animate.
        /// </summary>
        public FrameworkLayer Layer { get; set; }

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            TimeSpan? delay = Delay ?? delayHint;
            TimeSpan? duration = Duration ?? durationHint;

            if (KeyFrames.Count > 0)
            {
                return builder.Size().NormalizedKeyFrames(
                    delay: Delay ?? delayHint,
                    duration: Duration ?? durationHint,
                    build: b => KeyFrame<string, Vector3>.AppendToBuilder(b, KeyFrames));
            }

            Vector3 to = To!.ToVector3();
            Vector3? from = From?.ToVector3();
            EasingType easingType = EasingType ?? easingTypeHint ?? DefaultEasingType;
            EasingMode easingMode = EasingMode ?? easingModeHint ?? DefaultEasingMode;

            if (Layer == FrameworkLayer.Composition)
            {
                return builder.Size(to, from, delay, duration, easingType, easingMode);
            }
            else
            {
                Vector2 to2 = new(to.X, to.Y);
                Vector2? from2 = from is null ? null : new(from.Value.X, from.Value.Y);

                return builder.Size(to2, from2, delay, duration, easingType, easingMode, FrameworkLayer.Xaml);
            }
        }
    }
}
