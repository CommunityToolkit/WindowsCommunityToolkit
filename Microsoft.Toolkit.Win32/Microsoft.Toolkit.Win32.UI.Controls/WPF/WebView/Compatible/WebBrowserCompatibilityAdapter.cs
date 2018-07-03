using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class WebBrowserCompatibilityAdapter : IWebViewCompatible
    {
        public Uri Source { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

        public bool CanGoBack => throw new NotImplementedException();

        public bool CanGoForward => throw new NotImplementedException();

        public event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting;

        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        public event EventHandler<WebViewControlDOMContentLoadedEventArgs> DOMContentLoaded;

        public event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted;

        public bool GoBack()
        {
            throw new NotImplementedException();
        }

        public bool GoForward()
        {
            throw new NotImplementedException();
        }

        public string InvokeScript(string scriptName)
        {
            throw new NotImplementedException();
        }

        public void Navigate(Uri url)
        {
            throw new NotImplementedException();
        }

        public void Navigate(string url)
        {
            throw new NotImplementedException();
        }

        public void Refresh()
        {
            throw new NotImplementedException();
        }

        public void Stop()
        {
            throw new NotImplementedException();
        }
    }
}
