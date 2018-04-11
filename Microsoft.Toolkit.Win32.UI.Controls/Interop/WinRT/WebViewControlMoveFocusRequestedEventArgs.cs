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
  public sealed class WebViewControlMoveFocusRequestedEventArgs : EventArgs
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs _args;

    internal WebViewControlMoveFocusRequestedEventArgs(Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args)
    {
      _args = args ?? throw new ArgumentNullException(nameof(args));
    }

    public WebViewControlMoveFocusReason Reason => (WebViewControlMoveFocusReason)_args.Reason;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlMoveFocusRequestedEventArgs(Windows.Web.UI.Interop.WebViewControlMoveFocusRequestedEventArgs args) => new WebViewControlMoveFocusRequestedEventArgs(args);
  }
}