using System;
using Windows.UI.Popups;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Data;
using Microsoft.Windows.Toolkit.SampleApp.Models;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public class OptionMenuItem
    {
        public string Name { get; set; }
        public string Glyph { get; set; }
    }

    public sealed partial class HamburgerMenuPage
    {
        public HamburgerMenuPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }
            HamburgerMenu.ItemsSource = new PhotosDataSource().GetItems();

            HamburgerMenu.OptionsItemsSource = new [] { new OptionMenuItem { Glyph = "", Name = "About" } };
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            ContentGrid.DataContext = e.ClickedItem;
        }

        private async void HamburgerMenu_OnOptionsItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as OptionMenuItem;
            var dialog = new MessageDialog($"You clicked on {menuItem.Name} button");

            await dialog.ShowAsync();
        }
    }
}
