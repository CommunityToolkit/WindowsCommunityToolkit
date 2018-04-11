// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Security;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    public sealed class WebViewControlUnsupportedUriSchemeIdentifiedEventArgs : EventArgs
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs _args;

        internal WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args)
        {
            _args = args ?? throw new ArgumentNullException(nameof(args));
        }

        public Uri Uri => _args.Uri;

        public bool Handled
        {
            get => _args.Handled;
            set => _args.Handled = value;
        }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnsupportedUriSchemeIdentifiedEventArgs args) =>
            new WebViewControlUnsupportedUriSchemeIdentifiedEventArgs(args);
    }
}