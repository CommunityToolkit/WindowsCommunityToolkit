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

using Microsoft.Toolkit.Uwp.UI.Animations.Effects;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A partial for the AnimationExtension which includes saturation.
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets the saturation effect.
        /// </summary>
        /// <value>
        /// The saturation effect.
        /// </value>
        public static Saturation SaturationEffect { get; } = new Saturation();

        /// <summary>
        /// Saturates the FrameworkElement.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="value">The value, between 0 and 1. 0 is desaturated, 1 is saturated.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <returns>An animation set with saturation effects incorporated.</returns>
        public static AnimationSet Saturation(
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
            return animationSet.Saturation(value, duration, delay);
        }

        /// <summary>
        /// Saturates the visual within the animation set.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="value">The value. 0 is desaturated, 1 is saturated.</param>
        /// <param name="duration">The duration in milliseconds.</param>
        /// <param name="delay">The delay in milliseconds.</param>
        /// <returns>An animation set with saturation effects incorporated.</returns>
        public static AnimationSet Saturation(
            this AnimationSet animationSet,
            double value = 0d,
            double duration = 500d,
            double delay = 0d)
        {
            return SaturationEffect.EffectAnimation(animationSet, value, duration, delay);
        }
    }
}