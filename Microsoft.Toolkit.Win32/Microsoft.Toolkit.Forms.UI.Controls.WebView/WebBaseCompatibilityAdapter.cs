// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using System.Windows.Data;
using System.Windows.Forms;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    internal abstract class WebBaseCompatibilityAdapter : DependencyObject, IWebViewCompatibleAdapter
    {
        protected WebBaseCompatibilityAdapter()
        {
            Initialize();
        }

        public static DependencyProperty SourceProperty { get; } = DependencyProperty.Register(nameof(Source), typeof(Uri), typeof(WebBaseCompatibilityAdapter));

        public abstract Control View { get; }

        public abstract Uri Source { get; set; }

        public abstract bool CanGoBack { get; }

        public abstract bool CanGoForward { get; }

        public abstract event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting;

        public abstract event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        public abstract event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted;

        public abstract bool GoBack();

        public abstract bool GoForward();

        public abstract string InvokeScript(string scriptName);

        public abstract void Navigate(Uri url);

        public abstract void Navigate(string url);

        public abstract void Refresh();

        public abstract void Stop();

        protected abstract void Initialize();
    }
}
