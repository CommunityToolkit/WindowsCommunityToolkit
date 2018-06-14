// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Animations.Expressions;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides the ability to create a parallax effect to items within a ScrollViewer or List control
    /// </summary>
    [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
    public class ParallaxService
    {
        /// <summary>
        /// Identifies the ParallaxService.VerticalMultiplier XAML attached property.
        /// </summary>
        [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
        public static readonly DependencyProperty VerticalMultiplierProperty = DependencyProperty.RegisterAttached("VerticalMultiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0d, OnMultiplierChanged));

        /// <summary>
        /// Identifies the ParallaxService.HorizontalMultiplier attached property.
        /// </summary>
        [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
        public static readonly DependencyProperty HorizontalMultiplierProperty = DependencyProperty.RegisterAttached("HorizontalMultiplier", typeof(double), typeof(ParallaxService), new PropertyMetadata(0d, OnMultiplierChanged));

        /// <summary>
        /// Gets the ParallaxService.VerticalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value..</param>
        /// <returns>A value for how fast the parallax effect should scroll vertically.</returns>
        [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
        public static double GetVerticalMultiplier(UIElement element)
        {
            return (double)element.GetValue(VerticalMultiplierProperty);
        }

        /// <summary>
        /// Sets the ParallaxService.VerticalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value.</param>
        /// <param name="value">The value for how fast the parallax effect should scroll vertically.</param>
        [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
        public static void SetVerticalMultiplier(UIElement element, double value)
        {
            element.SetValue(VerticalMultiplierProperty, value);
        }

        /// <summary>
        /// Gets the ParallaxService.HorizontalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value..</param>
        /// <returns>A value for how fast the parallax effect should scroll vertically.</returns>
        [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
        public static double GetHorizontalMultiplier(UIElement element)
        {
            return (double)element.GetValue(HorizontalMultiplierProperty);
        }

        /// <summary>
        /// Sets the ParallaxService.HorizontalMultiplier attached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value.</param>
        /// <param name="value">The value for how fast the parallax effect should scroll horizontally.</param>
        [Obsolete("The ParallaxService will be removed in a future major release. Please use the ParallaxView control available in the Fall Creators Update")]
        public static void SetHorizontalMultiplier(UIElement element, double value)
        {
            element.SetValue(HorizontalMultiplierProperty, value);
        }

        /// <summary>
        /// Identifies the ParallaxService.ScrollingElement XAML attached property.
        /// </summary>
        private static readonly DependencyProperty ScrollingElementProperty = DependencyProperty.RegisterAttached("ScrollingElement", typeof(ScrollViewer), typeof(ParallaxService), new PropertyMetadata(null));

        /// <summary>
        /// Gets the ParallaxService.ScrollingElementattached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value..</param>
        /// <returns>A <see cref="FrameworkElement"/> that is, or contains a ScrollViewer.</returns>
        private static ScrollViewer GetScrollingElement(UIElement element)
        {
            return (ScrollViewer)element.GetValue(ScrollingElementProperty);
        }

        /// <summary>
        /// Sets the ParallaxService.ScrollingElementattached property value for the specified target element.
        /// </summary>
        /// <param name="element">The target element for the attached property value.</param>
        /// <param name="value">The element that is, or contains a ScrollViewer.</param>
        private static void SetScrollingElement(UIElement element, ScrollViewer value)
        {
            element.SetValue(ScrollingElementProperty, value);
        }

        private static void OnMultiplierChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var uiElement = (UIElement)d;
            var scrollViewer = GetScrollingElement(uiElement);
            if (scrollViewer == null)
            {
                var element = d as FrameworkElement;
                if (element != null)
                {
                    scrollViewer = element.FindAscendant<ScrollViewer>();
                    if (scrollViewer == null)
                    {
                        element.Loaded += OnElementLoaded;
                        return;
                    }

                    SetScrollingElement(uiElement, scrollViewer);
                }
            }

            CreateParallax(uiElement, scrollViewer, (double)d.GetValue(HorizontalMultiplierProperty), (double)d.GetValue(VerticalMultiplierProperty));
        }

        private static void OnElementLoaded(object sender, RoutedEventArgs e)
        {
            var element = (FrameworkElement)sender;
            element.Loaded -= OnElementLoaded;

            var scrollViewer = element.FindAscendant<ScrollViewer>();
            SetScrollingElement(element, scrollViewer);

            CreateParallax(element, scrollViewer, (double)element.GetValue(HorizontalMultiplierProperty), (double)element.GetValue(VerticalMultiplierProperty));
        }

        private static void CreateParallax(UIElement parallaxElement, ScrollViewer scroller, double horizontalMultiplier, double verticalMultiplier)
        {
            if ((parallaxElement == null) || (scroller == null))
            {
                return;
            }

            CompositionPropertySet scrollerViewerManipulation = ElementCompositionPreview.GetScrollViewerManipulationPropertySet(scroller);
            var scrollPropSet = scrollerViewerManipulation.GetSpecializedReference<ManipulationPropertySetReferenceNode>();

            var parallax = ExpressionFunctions.Vector3((float)horizontalMultiplier * scrollPropSet.Translation.X, (float)verticalMultiplier * scrollPropSet.Translation.Y, 0f);
            var expression = ExpressionFunctions.CreateTranslation(parallax);

            Visual visual = ElementCompositionPreview.GetElementVisual(parallaxElement);
            visual.StartAnimation("TransformMatrix", expression);
        }
    }
}
