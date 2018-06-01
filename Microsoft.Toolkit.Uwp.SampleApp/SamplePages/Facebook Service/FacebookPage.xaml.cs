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
using Microsoft.Toolkit.Uwp.Services.Facebook;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class FacebookPage
    {
        public FacebookPage()
        {
            InitializeComponent();

            QueryType.ItemsSource = new[] { "My feed", "My posts", "My tagged", "My albums" };
            QueryType.SelectedIndex = 0;

            ShareBox.Visibility = Visibility.Collapsed;
            PhotoBox.Visibility = Visibility.Collapsed;
            HidePostPanel();
            HidePhotoPanel();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(AppIDText.Text))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            FacebookService.Instance.Initialize(AppIDText.Text, FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts | FacebookPermissions.PublishActions | FacebookPermissions.UserPhotos);
            if (!await FacebookService.Instance.LoginAsync())
            {
                ShareBox.Visibility = Visibility.Collapsed;
                Shell.Current.DisplayWaitRing = false;
                var error = new MessageDialog("Unable to log to Facebook");
                await error.ShowAsync();
                return;
            }

            FacebookDataConfig config;
            switch (QueryType.SelectedIndex)
            {
                case 1:
                    config = FacebookDataConfig.MyPosts;
                    break;
                case 2:
                    config = FacebookDataConfig.MyTagged;
                    break;
                default:
                    config = FacebookDataConfig.MyFeed;
                    break;
            }

            if (QueryType.SelectedIndex == 3)
            {
                PhotoGridView.ItemsSource = await FacebookService.Instance.GetUserAlbumsAsync();
                ShareBox.Visibility = Visibility.Collapsed;
                PhotoBox.Visibility = Visibility.Visible;
                HidePostPanel();
                ShowPhotoPanel();
                PhotoGridView.Visibility = Visibility.Visible;
                PostListView.Visibility = Visibility.Collapsed;
            }
            else
            {
                PostListView.ItemsSource = await FacebookService.Instance.RequestAsync(config, 50);
                ShareBox.Visibility = Visibility.Visible;
                PhotoBox.Visibility = Visibility.Collapsed;
                ShowPostPanel();
                HidePhotoPanel();
                PhotoGridView.Visibility = Visibility.Collapsed;
                PostListView.Visibility = Visibility.Visible;
            }

            HideCredentialsPanel();

            ProfileImage.DataContext = await FacebookService.Instance.GetUserPictureInfoAsync();
            ProfileImage.Visibility = Visibility.Visible;
            Shell.Current.DisplayWaitRing = false;
        }

        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            await FacebookService.Instance.PostToFeedWithDialogAsync(UrlText.Text);
            var message = new MessageDialog("Post sent to facebook");
            await message.ShowAsync();
        }

        private async void SharePictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            var openPicker = new FileOpenPicker
            {
                ViewMode = PickerViewMode.Thumbnail,
                SuggestedStartLocation = PickerLocationId.PicturesLibrary
            };
            openPicker.FileTypeFilter.Add(".jpg");
            openPicker.FileTypeFilter.Add(".png");
            StorageFile picture = await openPicker.PickSingleFileAsync();
            if (picture != null)
            {
                using (var stream = await picture.OpenReadAsync())
                {
                    await FacebookService.Instance.PostPictureToFeedAsync(TitleText.Text, picture.Name, stream);
                }
            }
        }

        private void CredentialsBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (CredentialsBox.Visibility == Visibility.Visible)
            {
                HideCredentialsPanel();
            }
            else
            {
                ShowCredentialsPanel();
            }
        }

        private void PostBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (PostPanel.Visibility == Visibility.Visible)
            {
                HidePostPanel();
            }
            else
            {
                ShowPostPanel();
            }
        }

        private void PhotoBoxExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (PhotoScroller.Visibility == Visibility.Visible)
            {
                HidePhotoPanel();
            }
            else
            {
                ShowPhotoPanel();
            }
        }

        private void ShowCredentialsPanel()
        {
            CredentialsBoxExpandButton.Content = "";
            CredentialsBox.Visibility = Visibility.Visible;
        }

        private void HideCredentialsPanel()
        {
            CredentialsBoxExpandButton.Content = "";
            CredentialsBox.Visibility = Visibility.Collapsed;
        }

        private void ShowPostPanel()
        {
            PostBoxExpandButton.Content = "";
            PostPanel.Visibility = Visibility.Visible;
        }

        private void HidePostPanel()
        {
            PostBoxExpandButton.Content = "";
            PostPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowPhotoPanel()
        {
            PhotoBoxExpandButton.Content = "";
            PhotoScroller.Visibility = Visibility.Visible;
        }

        private void HidePhotoPanel()
        {
            PhotoBoxExpandButton.Content = "";
            PhotoScroller.Visibility = Visibility.Collapsed;
        }

        private async void PhotoGridView_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;

            var addedItem = e.AddedItems.Count > 0 ? e.AddedItems[0] as FacebookAlbum : null;

            if (addedItem != null)
            {
                PhotoGridView.ItemsSource = await FacebookService.Instance.GetUserPhotosByAlbumIdAsync(addedItem.Id);
            }

            Shell.Current.DisplayWaitRing = false;
        }
    }
}
