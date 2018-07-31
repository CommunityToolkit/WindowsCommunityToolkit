// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Represents the state of the <see cref="WebViewControlProcess"/>.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.Interop.WebViewControlProcessCapabilityState"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessCapabilityState"/>
    public enum WebViewControlProcessCapabilityState
    {
        /// <summary>
        /// The process is in an unknown state.
        /// </summary>
        Default,

        /// <summary>
        /// The process is disabled.
        /// </summary>
        Disabled,

        /// <summary>
        /// The process is enabled.
        /// </summary>
        Enabled,
    }
}