// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Graphics.Canvas.Effects;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Effects
{
    /// <summary>
    /// An animation effect that applies blur.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Effects.AnimationEffect" />
    public class Blur : AnimationEffect
    {
        /// <summary>
        /// Gets a value indicating whether blur is supported.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is supported; otherwise, <c>false</c>.
        /// </value>
        public override bool IsSupported
            => ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3);

        /// <summary>
        /// Gets the name of the effect.
        /// </summary>
        /// <value>
        /// The name of the effect.
        /// </value>
        public override string EffectName { get; } = "Blur";

        /// <summary>
        /// Applies the effect.
        /// </summary>
        /// <returns>
        /// An array of strings of the effect properties to change.
        /// </returns>
        public override string[] ApplyEffect()
        {
            var gaussianBlur = new GaussianBlurEffect
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