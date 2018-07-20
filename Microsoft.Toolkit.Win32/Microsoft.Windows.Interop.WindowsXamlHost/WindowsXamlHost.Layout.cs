// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop
{
    using System;
    using System.Windows;
    using System.Windows.Interop;

    partial class WindowsXamlHost : HwndHost
    {
        /// <summary>
        /// Measures wrapped UWP XAML content using passed in size constraint
        /// </summary>
        /// <param name="constraint">Available Size</param>
        /// <returns>XAML DesiredSize</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = new Size(0, 0);

            if (DesktopWindowXamlSource.Content != null)
            {
                DesktopWindowXamlSource.Content.Measure(new Windows.Foundation.Size(constraint.Width, constraint.Height));
                desiredSize.Width = DesktopWindowXamlSource.Content.DesiredSize.Width;
                desiredSize.Height = DesktopWindowXamlSource.Content.DesiredSize.Height;
            }

            desiredSize.Width = Math.Min(desiredSize.Width, constraint.Width);
            desiredSize.Height = Math.Min(desiredSize.Height, constraint.Height);

            return desiredSize;
        }

        /// <summary>
        /// Arranges wrapped UWP XAML content using passed in size constraint
        /// </summary>
        /// <param name="finalSize">Final Size</param>
        /// <returns>Size</returns>
        protected override Size ArrangeOverride(Size finalSize)
        {
            if (DesktopWindowXamlSource.Content != null)
            {
                // Arrange is required to support HorizontalAlignment and VerticalAlignment properties
                // set to 'Stretch'.  The UWP XAML content will be 0 in the stretch alignment direction
                // until Arrange is called, and the UWP XAML content is expanded to fill the available space.
                var finalRect = new Windows.Foundation.Rect(0, 0, finalSize.Width, finalSize.Height);
                DesktopWindowXamlSource.Content.Arrange(finalRect);
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// Notifies host control when wrapped UWP XAML content has become dirty and performed a layout pass
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">Object</param>
        private void XamlContentLayoutUpdated(object sender, object e)
        {
            // UWP XAML content has changed. Force parent control to re-measure.
            InvalidateMeasure();
        }

        /// <summary>
        /// UWP XAML content size changed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">SizeChangedEventArgs</param>
        private void XamlContentSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            ParentLayoutInvalidated(this);
        }
    }
}
