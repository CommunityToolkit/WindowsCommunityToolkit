using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.Compatible
{
    internal sealed class WebViewCompatibilityAdapter : WebBaseCompatibilityAdapter, IWebViewCompatibleAdapter
    {
        private WebView _webView = new WebView();

        public override Uri Source { get => _webView.Source; set => _webView.Source = value; }

        public override FrameworkElement View => _webView;

        public override bool CanGoBack => _webView.CanGoBack;

        public override bool CanGoForward => _webView.CanGoForward;

        public override event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting
        {
            add
            {
                _webView.NavigationStarting += value;
                _webView.FrameNavigationStarting += value;
            }

            remove
            {
                _webView.NavigationStarting -= value;
                _webView.FrameNavigationStarting -= value;
            }
        }

        public override event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading
        {
            add
            {
                _webView.ContentLoading += value;
                _webView.FrameContentLoading += value;
            }

            remove
            {
                _webView.ContentLoading -= value;
                _webView.FrameContentLoading -= value;
            }
        }

        public override event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted
        {
            add
            {
                _webView.NavigationCompleted += value;
                _webView.FrameNavigationCompleted += value;
            }

            remove
            {
                _webView.NavigationCompleted -= value;
                _webView.FrameNavigationCompleted -= value;
            }
        }

        public override bool GoBack() => _webView.GoBack();

        public override bool GoForward() => _webView.GoForward();

        public override string InvokeScript(string scriptName) => _webView.InvokeScript(scriptName);

        public override void Navigate(Uri url) => _webView.Navigate(url);

        public override void Navigate(string url) => _webView.Navigate(url);

        public override void Refresh() => _webView.Refresh();

        public override void Stop() => _webView.Stop();

        protected override void Initialize()
        {
            Bind(nameof(Source), SourceProperty, _webView);
        }
    }
}
