// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;
using Microsoft.Toolkit.Win32.UI.Controls.Test.WebView.Shared;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Should;

namespace Microsoft.Toolkit.Win32.UI.Controls.Test.WinForms.WebView.FunctionalTests.Cookies
{
    [TestCategory(TestConstants.Categories.Nav)]
    public abstract class CookieTestContext : HostFormWebViewContextSpecification
    {
        public Uri CookieUri { get; } = new Uri(TestConstants.Uris.HttpBin, "/cookies");

        protected abstract Task SetCookieAsync(Controls.WinForms.WebView webView);

        protected async Task SetCookieAsync(
            Controls.WinForms.WebView webView,
            string cookieName,
            string cookieValue,
            DateTime? expiry = null)
        {
            string formatExpiry(DateTime? e) => e != null
                ? $"; expires={e.Value.ToUniversalTime():R}"
                : string.Empty;

            var cookie = $"{cookieName}={cookieValue}{formatExpiry(expiry)}";
            if (webView != null)
            {
                await webView.InvokeScriptAsync("eval", $"document.cookie = \"{cookie}\"").ConfigureAwait(false);
            }
        }

        protected Task<string> GetCookiesAsync(Controls.WinForms.WebView webView)
        {
            return webView.InvokeScriptAsync("eval", "document.cookie");
        }

        protected override void When()
        {
            NavigateAndWaitForFormClose(CookieUri);
        }
    }

    public abstract class MultipleWebViewCookieTestContext : CookieTestContext
    {
        public Controls.WinForms.WebView SecondWebView { get; private set; }

        public string Webview1Cookie
        {
            get => _webview1Cookie;
            private set
            {
                WriteLine("WebView1 Cookie: {0}", value);
                _webview1Cookie = value;
            }
        }

        public string Webview2Cookie
        {
            get => _webview2Cookie;
            private set
            {
                WriteLine("WebView2 Cookie: {0}", value);
                _webview2Cookie = value;
            }
        }

        private string _webview2Cookie;
        private string _webview1Cookie;

        protected override void Given()
        {
            base.Given();

            WebView.NavigationCompleted += OnWebViewOnNavigationCompleted;
        }

        private async void OnWebViewOnNavigationCompleted(object sender, WebViewControlNavigationCompletedEventArgs e)
        {
            if (!e.IsSuccess)
            {
                Form.Close();
            }

            if (sender is Controls.WinForms.WebView wv)
            {
                if (SecondWebView == null)
                {
                    await SetCookieAsync(wv);
                    Webview1Cookie = await GetCookiesAsync(wv);


                    // Once the session cookie is set
                    SecondWebView = CreateSecondWebView(wv.Process);
                    SecondWebView.NavigationCompleted += OnWebViewOnNavigationCompleted;
                    SecondWebView.Navigate(CookieUri);
                }
                else
                {
                    Webview2Cookie = await GetCookiesAsync(wv);
                    Form.Close();
                }
            }
        }

        protected override void Cleanup()
        {
            SecondWebView?.Dispose();

            base.Cleanup();
        }

        protected abstract Controls.WinForms.WebView CreateSecondWebView(WebViewControlProcess process);
    }

    public abstract class WebViewDifferentProcessContext : MultipleWebViewCookieTestContext
    {
        protected override Controls.WinForms.WebView CreateSecondWebView(WebViewControlProcess process)
        {
            var webView = new Controls.WinForms.WebView();
            ((ISupportInitialize)webView).BeginInit();
            webView.IsScriptNotifyAllowed = WebView.IsScriptNotifyAllowed;
            webView.IsIndexedDBEnabled = WebView.IsIndexedDBEnabled;
            webView.IsJavaScriptEnabled = WebView.IsJavaScriptEnabled;
            ((ISupportInitialize)webView).EndInit();

            return webView;
        }
    }

    public abstract class WebViewSameProcessContext : MultipleWebViewCookieTestContext
    {
        protected override Controls.WinForms.WebView CreateSecondWebView(WebViewControlProcess process)
        {
            var webView = new Controls.WinForms.WebView(process);
            ((ISupportInitialize) webView).BeginInit();
            webView.IsScriptNotifyAllowed = WebView.IsScriptNotifyAllowed;
            webView.IsIndexedDBEnabled = WebView.IsIndexedDBEnabled;
            webView.IsJavaScriptEnabled = WebView.IsJavaScriptEnabled;
            ((ISupportInitialize) webView).EndInit();

            return webView;
        }


    }

    [TestClass]
    public class PersistentCookieSameProcessTests : WebViewSameProcessContext
    {
        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void PersistentCookieIsInNewWebView()
        {
            Webview1Cookie.ShouldNotBeNull();
            Webview2Cookie.ShouldNotBeNull();

            Webview2Cookie.ShouldEqual(Webview1Cookie);
        }

        protected override Task SetCookieAsync(Controls.WinForms.WebView webView)
        {
            return SetCookieAsync(webView, "persistent", "true", DateTime.UtcNow.Add(TimeSpan.FromSeconds(TestConstants.Timeouts.Longest)));
        }

        protected override void Cleanup()
        {
            // Best effort clean up. Initial cookie has a short TTL anyway
#pragma warning disable 4014
            SetCookieAsync(SecondWebView, "persistent", string.Empty, new DateTime(1970, 1, 1));
#pragma warning restore 4014

            base.Cleanup();
        }
    }

    [TestClass]
    public class PersistentCookieDifferentProcessTests : WebViewDifferentProcessContext
    {
        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void PersistentCookieIsInNewWebView()
        {
            Webview1Cookie.ShouldNotBeNull();
            Webview2Cookie.ShouldNotBeNull();

            Webview2Cookie.ShouldEqual(Webview1Cookie);
        }

        protected override Task SetCookieAsync(Controls.WinForms.WebView webView)
        {
            return SetCookieAsync(webView, "persistent", "true", DateTime.UtcNow.Add(TimeSpan.FromSeconds(TestConstants.Timeouts.Longest)));
        }

        protected override void Cleanup()
        {
            // Best effort clean up. Initial cookie has a short TTL anyway
#pragma warning disable 4014
            SetCookieAsync(SecondWebView, "persistent", string.Empty, new DateTime(1970, 1, 1));
#pragma warning restore 4014

            base.Cleanup();
        }
    }

    [TestClass]
    public class SessionCookieSameProcessTests : WebViewSameProcessContext
    {
        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void SessionCookieIsInNewWebView()
        {
            Webview1Cookie.ShouldNotBeNull();
            Webview2Cookie.ShouldNotBeNull();

            Webview2Cookie.ShouldEqual(Webview1Cookie);
        }

        protected override Task SetCookieAsync(Controls.WinForms.WebView webView)
        {
            return SetCookieAsync(webView, "session", "true");
        }
    }

    [TestClass]
    public class SessionCookieDifferentProcessTests : WebViewDifferentProcessContext
    {
        [TestMethod]
        [Timeout(TestConstants.Timeouts.Longest)]
        public void SessionCookieIsNotInNewWebView()
        {
            Webview1Cookie.ShouldNotBeNull();
            Webview2Cookie.ShouldNotBeNull();

            Webview2Cookie.ShouldNotEqual(Webview1Cookie);
        }

        protected override Task SetCookieAsync(Controls.WinForms.WebView webView)
        {
            return SetCookieAsync(webView, "session", "true");
        }
    }
}
