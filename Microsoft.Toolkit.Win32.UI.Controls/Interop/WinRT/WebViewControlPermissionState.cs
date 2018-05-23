// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    // Type is a copy. Information regarding the origination of the type is in summary comments
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
    /// <summary>
    /// Defines constants that specify the state of a <see cref="IWebView.PermissionRequested"/> event.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlPermissionState"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.WebViewControlPermissionState"/>
    public enum WebViewControlPermissionState
    {
        Unknown,
        Defer,
        Allow,
        Deny,
    }
}