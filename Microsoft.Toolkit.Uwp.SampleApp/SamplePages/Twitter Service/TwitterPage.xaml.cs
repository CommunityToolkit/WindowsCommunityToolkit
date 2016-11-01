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
using Microsoft.Toolkit.Uwp.Services.Twitter;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class TwitterPage
    {
        public TwitterPage()
        {
            InitializeComponent();

            ShareBox.Visibility = Visibility.Collapsed;
            SearchBox.Visibility = Visibility.Collapsed;
            HideSearchPanel();
            HideTweetPanel();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(ConsumerKey.Text) || string.IsNullOrEmpty(ConsumerSecret.Text) || string.IsNullOrEmpty(CallbackUri.Text))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            TwitterService.Instance.Initialize(ConsumerKey.Text, ConsumerSecret.Text, CallbackUri.Text);

            if (!await TwitterService.Instance.LoginAsync())
            {
                ShareBox.Visibility = Visibility.Collapsed;
                SearchBox.Visibility = Visibility.Collapsed;
                Shell.Current.DisplayWaitRing = false;
                var error = new MessageDialog("Unable to log to Twitter");
                await error.ShowAsync();
                return;
            }

            ShareBox.Visibility = Visibility.Visible;
            SearchBox.Visibility = Visibility.Visible;

            HideCredentialsPanel();
            ShowSearchPanel();
            ShowTweetPanel();

            TwitterUser user;
            try
            {
                user = await TwitterService.Instance.GetUserAsync();
            }
            catch (TwitterException ex)
            {
                if ((ex.Errors?.Errors?.Length > 0) && (ex.Errors.Errors[0].Code == 89))
                {
                    await new MessageDialog("Invalid or expired token. Logging out. Re-connect for new token.").ShowAsync();
                    TwitterService.Instance.Logout();
                    return;
                }
                else
                {
                    throw ex;
                }
            }

            ProfileImage.DataContext = user;
            ProfileImage.Visibility = Visibility.Visible;

            ListView.ItemsSource = await TwitterService.Instance.GetUserTimeLineAsync(user.ScreenName, 50);

            Shell.Current.DisplayWaitRing = false;
        }

        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            await TwitterService.Instance.TweetStatusAsync(TweetText.Text);
            Shell.Current.DisplayWaitRing = false;
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            ListView.ItemsSource = await TwitterService.Instance.SearchAsync(TagText.Text, 50);
            Shell.Current.DisplayWaitRing = false;
        }

        private async void SharePictureButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            FileOpenPicker openPicker = new FileOpenPicker
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
                    await TwitterService.Instance.TweetStatusAsync(TweetText.Text, stream);
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

        private void TweetBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (TweetPanel.Visibility == Visibility.Visible)
            {
                HideTweetPanel();
            }
            else
            {
                ShowTweetPanel();
            }
        }

        private void SearchBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (SearchPanel.Visibility == Visibility.Visible)
            {
                HideSearchPanel();
            }
            else
            {
                ShowSearchPanel();
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

        private void ShowTweetPanel()
        {
            TweetBoxExpandButton.Content = "";
            TweetPanel.Visibility = Visibility.Visible;
        }

        private void HideTweetPanel()
        {
            TweetBoxExpandButton.Content = "";
            TweetPanel.Visibility = Visibility.Collapsed;
        }

        private void ShowSearchPanel()
        {
            SearchBoxExpandButton.Content = "";
            SearchPanel.Visibility = Visibility.Visible;
        }

        private void HideSearchPanel()
        {
            SearchBoxExpandButton.Content = "";
            SearchPanel.Visibility = Visibility.Collapsed;
        }
    }
}
