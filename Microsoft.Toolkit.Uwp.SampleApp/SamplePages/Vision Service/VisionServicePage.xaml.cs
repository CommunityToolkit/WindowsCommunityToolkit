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

        private void VisionApiButton_Click(object sender, RoutedEventArgs e)
        {
            _visionService = new VisionService(ApiKey.Text);
        }

        private async void ProcessImageFromUrl_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null)
            {
                return;
            }

            var result =
                        await
                            _visionService.GetTagsAsync(
                                "http://globalpropertysystems.com/wp-content/uploads/2014/09/Real-Estate-Loans.jpg");

            ResultTextbox.Text = result.ToString();
        }

        private async void ProcessImageFromPicker_Click(object sender, RoutedEventArgs e)
        {
            if (_visionService == null)
            {
                return;
            }

            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");
            StorageFile file = await open.PickSingleFileAsync();
            if (file != null)
            {
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();

                    // clone stream because it can't be used twice
                    await bitmapImage.SetSourceAsync(fileStream.CloneStream());
                    image.Source = bitmapImage;

                    var result = await _visionService.GetTagsAsync(fileStream);
                    ResultTextbox.Text = result.ToString();
                }
            }
        }
    }
}
