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
    public class ParallaxService
    {
        public static readonly DependencyProperty ScrollingElementProperty = DependencyProperty.RegisterAttached("ScrollingElement", typeof(FrameworkElement), typeof(ParallaxService), new PropertyMetadata(null, OnScrollingElementChanged));

        public static readonly DependencyProperty MultiplierProperty = DependencyProperty.RegisterAttached("Multiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0.3d, OnMultiplierChanged));

        public static FrameworkElement GetScrollingElement(DependencyObject obj)
        {
            return (FrameworkElement)obj.GetValue(ScrollingElementProperty);
        }

        public static void SetScrollingElement(DependencyObject obj, FrameworkElement value)
        {
            obj.SetValue(ScrollingElementProperty, value);
        }

        public static double GetMultiplier(DependencyObject obj)
        {
            return (double)obj.GetValue(MultiplierProperty);
        }

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
