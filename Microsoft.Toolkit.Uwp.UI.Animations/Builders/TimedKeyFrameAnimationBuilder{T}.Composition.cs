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
    /// <inheritdoc cref="TimedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class TimedKeyFrameAnimationBuilder<T> : ITimedKeyFrameAnimationBuilder<T>
        where T : unmanaged
    {
        /// <summary>
        /// A custom <see cref="TimedKeyFrameAnimationBuilder{T}"/> class targeting the composition layer.
        /// </summary>
        public class Composition : TimedKeyFrameAnimationBuilder<T>, AnimationBuilder.ICompositionAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Composition"/> class.
            /// </summary>
            /// <inheritdoc cref="TimedKeyFrameAnimationBuilder{T}"/>
            public Composition(string property, TimeSpan? delay)
                : base(property, delay)
            {
            }

            /// <inheritdoc/>
            public unsafe CompositionAnimation GetAnimation(Visual visual)
            {
                KeyFrameAnimation animation;

                // We can retrieve the total duration from the last timed keyframe, and then set
                // this as the target duration and use it to normalize the keyframe progresses.
                TimeSpan duration = this.keyFrames[this.keyFrames.Count - 1].Progress;

                if (typeof(T) == typeof(bool))
                {
                    BooleanKeyFrameAnimation boolAnimation = visual.Compositor.CreateBooleanKeyFrameAnimation();

                    foreach (var keyFrame in this.keyFrames)
                    {
                        boolAnimation.InsertKeyFrame(
                            (float)keyFrame.GetNormalizedProgress(duration),
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
                            (float)keyFrame.GetNormalizedProgress(duration),
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
                            (float)keyFrame.GetNormalizedProgress(duration),
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
                            (float)keyFrame.GetNormalizedProgress(duration),
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
                            (float)keyFrame.GetNormalizedProgress(duration),
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
                            (float)keyFrame.GetNormalizedProgress(duration),
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
                            (float)keyFrame.GetNormalizedProgress(duration),
                            *(Quaternion*)&keyFrame.Value,
                            visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                    }

                    animation = quaternionAnimation;
                }
                else
                {
                    return ThrowHelper.ThrowInvalidOperationException<CompositionAnimation>("Invalid animation type");
                }

                animation.Duration = duration;

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
