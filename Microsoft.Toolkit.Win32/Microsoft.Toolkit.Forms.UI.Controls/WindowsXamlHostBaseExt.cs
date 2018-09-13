// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Forms.UI.XamlHost;
using Microsoft.Toolkit.Win32.UI.XamlHost;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    public abstract class WindowsXamlHostBaseExt : WindowsXamlHostBase
    {
        protected string InitialTypeName { get; }

        internal FrameworkElement XamlElement { get; private set; }

        protected WindowsXamlHostBaseExt(string typeName)
        {
            InitialTypeName = typeName;
            XamlElement = UWPTypeFactory.CreateXamlContentByType(InitialTypeName);
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            SetContent();
            XamlElement.SetWrapper(this);
        }

        protected virtual void SetContent()
        {
            if (_xamlSource != null)
            {
                _xamlSource.Content = XamlElement;
            }
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);
            XamlElement.ClearWrapper();
        }
    }
}