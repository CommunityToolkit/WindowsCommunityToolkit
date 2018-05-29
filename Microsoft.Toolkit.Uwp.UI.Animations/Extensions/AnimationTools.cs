// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// Internal tool to link composite transforms to elements
    /// </summary>
    internal class AnimationTools : DependencyObject
    {
        /// <summary>
        /// Attached property used to link composite transform with UIElement
        /// </summary>
        public static readonly DependencyProperty AnimationCompositeTransformIndexProperty = DependencyProperty.RegisterAttached(
            "AnimationCompositeTransformIndex",
            typeof(int),
            typeof(AnimationTools),
            new PropertyMetadata(-2));

        /// <summary>
        /// Attach a composite transform index to an UIElement.
        /// </summary>
        /// <param name="element">UIElement to use</param>
        /// <param name="value">Composite transform index</param>
        public static void SetAnimationCompositeTransformIndex(UIElement element, int value)
        {
            element.SetValue(AnimationCompositeTransformIndexProperty, value);
        }

        /// <summary>
        /// Get the composite transform index attached to an UIElement.
        /// </summary>
        /// <param name="element">UIElement to use</param>
        /// <returns>Composite transform index.</returns>
        public static int GetAnimationCompositeTransformIndex(UIElement element)
        {
            return (int)element.GetValue(AnimationCompositeTransformIndexProperty);
        }
    }
}
