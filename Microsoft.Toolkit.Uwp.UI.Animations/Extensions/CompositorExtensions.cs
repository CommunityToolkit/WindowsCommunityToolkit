// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Diagnostics.Contracts;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Extensions
{
    /// <summary>
    /// An extension <see langword="class"/> for the <see cref="Compositor"/> type.
    /// </summary>
    public static class CompositorExtensions
    {
        /// <summary>
        /// Creates a <see cref="CubicBezierEasingFunction"/> from the input control points.
        /// </summary>
        /// <param name="compositor">The source <see cref="CompositionObject"/> used to create the easing function.</param>
        /// <param name="x1">The X coordinate of the first control point.</param>
        /// <param name="y1">The Y coordinate of the first control point.</param>
        /// <param name="x2">The X coordinate of the second control point.</param>
        /// <param name="y2">The Y coordinate of the second control point.</param>
        /// <returns>A <see cref="CubicBezierEasingFunction"/> instance with the given control points.</returns>
        [Pure]
        public static CubicBezierEasingFunction CreateCubicBezierEasingFunction(this Compositor compositor, float x1, float y1, float x2, float y2)
        {
            return compositor.CreateCubicBezierEasingFunction(new(x1, y1), new(x2, y2));
        }

        /// <summary>
        /// Creates the appropriate <see cref="CubicBezierEasingFunction"/> from the given easing type and mode.
        /// </summary>
        /// <param name="compositor">The source <see cref="Compositor"/> used to create the easing function.</param>
        /// <param name="easingType">The target easing function to use.</param>
        /// <param name="easingMode">The target easing mode to use.</param>
        /// <returns>A <see cref="CubicBezierEasingFunction"/> instance with the specified easing.</returns>
        [Pure]
        public static CubicBezierEasingFunction CreateCubicBezierEasingFunction(this Compositor compositor, EasingType easingType, EasingMode easingMode)
        {
            var (a, b) = AnimationExtensions.EasingMaps[(easingType, easingMode)];

            return compositor.CreateCubicBezierEasingFunction(a, b);
        }

        /// <summary>
        /// Creates a <see cref="ScalarKeyFrameAnimation"/> instance with the given parameters to on a target element.
        /// </summary>
        /// <param name="compositor">The current <see cref="Compositor"/> instance used to create the animation.</param>
        /// <param name="target">The optional target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="ease">The optional easing function for the animation.</param>
        /// <returns>A <see cref="ScalarKeyFrameAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static ScalarKeyFrameAnimation CreateScalarKeyFrameAnimation(
            this Compositor compositor,
            string? target,
            float? from,
            float to,
            TimeSpan duration,
            TimeSpan? delay,
            CompositionEasingFunction? ease = null)
        {
            ScalarKeyFrameAnimation animation = compositor.CreateScalarKeyFrameAnimation();

            animation.Duration = duration;

            if (delay.HasValue)
            {
                animation.DelayTime = delay.Value;
            }

            animation.InsertKeyFrame(1, to, ease ?? compositor.CreateLinearEasingFunction());

            if (from.HasValue)
            {
                animation.InsertKeyFrame(0, from.Value);
            }

            animation.Target = target;

            return animation;
        }

        /// <summary>
        /// Creates a <see cref="ScalarKeyFrameAnimation"/> instance with the given parameters to on a target element.
        /// </summary>
        /// <param name="compositor">The current <see cref="Compositor"/> instance used to create the animation.</param>
        /// <param name="target">The optional target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="ease">The optional easing function for the animation.</param>
        /// <returns>A <see cref="Vector2KeyFrameAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static Vector2KeyFrameAnimation CreateVector2KeyFrameAnimation(
            this Compositor compositor,
            string? target,
            Vector2? from,
            Vector2 to,
            TimeSpan duration,
            TimeSpan? delay,
            CompositionEasingFunction? ease = null)
        {
            Vector2KeyFrameAnimation animation = compositor.CreateVector2KeyFrameAnimation();

            animation.Duration = duration;

            if (delay.HasValue)
            {
                animation.DelayTime = delay.Value;
            }

            animation.InsertKeyFrame(1, to, ease ?? compositor.CreateLinearEasingFunction());

            if (from.HasValue)
            {
                animation.InsertKeyFrame(0, from.Value);
            }

            animation.Target = target;

            return animation;
        }

        /// <summary>
        /// Creates a <see cref="ScalarKeyFrameAnimation"/> instance with the given parameters to on a target element.
        /// </summary>
        /// <param name="compositor">The current <see cref="Compositor"/> instance used to create the animation.</param>
        /// <param name="target">The optional target property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="ease">The optional easing function for the animation.</param>
        /// <returns>A <see cref="Vector3KeyFrameAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static Vector3KeyFrameAnimation CreateVector3KeyFrameAnimation(
            this Compositor compositor,
            string? target,
            Vector3? from,
            Vector3 to,
            TimeSpan duration,
            TimeSpan? delay,
            CompositionEasingFunction? ease = null)
        {
            Vector3KeyFrameAnimation animation = compositor.CreateVector3KeyFrameAnimation();

            animation.Duration = duration;

            if (delay.HasValue)
            {
                animation.DelayTime = delay.Value;
            }

            animation.InsertKeyFrame(1, to, ease ?? compositor.CreateLinearEasingFunction());

            if (from.HasValue)
            {
                animation.InsertKeyFrame(0, from.Value);
            }

            animation.Target = target;

            return animation;
        }
    }
}
