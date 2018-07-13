// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Foundation.Metadata;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// This class contains options that can be set when creating a <see cref="IWebView"/> instance.
    /// </summary>
    /// <remarks>
    /// Copy from <see cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> to avoid requirement to link Windows.winmd.
    /// </remarks>
    /// <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/>
    public sealed class WebViewControlProcessOptions
    {
        /// <summary>
        /// Gets or sets the enterprise identifier for apps that are WIP-enabled.
        /// </summary>
        /// <value>The enterprise identifier.</value>
        public string EnterpriseId { get; set; }

        /// <summary>
        /// Gets or sets the partition for the web view.
        /// </summary>
        /// <value>The partition.</value>
        public string Partition { get; set; }

        /// <summary>
        /// Gets or sets the private network client server capability.
        /// </summary>
        /// <value>The private network client server capability.</value>
        public WebViewControlProcessCapabilityState PrivateNetworkClientServerCapability { get; set; }

        /// <summary>
        /// Gets or sets the user agent.
        /// </summary>
        /// <value>The user agent.</value>
        public string UserAgent { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WebViewControlProcessOptions"/> class.
        /// </summary>
        public WebViewControlProcessOptions()
        {
            EnterpriseId = string.Empty;
            Partition = string.Empty;
            UserAgent = string.Empty;
            PrivateNetworkClientServerCapability = WebViewControlProcessCapabilityState.Default;
        }

        /// <summary>
        /// Performs an implicit conversion from <see cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> to <see cref="WebViewControlProcessOptions"/>.
        /// </summary>
        /// <param name="options">The options.</param>
        /// <returns>The result of the conversion.</returns>
        public static implicit operator WebViewControlProcessOptions(Windows.Web.UI.Interop.WebViewControlProcessOptions options) => ToWinRtWebViewControlProcessOptions(options);

        public static Windows.Web.UI.Interop.WebViewControlProcessOptions ToWinRtWebViewControlProcessOptions(WebViewControlProcessOptions options)
        {
            var retval = new Windows.Web.UI.Interop.WebViewControlProcessOptions();

            if (!string.IsNullOrEmpty(options?.EnterpriseId) && !StringComparer.InvariantCulture.Equals(retval.EnterpriseId, options?.EnterpriseId))
            {
                retval.EnterpriseId = options.EnterpriseId;
            }

            if (ApiInformation.IsPropertyPresent(
                   "Windows.Web.UI.Interop.WebViewControlProcessOptions",
                   "Partition"))
            {
                if (!string.IsNullOrEmpty(options?.Partition))
                {
                    retval.Partition = options.Partition;
                }
            }

            retval.PrivateNetworkClientServerCapability = (Windows.Web.UI.Interop.WebViewControlProcessCapabilityState)options?.PrivateNetworkClientServerCapability;

            if (ApiInformation.IsPropertyPresent(
                "Windows.Web.UI.Interop.WebViewControlProcessOptions",
                "UserAgent"))
            {
                if (!string.IsNullOrEmpty(options?.UserAgent))
                {
                    retval.UserAgent = options.UserAgent;
                }
            }

            return retval;
        }

        /// <summary>
        /// Converts this instance to a <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> instance.
        /// </summary>
        /// <returns>A <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/> instance.</returns>
        internal Windows.Web.UI.Interop.WebViewControlProcessOptions ToWinRtWebViewControlProcessOptions()
        {
            return ToWinRtWebViewControlProcessOptions(this);
        }
    }
}