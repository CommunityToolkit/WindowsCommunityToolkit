// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    internal sealed class WebViewCompatibilityAdapter : WebBaseCompatibilityAdapter
    {
        private WebView _webView = new WebView();

        public override Uri Source
        {
            get => _webView?.Source;
            set
            {
                if (_webView != null)
                {
                    _webView.Source = value;
                }
            }
        }

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

        public override bool GoBack() => _webView?.GoBack() ?? false;

        public override bool GoForward() => _webView?.GoForward() ?? false;

        public override string InvokeScript(string scriptName) => _webView?.InvokeScript(scriptName);

        public override void Navigate(Uri url) => _webView?.Navigate(url);

        public override void Navigate(string url) => _webView?.Navigate(url);

        public override void Refresh() => _webView?.Refresh();

        public override void Stop() => _webView?.Stop();

        public override void Initialize()
        {
            Bind(nameof(Source), SourceProperty, _webView);
        }

        protected internal override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _webView?.Dispose();
                _webView = null;
            }
        }
    }
}
