// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     A sample Windows Forms control that can be used to host XAML content
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        ///     Overrides the base class implementation of GetPreferredSize to provide
        ///     correct layout behavior for the hosted XAML content.
        /// </summary>
        /// <returns>preferred size</returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (DesignMode)
            {
                return Size;
            }

            if (desktopWindowXamlSource.Content != null)
            {
                double proposedWidth = proposedSize.Width;
                double proposedHeight = proposedSize.Height;

                // DockStyles will result in a constraint of 1 on the Docked axis. GetPreferredSize
                // must convert this into an unconstrained value.
                if (proposedSize.Height == int.MaxValue || proposedSize.Height == 1)
                {
                    proposedHeight = double.PositiveInfinity;
                }

                if (proposedSize.Width == int.MaxValue || proposedSize.Width == 1)
                {
                    proposedWidth = double.PositiveInfinity;
                }

                desktopWindowXamlSource.Content.Measure(new Windows.Foundation.Size(proposedWidth, proposedHeight));
            }

            var preferredSize = Size.Empty;
            if (desktopWindowXamlSource.Content != null)
            {
                preferredSize = new Size((int)desktopWindowXamlSource.Content.DesiredSize.Width, (int)desktopWindowXamlSource.Content.DesiredSize.Height);
            }

            return preferredSize;
        }

        /// <summary>
        ///     Gets XAML content's 'DesiredSize' post-Measure. Called by
        ///     XamlContentHost's XAML LayoutUpdated event handler.
        /// </summary>
        /// <returns>desired size</returns>
        private Size GetRootXamlElementDesiredSize()
        {
            var desiredSize = new Size((int)desktopWindowXamlSource.Content.DesiredSize.Width, (int)desktopWindowXamlSource.Content.DesiredSize.Height);

            return desiredSize;
        }

        /// <summary>
        ///     Gets the default size of the control.
        /// </summary>
        protected override Size DefaultSize
        {
            get
            {
                // XamlContentHost's DefaultSize is 0, 0
                var defaultSize = Size.Empty;

                return defaultSize;
            }
        }

        /// <summary>
        ///     Responds to UWP XAML's 'SizeChanged' event, fired when XAML content
        ///     layout has changed.  If 'DesiredSize' has changed, re-run
        ///     Windows Forms layout.
        /// </summary>
        protected void FrameworkElement_SizeChanged(object sender, object e)
        {
            if (DesignMode)
            {
                return;
            }

            // XAML content has changed. Re-run Windows.Forms.Control Layout if parent form is
            // set to AutoSize.
            if (AutoSize)
            {
                var prefSize = GetRootXamlElementDesiredSize();

                if (_lastXamlContentPreferredSize.Height != prefSize.Height || _lastXamlContentPreferredSize.Width != prefSize.Width)
                {
                    _lastXamlContentPreferredSize = prefSize;
                    PerformLayout();
                }
            }
        }

        /// <summary>
        ///     Event handler for XamlContentHost SizeChanged. If the size of the host control
        ///     has changed, re-run Windows Forms layout on this Control instance.
        /// </summary>
        private void WindowsXamlHost_SizeChanged(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            if (AutoSize)
            {
                if (desktopWindowXamlSource.Content != null)
                {
                    // XamlContenHost Control.Size has changed. XAML must perform an Arrange pass.
                    // The XAML Arrange pass will expand XAML content with 'HorizontalStretch' and
                    // 'VerticalStretch' properties to the bounds of the XamlContentHost Control.
                    var rect = new Windows.Foundation.Rect(0, 0, Width, Height);
                    desktopWindowXamlSource.Content.Measure(new Windows.Foundation.Size(Width, Height));
                    desktopWindowXamlSource.Content.Arrange(rect);
                    PerformLayout();
                }
            }
        }
    }
}
