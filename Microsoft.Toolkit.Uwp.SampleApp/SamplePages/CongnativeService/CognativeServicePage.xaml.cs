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
    public sealed partial class CognativeServicePage : Page
    {
        public CognativeServicePage()
        {
            InitializeComponent();
        }

        private async void SelectImage(object sender, RoutedEventArgs e)
        {
            FileOpenPicker open = new FileOpenPicker();
            open.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
            open.ViewMode = PickerViewMode.Thumbnail;

            // Filter to include a sample subset of file types
            open.FileTypeFilter.Clear();
            open.FileTypeFilter.Add(".bmp");
            open.FileTypeFilter.Add(".png");
            open.FileTypeFilter.Add(".jpeg");
            open.FileTypeFilter.Add(".jpg");

            // Open a stream for the selected file
            StorageFile file = await open.PickSingleFileAsync();

            // Ensure a file was selected
            if (file != null)
            {
                // Ensure the stream is disposed once the image is loaded
                using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                {
                    BitmapImage bitmapImage = new BitmapImage();
                    await bitmapImage.SetSourceAsync(fileStream.CloneStream());
                    image.Source = bitmapImage;
                    var service = new VisionService("");
                    //var result = await service.GetTagsAsync(fileStream);
                    var result2 =
                        await
                            service.GetTagsAsync(
                                "http://globalpropertysystems.com/wp-content/uploads/2014/09/Real-Estate-Loans.jpg");
                }
            }
        }
    }
}
