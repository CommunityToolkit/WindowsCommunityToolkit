// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public class InkCanvas : WindowsXamlHostBaseExt
    {
        internal Windows.UI.Xaml.Controls.InkCanvas UwpControl { get; set; }

        public InkCanvas()
            : this(typeof(Windows.UI.Xaml.Controls.InkCanvas).FullName)
        {
        }

        protected InkCanvas(string name)
            : base(name)
        {
            UwpControl = XamlElement as Windows.UI.Xaml.Controls.InkCanvas;
        }

        /// <summary>
        /// Gets <see cref="Windows.UI.Xaml.Controls.InkCanvas.InkPresenter"/>
        /// </summary>
        public InkPresenter InkPresenter => UwpControl.InkPresenter;
    }
}
