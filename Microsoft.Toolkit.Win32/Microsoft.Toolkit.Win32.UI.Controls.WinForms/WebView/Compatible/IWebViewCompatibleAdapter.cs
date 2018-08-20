// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows.Forms;

namespace Microsoft.Toolkit.Win32.UI.Controls.WinForms
{
    public interface IWebViewCompatibleAdapter : IWebViewCompatible
    {
        Control View { get; }
    }
}
