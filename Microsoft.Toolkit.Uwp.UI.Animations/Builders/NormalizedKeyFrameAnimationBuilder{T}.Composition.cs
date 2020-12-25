// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Microsoft.Toolkit.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Animations.Extensions;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
    internal abstract partial class NormalizedKeyFrameAnimationBuilder<T>
    {
        /// <summary>
        /// Gets a <see cref="CompositionAnimation"/> instance representing the animation to start.
        /// </summary>
        /// <typeparam name="TKeyFrame">The type of keyframes being used to define the animation.</typeparam>
        /// <param name="target">The target <see cref="CompositionObject"/> instance to animate.</param>
        /// <param name="property">The target property to animate.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="keyFrames">The list of keyframes to use to build the animation.</param>
        /// <returns>A <see cref="CompositionAnimation"/> instance with the specified animation.</returns>
        public static CompositionAnimation GetAnimation<TKeyFrame>(
            CompositionObject target,
            string property,
            TimeSpan? delay,
            TimeSpan duration,
            ReadOnlySpan<TKeyFrame> keyFrames)
            where TKeyFrame : struct, IKeyFrameInfo
        {
            KeyFrameAnimation animation;

            if (typeof(T) == typeof(bool))
            {
                BooleanKeyFrameAnimation boolAnimation = target.Compositor.CreateBooleanKeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(boolAnimation, duration))
                    {
                        continue;
                    }

                    boolAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<bool>());
                }

                animation = boolAnimation;
            }
            else if (typeof(T) == typeof(float))
            {
                ScalarKeyFrameAnimation scalarAnimation = target.Compositor.CreateScalarKeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(scalarAnimation, duration))
                    {
                        continue;
                    }

                    scalarAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<float>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = scalarAnimation;
            }
            else if (typeof(T) == typeof(double))
            {
                ScalarKeyFrameAnimation scalarAnimation = target.Compositor.CreateScalarKeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(scalarAnimation, duration))
                    {
                        continue;
                    }

                    scalarAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        (float)keyFrame.GetValueAs<double>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = scalarAnimation;
            }
            else if (typeof(T) == typeof(Vector2))
            {
                Vector2KeyFrameAnimation vector2Animation = target.Compositor.CreateVector2KeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(vector2Animation, duration))
                    {
                        continue;
                    }

                    vector2Animation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Vector2>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = vector2Animation;
            }
            else if (typeof(T) == typeof(Vector3))
            {
                Vector3KeyFrameAnimation vector3Animation = target.Compositor.CreateVector3KeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(vector3Animation, duration))
                    {
                        continue;
                    }

                    vector3Animation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Vector3>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = vector3Animation;
            }
            else if (typeof(T) == typeof(Vector4))
            {
                Vector4KeyFrameAnimation vector4Animation = target.Compositor.CreateVector4KeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(vector4Animation, duration))
                    {
                        continue;
                    }

                    vector4Animation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Vector4>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = vector4Animation;
            }
            else if (typeof(T) == typeof(Color))
            {
                ColorKeyFrameAnimation colorAnimation = target.Compositor.CreateColorKeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(colorAnimation, duration))
                    {
                        continue;
                    }

                    colorAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Color>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
                }

                animation = colorAnimation;
            }
            else if (typeof(T) == typeof(Quaternion))
            {
                QuaternionKeyFrameAnimation quaternionAnimation = target.Compositor.CreateQuaternionKeyFrameAnimation();

                foreach (ref readonly var keyFrame in keyFrames)
                {
                    if (keyFrame.TryInsertExpressionKeyFrame(quaternionAnimation, duration))
                    {
                        continue;
                    }

                    quaternionAnimation.InsertKeyFrame(
                        (float)keyFrame.GetNormalizedProgress(duration),
                        keyFrame.GetValueAs<Quaternion>(),
                        target.Compositor.CreateEasingFunction(keyFrame.EasingType, keyFrame.EasingMode));
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
            /// Initializes a new instance of the <see cref="NormalizedKeyFrameAnimationBuilder{T}.Composition"/> class.
            /// </summary>
            /// <inheritdoc cref="NormalizedKeyFrameAnimationBuilder{T}"/>
            public Composition(string property, TimeSpan? delay, TimeSpan duration)
                : base(property, delay, duration)
            {
            }

            /// <inheritdoc/>
            public override INormalizedKeyFrameAnimationBuilder<T> ExpressionKeyFrame(
                double progress,
                string expression,
                EasingType easingType,
                EasingMode easingMode)
            {
                this.keyFrames.Append(new(progress, expression, easingType, easingMode));

                return this;
            }

            /// <inheritdoc/>
            public CompositionAnimation GetAnimation(CompositionObject targetHint, out CompositionObject? target)
            {
                target = null;

                return GetAnimation(
                    targetHint,
                    this.property,
                    this.delay,
                    this.duration,
                    this.keyFrames.AsSpan());
            }
        }
    }
}
