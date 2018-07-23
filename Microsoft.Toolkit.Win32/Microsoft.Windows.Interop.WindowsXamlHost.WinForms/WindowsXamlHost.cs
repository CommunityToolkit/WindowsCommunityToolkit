// <copyright file="WindowsXamlHost.cs" company="Microsoft">
//     Copyright (c) Microsoft Corporation
// </copyright>
// <author>Microsoft</author>

namespace Microsoft.Toolkit.Win32.UI.Interop.WinForms
{
    using System;
    using System.ComponentModel;
    using System.Security;
    using System.Security.Permissions;
    using System.Windows.Forms;

    /// <summary>
    ///     A sample Windows Forms control that hosts XAML content
    /// </summary>
    [System.ComponentModel.DesignerCategory("code")]
    partial class WindowsXamlHost : WindowsXamlHostBase
    {

        #region Constructors
        /// <summary>
        ///     Initializes a new instance of the XamlContentHost class.
        /// </summary>
        [PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public WindowsXamlHost()
            : base()
        {
        }

        #endregion

        #region Events
        /// <summary>
        ///     Fired when XAML content has been updated
        /// </summary>
        [Browsable(true)]
        [Category("UWP XAML")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        [Description("Fired when UWP XAML content has been updated")]
        public event EventHandler XamlRootUpdated;
        #endregion

        #region Properties

        /// <summary>
        /// AutoSize determines whether the Control dynamically sizes to its content
        /// </summary>
        [ReadOnly(false)]
        [Browsable(true)]
        [DefaultValue(false)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize; 
            }

            set
            {
                base.AutoSize = value;
            }
        }

        /// <summary>
        /// AutoSizeMode determines whether the Control dynamically sizes to its content
        /// </summary>
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public AutoSizeMode AutoSizeMode
        {
            get
            {
                return GetAutoSizeMode();
            }

            set
            {
                SetAutoSizeMode(value);
            }
        }

        /// <summary>
        /// XAML Content by type name : MyNamespace.MyClass.MyType
        /// ex: XamlClassLibrary.MyUserControl
        /// (Content creation is deferred until after the parent hwnd has been created.)
        /// </summary>
        [Browsable(true)]
        [Category("XAML")]
        public string InitialTypeName
        {
            get; set;
        }

        /// <summary>
        ///    Gets or sets XAML content for XamlContentHost
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public global::Windows.UI.Xaml.UIElement XamlRoot
        {
            get
            {
                return this.desktopWindowXamlSource.Content;
            }

            set
            {
                if (!DesignMode)
                {
                    global::Windows.UI.Xaml.FrameworkElement newFrameworkElement = value as global::Windows.UI.Xaml.FrameworkElement;
                    global::Windows.UI.Xaml.FrameworkElement oldFrameworkElement = this.desktopWindowXamlSource.Content as global::Windows.UI.Xaml.FrameworkElement;

                    if (oldFrameworkElement != null)
                    {
                        oldFrameworkElement.SizeChanged -= this.FrameworkElement_SizeChanged;
                    }

                    if (newFrameworkElement != null)
                    {
                        // If XAML content has changed, check XAML size and WindowsXamlHost.AutoSize
                        // setting to determine if WindowsXamlHost needs to re-run layout.
                        newFrameworkElement.SizeChanged += this.FrameworkElement_SizeChanged;
                    }

                    this.desktopWindowXamlSource.Content = value;

                    this.PerformLayout();

                    if (XamlRootUpdated != null)
                    {
                        this.XamlRootUpdated(this, null);
                    }
                }
            }
        }

        #endregion

        /// <summary>
        /// Raises the HandleCreated event.  Assign window render target to UWP XAML content.
        /// </summary>
        /// <param name="e">EventArgs</param>
        protected override void OnHandleCreated(EventArgs e)
        {
            // Create content if TypeName has been set and xamlRoot has not been set
            if (!DesignMode && !string.IsNullOrEmpty(this.InitialTypeName) && this.XamlRoot == null)
            {
                this.XamlRoot = UWPTypeFactory.CreateXamlContentByType(this.InitialTypeName);
            }

            base.OnHandleCreated(e);
        }

        /// <summary>
        /// Cleanup hosted XAML content
        /// </summary>
        /// <param name="disposing">IsDisposing?</param>
        protected override void Dispose(bool disposing) 
        {
            base.Dispose(disposing);
        }
    }
}
