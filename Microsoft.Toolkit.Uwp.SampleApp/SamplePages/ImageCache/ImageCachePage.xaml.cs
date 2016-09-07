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
using System.Linq;
using System.Collections.ObjectModel;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ImageCachePage
    {
        private ObservableCollection<Uri> photoItems;
        private int imageIndex;

        public ImageCachePage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            //Shell.Current.RegisterNewCommand("PreCache Images", async (sender, args) =>
            //{
            //    ImageCache.MaxMemoryCacheSize = 0;

            //    await PreCacheImages(false);
            //});

            //Shell.Current.RegisterNewCommand("PreCache Image (In-memory store)", async (sender, args) =>
            //{
            //    ImageCache.MaxMemoryCacheSize = 150;

            //    await PreCacheImages(true);
            //});

            //Shell.Current.RegisterNewCommand("Load Images", (sender, args) =>
            //{
            //    control.ItemsSource = photoItems;
            //});

            await LoadDataAsync();
        }

        private async Task PreCacheImages(bool loadInMemory = false)
        {
            DisableButtons();

            ImageCache.MaxMemoryCacheSize = loadInMemory ? 200 : 0;

            DateTime dtStart = DateTime.Now;

            foreach (var item in photoItems)
            {
                await ImageCache.PreCacheAsync(item, loadInMemory);
            }

            var msg = $"Preloading {photoItems.Count} photo took {DateTime.Now.Subtract(dtStart).TotalSeconds} seconds";

            EnableButtons(msg);
        }

        private async Task LoadDataAsync()
        {
            photoItems = await new ImageCacheDataSource().GetItemsAsync();
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
            control.ItemsSource = photoItems;
        }

        private async void ClearCache_Tapped(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            DisableButtons();

            GC.Collect(); // Force GC to free file locks
            await ImageCache.ClearAsync();

            control.ItemsSource = null;

            var msg = "Cache cleared";

            EnableButtons(msg);
        }

        private void EnableButtons(string msg)
        {
            busyIndicator.IsActive = false;

            foreach (Button btn in spButtons.Children)
            {
                btn.IsEnabled = true;
            }

            message.Text = msg;
        }

        private void DisableButtons()
        {
            busyIndicator.IsActive = true;

            message.Text = string.Empty;

            foreach (Button btn in spButtons.Children)
            {
                btn.IsEnabled = false;
            }
        }
    }
}
