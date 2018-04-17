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

using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    /// <summary>
    /// A proxy for <seealso cref="Windows.Web.UI.Interop.WebViewControlProcessOptions"/>.
    /// </summary>
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