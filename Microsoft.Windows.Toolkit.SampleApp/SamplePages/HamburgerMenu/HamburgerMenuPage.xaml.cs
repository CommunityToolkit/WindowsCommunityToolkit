using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Models;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public class MenuItem
    {
        public string Name { get; set; }
        public string Icon { get; set; }
        public string Image { get; set; }
    }

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
            HamburgerMenu.ItemsSource = new[] { new MenuItem { Icon = "/Icons/Foundation.png", Name = "BigFourSummerHeat.png", Image= "/Assets/Photos/BigFourSummerHeat.png" } };

            HamburgerMenu.OptionsItemsSource = new [] { new OptionMenuItem { Glyph = "", Name = "About" } };
        }

        private void HamburgerMenu_OnItemClick(object sender, ItemClickEventArgs e)
        {
            var menuItem = e.ClickedItem as MenuItem;
            Header.Text = menuItem.Name;

            Image.Source = 
        }
    }
}
