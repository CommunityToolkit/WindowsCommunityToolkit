// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>
    /// </summary>
    [Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarCustomToolButton : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkToolbarCustomToolButton UwpControl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarCustomToolButton"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton"/>
        /// </summary>
        public InkToolbarCustomToolButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarCustomToolButton).FullName)
        {
        }

        protected InkToolbarCustomToolButton(string name)
            : base(name)
        {
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkToolbarCustomToolButton;
        }

        protected override void SetContent()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown { get => UwpControl.IsExtensionGlyphShown; set => UwpControl.IsExtensionGlyphShown = value; }

        /// <summary>
        /// Gets the underlying Uwp control's Toolkind property <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.ToolKind"/>
        /// </summary>
        public InkToolbarTool ToolKind { get => (InkToolbarTool)UwpControl.ToolKind; }

        /// <summary>
        /// Gets or sets the underlying Uwp control's ConfigurationContent property <see cref="Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContent"/>
        /// </summary>
        public object ConfigurationContent { get => UwpControl.ConfigurationContent; set => UwpControl.ConfigurationContent = value as Windows.UI.Xaml.UIElement; }
    }
}