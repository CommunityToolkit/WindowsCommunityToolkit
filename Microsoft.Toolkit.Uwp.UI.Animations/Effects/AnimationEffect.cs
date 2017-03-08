using System;
using System.Numerics;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Effects
{
    public abstract class AnimationEffect
    {
        private static string[] _effectProperties;

        public abstract bool IsSupported { get; }

        public abstract string EffectName { get; }

        public Compositor Compositor { get; set; }

        public CompositionEffectBrush EffectBrush { get; set; }

        public abstract string[] ApplyEffect();

        public AnimationSet EffectAnimation(
            AnimationSet animationSet,
            double value = 0d,
            double duration = 500d,
            double delay = 0d)
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

                associatedObject.SizeChanged += (s, e) =>
                {
                    sprite.Size = new Vector2((float)associatedObject.ActualWidth, (float)associatedObject.ActualHeight);
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
                    animation.InsertKeyFrame(1f, (float)value);
                    animation.Duration = TimeSpan.FromMilliseconds(duration);
                    animation.DelayTime = TimeSpan.FromMilliseconds(delay);

                    animationSet.AddCompositionEffectAnimation(EffectBrush, animation, effectProperty);
                }
            }

            if (EffectName == "Saturation" && value >= 1)
            {
                animationSet.Completed += AnimationSet_Completed;
            }
            else if (EffectName != "Saturation" && value == 0)
            {
                animationSet.Completed += AnimationSet_Completed;
            }

            return animationSet;
        }

        private void AnimationSet_Completed(object sender, EventArgs e)
        {
            var animationSet = sender as AnimationSet;
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
