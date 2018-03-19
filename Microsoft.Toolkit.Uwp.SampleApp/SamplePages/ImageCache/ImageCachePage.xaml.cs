// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Page shows how ImageCache is used
    /// </summary>
    public sealed partial class ImageCachePage : IXamlRenderListener
    {
        private ObservableCollection<PhotoDataItem> _photoItems;
        private ListView photoList;

        public ImageCachePage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            photoList = control.FindChildByName("PhotoList") as ListView;
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("PreCache photos", PreCache_Tapped);

            Shell.Current.RegisterNewCommand("PreCache photos in memory", PreCacheInMemory_Tapped);

            Shell.Current.RegisterNewCommand("Load photos", LoadImages_Tapped);

            Shell.Current.RegisterNewCommand("Clear cache", ClearCache_Tapped);

            await LoadDataAsync();
        }

        private async Task PreCacheImages(bool loadInMemory = false)
        {
            BusyIndicator.IsActive = true;

            ImageCache.Instance.MaxMemoryCacheCount = loadInMemory ? 200 : 0;

            DateTime dtStart = DateTime.Now;

            foreach (var item in _photoItems)
            {
                await ImageCache.Instance.PreCacheAsync(new Uri(item.Thumbnail), false, loadInMemory);
            }

            Message.Text = $"Preloading {_photoItems.Count} photo took {DateTime.Now.Subtract(dtStart).TotalSeconds} seconds";

            BusyIndicator.IsActive = false;
        }

        private async Task LoadDataAsync()
        {
            var source = new PhotosDataSource();
            _photoItems = await new PhotosDataSource().GetItemsAsync(true);
        }

        private async void PreCache_Tapped(object sender, RoutedEventArgs e)
        {
            await PreCacheImages(false);
        }

        private async void PreCacheInMemory_Tapped(object sender, RoutedEventArgs e)
        {
            await PreCacheImages(true);
        }

        private void LoadImages_Tapped(object sender, RoutedEventArgs e)
        {
            if (photoList != null)
            {
                photoList.ItemsSource = _photoItems;
            }
        }

        private async void ClearCache_Tapped(object sender, RoutedEventArgs e)
        {
            BusyIndicator.IsActive = true;

            GC.Collect(); // Force GC to free file locks
            await ImageCache.Instance.ClearAsync();

            if (photoList != null)
            {
                photoList.ItemsSource = null;
            }

            Message.Text = "Cache cleared";

            BusyIndicator.IsActive = false;
        }
    }
}
