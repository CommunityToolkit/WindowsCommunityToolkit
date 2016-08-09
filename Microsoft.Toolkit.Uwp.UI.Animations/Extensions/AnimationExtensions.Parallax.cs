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
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// These extension methods use composition to perform animation on visuals.
    /// </summary>
    public static partial class AnimationExtensions
    {
        /// <summary>
        /// Creates a Parallax effect on the specified element based on the supplied scroller element.
        /// </summary>
        /// <param name="element">The element.</param>
        /// <param name="scrollerElement">The scroller element.</param>
        /// <param name="isHorizontalEffect">if set to <c>true</c> [is horizontal effect].</param>
        /// <param name="multiplier">The multiplier (how fast it scrolls).</param>
        public static void Parallax(this UIElement element, FrameworkElement scrollerElement, bool isHorizontalEffect, float multiplier)
        {
            if (scrollerElement == default(FrameworkElement))
            {
                return;
            }

            var scroller = scrollerElement as ScrollViewer;
            if (scroller == null)
            {
                scroller = scrollerElement.FindDescendant<ScrollViewer>();
                if (scroller == null)
                {
                    return;
                }
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            var manipulationProperty = isHorizontalEffect ? "X" : "Y";
            var expression = compositor.CreateExpressionAnimation($"ScrollManipululation.Translation.{manipulationProperty} * ParallaxMultiplier");

            expression.SetScalarParameter("ParallaxMultiplier", multiplier);
            expression.SetReferenceParameter("ScrollManipululation", scrollerViewerManipulation);

            Visual textVisual = ElementCompositionPreview.GetElementVisual(element);
            textVisual.StartAnimation($"Offset.{manipulationProperty}", expression);
        }
    }
}