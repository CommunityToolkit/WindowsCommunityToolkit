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
    /// <inheritdoc cref="TimedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class TimedKeyFrameAnimationBuilder<T> : ITimedKeyFrameAnimationBuilder<T>
        where T : unmanaged
    {
        /// <summary>
        /// A custom <see cref="TimedKeyFrameAnimationBuilder{T}"/> class targeting the XAML layer.
        /// </summary>
        public class Xaml : TimedKeyFrameAnimationBuilder<T>, AnimationBuilder.IXamlAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Xaml"/> class.
            /// </summary>
            /// <inheritdoc cref="TimedKeyFrameAnimationBuilder{T}"/>
            public Xaml(string property, TimeSpan? delay)
                : base(property, delay)
            {
            }

            /// <inheritdoc/>
            public virtual unsafe Timeline GetAnimation(UIElement element)
            {
                Timeline animation;

                if (typeof(T) == typeof(double))
                {
                    DoubleAnimationUsingKeyFrames doubleAnimation = new() { EnableDependentAnimation = true };

                    foreach (var keyFrame in this.keyFrames)
                    {
                        doubleAnimation.KeyFrames.Add(new EasingDoubleKeyFrame()
                        {
                            KeyTime = keyFrame.Progress,
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
                            KeyTime = keyFrame.Progress,
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
                            KeyTime = keyFrame.Progress,
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

                Storyboard.SetTarget(animation, element);
                Storyboard.SetTargetProperty(animation, this.property);

                return animation;
            }
        }
    }
}
