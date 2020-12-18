// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Animations.Xaml;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Attached properties to support explicitly triggered animations for <see cref="UIElement"/> instances.
    /// </summary>
    public static class Explicit
    {
        /// <summary>
        /// Identifies the Implicit.ShowAnimations XAML attached property.
        /// </summary>
        public static readonly DependencyProperty Animations = DependencyProperty.RegisterAttached(
            "ShowAnimations",
            typeof(AnimationCollection),
            typeof(Implicit),
            new PropertyMetadata(null, OnAnimationsChanged));

        /// <summary>
        /// Gets the value of the <see cref="Animations"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
        /// <returns>The retrieved <see cref="AnimationCollection2"/> item.</returns>
        public static AnimationCollection2 GetAnimations(UIElement element)
        {
            if (element.GetValue(Animations) is AnimationCollection2 collection)
            {
                return collection;
            }

            collection = new AnimationCollection2();

            element.SetValue(Animations, collection);

            return collection;
        }

        /// <summary>
        /// Sets the value of the <see cref="Animations"/> property.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
        /// <param name="value">The <see cref="AnimationCollection2"/> value to set.</param>
        public static void SetAnimations(UIElement element, AnimationCollection2 value)
        {
            element.SetValue(Animations, value);
        }

        /// <summary>
        /// Callback to keep the attached parent in sync for animations linked to the <see cref="Animations"/> property.
        /// </summary>
        /// <param name="d">The target object the property was changed for.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
        private static void OnAnimationsChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (e.NewValue is AnimationCollection2 animationCollection && d is UIElement element)
            {
                animationCollection.Parent = element;
            }
        }
    }
}
