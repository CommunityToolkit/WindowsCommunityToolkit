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