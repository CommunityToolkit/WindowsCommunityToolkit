// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class CarouselPage : Page
    {
        private ObservableCollection<string> items = new ObservableCollection<string>();

        public CarouselPage()
        {
            this.InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            foreach (var photo in await new Data.PhotosDataSource().GetItemsAsync())
            {
                items.Add(photo.Thumbnail);
            }

        }
    }
}
