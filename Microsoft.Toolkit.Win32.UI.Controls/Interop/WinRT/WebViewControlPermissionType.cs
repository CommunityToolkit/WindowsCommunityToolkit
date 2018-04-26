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
    // Type is a copy. Information regarding the origination of the type is in summary comments
#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

    /// <summary>
    /// Defines constants that specify the state of a <see cref="IWebView.PermissionRequested"/> event.
    /// </summary>
    /// <remarks>Copy from <see cref="Windows.Web.UI.WebViewControlPermissionType"/> to avoid requirement to link Windows.winmd</remarks>
    /// <seealso cref="Windows.Web.UI.WebViewControlPermissionType"/>
    public enum WebViewControlPermissionType
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Geolocation", Justification = "Value from WinRT type")]
        Geolocation,
        UnlimitedIndexedDBQuota,
        Media,
        PointerLock,
        WebNotifications,
        Screen,
    }
}