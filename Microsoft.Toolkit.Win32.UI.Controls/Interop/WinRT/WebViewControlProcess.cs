using System;
using System.Security;
using System.Threading.Tasks;

using Windows.Foundation;
using Windows.Web.UI.Interop;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public class WebViewControlProcess
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.Interop.WebViewControlProcess _process;

        public WebViewControlProcess()
            : this(new WebViewControlProcessOptions())
        {
        }

        public WebViewControlProcess(WebViewControlProcessOptions processOptions)
        {
            if (processOptions is null) throw new ArgumentNullException(nameof(processOptions));

            _process = new Windows.Web.UI.Interop.WebViewControlProcess(processOptions.ToWinRtWebViewControlProcessOptions());
            SubscribeEvents();
        }

        internal WebViewControlProcess(Windows.Web.UI.Interop.WebViewControlProcess process)
        {
            _process = process ?? throw new ArgumentNullException(nameof(process));
            SubscribeEvents();
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1009:DeclareEventHandlersCorrectly")]
        public event EventHandler<object> ProcessExited = (sender, args) => { };

        public string EnterpriseId => _process.EnterpriseId;

        public bool IsPrivateNetworkClientServerCapabilityEnabled => _process.IsPrivateNetworkClientServerCapabilityEnabled;

        public uint ProcessId => _process.ProcessId;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlProcess(Windows.Web.UI.Interop.WebViewControlProcess process) => new WebViewControlProcess(process);

        public void Terminate() => _process.Terminate();

        internal IAsyncOperation<WebViewControl> CreateWebViewControlAsync(
            IntPtr hostWindowHandle,
            Rect bounds)
        {
            var hwh = hostWindowHandle.ToInt64();
            return CreateWebViewControlAsync(hwh, bounds);
        }

        [SecurityCritical]
        internal IAsyncOperation<WebViewControl> CreateWebViewControlAsync(
            long hostWindowHandle,
            Rect bounds
        )
        {
            Security.DemandUnamangedCode();
            if (hostWindowHandle == 0) throw new ArgumentNullException(nameof(hostWindowHandle));
            return _process.CreateWebViewControlAsync(hostWindowHandle, bounds);
        }

        internal async Task<WebViewControlHost> CreateWebViewControlHostAsync(IntPtr hostWindowHandle, Rect bounds)
        {
            if (hostWindowHandle == IntPtr.Zero) throw new ArgumentNullException(nameof(hostWindowHandle));

            var wvc = await await Task.Run(() => CreateWebViewControlAsync(hostWindowHandle, bounds)).ConfigureAwait(false);

            return new WebViewControlHost(wvc);
        }

        private void OnWebViewControlProcessExited(Windows.Web.UI.Interop.WebViewControlProcess sender, object args)
        {
            var handler = ProcessExited;
            if (handler != null)
            {
                handler(this, args);
            }
        }

        private void SubscribeEvents()
        {
            _process.ProcessExited += OnWebViewControlProcessExited;
        }

        private void UnsubscribeEvents()
        {
            _process.ProcessExited -= OnWebViewControlProcessExited;
        }
    }
}