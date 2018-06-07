// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class AadLoginPage : IXamlRenderListener
    {
        private AadLogin _aadLoginControl;

        public AadLoginPage()
        {
            InitializeComponent();
            Load();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            if (_aadLoginControl != null)
            {
                _aadLoginControl.SignInFailed -= AadLoginControl_SignInFailed;
            }

            _aadLoginControl = control.FindDescendantByName("AadLoginControl") as AadLogin;

            if (_aadLoginControl != null)
            {
                _aadLoginControl.SignInFailed += AadLoginControl_SignInFailed;
            }
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Change default image", async (sender, args) =>
            {
                if (_aadLoginControl != null)
                {
                    var openPicker = new FileOpenPicker
                    {
                        ViewMode = PickerViewMode.Thumbnail,
                        SuggestedStartLocation = PickerLocationId.PicturesLibrary
                    };
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".png");
                    openPicker.FileTypeFilter.Add(".gif");
                    openPicker.FileTypeFilter.Add(".bmp");

                    // Open a stream for the selected file
                    StorageFile file = await openPicker.PickSingleFileAsync();

                    // Ensure a file was selected
                    if (file != null)
                    {
                        using (IRandomAccessStream fileStream = await file.OpenAsync(FileAccessMode.Read))
                        {
                            // Set the image source to the selected bitmap
                            var defaultImage = new BitmapImage();
                            await defaultImage.SetSourceAsync(fileStream);
                            _aadLoginControl.DefaultImage = defaultImage;
                        }
                    }
                }
            });
        }

        private void AadLoginControl_SignInFailed(object sender, SignInFailedEventArgs e)
        {
            SampleController.Current.ShowExceptionNotification(e.Exception);
        }
    }
}
