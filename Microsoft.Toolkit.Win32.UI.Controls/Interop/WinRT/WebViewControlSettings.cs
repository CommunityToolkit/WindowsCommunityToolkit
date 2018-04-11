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
    public sealed class WebViewControlSettings
    {
        [SecurityCritical]
        private readonly Windows.Web.UI.WebViewControlSettings _settings;

        internal WebViewControlSettings(Windows.Web.UI.WebViewControlSettings settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        // ReSharper disable InconsistentNaming
        public bool IsIndexedDBEnabled
        // ReSharper restore InconsistentNaming
        {
            get => _settings.IsIndexedDBEnabled;
            set => _settings.IsIndexedDBEnabled = value;
        }

        public bool IsJavaScriptEnabled
        {
            get => _settings.IsJavaScriptEnabled;
            set => _settings.IsJavaScriptEnabled = value;
        }

        public bool IsScriptNotifyAllowed
        {
            get => _settings.IsScriptNotifyAllowed;
            set => _settings.IsScriptNotifyAllowed = value;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2225:OperatorOverloadsHaveNamedAlternates")]
        public static implicit operator WebViewControlSettings(Windows.Web.UI.WebViewControlSettings settings) => new WebViewControlSettings(settings);
    }
}