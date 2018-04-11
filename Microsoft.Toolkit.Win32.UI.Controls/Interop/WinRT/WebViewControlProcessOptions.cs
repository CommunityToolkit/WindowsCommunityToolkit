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