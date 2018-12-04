using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Graphics.Imaging;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// A page that shows how to use the ImageCropper control.
    /// </summary>
    public sealed partial class ImageCropperPage : Page, IXamlRenderListener
    {
        private ImageCropper imageCropper;

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageCropperPage"/> class.
        /// </summary>
        public ImageCropperPage()
        {
            this.InitializeComponent();
            Load();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            imageCropper = control.FindChildByName("ImageCropper") as ImageCropper;
            if (imageCropper != null)
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Photos/Owl.jpg"));
                await imageCropper.LoadImageFromFile(file);
            }
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Pick Image", async (sender, args) =>
            {
                await PickImage();
            });

            SampleController.Current.RegisterNewCommand("Crop image without aspect ratio", (sender, args) =>
            {
                if (imageCropper != null)
                {
                    imageCropper.AspectRatio = -1;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 1:1", (sender, args) =>
            {
                if (imageCropper != null)
                {
                    imageCropper.AspectRatio = 1;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 16:9", (sender, args) =>
            {
                if (imageCropper != null)
                {
                    imageCropper.AspectRatio = 16d / 9d;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 4:3", (sender, args) =>
            {
                if (imageCropper != null)
                {
                    imageCropper.AspectRatio = 4d / 3d;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 9:16", (sender, args) =>
            {
                if (imageCropper != null)
                {
                    imageCropper.AspectRatio = 9d / 16d;
                }
            });

            SampleController.Current.RegisterNewCommand("Save", async (sender, args) =>
             {
                 await SaveCroppedImage();
             });
        }

        private async Task PickImage()
        {
            var filePicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            filePicker.FileTypeFilter.Add(".png");
            filePicker.FileTypeFilter.Add(".jpg");
            filePicker.FileTypeFilter.Add(".jpeg");
            var file = await filePicker.PickSingleFileAsync();
            if (file != null && imageCropper != null)
            {
                await imageCropper.LoadImageFromFile(file);
            }
        }

        private async Task SaveCroppedImage()
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedFileName = "Cropped_Image"
            };
            savePicker.FileTypeChoices.Add("PNG Picture", new List<string> { ".png" });
            savePicker.FileTypeChoices.Add("JPG Picture", new List<string> { ".jpg" });
            var file = await savePicker.PickSaveFileAsync();
            if (file != null && imageCropper != null)
            {
                if (file.Name.ToLower().Contains(".png"))
                {
                    await imageCropper.SaveCroppedBitmapAsync(file, BitmapEncoder.PngEncoderId);
                }
                else
                {
                    await imageCropper.SaveCroppedBitmapAsync(file, BitmapEncoder.JpegEncoderId);
                }
            }
        }
    }
}
