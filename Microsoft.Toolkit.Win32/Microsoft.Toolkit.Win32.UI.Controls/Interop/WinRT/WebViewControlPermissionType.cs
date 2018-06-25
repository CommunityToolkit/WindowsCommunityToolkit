// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// Defines constants that specify the state of a <see cref="IWebView.PermissionRequested"/> event.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlPermissionType"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.WebViewControlPermissionType"/>
    public enum WebViewControlPermissionType
    {
        /// <summary>
        /// Permission is for geolocation.
        /// </summary>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Geolocation", Justification = "Value from WinRT type")]
        Geolocation,

        /// <summary>
        /// Permission is for unlimited IndexedDB data storage.
        /// </summary>
        UnlimitedIndexedDBQuota,

        /// <summary>
        /// Permission is for media.
        /// </summary>
        Media,

        /// <summary>
        /// Permission is for pointer lock.
        /// </summary>
        PointerLock,

        /// <summary>
        /// Permission is for web notifications.
        /// </summary>
        WebNotifications,

        /// <summary>
        /// Permission is for screen.
        /// </summary>
        Screen,
    }
}