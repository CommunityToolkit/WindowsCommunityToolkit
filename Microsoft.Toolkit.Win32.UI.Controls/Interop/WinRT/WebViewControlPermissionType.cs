// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

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