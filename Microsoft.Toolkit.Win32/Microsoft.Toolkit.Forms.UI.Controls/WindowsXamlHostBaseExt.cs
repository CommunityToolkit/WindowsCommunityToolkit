// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    public abstract class WindowsXamlHostBaseExt : WindowsXamlHostBase
    {
        private readonly string initialTypeName;

        protected FrameworkElement XamlElement { get; private set; }

        protected WindowsXamlHostBaseExt(string typeName)
        {
            initialTypeName = typeName;
            InitializeElement();
        }

        internal virtual void InitializeElement()
        {
            XamlElement = UWPTypeFactory.CreateXamlContentByType(initialTypeName);
            xamlSource.Content = XamlElement;
            XamlElement.SetWrapper(this);
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            XamlElement.ClearWrapper();
        }
    }
}