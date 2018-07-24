// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    using System;
    using System.Drawing;

    /// <summary>
    ///     A sample Windows Forms control that can be used to host XAML content
    /// </summary>
    partial class WindowsXamlHostBase : System.Windows.Forms.Control
    {
        /// <summary>
        ///     Overrides the base class implementation of GetPreferredSize to provide
        ///     correct layout behavior for the hosted XAML content.
        /// </summary>
        public override System.Drawing.Size GetPreferredSize(System.Drawing.Size proposedSize)
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
                if (proposedSize.Height == Int32.MaxValue || proposedSize.Height == 1)
                {
                    proposedHeight = double.PositiveInfinity;
                }

                if (proposedSize.Width == Int32.MaxValue || proposedSize.Width == 1)
                {
                    proposedWidth = double.PositiveInfinity;
                }

                this.desktopWindowXamlSource.Content.Measure(new global::Windows.Foundation.Size(proposedWidth, proposedHeight));
            }

            System.Drawing.Size preferredSize = System.Drawing.Size.Empty;
            if (desktopWindowXamlSource.Content != null)
            {
                preferredSize = new System.Drawing.Size((int)this.desktopWindowXamlSource.Content.DesiredSize.Width, (int)this.desktopWindowXamlSource.Content.DesiredSize.Height); 
            }

            return preferredSize;
        }

        /// <summary>
        ///     Gets XAML content's 'DesiredSize' post-Measure. Called by 
        ///     XamlContentHost's XAML LayoutUpdated event handler.
        /// </summary>
        private Size GetRootXamlElementDesiredSize()
        {
            Size desiredSize = new Size();

            desiredSize.Height = (int)desktopWindowXamlSource.Content.DesiredSize.Height;
            desiredSize.Width = (int)desktopWindowXamlSource.Content.DesiredSize.Width;

            return desiredSize;
        }

        /// <summary>
        ///     Gets the default size of the control.
        /// </summary>
        protected override System.Drawing.Size DefaultSize
        {
            get
            {
                // XamlContentHost's DefaultSize is 0, 0 
                Size defaultSize = Size.Empty;

                return defaultSize;
            }
        }
         
        /// <summary>
        /// Called when the location of the host Control changes
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnLocationChanged(EventArgs e)
        {
            base.OnLocationChanged(e);
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
                Size prefSize = GetRootXamlElementDesiredSize();

                if (lastXamlContentPreferredSize.Height != prefSize.Height || lastXamlContentPreferredSize.Width != prefSize.Width)
                {
                    lastXamlContentPreferredSize = prefSize;
                    this.PerformLayout();
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
                    global::Windows.Foundation.Rect rect = new global::Windows.Foundation.Rect(0, 0, this.Width, this.Height);
                    this.desktopWindowXamlSource.Content.Measure(new global::Windows.Foundation.Size(this.Width, this.Height));
                    this.desktopWindowXamlSource.Content.Arrange(rect);
                    this.PerformLayout();
                }
            }
        }
    }
}
