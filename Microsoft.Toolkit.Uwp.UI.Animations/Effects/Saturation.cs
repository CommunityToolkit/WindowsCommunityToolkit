using System;
using System.Collections.Generic;
using System.Numerics;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml.Hosting;
using Microsoft.Graphics.Canvas.Effects;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Effects
{
    public class Saturation : AnimationEffect
    {
        public override bool IsSupported => ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3);

        public override string EffectName { get; } = "Saturation";

        public override string[] ApplyEffect()
        {
            var saturationEffect = new SaturationEffect()
            {
                Saturation = 1f,
                Name = EffectName,
                Source = new CompositionEffectSourceParameter("source")
            };

            var propertyToChange = $"{EffectName}.Saturation";
            var propertiesToAnimate = new[] { propertyToChange };

            EffectBrush = Compositor.CreateEffectFactory(saturationEffect, propertiesToAnimate).CreateBrush();
            EffectBrush.SetSourceParameter("source", Compositor.CreateBackdropBrush());

            return propertiesToAnimate;
        }
    }
}
