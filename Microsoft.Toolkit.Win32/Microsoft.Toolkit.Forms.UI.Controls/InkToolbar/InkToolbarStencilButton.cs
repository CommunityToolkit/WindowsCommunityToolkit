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
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
    /// </summary>
    [Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarStencilButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarStencilButton UwpControl => ChildInternal as Windows.UI.Xaml.Controls.InkToolbarStencilButton;

        private System.Collections.Generic.Dictionary<string, object> DesignerProperties { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarStencilButton"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
        /// </summary>
        public InkToolbarStencilButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarStencilButton).FullName)
        {
        }

        protected InkToolbarStencilButton(string name)
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
        }

        /// <inheritdoc />
        protected override void SetContent(UIElement newValue)
        {
            // intentionally empty
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsExtensionGlyphShown { get => (bool)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets the underlying Uwp Control's SelectedStencil value <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencil"/>
        /// </summary>
        [DefaultValue(InkToolbarStencilKind.Ruler)]
        public InkToolbarStencilKind SelectedStencil { get => (InkToolbarStencilKind)this.GetUwpControlValue(); set => this.SetUwpControlValue((Windows.UI.Xaml.Controls.InkToolbarStencilKind)value); }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsRulerItemVisible property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisible"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsRulerItemVisible { get => (bool)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsProtractorItemVisible property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisible"/>
        /// </summary>
        [DefaultValue(false)]
        public bool IsProtractorItemVisible { get => (bool)this.GetUwpControlValue(); set => this.SetUwpControlValue(value); }

        /// <summary>
        /// Gets the underlying Uwp Control's Protractor value <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.Protractor"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InkPresenterProtractor Protractor { get => UwpControl?.Protractor; }

        /// <summary>
        /// Gets the underlying Uwp Control's Ruler value <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.Ruler"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InkPresenterRuler Ruler { get => UwpControl?.Ruler; }
    }
}