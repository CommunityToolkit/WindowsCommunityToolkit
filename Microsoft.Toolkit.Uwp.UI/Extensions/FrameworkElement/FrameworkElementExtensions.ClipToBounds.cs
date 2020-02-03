// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties for the <see cref="Windows.UI.Xaml.FrameworkElement"/>
    /// </summary>
    public static partial class FrameworkElementExtensions
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> to activate clipping of a <see cref="FrameworkElement"/> content inside the element bounds.
        /// </summary>
        /// <remarks>
        /// This property comes from WPF but is not supported natively in UWP apps.
        /// See https://docs.microsoft.com/en-us/dotnet/api/system.windows.uielement.cliptobounds?view=netframework-4.8.
        /// </remarks>
        public static readonly DependencyProperty ClipToBoundsProperty = DependencyProperty.RegisterAttached(
            "ClipToBounds",
            typeof(bool),
            typeof(FrameworkElementExtensions),
            new PropertyMetadata(false, OnClipToBoundsPropertyChanged));

        /// <summary>
        /// Get the value of <see cref="ClipToBoundsProperty"/>.
        /// </summary>
        /// <param name="obj">The object that will host the value.</param>
        /// <returns>The property value. <see cref="ClipToBoundsProperty"/>.</returns>
        public static bool GetClipToBounds(DependencyObject obj) => (bool)obj.GetValue(ClipToBoundsProperty);

        /// <summary>
        /// Set the value of <see cref="ClipToBoundsProperty"/>.
        /// </summary>
        /// <param name="obj">The target object where to store the value.</param>
        /// <param name="value">The value to store.</param>
        public static void SetClipToBounds(DependencyObject obj, bool value) => obj.SetValue(ClipToBoundsProperty, value);

        private static void OnClipToBoundsPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = d as FrameworkElement;
            Debug.Assert(element != null, "Property should only be attached to FrameworkElement");

            void OnSizeChanged(object sender, SizeChangedEventArgs sizeChangedEventArgs) => UpdateClipToBounds(element);

            var newValue = (bool)e.NewValue;
            if (newValue)
            {
                element.SizeChanged += OnSizeChanged;
                UpdateClipToBounds(element);
            }
            else
            {
                element.SizeChanged -= OnSizeChanged;
                element.Clip = null;
            }
        }

        private static void UpdateClipToBounds(FrameworkElement element)
            => element.Clip = new RectangleGeometry
            {
                Rect = new Rect(0, 0, element.ActualWidth, element.ActualHeight),
            };
    }
}
