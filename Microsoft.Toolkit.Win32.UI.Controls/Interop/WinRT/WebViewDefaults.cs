using System;

namespace Microsoft.Toolkit.Win32.UI.Controls.Interop.WinRT
{
    internal static class WebViewDefaults
    {
        public const string AboutBlank = "about:blank";
        public const bool IsIndexedDBEnabled = true;
        public const bool IsJavaScriptEnabled = true;
        public const bool IsPrivateNetworkEnabled = false;
        public const bool IsScriptNotifyEnabled = false;
        public static readonly Uri AboutBlankUri = new Uri(AboutBlank);
    }
}