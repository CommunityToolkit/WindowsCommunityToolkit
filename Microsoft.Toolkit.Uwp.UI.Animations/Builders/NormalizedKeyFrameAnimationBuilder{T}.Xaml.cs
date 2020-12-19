// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations.Extensions;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class NormalizedKeyFrameAnimationBuilder<T> : INormalizedKeyFrameAnimationBuilder<T>
        where T : unmanaged
    {
        /// <summary>
        /// A custom <see cref="NormalizedKeyFrameAnimationBuilder{T}"/> class targeting the XAML layer.
        /// </summary>
        public sealed class Xaml : NormalizedKeyFrameAnimationBuilder<T>, AnimationBuilder.IXamlAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Xaml"/> class.
            /// </summary>
            /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
            public Xaml(string property, TimeSpan? delay, TimeSpan? duration)
                : base(property, delay, duration)
            {
            }

            /// <inheritdoc/>
            public unsafe Timeline GetAnimation(UIElement element)
            {
                Timeline animation;

                if (typeof(T) == typeof(double))
                {
                    DoubleAnimationUsingKeyFrames doubleAnimation = new() { EnableDependentAnimation = true };

                    foreach (var keyFrame in this.keyFrames)
                    {
                        doubleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame()
                        {
                            KeyTime = keyFrame.GetKeyTime(this.duration.GetValueOrDefault()),
                            Value = *(double*)&keyFrame.Value,
                            EasingFunction = keyFrame.EasingType.ToEasingFunction(keyFrame.EasingMode)
                        });
                    }

                    animation = doubleAnimation;
                }
                else if (typeof(T) == typeof(Point))
                {
                    PointAnimationUsingKeyFrames pointAnimation = new() { EnableDependentAnimation = true };

                    foreach (var keyFrame in this.keyFrames)
                    {
                        pointAnimation.KeyFrames.Add(new EasingPointKeyFrame()
                        {
                            KeyTime = keyFrame.GetKeyTime(this.duration.GetValueOrDefault()),
                            Value = *(Point*)&keyFrame.Value,
                            EasingFunction = keyFrame.EasingType.ToEasingFunction(keyFrame.EasingMode)
                        });
                    }

                    animation = pointAnimation;
                }
                else if (typeof(T) == typeof(Color))
                {
                    ColorAnimationUsingKeyFrames colorAnimation = new() { EnableDependentAnimation = true };

                    foreach (var keyFrame in this.keyFrames)
                    {
                        colorAnimation.KeyFrames.Add(new EasingColorKeyFrame()
                        {
                            KeyTime = keyFrame.GetKeyTime(this.duration.GetValueOrDefault()),
                            Value = *(Color*)&keyFrame.Value,
                            EasingFunction = keyFrame.EasingType.ToEasingFunction(keyFrame.EasingMode)
                        });
                    }

                    animation = colorAnimation;
                }
                else
                {
                    return ThrowHelper.ThrowInvalidOperationException<Timeline>("Invalid animation type");
                }

                animation.BeginTime = this.delay;
                animation.Duration = this.duration.GetValueOrDefault();

                Storyboard.SetTarget(animation, element);
                Storyboard.SetTargetProperty(animation, this.property);

                return animation;
            }
        }
    }
}
