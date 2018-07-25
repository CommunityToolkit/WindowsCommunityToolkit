// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;

namespace Microsoft.Toolkit.Win32.UI.Interop.WPF
{
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        /// Measures wrapped UWP XAML content using passed in size constraint
        /// </summary>
        /// <param name="constraint">Available Size</param>
        /// <returns>XAML DesiredSize</returns>
        protected override Size MeasureOverride(Size constraint)
        {
            var desiredSize = new Size(0, 0);

            if (desktopWindowXamlSource.Content != null)
            {
                desktopWindowXamlSource.Content.Measure(new Windows.Foundation.Size(constraint.Width, constraint.Height));
                desiredSize.Width = desktopWindowXamlSource.Content.DesiredSize.Width;
                desiredSize.Height = desktopWindowXamlSource.Content.DesiredSize.Height;
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
            if (desktopWindowXamlSource.Content != null)
            {
                // Arrange is required to support HorizontalAlignment and VerticalAlignment properties
                // set to 'Stretch'.  The UWP XAML content will be 0 in the stretch alignment direction
                // until Arrange is called, and the UWP XAML content is expanded to fill the available space.
                var finalRect = new Windows.Foundation.Rect(0, 0, finalSize.Width, finalSize.Height);
                desktopWindowXamlSource.Content.Arrange(finalRect);
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// UWP XAML content size changed
        /// </summary>
        /// <param name="sender">Sender</param>
        /// <param name="e">SizeChangedEventArgs</param>
        protected void XamlContentSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
