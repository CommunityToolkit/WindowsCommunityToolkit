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

using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides the ability to create a parallax effect to items within a ScrollViewer or List control
    /// </summary>
    public class ParallaxService
    {
        /// <summary>
        /// Identifies the ScrollingElement attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ScrollingElementProperty = DependencyProperty.RegisterAttached("ScrollingElement", typeof(FrameworkElement), typeof(ParallaxService), new PropertyMetadata(null, OnScrollingElementChanged));

        /// <summary>
        /// Identifies the Multiplier attached dependency property.
        /// </summary>
        public static readonly DependencyProperty MultiplierProperty = DependencyProperty.RegisterAttached("Multiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0.3d, OnMultiplierChanged));

        /// <summary>
        /// Gets an object that is, or contains, a ScrollViewer
        /// </summary>
        /// <param name="obj">The object to get the ScrollViewer from.</param>
        /// <returns>A <see cref="FrameworkElement"/> that is, or contains a ScrollViewer.</returns>
        public static FrameworkElement GetScrollingElement(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(ScrollingElementProperty);
        }

        /// <summary>
        /// Sets the element that is, or contains, a ScrollerViewer.
        /// </summary>
        /// <param name="obj">The object to set the value on.</param>
        /// <param name="value">The element that is, or contains a ScrollViewer.</param>
        public static void SetScrollingElement(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(ScrollingElementProperty, value);
        }

        /// <summary>
        /// Gets a value for how fast the parallax effect should be.
        /// </summary>
        /// <param name="obj">The object to get the value from.</param>
        /// <returns>A value for how fast the parallax effect should be.</returns>
        public static double GetMultiplier(DependencyObject obj)
        {
            return (double)obj.GetValue(MultiplierProperty);
        }

        /// <summary>
        /// Sets the value for how fast the parallax effect should be.
        /// </summary>
        /// <param name="obj">The object to set the value on.</param>
        /// <param name="value">The value for how fast the parallax effect should be.</param>
        public static void SetMultiplier(DependencyObject obj, double value)
        {
            obj.SetValue(MultiplierProperty, value);
        }

        private static void OnScrollingElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateParallax(d as UIElement, GetScrollViewer(d), (double)d.GetValue(MultiplierProperty));
        }

        private static void OnMultiplierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateParallax(d as UIElement, GetScrollViewer(d), (double)d.GetValue(MultiplierProperty));
        }

        private static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            var element = obj.GetValue(ScrollingElementProperty) as DependencyObject;
            var scroller = element as ScrollViewer;
            return scroller ?? element?.FindDescendant<ScrollViewer>();
        }

        private static void CreateParallax(UIElement parallaxElement, ScrollViewer scroller, double multiplier)
        {
            if ((parallaxElement == null) || (scroller == null))
            {
                return;
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            ExpressionAnimation expression = compositor.CreateExpressionAnimation(multiplier > 0
                ? "ScrollManipulation.Translation.Y * ParallaxMultiplier - ScrollManipulation.Translation.Y"
                : "ScrollManipulation.Translation.Y * ParallaxMultiplier");

            expression.SetScalarParameter("ParallaxMultiplier", (float)multiplier);
            expression.SetReferenceParameter("ScrollManipulation", scrollerViewerManipulation);

            Visual visual = ElementCompositionPreview.GetElementVisual(parallaxElement);
            visual.StartAnimation("Offset.Y", expression);
        }
    }
}
