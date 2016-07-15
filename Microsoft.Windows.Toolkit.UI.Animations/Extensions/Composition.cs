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
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Windows.Toolkit.UI.Animations.Extensions
{
    /// <summary>
    /// These extension methods use composition to perform animation on visuals.
    /// </summary>
    public static class Composition
    {
        /// <summary>
        /// Scales the specified UI Element.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <param name="scaleX">The scale x.</param>
        /// <param name="scaleY">The scale y.</param>
        /// <param name="scaleZ">The scale z.</param>
        public static void Scale(
            this UIElement associatedObject,
            double duration = 0.1d,
            double delay = 0d,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f,
            float scaleX = 0f,
            float scaleY = 0f,
            float scaleZ = 0f)
        {
            var visual = ElementCompositionPreview.GetElementVisual(associatedObject);
            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return;
            }

            var animation = compositor.CreateVector3KeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, new Vector3(scaleX, scaleY, scaleZ));

            visual.CenterPoint = new Vector3(centerX, centerY, centerZ);

            visual.StartAnimation("Scale", animation);
        }

        /// <summary>
        /// Rotates the specified UI Element.
        /// </summary>
        /// <param name="associatedObject">The UI Element to rotate.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="value">The value in degrees to rotate.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        public static void Rotate(
            this UIElement associatedObject,
            double duration = 0.1d,
            double delay = 0d,
            float value = 0f,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f)
        {
            var visual = ElementCompositionPreview.GetElementVisual(associatedObject);
            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return;
            }

            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, value);

            visual.CenterPoint = new Vector3(centerX, centerY, centerZ);

            visual.StartAnimation("RotationAngleInDegrees", animation);
        }

        /// <summary>
        /// Changes the Opacity of the specified UI Element.
        /// </summary>
        /// <param name="associatedObject">The UI Element to change the opacity of.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="value">The value.</param>
        public static void Opacity(
            this UIElement associatedObject,
            double duration = 0.1d,
            double delay = 0d,
            float value = 0f)
        {
            var visual = ElementCompositionPreview.GetElementVisual(associatedObject);
            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return;
            }

            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, value);

            visual.StartAnimation("Opacity", animation);
        }

        /// <summary>
        /// Changes the Offset of the specified UI Element.
        /// </summary>
        /// <param name="associatedObject">The specified UI Element.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="offsetX">The offset x.</param>
        /// <param name="offsetY">The offset y.</param>
        /// <param name="offsetZ">The offset z.</param>
        public static void Offset(
            this UIElement associatedObject,
            double duration = 0.1d,
            double delay = 0d,
            float offsetX = 0f,
            float offsetY = 0f,
            float offsetZ = 0f)
        {
            var visual = ElementCompositionPreview.GetElementVisual(associatedObject);
            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return;
            }

            var animation = compositor.CreateVector3KeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, new Vector3(offsetX, offsetY, offsetZ));

            visual.StartAnimation("Offset", animation);
        }
    }
}