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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Page shows how ImageCache is used
    /// </summary>
    public sealed partial class ImageCachePage
    {
        private ObservableCollection<PhotoDataItem> _photoItems;

        public ImageCachePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            await LoadDataAsync();
        }

        private async Task PreCacheImages(bool loadInMemory = false)
        {
            DisableButtons();

            ImageCache.Instance.MaxMemoryCacheCount = loadInMemory ? 200 : 0;

            DateTime dtStart = DateTime.Now;

            foreach (var item in _photoItems)
            {
                await ImageCache.Instance.PreCacheAsync(new Uri(item.Thumbnail), Path.GetFileName(item.Thumbnail), false, loadInMemory);
            }

            var msg = $"Preloading {_photoItems.Count} photo took {DateTime.Now.Subtract(dtStart).TotalSeconds} seconds";

            EnableButtons(msg);
        }

        private async Task LoadDataAsync()
        {
            var source = new PhotosDataSource();
            _photoItems = await new PhotosDataSource().GetItemsAsync(true);
        }

        private async void PreCache_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await PreCacheImages(false);
        }

        private async void PreCacheInMemory_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            await PreCacheImages(true);
        }

        private void LoadImages_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            PhotoList.ItemsSource = _photoItems;
        }

        private async void ClearCache_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DisableButtons();

            GC.Collect(); // Force GC to free file locks
            await ImageCache.Instance.ClearAsync();

            PhotoList.ItemsSource = null;

            var msg = "Cache cleared";

            EnableButtons(msg);
        }

        private void EnableButtons(string msg)
        {
            BusyIndicator.IsActive = false;

            foreach (Button btn in ButtonsPanel.Children)
            {
                btn.IsEnabled = true;
            }

            Message.Text = msg;
        }

        private void DisableButtons()
        {
            BusyIndicator.IsActive = true;

            Message.Text = string.Empty;

            foreach (Button btn in ButtonsPanel.Children)
            {
                btn.IsEnabled = false;
            }
        }
    }
}
