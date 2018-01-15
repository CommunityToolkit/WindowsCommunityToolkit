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

using Microsoft.Graphics.Canvas.Effects;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Animations.Effects
{
    /// <summary>
    /// An animation effect that applies saturation.
    /// </summary>
    /// <seealso cref="Microsoft.Toolkit.Uwp.UI.Animations.Effects.AnimationEffect" />
    public class Saturation : AnimationEffect
    {
        /// <summary>
        /// Gets a value indicating whether Saturation is supported.
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
        public override string EffectName { get; } = "Saturation";

        /// <summary>
        /// Applies the effect.
        /// </summary>
        /// <returns>
        /// An array of strings of the effect properties to change.
        /// </returns>
        public override string[] ApplyEffect()
        {
            var saturationEffect = new SaturationEffect
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