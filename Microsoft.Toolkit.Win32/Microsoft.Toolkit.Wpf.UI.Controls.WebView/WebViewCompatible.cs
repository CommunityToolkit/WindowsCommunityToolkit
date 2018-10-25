// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using Microsoft.Toolkit.UI.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Wpf.UI.Controls
{
    public class WebViewCompatible : UserControl, IWebViewCompatible, IDisposable
    {
        public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(nameof(Source), typeof(Uri), typeof(WebViewCompatible));

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

            _implementation.Initialize();

            AddChild(_implementation.View);
            _implementation.View.BeginInit();
            _implementation.View.EndInit();
            var binder = new Binding()
            {
                Source = _implementation,
                Path = new PropertyPath(nameof(Source)),
                Mode = BindingMode.TwoWay
            };
            BindingOperations.SetBinding(this, SourceProperty, binder);
        }

        ~WebViewCompatible()
        {
            Dispose(false);
        }

        public static bool IsLegacy { get; } = !WebViewControlHost.IsSupported;

        private IWebViewCompatibleAdapter _implementation;

        public Uri Source { get => (Uri)GetValue(SourceProperty); set => SetValue(SourceProperty, value); }

        public bool CanGoBack => _implementation.CanGoBack;

        public bool CanGoForward => _implementation.CanGoForward;

        public FrameworkElement View { get => _implementation.View; }

        public event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting { add => _implementation.NavigationStarting += value; remove => _implementation.NavigationStarting -= value; }

        public event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading { add => _implementation.ContentLoading += value; remove => _implementation.ContentLoading -= value; }

        public event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted { add => _implementation.NavigationCompleted += value; remove => _implementation.NavigationCompleted -= value; }

        public bool GoBack() => _implementation.GoBack();

        public bool GoForward() => _implementation.GoForward();

        public void Navigate(Uri url) => _implementation.Navigate(url);

        public void Navigate(string url) => _implementation.Navigate(url);

        public void NavigateToString(string text) => _implementation.NavigateToString(text);

        public void Refresh() => _implementation.Refresh();

        public void Stop() => _implementation.Stop();

        public string InvokeScript(string scriptName) => _implementation.InvokeScript(scriptName);

        public void Dispose()
        {
            Dispose(true);

            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool isDisposing)
        {
            if (isDisposing)
            {
                if (_implementation is IDisposable disposable)
                {
                    disposable.Dispose();
                }
            }
        }
    }
}
