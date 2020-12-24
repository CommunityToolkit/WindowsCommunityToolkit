// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Extensions;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Microsoft.Toolkit.Uwp.UI.Media.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

namespace Microsoft.Toolkit.Uwp.UI.Media.Animations
{
    /// <summary>
    /// A set of animations that can be grouped together.
    /// </summary>
    public class EffectDoubleAnimation : Animation<double?, double>
    {
        /// <summary>
        /// Gets or sets the linked <see cref="IPipelineEffect"/> instance to animate.
        /// </summary>
        public IPipelineEffect Target { get; set; }

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            BlurEffect effect = (BlurEffect)Target;

            CompositionAnimation animation = effect.Brush.Compositor.CreateScalarKeyFrameAnimation(
                $"{effect.Id}.{nameof(GaussianBlurEffect.BlurAmount)}",
                (float)To,
                (float?)From,
                Delay ?? delayHint ?? DefaultDelay,
                Duration ?? durationHint ?? DefaultDuration,
                effect.Brush.Compositor.CreateEasingFunction(
                    EasingType ?? easingTypeHint ?? DefaultEasingType,
                    EasingMode ?? easingModeHint ?? DefaultEasingMode));

            return builder.ExternalAnimation(effect.Brush, animation);
        }
    }
}
