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
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the platform supports blur.
        /// </summary>
        /// <remarks>
        /// A check should always be made to IsBlurSupported prior to calling Blur,
        /// since older operating systems will not support blurs.
        /// </remarks>
        /// <seealso cref="Blur(FrameworkElement, double, double, double)"/>
        public static bool IsBlurSupported =>
            ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

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
            return animationSet.Blur(value, duration, delay);
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

                animationSet.AddCompositionEffectAnimation(blurBrush, blurAnimation, $"{blurName}.BlurAmount");
            }

            return animationSet;
        }
    }
}
