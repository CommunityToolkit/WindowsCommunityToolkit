// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Animations.Effects;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A partial for the AnimationExtension which includes saturation.
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets the saturation effect.
        /// </summary>
        /// <value>
        /// The saturation effect.
        /// </value>
        public static Saturation SaturationEffect { get; } = new Saturation();

        /// <summary>
        /// Saturates the FrameworkElement.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="value">The value, between 0 and 1. 0 is desaturated, 1 is saturated.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="easingType">The <see cref="EasingType"/></param>
        /// <param name="easingMode">The <see cref="EasingMode"/></param>
        /// <returns>An animation set with saturation effects incorporated.</returns>
        public static AnimationSet Saturation(
            this FrameworkElement associatedObject,
            double value = 0d,
            double duration = 500d,
            double delay = 0d,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseOut)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Saturation(value, duration, delay, easingType, easingMode);
        }

        /// <summary>
        /// Saturates the visual within the animation set.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The value. 0 is desaturated, 1 is saturated.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="easingType">The <see cref="EasingType"/></param>
        /// <param name="easingMode">The <see cref="EasingMode"/></param>
        /// <returns>An animation set with saturation effects incorporated.</returns>
        public static AnimationSet Saturation(
            this AnimationSet animationSet,
            double value = 0d,
            double duration = 500d,
            double delay = 0d,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseOut)
        {
            return SaturationEffect.EffectAnimation(animationSet, value, duration, delay, easingType, easingMode);
        }
    }
}