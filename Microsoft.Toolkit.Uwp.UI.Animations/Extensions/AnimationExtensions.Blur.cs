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
using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets the blur effect.
        /// </summary>
        /// <value>
        /// The blur effect.
        /// </value>
        public static Blur BlurEffect { get; } = new Blur();

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
            return BlurEffect.EffectAnimation(animationSet, value, duration, delay);
        }
    }
}
