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
    /// A custom animation targeting a property on an <see cref="IAttachedShadow"/> instance.
    /// </summary>
    /// <typeparam name="TValue">
    /// The type to use for the public <see cref="Animation{TValue,TKeyFrame}.To"/> and <see cref="Animation{TValue,TKeyFrame}.From"/>
    /// properties. This can differ from <typeparamref name="TKeyFrame"/> to facilitate XAML parsing.
    /// </typeparam>
    /// <typeparam name="TKeyFrame">The actual type of keyframe values in use.</typeparam>
    public abstract class ShadowAnimation<TValue, TKeyFrame> : Animation<TValue, TKeyFrame>, IAttachedTimeline
        where TKeyFrame : unmanaged
    {
        /// <summary>
        /// Gets or sets the linked <see cref="IAttachedShadow"/> instance to animate.
        /// </summary>
        public IAttachedShadow? Target
        {
            get => (IAttachedShadow?)GetValue(TargetProperty);
            set => SetValue(TargetProperty, value);
        }

        /// <summary>
        /// Identifies the <seealso cref="Target"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty TargetProperty = DependencyProperty.Register(
            nameof(Target),
            typeof(IAttachedShadow),
            typeof(ShadowAnimation<TValue, TKeyFrame>),
            new PropertyMetadata(null));

        /// <inheritdoc/>
        public override AnimationBuilder AppendToBuilder(AnimationBuilder builder, TimeSpan? delayHint, TimeSpan? durationHint, EasingType? easingTypeHint, EasingMode? easingModeHint)
        {
            throw new NotSupportedException();
        }

        /// <inheritdoc/>
        public AnimationBuilder AppendToBuilder(AnimationBuilder builder, UIElement parent, TimeSpan? delayHint = null, TimeSpan? durationHint = null, EasingType? easingTypeHint = null, EasingMode? easingModeHint = null)
        {
            if (ExplicitTarget is not string explicitTarget)
            {
                static AnimationBuilder ThrowArgumentNullException()
                {
                    throw new ArgumentNullException(
                        "The target shadow cannot be animated at this time.");
                }

                return ThrowArgumentNullException();
            }

            if (Target is IAttachedShadow allShadows)
            {
                // in this case we'll animate all the shadows being used.
                foreach (var context in allShadows.EnumerateElementContexts()) //// TODO: Find better way!!!
                {
                    NormalizedKeyFrameAnimationBuilder<TKeyFrame>.Composition keyFrameBuilder = new(
                        explicitTarget,
                        Delay ?? delayHint ?? DefaultDelay,
                        Duration ?? durationHint ?? DefaultDuration,
                        Repeat,
                        DelayBehavior);

                    AppendToBuilder(keyFrameBuilder, easingTypeHint, easingModeHint);

                    CompositionAnimation animation = keyFrameBuilder.GetAnimation(context.Shadow, out _);

                    builder.ExternalAnimation(context.Shadow, animation);
                }

                return builder;
            }
            else
            {
                var shadowBase = Effects.GetShadow(parent as FrameworkElement);
                if (shadowBase == null)
                {
                    static AnimationBuilder ThrowArgumentNullException() => throw new ArgumentNullException("The target's shadow is null, make sure to set the Target property to an element with a Shadow");

                    return ThrowArgumentNullException();
                }

                var shadow = shadowBase.GetElementContext((FrameworkElement)parent).Shadow;

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
}