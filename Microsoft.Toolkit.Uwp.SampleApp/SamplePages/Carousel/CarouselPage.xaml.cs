// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238
using System.Collections.ObjectModel;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Windows.UI.Xaml.Media;
using System;

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

        //private void CarouselItem_ItemGotCarouselFocus(object sender, System.EventArgs e)
        //{
        //    var item = sender as CarouselItem;
        //    var centerX = item.ActualWidth / 2;
        //    var centerY = item.ActualHeight / 2;

        //    item.Projection = new PlaneProjection()
        //    {
        //        CenterOfRotationY = 0.5
        //    };

        //    var projection = item.Projection as PlaneProjection;

        //    projection.RotationY = 0;

        //}

        //private void CarouselItem_ItemLostCarouselFocus(object sender, System.EventArgs e)
        //{
        //    var item = sender as CarouselItem;
        //    var centerX = item.ActualWidth / 2;
        //    var centerY = item.ActualHeight / 2;

        //    if (item.Projection == null)
        //    {
        //        item.Projection = new PlaneProjection()
        //        {
        //            CenterOfRotationY = 0.5
        //        };
        //    }

        //    var projection = item.Projection as PlaneProjection;

        //    projection.RotationY = item.CarouselItemLocation > 0 ? -40 : 40;
        //}

        //private void CarouselItem_Loaded(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        //{
        //    var item = sender as CarouselItem;
        //    var centerX = item.ActualWidth / 2;
        //    var centerY = item.ActualHeight / 2;

        //    if (item.Projection == null)
        //    {
        //        item.Projection = new PlaneProjection()
        //        {
        //            CenterOfRotationY = 0.5
        //        };
        //    }

        //    var projection = item.Projection as PlaneProjection;
        //    var rotation = item.CarouselItemLocation * -1 * 10;
        //    if (rotation > 70) rotation = 70;
        //    if (rotation < -70) rotation = -70;

        //    projection.RotationY = rotation;
        //}

        private void CarouselItem_CarouselItemLocationChanged(object sender, CarouselItemLocationChangedEventArgs e)
        {
            var item = sender as CarouselItem;
            float centerX = (float)item.ActualWidth / 2;
            float centerY = (float)item.ActualHeight / 2;

            //item.Projection = new PlaneProjection()
            //{
            //    CenterOfRotationY = 0.5
            //};

            //var projection = item.Projection as PlaneProjection;
            //var rotation = item.CarouselItemLocation * -1 * 20;
            //if (rotation > 70) rotation = 70;
            //if (rotation < -70) rotation = -70;

            //projection.RotationY = rotation;

            //float scale = 1 - Math.Abs(item.CarouselItemLocation) * 0.1f;

            //item.Scale(scale, scale, centerX, centerY, 200).Start();

            item.Rotate(item.CarouselItemLocation * 10f, centerX, centerY, 200).Start();
        }
    }
}
