// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Services.MicrosoftGraph;
using Microsoft.Toolkit.Uwp.UI.Controls.Graph;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.Storage.Streams;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ProfileCardPage : IXamlRenderListener
    {
        private MicrosoftGraphService GraphService => MicrosoftGraphService.Instance;

        private ProfileCard _profileCardControl;

        public ProfileCardPage()
        {
            InitializeComponent();
            Load();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            _profileCardControl = control.FindDescendantByName("ProfileCardControl") as ProfileCard;

            if (_profileCardControl != null)
            {
                GraphService.IsAuthenticatedChanged += async (s, e) =>
                {
                    if (GraphService.IsAuthenticated)
                    {
                        _profileCardControl.UserId = (await GraphService.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
                    }
                };

                if (GraphService.IsAuthenticated)
                {
                    _profileCardControl.UserId = (await GraphService.User.GetProfileAsync(new MicrosoftGraphUserFields[1] { MicrosoftGraphUserFields.Id })).Id;
                }
            }
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Change default image", async (sender, args) =>
            {
                if (_profileCardControl != null)
                {
                    FileOpenPicker openPicker = new FileOpenPicker
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
                            _profileCardControl.DefaultImage = defaultImage;
                        }
                    }
                }
            });
        }
    }
}
