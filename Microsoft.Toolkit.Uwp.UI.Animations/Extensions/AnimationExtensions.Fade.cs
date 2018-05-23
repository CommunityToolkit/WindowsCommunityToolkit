// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Animates the opacity of the UIElement.
        /// </summary>
        /// <param name="associatedObject">The UI Element to change the opacity of.</param>
        /// <param name="value">The fade value, between 0 and 1.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <param name="easingType">Used to describe how the animation interpolates between keyframes.</param>
        /// <param name="easingMode">The easing mode to use to interpolate between keyframes.</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Fade(
            this UIElement associatedObject,
            float value = 0f,
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
            return animationSet.Fade(value, duration, delay, easingType, easingMode);
        }

        /// <summary>
        /// Animates the opacity of the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The fade value, between 0 and 1.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <param name="easingType">Used to describe how the animation interpolates between keyframes.</param>
        /// <param name="easingMode">The EasingMode to use to interpolate between keyframes.</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Fade(
            this AnimationSet animationSet,
            float value = 0f,
            double duration = 500d,
            double delay = 0d,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseOut)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (!AnimationSet.UseComposition)
            {
                var animation = new DoubleAnimation
                {
                    To = value,
                    Duration = TimeSpan.FromMilliseconds(duration),
                    BeginTime = TimeSpan.FromMilliseconds(delay),
                    EasingFunction = GetEasingFunction(easingType, easingMode)
                };

                animationSet.AddStoryboardAnimation("Opacity", animation);
            }
            else
            {
                if (duration <= 0)
                {
                    animationSet.AddCompositionDirectPropertyChange("Opacity", value);
                    return animationSet;
                }

                var visual = animationSet.Visual;

                var compositor = visual?.Compositor;

                if (compositor == null)
                {
                    return null;
                }

                var animation = compositor.CreateScalarKeyFrameAnimation();
                animation.Duration = TimeSpan.FromMilliseconds(duration);
                animation.DelayTime = TimeSpan.FromMilliseconds(delay);
                animation.InsertKeyFrame(1f, value, GetCompositionEasingFunction(easingType, compositor, easingMode));

                animationSet.AddCompositionAnimation("Opacity", animation);
            }

            return animationSet;
        }
    }
}
