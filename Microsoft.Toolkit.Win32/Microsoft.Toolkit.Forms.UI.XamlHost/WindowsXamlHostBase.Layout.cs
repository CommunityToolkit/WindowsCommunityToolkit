// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Drawing;
using Microsoft.Toolkit.Forms.UI.XamlHost.Interop.Win32;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     WindowsXamlHostBase hosts UWP XAML content inside Windows Forms
    /// </summary>
    public partial class WindowsXamlHostBase
    {
        /// <summary>
        ///     Overrides the base class implementation of <see cref="GetPreferredSize(Size)" /> to provide
        ///     correct layout behavior for the hosted XAML content.
        /// </summary>
        /// <returns>preferred size</returns>
        public override Size GetPreferredSize(Size proposedSize)
        {
            if (DesignMode)
            {
                return Size;
            }

            if (ChildInternal != null)
            {
                double proposedWidth = proposedSize.Width;
                double proposedHeight = proposedSize.Height;

                // DockStyles will result in a constraint of 1 on the Docked axis. GetPreferredSize
                // must convert this into an unconstrained value.
                // Convert the proposed size from pixels to effective pixels if the XAML island scales the content
                if (proposedSize.Height == int.MaxValue || proposedSize.Height == 1)
                {
                    proposedHeight = double.PositiveInfinity;
                }
                else if (_xamlIslandHandlesDpiScaling)
                {
                    proposedHeight /= _lastDpi / 96.0f;
                }

                if (proposedSize.Width == int.MaxValue || proposedSize.Width == 1)
                {
                    proposedWidth = double.PositiveInfinity;
                }
                else if (_xamlIslandHandlesDpiScaling)
                {
                    proposedWidth /= _lastDpi / 96.0f;
                }

                _xamlSource.Content.Measure(new Windows.Foundation.Size(proposedWidth, proposedHeight));
            }

            var preferredSize = Size.Empty;
            if (ChildInternal != null)
            {
                // Convert effective pixels to pixels if necessary
                if (_xamlIslandHandlesDpiScaling)
                {
                    preferredSize = new Size((int)(_xamlSource.Content.DesiredSize.Width * _lastDpi / 96.0f), (int)(_xamlSource.Content.DesiredSize.Height * _lastDpi / 96.0f));
                }
                else
                {
                    preferredSize = new Size((int)_xamlSource.Content.DesiredSize.Width, (int)_xamlSource.Content.DesiredSize.Height);
                }
            }

            return preferredSize;
        }

        /// <summary>
        ///     Sets a scaling factor based on the current dpi value on the scaling panel
        /// </summary>
        protected void UpdateDpiScalingFactor()
        {
            DpiScalingPanel panel = _xamlSource.Content as DpiScalingPanel;
            double dpi = 96.0f;
            if (_xamlIslandWindowHandle != IntPtr.Zero)
            {
                uint windowDpi = SafeNativeMethods.GetDpiForWindow(_xamlIslandWindowHandle);
                if (windowDpi > 0)
                {
                    dpi = windowDpi;
                }
            }

            _lastDpi = dpi;

            double newScalingFactor = (_dpiScalingRenderTransformEnabled == true && _xamlIslandHandlesDpiScaling == false) ? (dpi / 96.0f) : 1.0f;

            panel.SetScalingFactor(newScalingFactor);
        }

        /// <summary>
        ///     Gets XAML content's 'DesiredSize' post-Measure.
        /// </summary>
        /// <returns>desired size</returns>
        /// <remarks>Called by <see cref="OnChildSizeChanged" /> event handler.
        /// </remarks>
        private Size GetRootXamlElementDesiredSize()
        {
            var desiredSize = new Size((int)_xamlSource.Content.DesiredSize.Width, (int)_xamlSource.Content.DesiredSize.Height);

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
        protected void OnChildSizeChanged(object sender, object e)
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
        ///     Event handler for <see cref="System.Windows.Forms.Control.SizeChanged" />. If the size of the host control
        ///     has changed, re-run Windows Forms layout on this Control instance.
        /// </summary>
        protected void OnWindowXamlHostSizeChanged(object sender, EventArgs e)
        {
            if (DesignMode)
            {
                return;
            }

            if (AutoSize)
            {
                if (ChildInternal != null)
                {
                    // XamlContenHost Control.Size has changed. XAML must perform an Arrange pass.
                    // The XAML Arrange pass will expand XAML content with 'HorizontalStretch' and
                    // 'VerticalStretch' properties to the bounds of the XamlContentHost Control.
                    var rect = new Windows.Foundation.Rect(0, 0, Width, Height);
                    if (_xamlIslandHandlesDpiScaling)
                    {
                        rect.Width /= _lastDpi / 96.0f;
                        rect.Height /= _lastDpi / 96.0f;
                    }

                    _xamlSource.Content.Measure(new Windows.Foundation.Size(rect.Width, rect.Height));
                    _xamlSource.Content.Arrange(rect);
                    PerformLayout();
                }
            }
        }
    }
}
