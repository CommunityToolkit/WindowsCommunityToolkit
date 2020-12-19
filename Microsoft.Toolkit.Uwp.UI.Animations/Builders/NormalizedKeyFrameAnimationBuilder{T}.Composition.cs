// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations.Extensions;
using Windows.UI;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class NormalizedKeyFrameAnimationBuilder<T>
    {
        /// <summary>
        /// A custom <see cref="NormalizedKeyFrameAnimationBuilder{T}"/> class targeting the composition layer.
        /// </summary>
        public sealed class Composition : NormalizedKeyFrameAnimationBuilder<T>, AnimationBuilder.ICompositionAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Composition"/> class.
            /// </summary>
            /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
            public Composition(string property, TimeSpan? delay, TimeSpan? duration)
                : base(property, delay, duration)
            {
            }

            /// <inheritdoc/>
            public unsafe CompositionAnimation GetAnimation(Visual visual)
            {
                KeyFrameAnimation animation;

                if (typeof(T) == typeof(bool))
                {
                    BooleanKeyFrameAnimation boolAnimation = visual.Compositor.CreateBooleanKeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        boolAnimation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(bool*)&keyFrame.Value);
                    }

                    animation = boolAnimation;
                }
                else if (typeof(T) == typeof(float))
                {
                    ScalarKeyFrameAnimation scalarAnimation = visual.Compositor.CreateScalarKeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        scalarAnimation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(float*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = scalarAnimation;
                }
                else if (typeof(T) == typeof(Vector2))
                {
                    Vector2KeyFrameAnimation vector2Animation = visual.Compositor.CreateVector2KeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        vector2Animation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(Vector2*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = vector2Animation;
                }
                else if (typeof(T) == typeof(Vector3))
                {
                    Vector3KeyFrameAnimation vector3Animation = visual.Compositor.CreateVector3KeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        vector3Animation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(Vector3*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = vector3Animation;
                }
                else if (typeof(T) == typeof(Vector4))
                {
                    Vector4KeyFrameAnimation vector4Animation = visual.Compositor.CreateVector4KeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        vector4Animation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(Vector4*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = vector4Animation;
                }
                else if (typeof(T) == typeof(Color))
                {
                    ColorKeyFrameAnimation colorAnimation = visual.Compositor.CreateColorKeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        colorAnimation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(Color*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = colorAnimation;
                }
                else if (typeof(T) == typeof(Quaternion))
                {
                    QuaternionKeyFrameAnimation quaternionAnimation = visual.Compositor.CreateQuaternionKeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        quaternionAnimation.InsertKeyFrame(
                            (float)keyFrame.Progress,
                            *(Quaternion*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = quaternionAnimation;
                }
                else
                {
                    return ThrowHelper.ThrowInvalidOperationException<CompositionAnimation>("Invalid animation type");
                }

                animation.Duration = this.duration.GetValueOrDefault();

                if (this.delay.HasValue)
                {
                    animation.DelayTime = this.delay!.Value;
                }

                animation.Target = this.property;

                return animation;
            }
        }
    }
}
