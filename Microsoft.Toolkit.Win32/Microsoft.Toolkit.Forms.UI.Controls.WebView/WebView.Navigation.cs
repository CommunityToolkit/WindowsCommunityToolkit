// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net.Http;
using Microsoft.Toolkit.Win32.UI.Controls;
using Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT;

namespace Microsoft.Toolkit.Forms.UI.Controls
{
    // Navigation Journaling

    /// <inheritdoc cref="IWebView" />
    public partial class WebView : IWebView
    {
        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGoBack => _webViewControl?.CanGoBack ?? false;

        /// <inheritdoc />
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool CanGoForward => _webViewControl?.CanGoForward ?? false;

        /// <inheritdoc />
        public bool GoBack()
        {
            var retval = true;
            try
            {
                _webViewControl.GoBack();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }

                retval = false;
            }

            return retval;
        }

        /// <inheritdoc />
        public bool GoForward()
        {
            var retval = true;
            try
            {
                _webViewControl.GoForward();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }

                retval = false;
            }

            return retval;
        }

        /// <inheritdoc cref="IWebView.Refresh" />
        public override void Refresh()
        {
            try
            {
                _webViewControl?.Refresh();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public void Stop()
        {
            try
            {
                _webViewControl?.Stop();
            }
            catch (Exception e)
            {
                if (e.IsSecurityOrCriticalException())
                {
                    throw;
                }
            }
        }

        /// <inheritdoc />
        public void Navigate(Uri source) => _webViewControl?.Navigate(source);

        /// <inheritdoc />
        public void Navigate(string source)
        {
            Verify.IsFalse(IsDisposed);
            Verify.IsNotNull(_webViewControl);
            _webViewControl?.Navigate(source);
        }

        /// <inheritdoc />
        [Obsolete("Use NavigateToLocalStreamUri(Uri, IUriToStreamResolver) instead")]
        public void NavigateToLocal(string relativePath) => _webViewControl?.NavigateToLocal(relativePath);

        /// <inheritdoc />
        public void NavigateToString(string text) => _webViewControl?.NavigateToString(text);

        /// <inheritdoc />
        public void NavigateToLocalStreamUri(Uri relativePath, IUriToStreamResolver streamResolver) => _webViewControl?.NavigateToLocalStreamUri(relativePath, streamResolver);

        /// <inheritdoc />
        public void Navigate(
            Uri requestUri,
            HttpMethod httpMethod,
            string content = null,
            IEnumerable<KeyValuePair<string, string>> headers = null) =>
            _webViewControl.Navigate(requestUri, httpMethod, content, headers);
    }
}