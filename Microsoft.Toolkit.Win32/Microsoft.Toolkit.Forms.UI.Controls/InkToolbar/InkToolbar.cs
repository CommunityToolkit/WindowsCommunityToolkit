// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbar"/>
    /// </summary>
    [Designer(typeof(InkToolbarDesigner))]
    public class InkToolbar : WindowsXamlHostBase
    {
        protected Windows.UI.Xaml.Controls.InkToolbar UwpControl => GetUwpInternalObject() as Windows.UI.Xaml.Controls.InkToolbar;

        private InkCanvas _targetInkCanvas;
        private WindowsXamlHostBase _activeTool;

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
            if (UwpControl != null)
            {
                ControlAdded += InkToolbar_ControlAdded;
                ControlRemoved += InkToolbar_ControlRemoved;
                UwpControl.Visibility = Windows.UI.Xaml.Visibility.Collapsed; // supports a workaround for a bug:  InkToolbar won't initialize if it's not initially collapsed.
                UwpControl.Loaded += OnUwpControlLoaded;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        /// <summary>
        /// supports a workaround for a bug:  InkToolbar won't initialize if it's not initially collapsed, so we update visibility on it's loaded event.
        /// </summary>
        /// <param name="sender">event sender</param>
        /// <param name="e">event parameters</param>
        private void OnUwpControlLoaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            UwpControl.Visibility = Windows.UI.Xaml.Visibility.Visible;
            UwpControl.Loaded -= OnUwpControlLoaded;
        }

        private void InkToolbar_ControlRemoved(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBase control)
            {
                UwpControl.Children.Remove(control.GetUwpInternalObject() as UIElement);
            }
        }

        private void InkToolbar_ControlAdded(object sender, System.Windows.Forms.ControlEventArgs e)
        {
            if (e.Control is WindowsXamlHostBase control)
            {
                UwpControl.Children.Add(control.GetUwpInternalObject() as UIElement);
            }
        }

        /// <summary>
        /// Gets or sets the underlying Uwp control's TargetInkCanvas property <see cref="Windows.UI.Xaml.Controls.InkToolbar.TargetInkCanvas"/>
        /// </summary>
        public InkCanvas TargetInkCanvas
        {
            get
            {
                if (DesignMode)
                {
                    return _targetInkCanvas;
                }

                if (UwpControl?.TargetInkCanvas == null)
                {
                    return null;
                }

                return UwpControl.TargetInkCanvas.GetWrapper() as InkCanvas;
            }

            set
            {
                if (DesignMode)
                {
                    _targetInkCanvas = value;
                    return;
                }

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
        public WindowsXamlHostBase ActiveTool
        {
            get
            {
                if (DesignMode)
                {
                    return _activeTool;
                }

                return (WindowsXamlHostBase)UwpControl?.ActiveTool?.GetWrapper();
            }

            set
            {
                if (DesignMode)
                {
                    _activeTool = value;
                    return;
                }

                UwpControl.ActiveTool = value.GetUwpInternalObject() as Windows.UI.Xaml.Controls.InkToolbarToolButton;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the control dynamically sizes to its content
        /// </summary>
        [ReadOnly(false)]
        [Browsable(true)]
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
        [ReadOnly(false)]
        [Browsable(true)]
        [Category("Layout")]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public System.Windows.Forms.AutoSizeMode AutoSizeMode
        {
            get => GetAutoSizeMode();

            set => SetAutoSizeMode(value);
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
        public event EventHandler<Microsoft.Toolkit.Forms.UI.Controls.InkToolbarIsStencilButtonCheckedChangedEventArgs> IsStencilButtonCheckedChanged = (sender, args) => { };

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
