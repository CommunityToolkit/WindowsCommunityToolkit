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
using Microsoft.Graphics.Canvas.Effects;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods use composition to perform animation on visuals.
    /// </summary>
    public static class Composition
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
                animationSet.AddDirectPropertyChange("Scale", scaleVector);
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

            animationSet.AddAnimation("Scale", animation);

            return animationSet;
        }

        /// <summary>
        /// Animates the rotation in degrees of the the UIElement.
        /// </summary>
        /// <param name="associatedObject">The UI Element to rotate.</param>
        /// <param name="value">The value in degrees to rotate.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Rotate(
            this UIElement associatedObject,
            float value = 0f,
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
            return animationSet.Rotate(value, centerX, centerY, centerZ, duration, delay);
        }

        /// <summary>
        /// Animates the rotation in degrees of the the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The value in degrees to rotate.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Rotate(
            this AnimationSet animationSet,
            float value = 0f,
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

            if (duration <= 0)
            {
                animationSet.AddDirectPropertyChange("RotationAngleInDegrees", value);
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
            animation.InsertKeyFrame(1f, value);

            animationSet.AddAnimation("RotationAngleInDegrees", animation);

            return animationSet;
        }

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

            if (duration <= 0)
            {
                animationSet.AddDirectPropertyChange("Opacity", value);
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

            animationSet.AddAnimation("Opacity", animation);

            return animationSet;
        }

        /// <summary>
        /// Animates the offset of the the UIElement.
        /// </summary>
        /// <param name="associatedObject">The specified UI Element.</param>
        /// <param name="offsetX">The offset on the x axis.</param>
        /// <param name="offsetY">The offset on the y axis.</param>
        /// <param name="offsetZ">The offset on the z axis.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Offset(
            this UIElement associatedObject,
            float offsetX = 0f,
            float offsetY = 0f,
            float offsetZ = 0f,
            double duration = 500d,
            double delay = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Offset(offsetX, offsetY, offsetZ, duration, delay);
        }

        /// <summary>
        /// Animates the offset of the the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="offsetX">The offset on the x axis.</param>
        /// <param name="offsetY">The offset on the y axis.</param>
        /// <param name="offsetZ">The offset on the z axis.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds. (ignored if duration == 0)</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet Offset(
            this AnimationSet animationSet,
            float offsetX = 0f,
            float offsetY = 0f,
            float offsetZ = 0f,
            double duration = 500d,
            double delay = 0d)
        {
            if (animationSet == null)
            {
                return null;
            }

            var visual = animationSet.Visual;
            var offsetVector = new Vector3(offsetX, offsetY, offsetZ);

            if (duration <= 0)
            {
                animationSet.AddDirectPropertyChange("Offset", offsetVector);
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
            animation.InsertKeyFrame(1f, offsetVector);

            animationSet.AddAnimation("Offset", animation);

            return animationSet;
        }

        /// <summary>
        /// Gets a value indicating whether the platform supports blur.
        /// </summary>
        /// <remarks>
        /// A check should always be made to IsBlurSupported prior to calling Blur/>,
        /// since older operating systems will not support blurs.
        /// </remarks>
        /// <seealso cref="Blur(FrameworkElement, double, double, double)"/>
        public static bool IsBlurSupported =>
            ApiInformation.IsMethodPresent(typeof(Compositor).FullName, nameof(Compositor.CreateEffectFactory));

        /// <summary>
        /// Animates the gaussian blur of the the UIElement.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="value">The blur amount.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <returns>
        /// An Animation Set.
        /// </returns>
        /// <seealso cref="IsBlurSupported" />
        public static AnimationSet Blur(
            this FrameworkElement associatedObject,
            double value = 0d,
            double duration = 500d,
            double delay = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Blur(duration, delay, value);
        }

        /// <summary>
        /// Animates the gaussian blur of the the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The blur amount.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay. (ignored if duration == 0)</param>
        /// <returns>
        /// An Animation Set.
        /// </returns>
        /// <seealso cref="IsBlurSupported" />
        public static AnimationSet Blur(
            this AnimationSet animationSet,
            double value = 0d,
            double duration = 500d,
            double delay = 0d)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (!IsBlurSupported)
            {
                // The operating system doesn't support blur.
                // Fail gracefully by not applying blur.
                // See 'IsBlurSupported' property
                return null;
            }

            var visual = animationSet.Visual;
            var associatedObject = animationSet.Element as FrameworkElement;

            if (associatedObject == null)
            {
                return animationSet;
            }

            var compositor = visual?.Compositor;
            const string blurName = "Blur";

            if (compositor == null)
            {
                return null;
            }

            // check to see if the visual already has a blur applied.
            var spriteVisual = ElementCompositionPreview.GetElementChildVisual(associatedObject) as SpriteVisual;
            var blurBrush = spriteVisual?.Brush as CompositionEffectBrush;

            if (blurBrush == null || blurBrush.Comment != blurName)
            {
                var blurEffect = new GaussianBlurEffect
                {
                    Name = blurName,
                    BlurAmount = 0f,
                    Optimization = EffectOptimization.Balanced,
                    BorderMode = EffectBorderMode.Hard,
                    Source = new CompositionEffectSourceParameter("source")
                };

                // Create a brush to which I want to apply. I also have noted that BlurAmount should be left out of the compiled shader.
                blurBrush = compositor.CreateEffectFactory(blurEffect, new[] { $"{blurName}.BlurAmount" }).CreateBrush();
                blurBrush.Comment = blurName;

                // Set the source of the blur as a backdrop brush
                blurBrush.SetSourceParameter("source", compositor.CreateBackdropBrush());

                var blurSprite = compositor.CreateSpriteVisual();
                blurSprite.Brush = blurBrush;
                ElementCompositionPreview.SetElementChildVisual(associatedObject, blurSprite);

                blurSprite.Size = new Vector2((float)associatedObject.ActualWidth, (float)associatedObject.ActualHeight);

                associatedObject.SizeChanged += (s, e) =>
                {
                    blurSprite.Size = new Vector2((float)associatedObject.ActualWidth, (float)associatedObject.ActualHeight);
                };
            }

            if (duration <= 0)
            {
                animationSet.AddEffectDirectPropertyChange(blurBrush, (float)value, $"{blurName}.BlurAmount");
            }
            else
            {
                // Create an animation to change the blur amount over time
                var blurAnimation = compositor.CreateScalarKeyFrameAnimation();
                blurAnimation.InsertKeyFrame(1f, (float)value);
                blurAnimation.Duration = TimeSpan.FromMilliseconds(duration);
                blurAnimation.DelayTime = TimeSpan.FromMilliseconds(delay);

                animationSet.AddEffectAnimation(blurBrush, blurAnimation, $"{blurName}.BlurAmount");
            }

            return animationSet;
        }

        /// <summary>
        /// Creates a Parallax effect on the specified element based on the supplied scroller element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="scrollerElement">The scroller element.</param>
        /// <param name="isHorizontalEffect">if set to <c>true</c> [is horizontal effect].</param>
        /// <param name="multiplier">The multiplier (how fast it scrolls).</param>
        public static void Parallax(this UIElement element, FrameworkElement scrollerElement, bool isHorizontalEffect, float multiplier)
        {
            if (scrollerElement == default(FrameworkElement))
            {
                return;
            }

            var scroller = scrollerElement as ScrollViewer;
            if (scroller == null)
            {
                scroller = scrollerElement.FindDescendant<ScrollViewer>();
                if (scroller == null)
                {
                    return;
                }
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            var manipulationProperty = isHorizontalEffect ? "X" : "Y";
            var expression = compositor.CreateExpressionAnimation($"ScrollManipululation.Translation.{manipulationProperty} * ParallaxMultiplier");

            expression.SetScalarParameter("ParallaxMultiplier", multiplier);
            expression.SetReferenceParameter("ScrollManipululation", scrollerViewerManipulation);

            Visual textVisual = ElementCompositionPreview.GetElementVisual(element);
            textVisual.StartAnimation($"Offset.{manipulationProperty}", expression);
        }
    }
}