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
        /// Animates the rotation in degrees of the UIElement.
        /// </summary>
        /// <param name="associatedObject">The UI Element to rotate.</param>
        /// <param name="value">The value in degrees to rotate.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <param name="easingType">Used to describe how the animation interpolates between keyframes.</param>
        /// <param name="easingMode">EasingMode used to interpolate between keyframes.</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Rotate(
            this UIElement associatedObject,
            float value = 0f,
            float centerX = 0f,
            float centerY = 0f,
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
            return animationSet.Rotate(value, centerX, centerY, duration, delay, easingType);
        }

        /// <summary>
        /// Animates the rotation in degrees of the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The value in degrees to rotate.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <param name="easingType">Used to describe how the animation interpolates between keyframes.</param>
        /// <param name="easingMode">The EasingMode to use to interpolate between keyframes.</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Rotate(
            this AnimationSet animationSet,
            float value = 0f,
            float centerX = 0f,
            float centerY = 0f,
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

                transform.CenterX = centerX;
                transform.CenterY = centerY;

                var animation = new DoubleAnimation
                {
                    To = value,
                    Duration = TimeSpan.FromMilliseconds(duration),
                    BeginTime = TimeSpan.FromMilliseconds(delay),
                    EasingFunction = GetEasingFunction(easingType, easingMode)
                };

                animationSet.AddStoryboardAnimation(GetAnimationPath(transform, element, "Rotation"), animation);
            }
            else
            {
                var visual = animationSet.Visual;
                visual.CenterPoint = new Vector3(centerX, centerY, 0);

                if (duration <= 0)
                {
                    animationSet.AddCompositionDirectPropertyChange("RotationAngleInDegrees", value);
                    return animationSet;
                }

                var compositor = visual.Compositor;

                if (compositor == null)
                {
                    return null;
                }

                var animation = compositor.CreateScalarKeyFrameAnimation();
                animation.Duration = TimeSpan.FromMilliseconds(duration);
                animation.DelayTime = TimeSpan.FromMilliseconds(delay);
                animation.InsertKeyFrame(1f, value, GetCompositionEasingFunction(easingType, compositor, easingMode));

                animationSet.AddCompositionAnimation("RotationAngleInDegrees", animation);
            }

            return animationSet;
        }
    }
}
