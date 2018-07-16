// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Windows;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF.Compatible
{
    public interface IWebViewCompatible
    {
        Uri Source { get; set; }

        void Navigate(Uri url);

        void Navigate(string url);

        event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting;

        event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted;

        bool CanGoBack { get; }

        bool CanGoForward { get; }

        bool GoBack();

        bool GoForward();

        void Refresh();

        void Stop();

        string InvokeScript(string scriptName);
    }
}