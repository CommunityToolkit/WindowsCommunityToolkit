// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Win32.UI.Interop;
using Microsoft.Toolkit.Win32.UI.Interop.WinForms;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public abstract class WindowsXamlHostBaseExt : WindowsXamlHostBase
    {
        protected string InitialTypeName { get; set; }

        internal FrameworkElement XamlElement { get; set; }

        protected WindowsXamlHostBaseExt(string typeName)
        {
            InitialTypeName = typeName;
            InitializeElement();
        }

        internal virtual void InitializeElement()
        {
            XamlElement = UWPTypeFactory.CreateXamlContentByType(InitialTypeName);
            desktopWindowXamlSource.Content = XamlElement;
            XamlElement.SetWrapper(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            XamlElement.ClearWrapper();
        }
    }
}