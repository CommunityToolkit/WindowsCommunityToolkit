// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;

namespace Microsoft.Toolkit.Wpf.UI.XamlHost
{
    /// <summary>
    /// Integrates UWP XAML in to WPF's layout system
    /// </summary>
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

            if (_xamlSource.Content != null)
            {
                _xamlSource.Content.Measure(new Windows.Foundation.Size(constraint.Width, constraint.Height));
                desiredSize.Width = _xamlSource.Content.DesiredSize.Width;
                desiredSize.Height = _xamlSource.Content.DesiredSize.Height;
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
            if (_xamlSource.Content != null)
            {
                // Arrange is required to support HorizontalAlignment and VerticalAlignment properties
                // set to 'Stretch'.  The UWP XAML content will be 0 in the stretch alignment direction
                // until Arrange is called, and the UWP XAML content is expanded to fill the available space.
                var finalRect = new Windows.Foundation.Rect(0, 0, finalSize.Width, finalSize.Height);
                _xamlSource.Content.Arrange(finalRect);
            }

            return base.ArrangeOverride(finalSize);
        }

        /// <summary>
        /// UWP XAML content size changed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="Windows.UI.Xaml.SizeChangedEventArgs"/> instance containing the event data.</param>
        protected void XamlContentSizeChanged(object sender, Windows.UI.Xaml.SizeChangedEventArgs e)
        {
            InvalidateMeasure();
        }
    }
}
