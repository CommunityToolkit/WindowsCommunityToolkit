// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Extensions
{
    /// <summary>
    /// An extension <see langword="class"/> for the <see cref="CompositionObject"/> type.
    /// </summary>
    public static class CompositionObjectExtensions
    {
        /// <summary>
        /// Creates and starts a scalar animation on the current <see cref="CompositionObject"/>.
        /// </summary>
        /// <param name="target">The target to animate.</param>
        /// <param name="propertyPath">The path that identifies the property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="ease">The optional easing function for the animation.</param>
        public static void StartScalarAnimation(
            this CompositionObject target,
            string propertyPath,
            float? from,
            float to,
            TimeSpan duration,
            TimeSpan? delay,
            CompositionEasingFunction? ease = null)
        {
            target.StartAnimation(propertyPath, target.Compositor.CreateScalarKeyFrameAnimation(from, to, duration, delay, ease));
        }

        /// <summary>
        /// Creates and starts a <see cref="Vector2"/> animation on the current <see cref="CompositionObject"/>.
        /// </summary>
        /// <param name="target">The target to animate.</param>
        /// <param name="propertyPath">The path that identifies the property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="ease">The optional easing function for the animation.</param>
        public static void StartVector2Animation(
            this CompositionObject target,
            string propertyPath,
            Vector2? from,
            Vector2 to,
            TimeSpan duration,
            TimeSpan? delay,
            CompositionEasingFunction? ease = null)
        {
            target.StartAnimation(propertyPath, target.Compositor.CreateVector2KeyFrameAnimation(from, to, duration, delay, ease));
        }

        /// <summary>
        /// Creates and starts a <see cref="Vector3"/> animation on the current <see cref="CompositionObject"/>.
        /// </summary>
        /// <param name="target">The target to animate.</param>
        /// <param name="propertyPath">The path that identifies the property to animate.</param>
        /// <param name="from">The optional starting value for the animation.</param>
        /// <param name="to">The final value for the animation.</param>
        /// <param name="duration">The animation duration.</param>
        /// <param name="delay">The optional initial delay for the animation.</param>
        /// <param name="ease">The optional easing function for the animation.</param>
        public static void StartVector3Animation(
            this CompositionObject target,
            string propertyPath,
            Vector3? from,
            Vector3 to,
            TimeSpan duration,
            TimeSpan? delay,
            CompositionEasingFunction? ease = null)
        {
            target.StartAnimation(propertyPath, target.Compositor.CreateVector3KeyFrameAnimation(from, to, duration, delay, ease));
        }
    }
}
