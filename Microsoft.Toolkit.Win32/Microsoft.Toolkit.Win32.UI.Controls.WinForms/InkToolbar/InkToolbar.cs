// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Interop;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>
    /// </summary>
    [Designer(typeof(InkToolbarDesigner))]
    public class InkToolbar : WindowsXamlHostBaseExt
    {
        protected Windows.UI.Xaml.Controls.InkToolbar UwpControl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbar"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>
        /// </summary>
        public InkToolbar()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbar).FullName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbar"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>
        /// </summary>
        protected InkToolbar(string name)
            : base(name)
        {
            InitializeElement();
        }

        internal override void InitializeElement()
        {
            base.InitializeElement();
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkToolbar;
            ControlAdded += InkToolbar_ControlAdded;
            ControlRemoved += InkToolbar_ControlRemoved;
        }

        private void InkToolbar_ControlRemoved(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBaseExt control)
            {
                UwpControl.Children.Remove(control.XamlElement);
            }
        }

        private void InkToolbar_ControlAdded(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBaseExt control)
            {
                UwpControl.Children.Add(control.XamlElement);
            }
        }

        /// <summary>
        /// Gets or sets the underlying Uwp control's TargetInkCanvas property <see cref="Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvas"/>
        /// </summary>
        public InkCanvas TargetInkCanvas
        {
            get
            {
                if (UwpControl?.TargetInkCanvas == null)
                {
                    return null;
                }

                return UwpControl.TargetInkCanvas.GetWrapper() as InkCanvas;
            }

            set
            {
                if (UwpControl != null)
                {
                    UwpControl.TargetInkCanvas = value?.UwpControl;
                }
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonChecked"/>
        /// </summary>
        public bool IsRulerButtonChecked
        {
            get => UwpControl.IsRulerButtonChecked;
            set => UwpControl.IsRulerButtonChecked = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.InitialControls"/>
        /// </summary>
        public InkToolbarInitialControls InitialControls
        {
            get => (InkToolbarInitialControls)UwpControl.InitialControls;
            set => UwpControl.InitialControls = (Windows.UI.Xaml.Controls.InkToolbarInitialControls)value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.ActiveTool"/>
        /// </summary>
        public Microsoft.Toolkit.Win32.UI.Controls.WinForms.WindowsXamlHostBaseExt ActiveTool
        {
            get => (WindowsXamlHostBaseExt)UwpControl.ActiveTool.GetWrapper();
            set => UwpControl.ActiveTool = value.XamlElement as Windows.UI.Xaml.Controls.InkToolbarToolButton;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributes"/>
        /// </summary>
        public InkDrawingAttributes InkDrawingAttributes
        {
            get => (InkDrawingAttributes)UwpControl.InkDrawingAttributes;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.Orientation"/>
        /// </summary>
        public Orientation Orientation
        {
            get => (Orientation)UwpControl.Orientation;
            set => UwpControl.Orientation = (Windows.UI.Xaml.Controls.Orientation)value;
        }

        /// <summary>
        /// Gets or sets a value indicating whether <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonChecked"/>
        /// </summary>
        public bool IsStencilButtonChecked
        {
            get => UwpControl.IsStencilButtonChecked;
            set => UwpControl.IsStencilButtonChecked = value;
        }

        /// <summary>
        /// Gets or sets <see cref="Windows.UI.Xaml.Controls.InkToolbar.ButtonFlyoutPlacement"/>
        /// </summary>
        public InkToolbarButtonFlyoutPlacement ButtonFlyoutPlacement
        {
            get => (InkToolbarButtonFlyoutPlacement)UwpControl.ButtonFlyoutPlacement;
            set => UwpControl.ButtonFlyoutPlacement = (Windows.UI.Xaml.Controls.InkToolbarButtonFlyoutPlacement)value;
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.ActiveToolChanged"/>
        /// </summary>
        public event EventHandler<object> ActiveToolChanged = (sender, args) => { };

        private void OnActiveToolChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            ActiveToolChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.EraseAllClicked"/>
        /// </summary>
        public event EventHandler<object> EraseAllClicked = (sender, args) => { };

        private void OnEraseAllClicked(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            EraseAllClicked?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.InkDrawingAttributesChanged"/>
        /// </summary>
        public event EventHandler<object> InkDrawingAttributesChanged = (sender, args) => { };

        private void OnInkDrawingAttributesChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            InkDrawingAttributesChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsRulerButtonCheckedChanged"/>
        /// </summary>
        public event EventHandler<object> IsRulerButtonCheckedChanged = (sender, args) => { };

        private void OnIsRulerButtonCheckedChanged(Windows.UI.Xaml.Controls.InkToolbar sender, object args)
        {
            IsRulerButtonCheckedChanged?.Invoke(this, args);
        }

        /// <summary>
        /// <see cref="Windows.UI.Xaml.Controls.InkToolbar.IsStencilButtonCheckedChanged"/>
        /// </summary>
        public event EventHandler<Microsoft.Toolkit.Win32.UI.Controls.WinForms.InkToolbarIsStencilButtonCheckedChangedEventArgs> IsStencilButtonCheckedChanged = (sender, args) => { };

        private void OnIsStencilButtonCheckedChanged(Windows.UI.Xaml.Controls.InkToolbar sender, Windows.UI.Xaml.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs args)
        {
            IsStencilButtonCheckedChanged?.Invoke(this, args);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                ControlAdded -= InkToolbar_ControlAdded;
                ControlRemoved -= InkToolbar_ControlRemoved;
            }
        }
    }
}
