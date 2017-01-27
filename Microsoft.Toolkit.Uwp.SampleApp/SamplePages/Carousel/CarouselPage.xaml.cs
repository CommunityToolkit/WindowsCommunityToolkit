using System;
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class CarouselPage : Page
    {
        private ObservableCollection<PhotoDataItem> items = new ObservableCollection<PhotoDataItem>();

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
                items.Add(photo);
            }
        }

        private void CarouselItem_CarouselItemLocationChanged(object sender, CarouselItemLocationChangedEventArgs e)
        {
            var item = sender as CarouselItem;

            if (item.DefaultAnimationEnabled)
            {
                return;
            }

            float centerX = (float)item.ActualWidth / 2;
            float centerY = (float)item.ActualHeight / 2;

            item.Rotate(item.CarouselItemLocation * 10f, centerX, centerY, 200).Start();

            var text = item.FindName("Text") as TextBlock;
            if (text != null)
            {
                centerX = (float)text.ActualWidth / 2;
                if (item.CarouselItemLocation == 0)
                {
                    if (CarouselControl.Orientation == Orientation.Horizontal)
                    {
                        text.Scale(scaleX: 2, scaleY: 2, centerX: centerX).Start();
                        text.Offset(offsetY: 200).Start();
                    }
                    else
                    {
                        text.Scale(scaleX: 2, scaleY: 2).Start();
                        text.Offset(offsetX: 200, offsetY: -90).Start();
                    }
                }
                else
                {
                    text.Scale(scaleX: 1, scaleY: 1, centerX: centerX).Start();
                    text.Offset(offsetY: 0, offsetX: 0).Start();
                }
            }
        }
    }
}
