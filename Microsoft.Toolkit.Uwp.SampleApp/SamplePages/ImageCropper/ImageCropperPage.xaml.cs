// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        private ImageCropper _imageCropper;

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
            _imageCropper = control.FindChildByName("ImageCropper") as ImageCropper;
            if (_imageCropper != null)
            {
                var file = await StorageFile.GetFileFromApplicationUriAsync(new Uri("ms-appx:///Assets/Photos/Owl.jpg"));
                await _imageCropper.LoadImageFromFile(file);
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
                if (_imageCropper != null)
                {
                    _imageCropper.AspectRatio = -1;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 1:1", (sender, args) =>
            {
                if (_imageCropper != null)
                {
                    _imageCropper.AspectRatio = 1;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 16:9", (sender, args) =>
            {
                if (_imageCropper != null)
                {
                    _imageCropper.AspectRatio = 16d / 9d;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 4:3", (sender, args) =>
            {
                if (_imageCropper != null)
                {
                    _imageCropper.AspectRatio = 4d / 3d;
                }
            });

            SampleController.Current.RegisterNewCommand("Crop image with aspect ratio = 9:16", (sender, args) =>
            {
                if (_imageCropper != null)
                {
                    _imageCropper.AspectRatio = 9d / 16d;
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
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                FileTypeFilter =
                {
                    ".png", ".jpg", ".jpeg"
                }
            };
            var file = await filePicker.PickSingleFileAsync();
            if (file != null && _imageCropper != null)
            {
                await _imageCropper.LoadImageFromFile(file);
            }
        }

        private async Task SaveCroppedImage()
        {
            var savePicker = new FileSavePicker
            {
                SuggestedStartLocation = PickerLocationId.PicturesLibrary,
                SuggestedFileName = "Cropped_Image",
                FileTypeChoices =
                {
                    { "PNG Picture", new List<string> { ".png" } },
                    { "JPG Picture", new List<string> { ".jpg" } }
                }
            };
            var file = await savePicker.PickSaveFileAsync();
            if (file != null && _imageCropper != null)
            {
                if (file.Name.ToLower().Contains(".png"))
                {
                    await _imageCropper.SaveCroppedBitmapAsync(file, BitmapEncoder.PngEncoderId);
                }
                else
                {
                    await _imageCropper.SaveCroppedBitmapAsync(file, BitmapEncoder.JpegEncoderId);
                }
            }
        }
    }
}
