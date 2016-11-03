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

using System.Numerics;
using Windows.UI;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
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
        /// Create dropshadow for an element, prefered Image, Shape or Textblock.
        /// </summary>
        /// <param name="associatedObject">Element to drop shadow</param>
        /// <param name="offsetX">Shadow X offset</param>
        /// <param name="offsetY">Shadow Y offset</param>
        /// <param name="offsetZ">Shadow Z offset</param>
        /// <param name="color">Shadow color (default black)</param>
        /// <param name="opacity">Shadow opacity</param>
        /// <param name="blurRadius">Shadow blur radius</param>
        /// <param name="sizeX">Shadow size X</param>
        /// <param name="sizeY">Shadow size Y</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet DropShadow(
            this UIElement associatedObject,
            float offsetX = 10f,
            float offsetY = 10f,
            float offsetZ = 10f,
            Color color = default(Color),
            float opacity = .5f,
            float blurRadius = 1f,
            float sizeX = 50f,
            float sizeY = 50f)
        {
            if (associatedObject == null)
            {
                return null;
            }

            var animationSet = new AnimationSet(associatedObject);
            return animationSet.DropShadow(offsetX, offsetY, offsetZ, color, opacity, blurRadius, sizeX, sizeY);
        }

        /// <summary>
        /// Create dropshadow for an element, prefered Image, Shape or Textblock.
        /// </summary>
        /// <param name="animationSet">The animationSet object.</param>
        /// <param name="offsetX">Shadow X offset</param>
        /// <param name="offsetY">Shadow Y offset</param>
        /// <param name="offsetZ">Shadow Z offset</param>
        /// <param name="color">Shadow color (default black)</param>
        /// <param name="opacity">Shadow opacity</param>
        /// <param name="blurRadius">Shadow blur radius</param>
        /// <param name="sizeX">Shadow size X</param>
        /// <param name="sizeY">Shadow size Y</param>
        /// <returns>
        /// An AnimationSet.
        /// </returns>
        public static AnimationSet DropShadow(
            this AnimationSet animationSet,
            float offsetX = 10f,
            float offsetY = 10f,
            float offsetZ = 10f,
            Color color = default(Color),
            float opacity = .5f,
            float blurRadius = 10f,
            float sizeX = 50f,
            float sizeY = 50f)
        {
            var element = animationSet?.Element;
            if (element == null)
            {
                return null;
            }

            var visual = animationSet.Visual;
            var compositor = visual?.Compositor;
            if (compositor == null)
            {
                return null;
            }

            if (color == default(Color))
            {
                color = Colors.Black;
            }

            CompositionBrush mask = null;

            // Only Image, shape and textblock has method GetAlphaMask
            var image = element as Image;
            if (image != null)
            {
                mask = image.GetAlphaMask();
            }
            else
            {
                var shape = element as Shape;
                if (shape != null)
                {
                    mask = shape.GetAlphaMask();
                }
                else
                {
                    var textblock = element as TextBlock;
                    if (textblock != null)
                    {
                        mask = textblock.GetAlphaMask();
                    }
                }
            }

            var shadowVisual = compositor.CreateSpriteVisual();
            DropShadow dropShadow = compositor.CreateDropShadow();
            dropShadow.Offset = new Vector3(new Vector2(offsetX, offsetY), offsetZ);
            dropShadow.Color = color;
            dropShadow.Opacity = opacity;
            dropShadow.BlurRadius = blurRadius;
            if (mask != null)
            {
                dropShadow.Mask = mask;
            }

            shadowVisual.Shadow = dropShadow;
            shadowVisual.Size = new Vector2(sizeX, sizeY);
            ElementCompositionPreview.SetElementChildVisual(element, shadowVisual);

            // Make sure size of shadow host and shadow visual always stay in sync
            ExpressionAnimation bindSizeAnimation = compositor.CreateExpressionAnimation("hostVisual.Size");
            bindSizeAnimation.SetReferenceParameter("hostVisual", visual);
            animationSet.AddCompositionAnimation("Size", bindSizeAnimation);

            return animationSet;
        }
    }
}
