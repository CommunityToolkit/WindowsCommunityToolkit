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
        /// Identifies the ParallaxService.ScrollingElement XAML attached property.
        /// </summary>
        public static readonly DependencyProperty ScrollingElementProperty = DependencyProperty.RegisterAttached("ScrollingElement", typeof(FrameworkElement), typeof(ParallaxService), new PropertyMetadata(null, OnScrollingElementChanged));

        /// <summary>
        /// Identifies the ParallaxService.VerticalMultiplier XAML attached property.
        /// </summary>
        public static readonly DependencyProperty VerticalMultiplierProperty = DependencyProperty.RegisterAttached("VerticalMultiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0d, OnMultiplierChanged));

        /// <summary>
        /// Identifies the ParallaxService.HorizontalMultiplier attached property.
        /// </summary>
        public static readonly DependencyProperty HorizontalMultiplierProperty = DependencyProperty.RegisterAttached("HorizontalMultiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0d, OnMultiplierChanged));

        /// <summary>
        /// Gets the ParallaxService.ScrollingElementattached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value..</param>
        /// <returns>A <see cref="FrameworkElement"/> that is, or contains a ScrollViewer.</returns>
        public static FrameworkElement GetScrollingElement(DependencyObject element)
        {
            return (FrameworkElement)element.GetValue(ScrollingElementProperty);
        }

        /// <summary>
        /// Sets the ParallaxService.ScrollingElementattached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value.</param>
        /// <param name="value">The element that is, or contains a ScrollViewer.</param>
        public static void SetScrollingElement(DependencyObject element, FrameworkElement value)
        {
            element.SetValue(ScrollingElementProperty, value);
        }

        /// <summary>
        /// Gets the ParallaxService.VerticalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value..</param>
        /// <returns>A value for how fast the parallax effect should scroll vertically.</returns>
        public static double GetVerticalMultiplier(DependencyObject element)
        {
            return (double)element.GetValue(VerticalMultiplierProperty);
        }

        /// <summary>
        /// Sets the ParallaxService.VerticalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value.</param>
        /// <param name="value">The value for how fast the parallax effect should scroll vertically.</param>
        public static void SetVerticalMultiplier(DependencyObject element, double value)
        {
            element.SetValue(VerticalMultiplierProperty, value);
        }

        /// <summary>
        /// Gets the ParallaxService.HorizontalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value..</param>
        /// <returns>A value for how fast the parallax effect should scroll vertically.</returns>
        public static double GetHorizontalMultiplier(DependencyObject element)
        {
            return (double)element.GetValue(HorizontalMultiplierProperty);
        }

        /// <summary>
        /// Sets the ParallaxService.HorizontalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value.</param>
        /// <param name="value">The value for how fast the parallax effect should scroll horizontally.</param>
        public static void SetHorizontalMultiplier(DependencyObject element, double value)
        {
            element.SetValue(HorizontalMultiplierProperty, value);
        }

        private static void OnScrollingElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateParallax(d as UIElement, GetScrollViewer(d), (double)d.GetValue(HorizontalMultiplierProperty), (double)d.GetValue(VerticalMultiplierProperty));
        }

        private static void OnMultiplierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateParallax(d as UIElement, GetScrollViewer(d), (double)d.GetValue(HorizontalMultiplierProperty), (double)d.GetValue(VerticalMultiplierProperty));
        }

        private static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            var element = obj.GetValue(ScrollingElementProperty) as DependencyObject;
            var scroller = element as ScrollViewer;
            return scroller ?? element?.FindDescendant<ScrollViewer>();
        }

        private static void CreateParallax(UIElement parallaxElement, ScrollViewer scroller, double horizontalMultiplier, double verticalMultiplier)
        {
            if ((parallaxElement == null) || (scroller == null))
            {
                return;
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            ExpressionAnimation expression = compositor.CreateExpressionAnimation(
                "Matrix4x4.CreateFromTranslation(Vector3(HorizontalMultiplier * scroller.Translation.X, VerticalMultiplier * scroller.Translation.Y, 0.0f))");
            expression.SetReferenceParameter("scroller", scrollerViewerManipulation);
            expression.SetScalarParameter("HorizontalMultiplier", (float)horizontalMultiplier);
            expression.SetScalarParameter("VerticalMultiplier", (float)verticalMultiplier);

            Visual visual = ElementCompositionPreview.GetElementVisual(parallaxElement);
            visual.StartAnimation("TransformMatrix", expression);
        }
    }
}
