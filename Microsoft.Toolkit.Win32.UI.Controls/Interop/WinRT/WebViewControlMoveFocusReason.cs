// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// An enum that describes the reason for moving the focus.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.Interop.WebViewControlMoveFocusReason"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlMoveFocusReason"/>
    public enum WebViewControlMoveFocusReason
    {
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member
        Programmatic,
        Next,
        Previous,
#pragma warning restore CS1591 // Missing XML comment for publicly visible type or member
    }
}