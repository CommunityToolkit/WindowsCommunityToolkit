// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation.Metadata;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Hosting;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Provides the ability to assign a reorder animation to a GridView.
    /// </summary>
    public class ReorderGridAnimation
    {
        private static readonly DependencyProperty ReorderAnimationProperty =
            DependencyProperty.RegisterAttached("ReorderAnimation", typeof(bool), typeof(ImplicitAnimationCollection), new PropertyMetadata(null));

        /// <summary>
        /// Gets a value indicating whether the platform supports the animation.
        /// </summary>
        /// <remarks>
        /// On platforms not supporting the animation, this class has no effect.
        /// </remarks>
        public static bool IsSupported =>
            Windows.ApplicationModel.DesignMode.DesignModeEnabled ? false :
            ApiInformation.IsApiContractPresent("Windows.Foundation.UniversalApiContract", 3); // SDK >= 14393

        /// <summary>
        /// Identifies the Duration attached dependency property.
        /// </summary>
        /// <returns>The identifier for the Duration attached dependency property.</returns>
        public static readonly DependencyProperty DurationProperty = DependencyProperty.RegisterAttached("Duration", typeof(double), typeof(ReorderGridAnimation), new PropertyMetadata(double.NaN, OnDurationChanged));

        /// <summary>
        /// Gets a value indicating the duration, in milliseconds, the animation should take.
        /// </summary>
        /// <param name="obj">The object to get the value from.</param>
        /// <returns>A value indicating the duration for the animation.</returns>
        public static double GetDuration(DependencyObject obj)
        {
            return (double)obj.GetValue(DurationProperty);
        }

        /// <summary>
        /// Sets a value for the duration, in milliseconds, the animation should take.
        /// </summary>
        /// <param name="obj">the object to set the value on.</param>
        /// <param name="value">The duration in milliseonds.</param>
        public static void SetDuration(DependencyObject obj, double value)
        {
            obj.SetValue(DurationProperty, value);
        }

        private static void OnDurationChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (IsSupported == false)
            {
                return;
            }

            GridView view = d as GridView;
            if (view != null)
            {
                AssignReorderAnimation(view);

                view.ContainerContentChanging -= OnContainerContentChanging;
                view.ContainerContentChanging += OnContainerContentChanging;

                view.ChoosingItemContainer -= OnChoosingItemContainer;
                view.ChoosingItemContainer += OnChoosingItemContainer;
            }
        }

        private static void AssignReorderAnimation(GridView view)
        {
            var compositor = ElementCompositionPreview.GetElementVisual(view).Compositor;
            var elementImplicitAnimation = view.GetValue(ReorderAnimationProperty) as ImplicitAnimationCollection;
            if (elementImplicitAnimation == null)
            {
                elementImplicitAnimation = compositor.CreateImplicitAnimationCollection();
                view.SetValue(ReorderAnimationProperty, elementImplicitAnimation);
            }

            double duration = (double)view.GetValue(DurationProperty);
            elementImplicitAnimation[nameof(Visual.Offset)] = CreateOffsetAnimation(compositor, duration);
        }

        private static void OnContainerContentChanging(ListViewBase sender, ContainerContentChangingEventArgs args)
        {
            var elementVisual = ElementCompositionPreview.GetElementVisual(args.ItemContainer);
            if (args.InRecycleQueue)
            {
                PokeUIElementZIndex(args.ItemContainer);
            }
            else
            {
                var elementImplicitAnimation = sender.GetValue(ReorderAnimationProperty) as ImplicitAnimationCollection;
                elementVisual.ImplicitAnimations = elementImplicitAnimation;
            }
        }

        private static void OnChoosingItemContainer(ListViewBase sender, ChoosingItemContainerEventArgs args)
        {
            if (args.ItemContainer != null)
            {
                PokeUIElementZIndex(args.ItemContainer);
            }
        }

        private static CompositionAnimationGroup CreateOffsetAnimation(Compositor compositor, double duration)
        {
            Vector3KeyFrameAnimation offsetAnimation = compositor.CreateVector3KeyFrameAnimation();
            offsetAnimation.InsertExpressionKeyFrame(1.0f, "this.FinalValue");
            offsetAnimation.Duration = TimeSpan.FromMilliseconds(duration);
            offsetAnimation.Target = nameof(Visual.Offset);

            CompositionAnimationGroup animationGroup = compositor.CreateAnimationGroup();
            animationGroup.Add(offsetAnimation);

            return animationGroup;
        }

        private static void PokeUIElementZIndex(UIElement element)
        {
            var oldZIndex = Canvas.GetZIndex(element);
            Canvas.SetZIndex(element, oldZIndex + 1);
            Canvas.SetZIndex(element, oldZIndex);
        }
    }
}