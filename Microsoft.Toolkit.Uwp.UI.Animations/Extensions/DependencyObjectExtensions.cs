// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics.Contracts;
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
        /// <param name="from">The optional initial property value.</param>
        /// <param name="to">The final property value.</param>
        /// <param name="duration">The duration of the <see cref="DoubleAnimation"/>.</param>
        /// <param name="easing">The easing function to use inside the <see cref="DoubleAnimation"/>.</param>
        /// <param name="enableDependecyAnimations">Indicates whether or not to apply this animation to elements that need the visual tree to be rearranged.</param>
        /// <returns>A <see cref="DoubleAnimation"/> instance with the specified parameters.</returns>
        [Pure]
        public static DoubleAnimation CreateDoubleAnimation(
            this DependencyObject target,
            string property,
            double? from,
            double to,
            TimeSpan duration,
            EasingFunctionBase? easing = null,
            bool enableDependecyAnimations = false)
        {
            DoubleAnimation animation = new()
            {
                From = from,
                To = to,
                Duration = duration,
                EasingFunction = easing,
                EnableDependentAnimation = enableDependecyAnimations,
            };

            Storyboard.SetTarget(animation, target);
            Storyboard.SetTargetProperty(animation, property);

            return animation;
        }
    }
}
