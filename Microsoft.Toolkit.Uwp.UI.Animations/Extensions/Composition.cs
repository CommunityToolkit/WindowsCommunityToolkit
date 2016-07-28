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
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Extensions
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
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Scale(
            this UIElement associatedObject,
            double duration = 0.5d,
            double delay = 0d,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f,
            float scaleX = 0f,
            float scaleY = 0f,
            float scaleZ = 0f)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Scale(duration, delay, centerX, centerY, centerZ, scaleX, scaleY, scaleZ);
        }

        /// <summary>
        /// Scales the specified UI Element.
        /// </summary>
        /// <param name="animationSet">The animationSet object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <param name="scaleX">The scale x.</param>
        /// <param name="scaleY">The scale y.</param>
        /// <param name="scaleZ">The scale z.</param>
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Scale(
            this AnimationSet animationSet,
            double duration = 0.5d,
            double delay = 0d,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f,
            float scaleX = 0f,
            float scaleY = 0f,
            float scaleZ = 0f)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException("The argument duration must be greater than 0.");
            }

            var visual = animationSet.Visual;
            visual.CenterPoint = new Vector3(centerX, centerY, centerZ);
            var scaleVector = new Vector3(scaleX, scaleY, scaleZ);

            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return null;
            }

            var animation = compositor.CreateVector3KeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, scaleVector);

            animationSet.AddAnimation("Scale", animation);

            return animationSet;
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
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Rotate(
            this UIElement associatedObject,
            double duration = 0.5d,
            double delay = 0d,
            float value = 0f,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Rotate(duration, delay, value, centerX, centerY, centerZ);
        }

        /// <summary>
        /// Rotates the specified UI Element.
        /// </summary>
        /// <param name="animationSet">The animationSet object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="value">The value in degrees to rotate.</param>
        /// <param name="centerX">The center x in pixels.</param>
        /// <param name="centerY">The center y in pixels.</param>
        /// <param name="centerZ">The center z in pixels.</param>
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Rotate(
            this AnimationSet animationSet,
            double duration = 0.5d,
            double delay = 0d,
            float value = 0f,
            float centerX = 0f,
            float centerY = 0f,
            float centerZ = 0f)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException("The argument duration must be greater than 0.");
            }

            var visual = animationSet.Visual;
            visual.CenterPoint = new Vector3(centerX, centerY, centerZ);

            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return null;
            }

            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, value);

            animationSet.AddAnimation("RotationAngleInDegrees", animation);

            return animationSet;
        }

        /// <summary>
        /// Changes the Opacity of the specified UI Element.
        /// </summary>
        /// <param name="associatedObject">The UI Element to change the opacity of.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="value">The value.</param>
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Opacity(
            this UIElement associatedObject,
            double duration = 0.5d,
            double delay = 0d,
            float value = 0f)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Opacity(duration, delay, value);
        }

        /// <summary>
        /// Changes the Opacity of the specified UI Element.
        /// </summary>
        /// <param name="animationSet">The animationSet object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="value">The value.</param>
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Opacity(
            this AnimationSet animationSet,
            double duration = 0.5d,
            double delay = 0d,
            float value = 0f)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException("The argument duration must be greater than 0.");
            }

            var visual = animationSet.Visual;

            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return null;
            }

            var animation = compositor.CreateScalarKeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, value);

            animationSet.AddAnimation("Opacity", animation);

            return animationSet;
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
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Offset(
            this UIElement associatedObject,
            double duration = 0.5d,
            double delay = 0d,
            float offsetX = 0f,
            float offsetY = 0f,
            float offsetZ = 0f)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Offset(duration, delay, offsetX, offsetY, offsetZ);
        }

        /// <summary>
        /// Changes the Offset of the specified UI Element.
        /// </summary>
        /// <param name="animationSet">The animationSet object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="offsetX">The offset x.</param>
        /// <param name="offsetY">The offset y.</param>
        /// <param name="offsetZ">The offset z.</param>
        /// <returns>The visual of the UIElement.</returns>
        public static AnimationSet Offset(
            this AnimationSet animationSet,
            double duration = 0.5d,
            double delay = 0d,
            float offsetX = 0f,
            float offsetY = 0f,
            float offsetZ = 0f)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (duration <= 0)
            {
                throw new ArgumentOutOfRangeException("The argument duration must be greater than 0.");
            }

            var visual = animationSet.Visual;
            var offsetVector = new Vector3(offsetX, offsetY, offsetZ);

            var compositor = visual?.Compositor;

            if (compositor == null)
            {
                return null;
            }

            var animation = compositor.CreateVector3KeyFrameAnimation();
            animation.Duration = TimeSpan.FromSeconds(duration);
            animation.DelayTime = TimeSpan.FromSeconds(delay);
            animation.InsertKeyFrame(1f, offsetVector);

            animationSet.AddAnimation("Offset", animation);

            return animationSet;
        }

        /// <summary>
        /// Gets a value indicating whether the platform supports blur.
        /// </summary>
        /// <remarks>
        /// A check should always be made to IsBlurSupported prior to calling <seealso cref="Blur"/>,
        /// since older operating systems will not support blurs.
        /// </remarks>
        /// <seealso cref="Blur(FrameworkElement, double, double, double)"/>
        public static bool IsBlurSupported
        {
            get
            {
                return global::Windows.Foundation.Metadata.ApiInformation.IsMethodPresent(typeof(Compositor).FullName, nameof(Compositor.CreateEffectFactory));
            }
        }

        /// <summary>
        /// Blurs the specified framework element.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="blurAmount">The blur amount.</param>
        /// <returns>The Composition Effect Brush of the blur so you can control animations manually.</returns>
        /// <seealso cref="IsBlurSupported" />
        public static AnimationSet Blur(
            this FrameworkElement associatedObject,
            double duration = 0.5d,
            double delay = 0d,
            double blurAmount = 0d)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.Blur(duration, delay, blurAmount);
        }

        /// <summary>
        /// Blurs the specified framework element.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="delay">The delay.</param>
        /// <param name="blurAmount">The blur amount.</param>
        /// <returns>The Composition Effect Brush of the blur so you can control animations manually.</returns>
        /// <seealso cref="IsBlurSupported" />
        public static AnimationSet Blur(
            this AnimationSet animationSet,
            double duration = 0.5d,
            double delay = 0d,
            double blurAmount = 0d)
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

            // Create an animation to change the blur amount over time
            var blurAnimation = compositor.CreateScalarKeyFrameAnimation();
            blurAnimation.InsertKeyFrame(1f, (float)blurAmount);
            blurAnimation.Duration = TimeSpan.FromSeconds(duration);
            blurAnimation.DelayTime = TimeSpan.FromSeconds(delay);

            // check to see if the visual already has a blur applied.
            var spriteVisual = ElementCompositionPreview.GetElementChildVisual(associatedObject) as SpriteVisual;
            var blurBrush = spriteVisual?.Brush as CompositionEffectBrush;

            if (blurBrush != null)
            {
                if (blurBrush.Comment == blurName)
                {
                    animationSet.AddEffectAnimation(blurBrush, blurAnimation, $"{blurName}.BlurAmount");
                    return animationSet;
                }
            }

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
            animationSet.AddEffectAnimation(blurBrush, blurAnimation, $"{blurName}.BlurAmount");

            associatedObject.SizeChanged += (s, e) =>
            {
                blurSprite.Size = new Vector2((float)associatedObject.ActualWidth, (float)associatedObject.ActualHeight);
                blurBrush.StartAnimation($"{blurName}.BlurAmount", blurAnimation);
            };

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