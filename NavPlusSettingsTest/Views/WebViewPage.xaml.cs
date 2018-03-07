using System;

using NavPlusSettingsTest.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavPlusSettingsTest.Views
{
    public sealed partial class WebViewPage : Page
    {
        public WebViewViewModel ViewModel { get; } = new WebViewViewModel();

        public WebViewPage()
        {
            InitializeComponent();
            ViewModel.Initialize(webView);
        }
    }
}
