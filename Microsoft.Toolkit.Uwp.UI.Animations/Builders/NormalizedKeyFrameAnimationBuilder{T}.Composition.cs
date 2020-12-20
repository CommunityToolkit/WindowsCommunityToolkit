// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
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
        /// Gets a <see cref="CompositionAnimation"/> instance representing the animation to start.
        /// </summary>
        /// <typeparam name="TKeyFrame">The type of keyframes being used to define the animation.</typeparam>
        /// <param name="visual">The target <see cref="Visual"/> instance to animate.</param>
        /// <param name="property">The target property to animate.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="keyFrames">The list of keyframes to use to build the animation.</param>
        /// <returns>A <see cref="CompositionAnimation"/> instance with the specified animation.</returns>
        public static CompositionAnimation GetAnimation<TKeyFrame>(
            Visual visual,
            string property,
            TimeSpan? delay,
            TimeSpan duration,
            List<TKeyFrame> keyFrames)
            where TKeyFrame : struct, IKeyFrameInfo
        {
            KeyFrameAnimation animation;

            if (typeof(T) == typeof(bool))
            {
                BooleanKeyFrameAnimation boolAnimation = visual.Compositor.CreateBooleanKeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    boolAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<bool>());
                }

                animation = boolAnimation;
            }
            else if (typeof(T) == typeof(float))
            {
                ScalarKeyFrameAnimation scalarAnimation = visual.Compositor.CreateScalarKeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    scalarAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<float>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = scalarAnimation;
            }
            else if (typeof(T) == typeof(double))
            {
                ScalarKeyFrameAnimation scalarAnimation = visual.Compositor.CreateScalarKeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    scalarAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        (float)keyFrame.GetValueAs<double>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = scalarAnimation;
            }
            else if (typeof(T) == typeof(Vector2))
            {
                Vector2KeyFrameAnimation vector2Animation = visual.Compositor.CreateVector2KeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    vector2Animation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Vector2>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = vector2Animation;
            }
            else if (typeof(T) == typeof(Vector3))
            {
                Vector3KeyFrameAnimation vector3Animation = visual.Compositor.CreateVector3KeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    vector3Animation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Vector3>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = vector3Animation;
            }
            else if (typeof(T) == typeof(Vector4))
            {
                Vector4KeyFrameAnimation vector4Animation = visual.Compositor.CreateVector4KeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    vector4Animation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Vector4>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = vector4Animation;
            }
            else if (typeof(T) == typeof(Color))
            {
                ColorKeyFrameAnimation colorAnimation = visual.Compositor.CreateColorKeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    colorAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Color>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = colorAnimation;
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                QuaternionKeyFrameAnimation quaternionAnimation = visual.Compositor.CreateQuaternionKeyFrameAnimation();

                foreach (var keyFrame in keyFrames)
                {
                    quaternionAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Quaternion>(),
                        visual.Compositor.CreateCubicBezierEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = quaternionAnimation;
            }
            else
            {
                return ThrowHelper.ThrowInvalidOperationException<CompositionAnimation>("Invalid animation type");
            }

            animation.Duration = duration;

            if (delay.HasValue)
            {
                animation.DelayTime = delay!.Value;
            }

            animation.Target = property;

            return animation;
        }

        /// <summary>
        /// A custom <see cref="NormalizedKeyFrameAnimationBuilder{T}"/> class targeting the composition layer.
        /// </summary>
        public sealed class Composition : NormalizedKeyFrameAnimationBuilder<T>, AnimationBuilder.ICompositionAnimationFactory
        {
            /// <summary>
            /// Initializes a new instance of the <see cref="Composition"/> class.
            /// </summary>
            /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
            public Composition(string property, TimeSpan? delay, TimeSpan duration)
                : base(property, delay, duration)
            {
            }

            /// <inheritdoc/>
            public CompositionAnimation GetAnimation(Visual visual)
            {
                return GetAnimation(
                    visual,
                    this.property,
                    this.delay,
                    this.duration,
                    this.keyFrames);
            }
        }
    }
}
