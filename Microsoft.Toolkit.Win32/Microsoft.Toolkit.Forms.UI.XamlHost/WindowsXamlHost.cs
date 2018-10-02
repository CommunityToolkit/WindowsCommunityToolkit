// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.XamlHost;

namespace Microsoft.Toolkit.Forms.UI.XamlHost
{
    /// <summary>
    ///     A sample Windows Forms control that hosts XAML content
    /// </summary>
    [DesignerCategory("code")]
    public class WindowsXamlHost : WindowsXamlHostBase
    {
        /// <summary>
        /// Gets or sets a value indicating whether the control dynamically sizes to its content
        /// </summary>
        [ReadOnly(false)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get => base.AutoSize;

            set => base.AutoSize = value;
        }

        /// <summary>
        /// Gets or sets the automatic size mode.
        /// </summary>
        /// <value>The automatic size mode.</value>
        /// <remarks>A value indicating if the control dynamically sizes to its content.</remarks>
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public AutoSizeMode AutoSizeMode
        {
            get => GetAutoSizeMode();

            set => SetAutoSizeMode(value);
        }

        /// <summary>
        /// Gets or sets XAML Content by type name
        /// </summary>
        /// <example><code>XamlClassLibrary.MyUserControl</code></example>
        /// <remarks>
        /// Content creation is deferred until after the parent hwnd has been created.
        /// </remarks>
        [Browsable(true)]
        [Category("XAML")]
        public string InitialTypeName
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether a render transform is added to the UWP control corresponding to the current dpi scaling factor
        /// </summary>
        /// <value>The dpi scaling mode.</value>
        /// <remarks>A custom render transform added to the root UWP control will be overwritten.</remarks>
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public bool DpiScalingRenderTransform
        {
            get => _dpiScalingRenderTransformEnabled;

            set
            {
                _dpiScalingRenderTransformEnabled = value;
                UpdateDpiScalingFactor();
                PerformLayout();
            }
        }

        /// <summary>
        /// Gets or sets XAML content for XamlContentHost
        /// </summary>
        /// <value>The <see cref="Windows.UI.Xaml.UIElement"/>.</value>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Windows.UI.Xaml.UIElement Child
        {
            get => (_xamlSource.Content as DpiScalingPanel).Child;

            set => ChildInternal = value;
        }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Forms.Control.HandleCreated" /> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs" /> that contains the event data.</param>
        /// <remarks>Assign window render target to UWP XAML content.</remarks>
        protected override void OnHandleCreated(EventArgs e)
        {
            // Create content if TypeName has been set and xamlRoot has not been set
            if (!DesignMode && !string.IsNullOrEmpty(InitialTypeName) && Child == null)
            {
                Child = UWPTypeFactory.CreateXamlContentByType(InitialTypeName);
            }

            base.OnHandleCreated(e);
        }
    }
}
