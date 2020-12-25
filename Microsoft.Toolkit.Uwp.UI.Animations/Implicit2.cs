// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Windows.UI.Composition;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Hosting;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Attached properties to support implicitly triggered animations for <see cref="UIElement"/> instances.
    /// </summary>
    public static class Implicit2
    {
        /// <summary>
        /// The attached "ShowAnimations" property.
        /// </summary>
        public static readonly DependencyProperty ShowAnimationsProperty = DependencyProperty.RegisterAttached(
            "ShowAnimations",
            typeof(ImplicitCompositionAnimationCollection),
            typeof(Implicit2),
            new PropertyMetadata(null, OnShowAnimationsPropertyChanged));

        /// <summary>
        /// The attached "HideAnimations" property.
        /// </summary>
        public static readonly DependencyProperty HideAnimationsProperty = DependencyProperty.RegisterAttached(
            "HideAnimations",
            typeof(ImplicitCompositionAnimationCollection),
            typeof(Implicit2),
            new PropertyMetadata(null, OnHideAnimationsPropertyChanged));

        /// <summary>
        /// The attached "Animations" property.
        /// </summary>
        public static readonly DependencyProperty AnimationsProperty = DependencyProperty.RegisterAttached(
            "Animations",
            typeof(ImplicitCompositionAnimationCollection),
            typeof(Implicit2),
            new PropertyMetadata(null, OnAnimationsPropertyChanged));

        /// <summary>
        /// Gets the value of the <see cref="ShowAnimationsProperty"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
        /// <returns>The retrieved <see cref="ImplicitCompositionAnimationCollection"/> value.</returns>
        public static ImplicitCompositionAnimationCollection GetShowAnimations(UIElement element)
        {
            var collection = (ImplicitCompositionAnimationCollection)element.GetValue(ShowAnimationsProperty);

            if (collection is null)
            {
                element.SetValue(ShowAnimationsProperty, collection = new());
            }

            return collection;
        }

        /// <summary>
        /// Sets the value of the <see cref="ShowAnimationsProperty"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
        /// <param name="value">The <see cref="ImplicitCompositionAnimationCollection"/> value to set.</param>
        public static void SetShowAnimations(UIElement element, ImplicitCompositionAnimationCollection value)
        {
            element.SetValue(ShowAnimationsProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="HideAnimationsProperty"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
        /// <returns>The retrieved <see cref="ImplicitCompositionAnimationCollection"/> value.</returns>
        public static ImplicitCompositionAnimationCollection GetHideAnimations(UIElement element)
        {
            var collection = (ImplicitCompositionAnimationCollection)element.GetValue(HideAnimationsProperty);

            if (collection is null)
            {
                element.SetValue(HideAnimationsProperty, collection = new());
            }

            return collection;
        }

        /// <summary>
        /// Sets the value of the <see cref="HideAnimationsProperty"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
        /// <param name="value">The <see cref="ImplicitCompositionAnimationCollection"/> value to set.</param>
        public static void SetHideAnimations(UIElement element, ImplicitCompositionAnimationCollection value)
        {
            element.SetValue(HideAnimationsProperty, value);
        }

        /// <summary>
        /// Gets the value of the <see cref="AnimationsProperty"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
        /// <returns>The retrieved <see cref="ImplicitCompositionAnimationCollection"/> value.</returns>
        public static ImplicitCompositionAnimationCollection GetAnimations(UIElement element)
        {
            var collection = (ImplicitCompositionAnimationCollection)element.GetValue(AnimationsProperty);

            if (collection is null)
            {
                element.SetValue(AnimationsProperty, collection = new());
            }

            return collection;
        }

        /// <summary>
        /// Sets the value of the <see cref="AnimationsProperty"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
        /// <param name="value">The <see cref="AnimationCollection2"/> value to set.</param>
        public static void SetAnimations(UIElement element, ImplicitCompositionAnimationCollection value)
        {
            element.SetValue(AnimationsProperty, value);
        }

        /// <summary>
        /// Callback to keep the attached parent in sync for animations linked to the <see cref="ShowAnimationsProperty"/> property.
        /// </summary>
        /// <param name="d">The target object the property was changed for.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
        private static void OnShowAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            static void OnAnimationsChanged(object sender, EventArgs e)
            {
                var collection = (ImplicitCompositionAnimationCollection)sender;

                if (collection.ParentReference!.TryGetTarget(out UIElement element))
                {
                    ElementCompositionPreview.SetImplicitShowAnimation(element, collection.GetCompositionAnimationGroup());
                }
            }

            if (e.OldValue is ImplicitCompositionAnimationCollection oldCollection)
            {
                oldCollection.AnimationsChanged -= OnAnimationsChanged;
            }

            if (d is UIElement element &&
                e.NewValue is ImplicitCompositionAnimationCollection collection)
            {
                collection.ParentReference = new(element);
                collection.AnimationsChanged -= OnAnimationsChanged;
                collection.AnimationsChanged += OnAnimationsChanged;

                ElementCompositionPreview.SetImplicitShowAnimation(element, collection.GetCompositionAnimationGroup());
            }
        }

        /// <summary>
        /// Callback to keep the attached parent in sync for animations linked to the <see cref="HideAnimationsProperty"/> property.
        /// </summary>
        /// <param name="d">The target object the property was changed for.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
        private static void OnHideAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            static void OnAnimationsChanged(object sender, EventArgs e)
            {
                var collection = (ImplicitCompositionAnimationCollection)sender;

                if (collection.ParentReference!.TryGetTarget(out UIElement element))
                {
                    ElementCompositionPreview.SetImplicitHideAnimation(element, collection.GetCompositionAnimationGroup());
                }
            }

            if (e.OldValue is ImplicitCompositionAnimationCollection oldCollection)
            {
                oldCollection.AnimationsChanged -= OnAnimationsChanged;
            }

            if (d is UIElement element &&
                e.NewValue is ImplicitCompositionAnimationCollection collection)
            {
                collection.ParentReference = new(element);
                collection.AnimationsChanged -= OnAnimationsChanged;
                collection.AnimationsChanged += OnAnimationsChanged;

                ElementCompositionPreview.SetImplicitHideAnimation(element, collection.GetCompositionAnimationGroup());
            }
        }

        /// <summary>
        /// Callback to keep the attached parent in sync for animations linked to the <see cref="AnimationsProperty"/> property.
        /// </summary>
        /// <param name="d">The target object the property was changed for.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
        private static void OnAnimationsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            static void OnAnimationsChanged(object sender, EventArgs e)
            {
                var collection = (ImplicitCompositionAnimationCollection)sender;

                if (collection.ParentReference!.TryGetTarget(out UIElement element))
                {
                    ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = collection.GetImplicitAnimationCollection();
                }
            }

            if (e.OldValue is ImplicitCompositionAnimationCollection oldCollection)
            {
                oldCollection.AnimationsChanged -= OnAnimationsChanged;
            }

            if (d is UIElement element &&
                e.NewValue is ImplicitCompositionAnimationCollection collection)
            {
                collection.ParentReference = new(element);
                collection.AnimationsChanged -= OnAnimationsChanged;
                collection.AnimationsChanged += OnAnimationsChanged;

                ElementCompositionPreview.GetElementVisual(element).ImplicitAnimations = collection.GetImplicitAnimationCollection();
            }
        }
    }
}
