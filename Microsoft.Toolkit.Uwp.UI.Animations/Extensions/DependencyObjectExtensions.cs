// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#nullable enable

using System;
using System.Diagnostics.Contracts;
using Windows.Foundation;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Extensions
{
    /// <summary>
    /// An extension <see langword="class"/> for the <see cref="DependencyObject"/> type.
    /// </summary>
    public static class DependencyObjectExtensions
    {
        /// <summary>
        /// Prepares a <see cref="DoubleAnimation"/> with the given info.
        /// </summary>
        /// <param name="target">The target <see cref="DependencyObject"/> to animate.</param>
        /// <param name="property">The property to animate inside the target <see cref="DependencyObject"/>.</param>
        /// <param name="to">The final property value.</param>
        /// <param name="from">The optional initial property value.</param>
        /// <param name="delay">The optional delay for the animation.</param>
        /// <param name="duration">The duration of the <see cref="DoubleAnimation"/>.</param>
        /// <param name="easing">The easing function to use inside the <see cref="DoubleAnimation"/>.</param>
        /// <param name="enableDependecyAnimations">Indicates whether or not to apply this animation to elements that need the visual tree to be rearranged.</param>
        /// <returns>A <see cref="DoubleAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static DoubleAnimation CreateDoubleAnimation(
            this DependencyObject target,
            string property,
            double to,
            double? from,
            TimeSpan? delay,
            TimeSpan duration,
            EasingFunctionBase? easing = null,
            bool enableDependecyAnimations = false)
        {
            DoubleAnimation animation = new()
            {
                To = to,
                From = from,
                BeginTime = delay,
                Duration = duration,
                EasingFunction = easing,
                EnableDependentAnimation = enableDependecyAnimations
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, property);

            return animation;
        }

        /// <summary>
        /// Prepares a <see cref="PointAnimation"/> with the given info.
        /// </summary>
        /// <param name="target">The target <see cref="DependencyObject"/> to animate.</param>
        /// <param name="property">The property to animate inside the target <see cref="DependencyObject"/>.</param>
        /// <param name="to">The final property value.</param>
        /// <param name="from">The optional initial property value.</param>
        /// <param name="delay">The optional delay for the animation.</param>
        /// <param name="duration">The duration of the <see cref="PointAnimation"/>.</param>
        /// <param name="easing">The easing function to use inside the <see cref="PointAnimation"/>.</param>
        /// <param name="enableDependecyAnimations">Indicates whether or not to apply this animation to elements that need the visual tree to be rearranged.</param>
        /// <returns>A <see cref="PointAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static PointAnimation CreatePointAnimation(
            this DependencyObject target,
            string property,
            Point to,
            Point? from,
            TimeSpan? delay,
            TimeSpan duration,
            EasingFunctionBase? easing = null,
            bool enableDependecyAnimations = false)
        {
            PointAnimation animation = new()
            {
                To = to,
                From = from,
                BeginTime = delay,
                Duration = duration,
                EasingFunction = easing,
                EnableDependentAnimation = enableDependecyAnimations
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, property);

            return animation;
        }

        /// <summary>
        /// Prepares a <see cref="ColorAnimation"/> with the given info.
        /// </summary>
        /// <param name="target">The target <see cref="DependencyObject"/> to animate.</param>
        /// <param name="property">The property to animate inside the target <see cref="DependencyObject"/>.</param>
        /// <param name="to">The final property value.</param>
        /// <param name="from">The optional initial property value.</param>
        /// <param name="delay">The optional delay for the animation.</param>
        /// <param name="duration">The duration of the <see cref="ColorAnimation"/>.</param>
        /// <param name="easing">The easing function to use inside the <see cref="ColorAnimation"/>.</param>
        /// <returns>A <see cref="ColorAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static ColorAnimation CreateColorAnimation(
            this DependencyObject target,
            string property,
            Color to,
            Color? from,
            TimeSpan? delay,
            TimeSpan duration,
            EasingFunctionBase? easing = null)
        {
            ColorAnimation animation = new()
            {
                To = to,
                From = from,
                BeginTime = delay,
                Duration = duration,
                EasingFunction = easing
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, property);

            return animation;
        }
    }
}
