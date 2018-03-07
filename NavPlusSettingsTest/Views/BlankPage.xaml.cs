using System;

using NavPlusSettingsTest.ViewModels;

using Windows.UI.Xaml.Controls;

namespace NavPlusSettingsTest.Views
{
    public sealed partial class BlankPage : Page
    {
        public BlankViewModel ViewModel { get; } = new BlankViewModel();

        public BlankPage()
        {
            InitializeComponent();
        }
    }
}
