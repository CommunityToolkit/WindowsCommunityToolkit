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
    public sealed class WebViewNavigationCompletedEventArgs : EventArgs
    {
        internal WebViewNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            IsSuccess = args.IsSuccess;
            Uri = args.Uri;
            WebErrorStatus = (WebErrorStatus)args.WebErrorStatus;
        }

        internal WebViewNavigationCompletedEventArgs(Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args, Uri uri)
        : this(args)
        {
            Uri = uri;
        }

        public bool IsSuccess { get; }

        public Uri Uri { get; }

        public WebErrorStatus WebErrorStatus { get; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewNavigationCompletedEventArgs(
            Windows.Web.UI.WebViewControlNavigationCompletedEventArgs args)
        {
            return new WebViewNavigationCompletedEventArgs(args);
        }
    }
}