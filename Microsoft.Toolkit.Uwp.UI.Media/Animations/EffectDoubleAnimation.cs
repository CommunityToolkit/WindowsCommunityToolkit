// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Microsoft.Toolkit.Uwp.UI.Media.Effects;

namespace Microsoft.Toolkit.Uwp.UI.Media.Animations
{
    /// <summary>
    /// A set of animations that can be grouped together.
    /// </summary>
    public class EffectDoubleAnimation : TypedAnimation<double>, ITimeline
    {
        public IPipelineEffect Target { get; set; }

        /// <inheritdoc/>
        AnimationBuilder ITimeline.AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint)
        {
            BlurEffect effect = (BlurEffect)Target;

            return builder.DoubleAnimation(
                effect.brush,
                $"{effect.id}.{nameof(GaussianBlurEffect.BlurAmount)}",
                From,
                To,
                Delay ?? delayHint,
                Duration ?? durationHint.GetValueOrDefault(),
                EasingType,
                EasingMode);
        }
    }
}
