// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton"/>
    /// </summary>
    [Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarStencilButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarStencilButton UwpControl { get; set; }

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
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkToolbarStencilButton;
        }

        protected override void SetContent()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown { get => UwpControl.IsExtensionGlyphShown; set => UwpControl.IsExtensionGlyphShown = value; }

        /// <summary>
        /// Gets or sets the underlying Uwp Control's SelectedStencil value <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.SelectedStencil"/>
        /// </summary>
        public InkToolbarStencilKind SelectedStencil { get => (InkToolbarStencilKind)UwpControl.SelectedStencil; set => UwpControl.SelectedStencil = (Windows.UI.Xaml.Controls.InkToolbarStencilKind)value; }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsRulerItemVisible property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsRulerItemVisible"/>
        /// </summary>
        public bool IsRulerItemVisible { get => UwpControl.IsRulerItemVisible; set => UwpControl.IsRulerItemVisible = value; }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsProtractorItemVisible property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.IsProtractorItemVisible"/>
        /// </summary>
        public bool IsProtractorItemVisible { get => UwpControl.IsProtractorItemVisible; set => UwpControl.IsProtractorItemVisible = value; }

        /// <summary>
        /// Gets the underlying Uwp Control's Protractor value <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.Protractor"/>
        /// </summary>
        public InkPresenterProtractor Protractor { get => UwpControl.Protractor; }

        /// <summary>
        /// Gets the underlying Uwp Control's Ruler value <see cref="Windows.UI.Xaml.Controls.InkToolbarStencilButton.Ruler"/>
        /// </summary>
        public InkPresenterRuler Ruler { get => UwpControl.Ruler; }
    }
}