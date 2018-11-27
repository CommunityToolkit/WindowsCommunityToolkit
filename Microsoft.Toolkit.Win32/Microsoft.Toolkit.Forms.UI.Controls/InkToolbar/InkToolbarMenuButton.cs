// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>
    /// </summary>
    [Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarMenuButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarMenuButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarMenuButton;

        private System.Collections.Generic.Dictionary<string, object> DesignerProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarMenuButton"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton"/>
        /// </summary>
        public InkToolbarMenuButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarMenuButton).FullName)
        {
        }

        protected InkToolbarMenuButton(string name)
            : base(name)
        {
            // Return immediately if control is instantiated by the Visual Studio Designer
            // https://stackoverflow.com/questions/1166226/detecting-design-mode-from-a-controls-constructor
            if (LicenseManager.UsageMode == LicenseUsageMode.Designtime)
            {
                return;
            }
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            if (UwpControl != null)
            {
                UwpControl.Checked += UwpControl_Checked;
                UwpControl.Indeterminate += UwpControl_Indeterminate;
                UwpControl.Unchecked += UwpControl_Unchecked;
            }
        }

        /// <inheritdoc />
        protected override void SetContent(UIElement newValue)
        {
            // intentionally empty
        }

        /// <summary>
        /// Raised when the underlying Uwp control's Checked event is fired. <see cref="Windows.UI.Xaml.Controls.Primitives.ToggleButton.Checked"/>
        /// </summary>
        public event EventHandler Checked;

        /// <summary>
        /// Raised when the underlying Uwp control's Indeterminate event is fired. <see cref="Windows.UI.Xaml.Controls.Primitives.ToggleButton.Indeterminate"/>
        /// </summary>
        public event EventHandler Indeterminate;

        /// <summary>
        /// Raised when the underlying Uwp control's Unchecked event is fired. <see cref="Windows.UI.Xaml.Controls.Primitives.ToggleButton.Unchecked"/>
        /// </summary>
        public event EventHandler Unchecked;

        private void UwpControl_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Unchecked?.Invoke(this, EventArgs.Empty);
        }

        private void UwpControl_Indeterminate(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Indeterminate?.Invoke(this, EventArgs.Empty);
        }

        private void UwpControl_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            Checked?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown is set. <see cref="Windows.UI.Xaml.Controls.InkToolbarMenuButton.IsExtensionGlyphShown"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsExtensionGlyphShown { get => (bool)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsThreeState is set. <see cref="Windows.UI.Xaml.Controls.Primitives.ToggleButton.IsThreeState"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsThreeState { get => (bool)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsChecked is set. <see cref="Windows.UI.Xaml.Controls.Primitives.ToggleButton.IsChecked"/>
        /// </summary>
        [DefaultValue(false)]
        public bool? IsChecked { get => (bool?)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            if (disposing)
            {
                UwpControl.Checked -= UwpControl_Checked;
                UwpControl.Unchecked -= UwpControl_Unchecked;
                UwpControl.Indeterminate -= UwpControl_Indeterminate;
            }
        }
    }
}