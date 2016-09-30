using System;
using Microsoft.Toolkit.Uwp.Services.CognitiveServices;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class VisionServicePage : Page
    {
        private VisionService _visionService;

        public VisionServicePage()
        {
            InitializeComponent();
        }

        private void SetApiKeyButton_Click(object sender, RoutedEventArgs e)
        {
            _visionService = new VisionService(ApiKey.Text);
        }

        private async void TagImageFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null)
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".gif");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");
            StorageFile file = await open.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream.CloneStream());
                    TagImage.Source = bitmapImage;

                    var result = await _visionService.GetTagsAsync(fileStream);
                    ResultTextbox.Text = result.ToString();
                }
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private async void TagUrlFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null || string.IsNullOrWhiteSpace(ImageUrl.Text))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ResultTextbox.Text = string.Empty;

            var result =
                        await
                            _visionService.GetTagsAsync(ImageUrl.Text);

            ResultTextbox.Text = result.ToString();
            Shell.Current.DisplayWaitRing = false;
        }

        private void ApiKeyPanelExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (ApiKeyPanel.Visibility == Visibility.Visible)
            {
                HideApiPanel();
            }
            else
            {
                ShowApiPanel();
            }
        }

        private void ShowApiPanel()
        {
            ApiKeyPanelExpandButton.Content = "";
            ApiKeyPanel.Visibility = Visibility.Visible;
        }

        private void HideApiPanel()
        {
            ApiKeyPanelExpandButton.Content = "";
            ApiKeyPanel.Visibility = Visibility.Collapsed;
        }

        private void TagImagesExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagImagePanel.Visibility == Visibility.Visible)
            {
                HideTagImagePanel();
            }
            else
            {
                ShowTagImagePanel();
            }
        }

        private void ShowTagImagePanel()
        {
            TagImagesExpandButton.Content = "";
            TagImagePanel.Visibility = Visibility.Visible;
        }

        private void HideTagImagePanel()
        {
            TagImagesExpandButton.Content = "";
            TagImagePanel.Visibility = Visibility.Collapsed;
        }

        private void TagUrlExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (TagUrlPanel.Visibility == Visibility.Visible)
            {
                HideTagUrlPanel();
            }
            else
            {
                ShowTagUrlPanel();
            }
        }

        private void ShowTagUrlPanel()
        {
            TagUrlExpandButton.Content = "";
            TagUrlPanel.Visibility = Visibility.Visible;
        }

        private void HideTagUrlPanel()
        {
            TagUrlExpandButton.Content = "";
            TagUrlPanel.Visibility = Visibility.Collapsed;
        }
    }
}
