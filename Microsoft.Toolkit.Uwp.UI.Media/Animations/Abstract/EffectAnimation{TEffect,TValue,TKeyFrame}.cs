// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Effects;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.Extensions.AnimationExtensions;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A custom animation targeting a property on an <see cref="IPipelineEffect"/> instance.
    /// </summary>
    /// <typeparam name="TEffect">The type of effect to animate.</typeparam>
    /// <typeparam name="TValue">
    /// The type to use for the public <see cref="Animation{TValue,TKeyFrame}.To"/> and <see cref="Animation{TValue,TKeyFrame}.From"/>
    /// properties. This can differ from <typeparamref name="TKeyFrame"/> to facilitate XAML parsing.
    /// </typeparam>
    /// <typeparam name="TKeyFrame">The actual type of keyframe values in use.</typeparam>
    public abstract class EffectAnimation<TEffect, TValue, TKeyFrame> : Animation<TValue, TKeyFrame>
        where TEffect : class, IPipelineEffect
        where TKeyFrame : unmanaged
    {
        /// <summary>
        /// Gets or sets the linked <typeparamref name="TEffect"/> instance to animate.
        /// </summary>
        public TEffect? Target { get; set; }

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            NormalizedKeyFrameAnimationBuilder<TKeyFrame>.Composition keyFrameBuilder = new(
                ExplicitTarget,
                Delay ?? delayHint ?? DefaultDelay,
                Duration ?? durationHint ?? DefaultDuration);

            AppendToBuilder(keyFrameBuilder, easingTypeHint, easingModeHint);

            CompositionAnimation animation = keyFrameBuilder.GetAnimation(Target!.Brush!, out _);

            return builder.ExternalAnimation(Target.Brush, animation);
        }
    }
}
