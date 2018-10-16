// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    internal sealed class WebBrowserCompatibilityAdapter : WebBaseCompatibilityAdapter
    {
        private WebBrowser _browser = new WebBrowser();

        private void OnBrowserNavigated(object sender, WebBrowserNavigatedEventArgs e)
        {
            NavigationCompleted?.Invoke(sender, e);
        }

        private void OnBrowserNavigating(object sender, WebBrowserNavigatingEventArgs e)
        {
            NavigationStarting?.Invoke(sender, e);
            ContentLoading?.Invoke(sender, e);
        }

        public override Uri Source { get => _browser.Url; set => _browser.Url = value; }

        public override bool CanGoBack => _browser.CanGoBack;

        public override bool CanGoForward => _browser.CanGoForward;

        public override Control View => _browser;

        public override event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting;

        public override event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        public override event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted;

        public override bool GoBack()
        {
            return _browser.GoBack();
        }

        public override bool GoForward()
        {
            return _browser.GoForward();
        }

        public override string InvokeScript(string scriptName)
        {
            throw new NotImplementedException();
        }

        public override void Navigate(Uri url)
        {
            _browser.Navigate(url);
        }

        public override void Navigate(string url)
        {
            _browser.Navigate(url);
        }

        public override void Refresh()
        {
            _browser.Refresh();
        }

        public override void Stop()
        {
        }

        protected override void Initialize()
        {
            _browser.Navigating += OnBrowserNavigating;
            _browser.Navigated += OnBrowserNavigated;
        }
    }
}
