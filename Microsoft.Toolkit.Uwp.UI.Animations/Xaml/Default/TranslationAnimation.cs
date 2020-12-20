// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Xaml
{
    /// <summary>
    /// A translation animation working on the composition or XAML layer.
    /// This animation maps to <see cref="AnimationBuilder.Translation(System.Numerics.Vector3, System.Numerics.Vector3?, TimeSpan?, TimeSpan?, EasingType, EasingMode)"/>.
    /// </summary>
    public class TranslationAnimation : TypedAnimation<string>, ITimeline
    {
        /// <summary>
        /// Gets or sets the target framework layer to animate.
        /// </summary>
        public FrameworkLayer Layer { get; set; }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            TimeSpan? delay = Delay ?? delayHint;
            TimeSpan? duration = Duration ?? durationHint;
            EasingType easingType = EasingType ?? easingTypeHint ?? DefaultEasingType;
            EasingMode easingMode = EasingMode ?? easingModeHint ?? DefaultEasingMode;

            if (Layer == FrameworkLayer.Composition)
            {
                return builder.Translation(To!.ToVector3(), From?.ToVector3(), delay, duration, easingType, easingMode);
            }
            else
            {
                return builder.Translation(To!.ToVector2(), From?.ToVector2(), delay, duration, easingType, easingMode, FrameworkLayer.Xaml);
            }
        }
    }
}
