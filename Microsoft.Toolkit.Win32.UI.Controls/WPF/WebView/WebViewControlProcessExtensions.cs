using System;
using System.Security;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;


namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public static class WebViewControlProcessExtensions
    {
        [SecuritySafeCritical]
        internal static WebViewControlHost CreateWebViewControlHost(
            this WebViewControlProcess process,
            IntPtr hostWindowHandle,
            Size desiredSize)
        {
            Verify.IsNotNull(process);

            var rect = new Windows.Foundation.Rect(0, 0, (int)desiredSize.Width, (int)desiredSize.Height);

            return process
                    .CreateWebViewControlHostAsync(hostWindowHandle, rect)
                    .GetAwaiter()
                    .GetResult();
        }
    }
}
