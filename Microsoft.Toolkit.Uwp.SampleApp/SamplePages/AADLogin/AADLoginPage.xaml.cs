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
using Microsoft.Graph;
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
    public sealed partial class AADLoginPage : IXamlRenderListener
    {
        private AADLogin aadLoginControl;

        public AADLoginPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            aadLoginControl = control.FindDescendantByName("AADLoginControl") as AADLogin;

            if (aadLoginControl != null)
            {
                aadLoginControl.SignInCompleted += AadLoginControl_SignInCompleted;
                aadLoginControl.SignOutCompleted += AadLoginControl_SignOutCompleted;
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Change default image", async (sender, args) =>
            {
                if (aadLoginControl != null)
                {
                    FileOpenPicker openPicker = new FileOpenPicker();
                    openPicker.ViewMode = PickerViewMode.Thumbnail;
                    openPicker.SuggestedStartLocation = PickerLocationId.PicturesLibrary;
                    openPicker.FileTypeFilter.Add(".jpg");
                    openPicker.FileTypeFilter.Add(".jpeg");
                    openPicker.FileTypeFilter.Add(".png");

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
                            aadLoginControl.DefaultImage = defaultImage;
                        }
                    }
                }
            });
        }

        private void AadLoginControl_SignInCompleted(object sender, SignInEventArgs e)
        {
            string graphAccessToken = e.GraphAccessToken;
            GraphServiceClient graphServiceClient = e.GraphClient;

            // TODO
        }

        private void AadLoginControl_SignOutCompleted(object sender, System.EventArgs e)
        {
            // TODO
        }
    }
}
