// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs"/>
    /// <summary>This class provides information for the <see cref="IWebView.AcceleratorKeyPressed"/> event.</summary>
    /// <seealso cref="System.EventArgs" />
    public class WebViewControlAcceleratorKeyPressedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs _args;

        [SecurityCritical]
        internal WebViewControlAcceleratorKeyPressedEventArgs(Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            _args = args;
        }

        /// <summary>
        /// Gets the type of the event.
        /// </summary>
        /// <value>The type of the event.</value>
        public CoreAcceleratorKeyEventType EventType
        {
            [SecurityCritical]
            get { return (CoreAcceleratorKeyEventType)_args.EventType; }
        }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="WebViewControlAcceleratorKeyPressedEventArgs"/> is handled.
        /// </summary>
        /// <value><see langword="true" /> if handled; otherwise, <see langword="false" />.</value>
        public bool Handled
        {
            [SecurityCritical]
            get => _args.Handled;
            [SecurityCritical]
            set => _args.Handled = value;
        }

        /// <summary>
        /// Gets the routing stage.
        /// </summary>
        /// <value>The routing stage.</value>
        public WebViewControlAcceleratorKeyRoutingStage RoutingStage
        {
            [SecurityCritical]
            get { return (WebViewControlAcceleratorKeyRoutingStage)_args.RoutingStage; }
        }

        /// <summary>
        /// Gets the virtual key.
        /// </summary>
        /// <value>The virtual key.</value>
        public VirtualKey VirtualKey
        {
            [SecurityCritical]
            get { return (VirtualKey)_args.VirtualKey; }
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs"/> to <see cref="WebViewControlAcceleratorKeyPressedEventArgs"/>.
        /// </summary>
        /// <param name="args">The <see cref="Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs"/> instance containing the event data.</param>
        /// <returns>The result of the conversion.</returns>
        [SecurityCritical]
        public static implicit operator WebViewControlAcceleratorKeyPressedEventArgs(
            Windows.Web.UI.Interop.WebViewControlAcceleratorKeyPressedEventArgs args)
        {
            return new WebViewControlAcceleratorKeyPressedEventArgs(args);
        }
    }
}