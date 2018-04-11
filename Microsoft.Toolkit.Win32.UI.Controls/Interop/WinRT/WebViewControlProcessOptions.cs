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
    public class WebViewControlProcessOptions
    {
        public string EnterpriseId { get; set; }
        public WebViewControlProcessCapabilityState PrivateNetworkClientServerCapability { get; set; }

        public WebViewControlProcessOptions()
        {
            EnterpriseId = string.Empty;
            PrivateNetworkClientServerCapability = WebViewControlProcessCapabilityState.Default;
        }

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