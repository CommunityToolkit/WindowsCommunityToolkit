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
        /// Identifies the VerticalMultiplier attached dependency property.
        /// </summary>
        public static readonly DependencyProperty VerticalMultiplierProperty = DependencyProperty.RegisterAttached("VerticalMultiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0d, OnMultiplierChanged));

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
        /// Gets a value for how fast the parallax effect should scroll vertically.
        /// </summary>
        /// <param name="obj">The object to get the value from.</param>
        /// <returns>A value for how fast the parallax effect should scroll vertically.</returns>
        public static double GetVerticalMultiplier(DependencyObject obj)
        {
            return (double)obj.GetValue(VerticalMultiplierProperty);
        }

        /// <summary>
        /// Sets the value for how fast the parallax effect should scroll vertically.
        /// </summary>
        /// <param name="obj">The object to set the value on.</param>
        /// <param name="value">The value for how fast the parallax effect should scroll vertically.</param>
        public static void SetVerticalMultiplier(DependencyObject obj, double value)
        {
            obj.SetValue(VerticalMultiplierProperty, value);
        }

        private static void OnScrollingElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateParallax(d as UIElement, GetScrollViewer(d), (double)d.GetValue(VerticalMultiplierProperty));
        }

        private static void OnMultiplierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            CreateParallax(d as UIElement, GetScrollViewer(d), (double)d.GetValue(VerticalMultiplierProperty));
        }

        private static ScrollViewer GetScrollViewer(DependencyObject obj)
        {
            var element = obj.GetValue(ScrollingElementProperty) as DependencyObject;
            var scroller = element as ScrollViewer;
            return scroller ?? element?.FindDescendant<ScrollViewer>();
        }

        private static void CreateParallax(UIElement parallaxElement, ScrollViewer scroller, double verticalMultiplier)
        {
            if ((parallaxElement == null) || (scroller == null))
            {
                return;
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);

            Compositor compositor = scrollerViewerManipulation.Compositor;

            ExpressionAnimation expression = compositor.CreateExpressionAnimation(
                "Matrix4x4.CreateFromTranslation(Vector3(0.0f, VerticalMultiplier * scroller.Translation.Y, 0.0f))");
            expression.SetReferenceParameter("scroller", scrollerViewerManipulation);
            expression.SetScalarParameter("VerticalMultiplier", (float)verticalMultiplier);

            Visual visual = ElementCompositionPreview.GetElementVisual(parallaxElement);
            visual.StartAnimation("TransformMatrix", expression);
        }
    }
}
