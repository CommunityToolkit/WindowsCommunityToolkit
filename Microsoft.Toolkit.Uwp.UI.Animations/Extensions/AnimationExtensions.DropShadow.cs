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
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;
using Windows.UI.Xaml.Shapes;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods perform animation on UIElements
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the platform supports drop shadow.
        /// </summary>
        /// <remarks>
        /// A check should always be made to IsDropShadowSupported prior to calling DropShadow,
        /// since older operating systems will not support drop shadow.
        /// </remarks>
        /// <seealso cref="DropShadow(FrameworkElement, Shape, Color, Vector3, float)"/>
        public static bool IsDropShadowSupported =>
            ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

        /// <summary>
        /// Animates the drop shadow of the UIElement.
        /// </summary>
        /// <param name="associatedObject">The associated object.</param>
        /// <param name="shapeMask">Shape UI element that will handle the shadow</param>
        /// <param name="color">The color of the shadow</param>
        /// <param name="offset">Offset of the shadow</param>
        /// <param name="blurRadius">Radius of the shadow</param>
        /// <returns>
        /// An Animation Set.
        /// </returns>
        /// <seealso cref="IsDropShadowSupported" />
        public static AnimationSet DropShadow(
            this FrameworkElement associatedObject,
            Shape shapeMask,
            Color color,
            Vector3 offset = default(Vector3),
            float blurRadius = 0f)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.DropShadow(shapeMask, color, offset, blurRadius);
        }

        /// <summary>
        /// Animates the drop shadow of the the UIElement.
        /// </summary>
        /// <param name="animationSet">The animation set.</param>
        /// <param name="shapeMask">Shape UI element that will handle the shadow</param>
        /// <param name="color">The color of the shadow</param>
        /// <param name="offset">Offset of the shadow</param>
        /// <param name="blurRadius">Radius of the shadow</param>
        /// <returns>
        /// An Animation Set.
        /// </returns>
        /// <seealso cref="IsDropShadowSupported" />
        public static AnimationSet DropShadow(
            this AnimationSet animationSet,
            Shape shapeMask,
            Color color,
            Vector3 offset = default(Vector3),
            float blurRadius = 0f)
        {
            if (animationSet == null)
            {
                return null;
            }

            if (!IsDropShadowSupported)
            {
                // The operating system doesn't support drop shadow.
                // Fail gracefully by not applying drop shadow.
                // See 'IsDropShadowSupported' property
                return null;
            }

            var hostVisual = animationSet.Visual;
            var associatedObject = animationSet.Element as FrameworkElement;

            if (associatedObject == null)
            {
                return animationSet;
            }

            var compositor = hostVisual?.Compositor;

            if (compositor == null)
            {
                return null;
            }

            // Create a drop shadow
            var dropShadow = compositor.CreateDropShadow();
            dropShadow.BlurRadius = blurRadius;
            dropShadow.Offset = offset;
            dropShadow.Color = color;

            // Associate the shape of the shadow with the shape of the target element
            dropShadow.Mask = shapeMask.GetAlphaMask();

            // Create a Visual to hold the shadow
            var shadowVisual = compositor.CreateSpriteVisual();
            shadowVisual.Shadow = dropShadow;

            // Add the shadow as a child of the host in the visual tree
            ElementCompositionPreview.SetElementChildVisual(animationSet.Element, shadowVisual);

            // Make sure size of shadow host and shadow visual always stay in sync
            var bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", hostVisual);
            shadowVisual.StartAnimation("Size", bindSizeAnimation);

            return animationSet;
        }
    }
}
