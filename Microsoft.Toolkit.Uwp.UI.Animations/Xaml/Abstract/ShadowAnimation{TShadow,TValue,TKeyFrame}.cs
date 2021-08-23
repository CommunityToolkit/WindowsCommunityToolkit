// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;
using static Microsoft.Toolkit.Uwp.UI.Animations.AnimationExtensions;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A custom animation targeting a property on an <see cref="AttachedShadowBase"/> instance.
    /// </summary>
    /// <typeparam name="TShadow">The <see cref="FrameworkElement"/> containing the shadow to animate.</typeparam>
    /// <typeparam name="TValue">
    /// The type to use for the public <see cref="Animation{TValue,TKeyFrame}.To"/> and <see cref="Animation{TValue,TKeyFrame}.From"/>
    /// properties. This can differ from <typeparamref name="TKeyFrame"/> to facilitate XAML parsing.
    /// </typeparam>
    /// <typeparam name="TKeyFrame">The actual type of keyframe values in use.</typeparam>
    public abstract class ShadowAnimation<TShadow, TValue, TKeyFrame> : Animation<TValue, TKeyFrame>
        where TShadow : FrameworkElement
        where TKeyFrame : unmanaged
    {
        /// <summary>
        /// Gets or sets the linked <typeparamref name="TShadow"/> instance to animate.
        /// </summary>
        public TShadow? Target
        {
            get => (TShadow?)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Target"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target),
            typeof(TShadow),
            typeof(ShadowAnimation<TShadow, TValue, TKeyFrame>),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            if (Target is not TShadow target)
            {
                static AnimationBuilder ThrowTargetNullException() => throw new ArgumentNullException("The target element is null, make sure to set the Target property");

                return ThrowTargetNullException();
            }

            var shadowBase = Effects.GetShadow(Target);
            if (shadowBase == null)
            {
                static AnimationBuilder ThrowArgumentNullException() => throw new ArgumentNullException("The target's shadow is null, make sure to set the Target property to an element with a Shadow");

                return ThrowArgumentNullException();
            }

            if (ExplicitTarget is not string explicitTarget)
            {
                static AnimationBuilder ThrowArgumentNullException()
                {
                    throw new ArgumentNullException(
                        "The target shadow cannot be animated at this time.");
                }

                return ThrowArgumentNullException();
            }

            var shadow = shadowBase.GetElementContext(Target).Shadow;

            NormalizedKeyFrameAnimationBuilder<TKeyFrame>.Composition keyFrameBuilder = new(
                explicitTarget,
                Delay ?? delayHint ?? DefaultDelay,
                Duration ?? durationHint ?? DefaultDuration,
                Repeat,
                DelayBehavior);

            AppendToBuilder(keyFrameBuilder, easingTypeHint, easingModeHint);

            CompositionAnimation animation = keyFrameBuilder.GetAnimation(shadow, out _);

            return builder.ExternalAnimation(shadow, animation);
        }
    }
}