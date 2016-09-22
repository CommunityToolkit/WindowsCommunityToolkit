// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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
        /// Animates the opacity of the the UIElement.
        /// </summary>
        /// <param name="associatedObject">The UI Element to change the opacity of.</param>
        /// <param name="value">The fade value, between 0 and 1.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Fade(
            this UIElement associatedObject,
            float value = 0f,
            double duration = 500d,
            double delay = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Fade(value, duration, delay);
        }

        /// <summary>
        /// Animates the opacity of the the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The fade value, between 0 and 1.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Fade(
            this AnimationSet animationSet,
            float value = 0f,
            double duration = 500d,
            double delay = 0d)
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
                    EasingFunction = _defaultStoryboardEasingFunction
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
                animation.InsertKeyFrame(1f, value);

                animationSet.AddCompositionAnimation("Opacity", animation);
            }

            return animationSet;
        }
    }
}
