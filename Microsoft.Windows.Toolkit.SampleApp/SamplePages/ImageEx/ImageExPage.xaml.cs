using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Windows.Toolkit.SampleApp.Models;
using Microsoft.Windows.Toolkit.UI;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class ImageExPage
    {
        public ImageExPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Reset Cache", async (sender, args) =>
            {
                control.ItemsSource = null;
                System.GC.Collect(); // Force GC to free file locks
                await ImageCache.ClearAsync();
            });

            Shell.Current.RegisterNewCommand("Reload content", (sender, args) =>
            {
                LoadData();
            });

            LoadData();
        }

        private void LoadData()
        {
            control.ItemsSource = new Data.PhotosDataSource().GetItems(true);
        }
    }
}
