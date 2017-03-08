using System.Collections.Generic;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Microsoft.Graphics.Canvas.Effects;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Effects
{
    public class Blur : AnimationEffect
    {
        public override bool IsSupported => ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3);
        public override string EffectName { get; } = "Blur";
        public override string[] ApplyEffect()
        {
            var gaussianBlur = new GaussianBlurEffect()
            {
                Name = EffectName,
                BlurAmount = 0f,
                Optimization = EffectOptimization.Balanced,
                BorderMode = EffectBorderMode.Hard,
                Source = new CompositionEffectSourceParameter("source")
            };

            var propertyToChange = $"{EffectName}.BlurAmount";
            var propertiesToAnimate = new[] { propertyToChange };

            EffectBrush = Compositor.CreateEffectFactory(gaussianBlur, propertiesToAnimate).CreateBrush();
            EffectBrush.SetSourceParameter("source", Compositor.CreateBackdropBrush());

            return propertiesToAnimate;
        }
    }
}
