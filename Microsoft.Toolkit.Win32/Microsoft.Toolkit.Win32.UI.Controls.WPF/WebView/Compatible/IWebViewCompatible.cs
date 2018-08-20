// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public interface IWebViewCompatible
    {
        event EventHandler<WebViewControlContentLoadingEventArgs> ContentLoading;

        event EventHandler<WebViewControlNavigationCompletedEventArgs> NavigationCompleted;

        event EventHandler<WebViewControlNavigationStartingEventArgs> NavigationStarting;

        bool CanGoBack { get; }

        bool CanGoForward { get; }

        Uri Source { get; set; }

        bool GoBack();

        bool GoForward();

        string InvokeScript(string scriptName);

        void Navigate(Uri url);

        void Navigate(string url);

        void Refresh();

        void Stop();
    }
}
