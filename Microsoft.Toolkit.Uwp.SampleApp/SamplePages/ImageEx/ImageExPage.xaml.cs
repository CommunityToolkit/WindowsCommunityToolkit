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
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ImageExPage : IXamlRenderListener
    {
        private ObservableCollection<PhotoDataItem> photos;
        private int imageIndex;
        private StackPanel container;

        public ImageExPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            // Need to use logical tree here as scrollviewer hasn't initialized yet even with dispatch.
            container = control.FindChildByName("Container") as StackPanel;
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

            Shell.Current.RegisterNewCommand("Round Image with placeholder", (sender, args) =>
            {
                AddImage(false, true, true);
            });

            Shell.Current.RegisterNewCommand("Round Image with placeholder (invalid link or offline)", (sender, args) =>
            {
                AddImage(true, true, true);
            });

            Shell.Current.RegisterNewCommand("Round Image without placeholder", (sender, args) =>
            {
                AddImage(false, false, true);
            });

            Shell.Current.RegisterNewCommand("Clear image cache", async (sender, args) =>
            {
                container?.Children?.Clear();
                GC.Collect(); // Force GC to free file locks
                await ImageCache.Instance.ClearAsync();
            });

            await LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            photos = await new PhotosDataSource().GetItemsAsync(true);
        }

        private void AddImage(bool broken, bool placeholder, bool round = false)
        {
            ImageExBase newImage = null;
            if (round)
            {
                newImage = new RoundImageEx
                {
                    Height = 200,
                    Width = 200,
                    CornerRadius = 999
                };
            }
            else
            {
                newImage = new ImageEx();
            }

            newImage.IsCacheEnabled = true;
            newImage.Stretch = Stretch.UniformToFill;
            newImage.Source = broken ? photos[imageIndex].Thumbnail + "broken" : photos[imageIndex].Thumbnail;
            newImage.HorizontalAlignment = HorizontalAlignment.Center;
            newImage.VerticalAlignment = VerticalAlignment.Center;
            newImage.MaxWidth = 300;
            newImage.Background = new SolidColorBrush(Colors.Transparent);
            newImage.Foreground = new SolidColorBrush(Colors.White);

            if (placeholder)
            {
                newImage.PlaceholderSource = new BitmapImage(new Uri("ms-appx:///Assets/Photos/ImageExPlaceholder.jpg"));
                newImage.PlaceholderStretch = Stretch.UniformToFill;
            }

            container?.Children?.Add(newImage);

            // Move to next image
            imageIndex++;
            if (imageIndex >= photos.Count)
            {
                imageIndex = 0;
            }
        }
    }
}