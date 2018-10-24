// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Windows.Forms;
using Microsoft.Toolkit.UI.Controls;
using Microsoft.Toolkit.Win32.UI.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    [DefaultProperty(Constants.ComponentDefaultProperty)]
    [DefaultEvent(Constants.ComponentDefaultEvent)]
    [Docking(DockingBehavior.AutoDock)]
    [Description("Embeds a view into your application that renders web content using the a rendering engine")]
    public class WebViewCompatible : Control, IWebViewCompatible
    {
        private const string WinRtType = "Windows.Web.UI.Interop.WebViewControl";
        private readonly IWebViewCompatibleAdapter _implementation;

        public WebViewCompatible()
            : base()
        {
            if (WebViewControlHost.IsSupported)
            {
                _implementation = new WebViewCompatibilityAdapter();
            }
            else
            {
                _implementation = new WebBrowserCompatibilityAdapter();
            }

            _implementation.View.Dock = DockStyle.Fill;
            Controls.Add(_implementation.View);
        }

        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading { add => _implementation.ContentLoading += value; remove => _implementation.ContentLoading -= value; }

        public event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted { add => _implementation.NavigationCompleted += value; remove => _implementation.NavigationCompleted -= value; }

        public event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting { add => _implementation.NavigationStarting += value; remove => _implementation.NavigationStarting -= value; }

        public bool CanGoBack => _implementation.CanGoBack;

        public bool CanGoForward => _implementation.CanGoForward;

        public bool IsLegacy => !WebViewControlHost.IsSupported;

        [Category("Web")]
        [Bindable(true)]
        [DefaultValue(null)]
        [TypeConverter(typeof(WebBrowserUriTypeConverter))]
        public Uri Source { get => _implementation.Source; set => _implementation.Source = value; }

        public Control View { get => _implementation.View; }

        public bool GoBack() => _implementation.GoBack();

        public bool GoForward() => _implementation.GoForward();

        public string InvokeScript(string scriptName) => _implementation.InvokeScript(scriptName);

        public void Navigate(Uri url) => _implementation.Navigate(url);

        public void Navigate(string url) => _implementation.Navigate(url);

        public override void Refresh() => _implementation.Refresh();

        public void Stop() => _implementation.Stop();

        protected override void Dispose(bool disposing)
        {
            try
            {
                if (_implementation is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
            finally
            {
                base.Dispose(disposing);
            }
        }
    }
}
