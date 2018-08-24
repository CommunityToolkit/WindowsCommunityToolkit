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
        ///     Fired when XAML content has been updated
        /// </summary>
        [Browsable(true)]
        [Category("UWP XAML")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Fired when UWP XAML content has been updated")]
        public event EventHandler XamlRootUpdated;

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
        /// Gets or sets autoSizeMode, a value indicating if the control dynamically sizes to its content
        /// </summary>
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
        /// Gets or sets xAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// (Content creation is deferred until after the parent hwnd has been created.)
        /// </summary>
        [Browsable(true)]
        [Category("XAML")]
        public string InitialTypeName
        {
            get;
            set;
        }

        /// <summary>
        ///    Gets or sets XAML content for XamlContentHost
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public Windows.UI.Xaml.UIElement XamlRoot
        {
            get => desktopWindowXamlSource.Content;

            set
            {
                if (!DesignMode)
                {
                    var newFrameworkElement = value as Windows.UI.Xaml.FrameworkElement;
                    var oldFrameworkElement = desktopWindowXamlSource.Content as Windows.UI.Xaml.FrameworkElement;

                    if (oldFrameworkElement != null)
                    {
                        oldFrameworkElement.SizeChanged -= FrameworkElement_SizeChanged;
                    }

                    if (newFrameworkElement != null)
                    {
                        // If XAML content has changed, check XAML size and WindowsXamlHost.AutoSize
                        // setting to determine if WindowsXamlHost needs to re-run layout.
                        newFrameworkElement.SizeChanged += FrameworkElement_SizeChanged;
                    }

                    desktopWindowXamlSource.Content = value;

                    PerformLayout();

                    if (XamlRootUpdated != null)
                    {
                        XamlRootUpdated(this, null);
                    }
                }
            }
        }

        /// <summary>
        /// Raises the HandleCreated event.  Assign window render target to UWP XAML content.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            // Create content if TypeName has been set and xamlRoot has not been set
            if (!DesignMode && !string.IsNullOrEmpty(InitialTypeName) && XamlRoot == null)
            {
                XamlRoot = UWPTypeFactory.CreateXamlContentByType(InitialTypeName);
            }

            base.OnHandleCreated(e);
        }
    }
}
