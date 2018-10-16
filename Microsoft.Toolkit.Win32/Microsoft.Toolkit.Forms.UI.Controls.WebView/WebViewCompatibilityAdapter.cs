// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    internal sealed class WebViewCompatibilityAdapter : WebBaseCompatibilityAdapter, IWebViewCompatibleAdapter, IDisposable
    {
        private WebView _webView = new WebView();

        ~WebViewCompatibilityAdapter()
        {
            Dispose(false);
        }

        public override event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading
        {
            add
            {
                _webView.ContentLoading += value;
            }

            remove
            {
                _webView.ContentLoading -= value;
            }
        }

        public event EventHandler<WebViewControlContentLoadingEventArgs> FrameContentLoading
        {
            add
            {
                _webView.FrameContentLoading += value;
            }

            remove
            {
                _webView.FrameContentLoading -= value;
            }
        }

        public override event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted
        {
            add
            {
                _webView.NavigationCompleted += value;
            }

            remove
            {
                _webView.NavigationCompleted -= value;
            }
        }

        public override event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting
        {
            add
            {
                _webView.NavigationStarting += value;
            }

            remove
            {
                _webView.NavigationStarting -= value;
            }
        }

        public event EventHandler<WebViewControlNavigationStartingEventArgs> FrameNavigationStarting
        {
            add
            {
                _webView.FrameNavigationStarting += value;
            }

            remove
            {
                _webView.FrameNavigationStarting -= value;
            }
        }

        public event EventHandler<WebViewControlNavigationCompletedEventArgs> FrameNavigationCompleted
        {
            add
            {
                _webView.FrameNavigationCompleted += value;
            }

            remove
            {
                _webView.FrameNavigationCompleted -= value;
            }
        }

        public override bool CanGoBack => _webView.CanGoBack;

        public override bool CanGoForward => _webView.CanGoForward;

        public override Uri Source { get => _webView.Source; set => _webView.Source = value; }

        public override Control View => _webView;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
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
            var initWebView = (ISupportInitialize)_webView;
            initWebView.BeginInit();
            _webView.Dock = DockStyle.Fill;
            initWebView.EndInit();
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _webView?.Dispose();
                _webView = null;
            }
        }
    }
}
