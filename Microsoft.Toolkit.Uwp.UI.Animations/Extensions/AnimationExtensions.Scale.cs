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
using System.Numerics;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Animates the scale of the the specified UIElement.
        /// </summary>
        /// <param name="associatedObject">The associated UIElement.</param>
        /// <param name="scaleX">The scale on the x axis.</param>
        /// <param name="scaleY">The scale on the y axis.</param>
        /// <param name="scaleZ">The scale on the z axis.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <param name="duration">The duration in millisecond.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Scale(
            this UIElement associatedObject,
            float scaleX = 0f,
            float scaleY = 0f,
            float scaleZ = 0f,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f,
            double duration = 500d,
            double delay = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Scale(scaleX, scaleY, scaleZ, centerX, centerY, centerZ, duration, delay);
        }

        /// <summary>
        /// Animates the scale of the the specified UIElement.
        /// </summary>
        /// <param name="animationSet">The animationSet object.</param>
        /// <param name="scaleX">The scale on the x axis.</param>
        /// <param name="scaleY">The scale on the y axis.</param>
        /// <param name="scaleZ">The scale on the z axis.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Scale(
            this AnimationSet animationSet,
            float scaleX = 0f,
            float scaleY = 0f,
            float scaleZ = 0f,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f,
            double duration = 500d,
            double delay = 0d)
        {
            if (animationSet == null)
            {
                return null;
            }

            var visual = animationSet.Visual;
            visual.CenterPoint = new Vector3(centerX, centerY, centerZ);
            var scaleVector = new Vector3(scaleX, scaleY, scaleZ);

            if (duration <= 0)
            {
                animationSet.AddCompositionDirectPropertyChange("Scale", scaleVector);
                return animationSet;
            }

            var compositor = visual.Compositor;

            if (compositor == null)
            {
                return null;
            }

            var animation = compositor.CreateVector3KeyFrameAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(duration);
            animation.DelayTime = TimeSpan.FromMilliseconds(delay);
            animation.InsertKeyFrame(1f, scaleVector);

            animationSet.AddCompositionAnimation("Scale", animation);

            return animationSet;
        }
    }
}
