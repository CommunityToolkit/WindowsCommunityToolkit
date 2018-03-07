using System;

using NavPlusSettingsTest.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavPlusSettingsTest.Views
{
    public sealed partial class MainPage : Page
    {
        public MainViewModel ViewModel { get; } = new MainViewModel();

        public MainPage()
        {
            InitializeComponent();
        }
    }
}
