// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Media.Animation;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Effects
{
    /// <summary>
    /// An abstract class that provides the mechanism to create
    /// an effect using composition.
    /// </summary>
    public abstract class AnimationEffect
    {
        private static string[] _effectProperties;

        /// <summary>
        /// Gets a value indicating whether this instance is supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is supported; otherwise, <c>false</c>.
        /// </value>
        public abstract bool IsSupported { get; }

        /// <summary>
        /// Gets the name of the effect.
        /// </summary>
        /// <value>
        /// The name of the effect.
        /// </value>
        public abstract string EffectName { get; }

        /// <summary>
        /// Gets or sets the compositor.
        /// </summary>
        /// <value>
        /// The compositor.
        /// </value>
        public Compositor Compositor { get; set; }

        /// <summary>
        /// Gets or sets the effect brush.
        /// </summary>
        /// <value>
        /// The effect brush.
        /// </value>
        public CompositionEffectBrush EffectBrush { get; set; }

        /// <summary>
        /// Applies the effect.
        /// </summary>
        /// <returns>An array of strings of the effect properties to change.</returns>
        public abstract string[] ApplyEffect();

        /// <summary>
        /// An animation which will apply the derived effect.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The value.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <param name="easingType">The easing function to use</param>
        /// <param name="easingMode">The easing mode to use</param>
        /// <returns>An animation set with the effect added to it.</returns>
        public AnimationSet EffectAnimation(
            AnimationSet animationSet,
            double value = 0d,
            double duration = 500d,
            double delay = 0d,
            EasingType easingType = EasingType.Default,
            EasingMode easingMode = EasingMode.EaseOut)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (!IsSupported)
            {
                return null;
            }

            var visual = animationSet.Visual;
            var associatedObject = animationSet.Element as FrameworkElement;

            if (associatedObject == null)
            {
                return animationSet;
            }

            Compositor = visual?.Compositor;

            if (Compositor == null)
            {
                return null;
            }

            // check to see if the visual already has an effect applied.
            var spriteVisual = ElementCompositionPreview.GetElementChildVisual(associatedObject) as SpriteVisual;
            EffectBrush = spriteVisual?.Brush as CompositionEffectBrush;

            if (EffectBrush == null || EffectBrush?.Comment != EffectName)
            {
                _effectProperties = ApplyEffect();
                EffectBrush.Comment = EffectName;

                var sprite = Compositor.CreateSpriteVisual();
                sprite.Brush = EffectBrush;
                ElementCompositionPreview.SetElementChildVisual(associatedObject, sprite);

                sprite.Size = new Vector2((float)associatedObject.ActualWidth, (float)associatedObject.ActualHeight);

                associatedObject.SizeChanged +=
                    (s, e) =>
                    {
                        sprite.Size = new Vector2(
                            (float)associatedObject.ActualWidth,
                            (float)associatedObject.ActualHeight);
                    };
            }

            if (duration <= 0)
            {
                foreach (var effectProperty in _effectProperties)
                {
                    animationSet.AddEffectDirectPropertyChange(EffectBrush, (float)value, effectProperty);
                }
            }
            else
            {
                foreach (var effectProperty in _effectProperties)
                {
                    var animation = Compositor.CreateScalarKeyFrameAnimation();
                    animation.InsertKeyFrame(1f, (float)value, AnimationExtensions.GetCompositionEasingFunction(easingType, Compositor, easingMode));

                    animation.Duration = TimeSpan.FromMilliseconds(duration);
                    animation.DelayTime = TimeSpan.FromMilliseconds(delay);

                    animationSet.AddCompositionEffectAnimation(EffectBrush, animation, effectProperty);
                }
            }

            // Saturation starts from 1 to 0, instead of 0 to 1 so this makes sure the
            // the brush isn't removed from the UI element incorrectly. Completing on
            // Saturation as it's reusing the same sprite visual. Removing the Sprite removes the effect.
            if (EffectName != "Saturation" && value == 0)
            {
                animationSet.Completed += AnimationSet_Completed;
            }

            return animationSet;
        }

        /// <summary>
        /// Handles the Completed event of the AnimationSet control.
        /// When an animation is completed the brush is removed from the sprite.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="EventArgs"/> instance containing the event data.</param>
        private void AnimationSet_Completed(object sender, EventArgs e)
        {
            var animationSet = sender as AnimationSet;

            if (animationSet != null)
            {
                animationSet.Completed -= AnimationSet_Completed;

                var spriteVisual = ElementCompositionPreview.GetElementChildVisual(animationSet.Element) as SpriteVisual;
                var brush = spriteVisual?.Brush as CompositionEffectBrush;

                if (brush != null && brush.Comment == EffectName)
                {
                    spriteVisual.Brush = null;
                }
            }
        }
    }
}