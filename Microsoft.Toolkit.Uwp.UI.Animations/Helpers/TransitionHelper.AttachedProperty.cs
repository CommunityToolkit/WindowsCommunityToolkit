// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Animations
{
    /// <summary>
    /// A animation helper that morphs between two controls.
    /// </summary>
    public sealed partial class TransitionHelper
    {
        private const string IdPropertyName = "Id";
        private const string IsIgnoredPropertyName = "IsIgnored";

        /// <summary>
        /// Get the animation id of the UI element.
        /// </summary>
        /// <returns>The animation id of the UI element</returns>
        public static string GetId(DependencyObject obj)
        {
            return (string)obj.GetValue(IdProperty);
        }

        /// <summary>
        /// Set the animation id of the UI element.
        /// </summary>
        public static void SetId(DependencyObject obj, string value)
        {
            obj.SetValue(IdProperty, value);
        }

        /// <summary>
        /// Id is used to mark the animation id of UI elements.
        /// Two elements of the same id on different controls will be connected by animation.
        /// </summary>
        public static readonly DependencyProperty IdProperty =
            DependencyProperty.RegisterAttached(IdPropertyName, typeof(string), typeof(TransitionHelper), null);

        /// <summary>
        /// Get the value indicating whether the UI element needs to be connected by animation.
        /// </summary>
        /// <returns>A bool value indicating whether the UI element needs to be connected by animation.</returns>
        public static bool GetIsIgnored(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsIgnoredProperty);
        }

        /// <summary>
        /// Set the value indicating whether the UI element needs to be connected by animation.
        /// </summary>
        public static void SetIsIgnored(DependencyObject obj, bool value)
        {
            obj.SetValue(IsIgnoredProperty, value);
        }

        /// <summary>
        /// IsIgnored is used to mark controls that do not need to be connected by animation, it will disappear/show independently.
        /// </summary>
        public static readonly DependencyProperty IsIgnoredProperty =
            DependencyProperty.RegisterAttached(IsIgnoredPropertyName, typeof(bool), typeof(TransitionHelper), new PropertyMetadata(false));

        private static IEnumerable<UIElement> GetAnimatedElements(UIElement targetElement, IEnumerable<string> filters)
        {
            if (targetElement == null)
            {
                return null;
            }

            var elements = targetElement.FindDescendants().ToList() ?? new List<DependencyObject>();
            elements.Add(targetElement);

            return filters == null
                ? elements.Where(element => GetId(element) != null).OfType<UIElement>()
                : elements.Where(element => GetId(element) != null && filters.Contains(GetId(element))).OfType<UIElement>();
        }

        private static IEnumerable<UIElement> GetIgnoredElements(UIElement targetElement)
        {
            if (targetElement == null)
            {
                return null;
            }

            var elements = targetElement.FindDescendants().ToList() ?? new List<DependencyObject>();
            elements.Add(targetElement);

            return elements.Where(element => GetIsIgnored(element)).OfType<UIElement>();
        }
    }
}
