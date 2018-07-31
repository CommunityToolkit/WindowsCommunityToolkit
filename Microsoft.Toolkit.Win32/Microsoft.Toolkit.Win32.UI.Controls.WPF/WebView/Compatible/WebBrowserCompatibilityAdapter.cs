// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Navigation;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    internal sealed class WebBrowserCompatibilityAdapter : WebBaseCompatibilityAdapter
    {
        private WebBrowser _browser = new WebBrowser();

        private void OnBrowserNavigated(object sender, NavigationEventArgs e)
        {
            NavigationCompleted?.Invoke(sender, e);
        }

        private void OnBrowserNavigating(object sender, NavigatingCancelEventArgs e)
        {
            NavigationStarting?.Invoke(sender, e);
            ContentLoading?.Invoke(sender, e);
        }

        public override Uri Source
        {
            get => _browser?.Source;
            set
            {
                if (_browser != null)
                {
                    _browser.Source = value;
                }
            }
        }

        public override bool CanGoBack => _browser?.CanGoBack ?? false;

        public override bool CanGoForward => _browser?.CanGoForward ?? false;

        public override FrameworkElement View => _browser;

        public override event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting;

        public override event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        public override event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted;

        public override bool GoBack()
        {
            _browser?.GoBack();
            return true;
        }

        public override bool GoForward()
        {
            _browser?.GoForward();
            return true;
        }

        public override string InvokeScript(string scriptName)
        {
            return _browser?.InvokeScript(scriptName)?.ToString();
        }

        public override void Navigate(Uri url)
        {
            _browser?.Navigate(url);
        }

        public override void Navigate(string url)
        {
            _browser?.Navigate(url);
        }

        public override void Refresh()
        {
            _browser?.Refresh();
        }

        public override void Stop()
        {
            // REVIEW: Not supported? Would need to track navigation state internally and invoke cancel on navigating
        }

        public override void Initialize()
        {
            _browser.Navigating += OnBrowserNavigating;
            _browser.LoadCompleted += OnBrowserNavigated;
            Bind(nameof(Source), SourceProperty, _browser);
        }

        protected internal override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _browser?.Dispose();
                _browser = null;
            }
        }
    }
}
