// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>An enumeration that describes the accelerator key routing stage.</summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.Interop.WebViewControlAcceleratorKeyRoutingStage"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlAcceleratorKeyRoutingStage"/>
    public enum WebViewControlAcceleratorKeyRoutingStage
    {
#pragma warning disable 1591
        Tunneling,
        Bubbling,
#pragma warning restore 1591
    }
}