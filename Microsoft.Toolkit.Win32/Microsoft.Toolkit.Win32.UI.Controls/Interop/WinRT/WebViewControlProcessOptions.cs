// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// This class contains options that can be set when creating a <see cref="IWebView"/> instance.
    /// </summary>
    /// <remarks>
    /// Copy from <see cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> to avoid requirement to link Windows.winmd.
    /// </remarks>
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/>
    public class WebViewControlProcessOptions
    {
        /// <summary>
        /// Gets or sets the enterprise identifier for apps that are WIP-enabled.
        /// </summary>
        /// <value>The enterprise identifier.</value>
        public string EnterpriseId { get; set; }

        /// <summary>
        /// Gets or sets the private network client server capability.
        /// </summary>
        /// <value>The private network client server capability.</value>
        public WebViewControlProcessCapabilityState PrivateNetworkClientServerCapability { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewControlProcessOptions"/> class.
        /// </summary>
        public WebViewControlProcessOptions()
        {
            EnterpriseId = string.Empty;
            PrivateNetworkClientServerCapability = WebViewControlProcessCapabilityState.Default;
        }

        /// <summary>
        /// Converts this instance to a <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> instance.
        /// </summary>
        /// <returns>A <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> instance.</returns>
        internal Windows.Web.UI.Interop.WebViewControlProcessOptions ToWinRtWebViewControlProcessOptions()
        {
            var retval = new Windows.Web.UI.Interop.WebViewControlProcessOptions();

            if (!string.IsNullOrEmpty(EnterpriseId) && !StringComparer.InvariantCulture.Equals(retval.EnterpriseId, EnterpriseId))
            {
                retval.EnterpriseId = EnterpriseId;
            }

            retval.PrivateNetworkClientServerCapability = (Windows.Web.UI.Interop.WebViewControlProcessCapabilityState)PrivateNetworkClientServerCapability;

            return retval;
        }
    }
}