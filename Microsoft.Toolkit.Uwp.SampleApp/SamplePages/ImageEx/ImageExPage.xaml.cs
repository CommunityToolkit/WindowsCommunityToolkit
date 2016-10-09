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
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ImageExPage
    {
        private ObservableCollection<PhotoDataItem> photos;
        private int imageIndex;

        public ImageExPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Image with placeholder", (sender, args) =>
            {
                AddImage(false, true);
            });

            Shell.Current.RegisterNewCommand("Image with placeholder (invalid link or offline)", (sender, args) =>
            {
                AddImage(true, true);
            });

            Shell.Current.RegisterNewCommand("Image without placeholder", (sender, args) =>
            {
                AddImage(false, false);
            });

            Shell.Current.RegisterNewCommand("Clear image cache", async (sender, args) =>
            {
                Container.Children.Clear();
                GC.Collect(); // Force GC to free file locks
                await ImageCache.Instance.ClearAsync();
            });

            await LoadDataAsync();

            AddImage(false, true);
        }

        private async Task LoadDataAsync()
        {
            photos = await new PhotosDataSource().GetItemsAsync(true);
        }

        private void AddImage(bool broken, bool placeholder)
        {
            var newImage = new ImageEx
            {
                IsCacheEnabled = true,
                Stretch = Stretch.UniformToFill,
                Source = broken ? photos[imageIndex].Thumbnail + "broken" : photos[imageIndex].Thumbnail,
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                MaxWidth = 300,
                Background = new SolidColorBrush(Colors.Transparent),
                Foreground = new SolidColorBrush(Colors.White)
            };

            if (placeholder)
            {
                newImage.PlaceholderSource = new BitmapImage(new Uri("ms-appx:///Assets/Photos/ImageExPlaceholder.jpg"));
                newImage.PlaceholderStretch = Stretch.UniformToFill;
            }

            Container.Children.Add(newImage);

            // Move to next image
            imageIndex++;
            if (imageIndex >= photos.Count)
            {
                imageIndex = 0;
            }
        }
    }
}
