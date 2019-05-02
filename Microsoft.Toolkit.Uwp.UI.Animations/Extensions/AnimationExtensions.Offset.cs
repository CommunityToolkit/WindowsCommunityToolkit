// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
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
        /// Animates the offset of the UIElement.
        /// </summary>
        /// <param name="associatedObject">The specified UI Element.</param>
        /// <param name="offsetX">The offset on the x axis.</param>
        /// <param name="offsetY">The offset on the y axis.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <param name="easingType">Used to describe how the animation interpolates between keyframes.</param>
        /// <param name="easingMode">The EasingMode to use to interpolate between keyframes.</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Offset(
            this UIElement associatedObject,
            float offsetX = 0f,
            float offsetY = 0f,
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
            return animationSet.Offset(offsetX, offsetY, duration, delay, easingType, easingMode);
        }

        /// <summary>
        /// Animates the offset of the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="offsetX">The offset on the x axis.</param>
        /// <param name="offsetY">The offset on the y axis.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <param name="easingType">Used to describe how the animation interpolates between keyframes.</param>
        /// <param name="easingMode">The EasingMode to use to interpolate between keyframes.</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Offset(
            this AnimationSet animationSet,
            float offsetX = 0f,
            float offsetY = 0f,
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
                var element = animationSet.Element;
                var transform = GetAttachedCompositeTransform(element);

                var animationX = new DoubleAnimation();
                var animationY = new DoubleAnimation();

                animationX.To = offsetX;
                animationY.To = offsetY;

                animationX.Duration = animationY.Duration = TimeSpan.FromMilliseconds(duration);
                animationX.BeginTime = animationY.BeginTime = TimeSpan.FromMilliseconds(delay);
                animationX.EasingFunction = animationY.EasingFunction = GetEasingFunction(easingType, easingMode);

                animationSet.AddStoryboardAnimation(GetAnimationPath(transform, element, "TranslateX"), animationX);
                animationSet.AddStoryboardAnimation(GetAnimationPath(transform, element, "TranslateY"), animationY);
            }
            else
            {
                var visual = animationSet.Visual;
                var offsetVector = new Vector3(offsetX, offsetY, 0);

                if (duration <= 0)
                {
                    animationSet.AddCompositionDirectPropertyChange("Offset", offsetVector);
                    return animationSet;
                }

                var compositor = visual?.Compositor;

                if (compositor == null)
                {
                    return null;
                }

                var animation = compositor.CreateVector3KeyFrameAnimation();
                animation.Duration = TimeSpan.FromMilliseconds(duration);
                animation.DelayTime = TimeSpan.FromMilliseconds(delay);
                animation.InsertKeyFrame(1f, offsetVector, GetCompositionEasingFunction(easingType, compositor, easingMode));

                animationSet.AddCompositionAnimation("Offset", animation);
            }

            return animationSet;
        }
    }
}
