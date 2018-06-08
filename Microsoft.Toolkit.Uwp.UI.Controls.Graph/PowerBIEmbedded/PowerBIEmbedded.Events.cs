// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.Graphics.Display;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Defines the events for the <see cref="PowerBIEmbedded"/> control.
    /// </summary>
    public partial class PowerBIEmbedded : Control
    {
        private static void OnPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e) => (d as PowerBIEmbedded).LoadAllAsync();

        private void WebViewReportFrame_DOMContentLoaded(WebView sender, WebViewDOMContentLoadedEventArgs args)
        {
            _webViewInitializedTask.TrySetResult(true);
        }

        private void DisplayInformation_OrientationChanged(DisplayInformation sender, object args)
        {
            if (IsWindowsPhone)
            {
                InvokeScriptAsync($"rotate('{sender.CurrentOrientation.ToString()}')");
            }
        }

        private async void TokenExpirationRefreshTimer_Tick(object sender, object e)
        {
            if (_tokenForUser != null && _expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                string token = await GetUserTokenAsync();
                if (!string.IsNullOrEmpty(token))
                {
                    InvokeScriptAsync($"refreshToken('{token}')");
                }
            }
        }
    }
}
