// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.Toolkit.Win32.UI.Interop;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    [Designer(typeof(InkToolbarDesigner))]
    public class InkToolbar : WindowsXamlHostBaseExt
    {
        protected Windows.UI.Xaml.Controls.InkToolbar UwpControl { get; set; }

        public InkToolbar()
            : this(typeof(global::Windows.UI.Xaml.Controls.InkToolbar).FullName)
        {
        }

        protected InkToolbar(string name)
            : base(name)
        {
            InitializeElement();
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkToolbar;
        }

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
    }
}
