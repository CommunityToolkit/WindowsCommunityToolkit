// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    /// <summary>
    /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton"/>
    /// </summary>
    [Designer(typeof(InkToolbarToolButtonDesigner))]
    public class InkToolbarToolButton : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkToolbarToolButton UwpControl { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="InkToolbarToolButton"/> class, a
        /// WinForms-enabled wrapper for <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton"/>
        /// </summary>
        public InkToolbarToolButton()
            : this(typeof(Windows.UI.Xaml.Controls.InkToolbarToolButton).FullName)
        {
        }

        protected InkToolbarToolButton(string name)
            : base(name)
        {
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            UwpControl = GetUwpInternalObject() as Windows.UI.Xaml.Controls.InkToolbarToolButton;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the underlying Uwp control's IsExtensionGlyphShown property is set <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.IsExtensionGlyphShown"/>
        /// </summary>
        public bool IsExtensionGlyphShown { get => UwpControl.IsExtensionGlyphShown; set => UwpControl.IsExtensionGlyphShown = value; }

        /// <summary>
        /// Gets the underlying Uwp control's Toolkind property <see cref="Windows.UI.Xaml.Controls.InkToolbarToolButton.ToolKind"/>
        /// </summary>
        public InkToolbarTool ToolKind { get => (InkToolbarTool)UwpControl.ToolKind; }
    }
}
