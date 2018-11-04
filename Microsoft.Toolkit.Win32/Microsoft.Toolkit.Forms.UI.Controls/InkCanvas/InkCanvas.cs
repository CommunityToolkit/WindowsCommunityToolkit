// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.ComponentModel;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    public class InkCanvas : WindowsXamlHostBase
    {
        internal Windows.UI.Xaml.Controls.InkCanvas UwpControl => GetUwpInternalObject() as Windows.UI.Xaml.Controls.InkCanvas;

        public InkCanvas()
            : this(typeof(Windows.UI.Xaml.Controls.InkCanvas).FullName)
        {
        }

        protected InkCanvas(string name)
            : base(name)
        {
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkCanvas.InkPresenter"/>
        /// </summary>
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public InkPresenter InkPresenter => UwpControl.InkPresenter;
    }
}
