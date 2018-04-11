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
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1704:IdentifiersShouldBeSpelledCorrectly", MessageId = "Unviewable")]
    public sealed class WebViewControlUnviewableContentIdentifiedEventArgs : EventArgs
  {
    [SecurityCritical]
    private readonly Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs _args;

    internal WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args)
    {
      _args = args ?? throw new ArgumentNullException(nameof(args));
    }

    public Uri Uri => _args.Uri;

    public Uri Referrer => _args.Referrer;

    public string MediaType => _args.MediaType;

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlUnviewableContentIdentifiedEventArgs(Windows.Web.UI.WebViewControlUnviewableContentIdentifiedEventArgs args) => new WebViewControlUnviewableContentIdentifiedEventArgs(args);
  }
}
