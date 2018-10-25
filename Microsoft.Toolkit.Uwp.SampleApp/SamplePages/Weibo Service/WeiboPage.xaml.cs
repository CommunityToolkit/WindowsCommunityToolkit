// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.IO;
using Microsoft.Toolkit.Services.Weibo;
using Windows.Storage;
using Windows.Storage.Pickers;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class WeiboPage
    {
        private ObservableCollection<WeiboStatus> _statuses;

        public WeiboPage()
        {
            InitializeComponent();

            ShareBox.Visibility = Visibility.Collapsed;
            HideStatusPanel();

            AppKey.Text = string.Empty;
            AppSecret.Text = string.Empty;
            RedirectUri.Text = string.Empty;
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(AppKey.Text) || string.IsNullOrEmpty(AppSecret.Text) || string.IsNullOrEmpty(RedirectUri.Text))
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;
            WeiboService.Instance.Initialize(AppKey.Text, AppSecret.Text, RedirectUri.Text);

            if (!await WeiboService.Instance.LoginAsync())
            {
                SampleController.Current.DisplayWaitRing = false;
                var error = new MessageDialog("Unable to log to Weibo");
                await error.ShowAsync();
                return;
            }

            ShareBox.Visibility = Visibility.Visible;

            HideCredentialsPanel();
            ShowStatusPanel();

            WeiboUser user;
            try
            {
                user = await WeiboService.Instance.GetUserAsync();
            }
            catch (WeiboException ex)
            {
                if (ex.Error.Code == 21332)
                {
                    await new MessageDialog("Invalid or expired token. Logging out. Re-connect for new token.").ShowAsync();
                    await WeiboService.Instance.LogoutAsync();
                    return;
                }
                else
                {
                    throw ex;
                }
            }

            ProfileImage.DataContext = user;
            ProfileImage.Visibility = Visibility.Visible;

            _statuses = new ObservableCollection<WeiboStatus>(await WeiboService.Instance.GetUserTimeLineAsync(user.ScreenName, 50));

            ListView.ItemsSource = _statuses;

            SampleController.Current.DisplayWaitRing = false;
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

        private void HideCredentialsPanel()
        {
            CredentialsBoxExpandButton.Content = "";
            CredentialsBox.Visibility = Visibility.Collapsed;
        }

        private void ShowCredentialsPanel()
        {
            CredentialsBoxExpandButton.Content = "";
            CredentialsBox.Visibility = Visibility.Visible;
        }

        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            SampleController.Current.DisplayWaitRing = true;
            await WeiboService.Instance.PostStatusAsync(StatusText.Text);
            SampleController.Current.DisplayWaitRing = false;
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
                    SampleController.Current.DisplayWaitRing = true;
                    await WeiboService.Instance.PostStatusAsync(StatusText.Text, stream.AsStream());
                    SampleController.Current.DisplayWaitRing = false;
                }
            }
        }

        private void StatusBoxExpandButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (StatusPanel.Visibility == Visibility.Visible)
            {
                HideStatusPanel();
            }
            else
            {
                ShowStatusPanel();
            }
        }

        private void ShowStatusPanel()
        {
            StatusBoxExpandButton.Content = "";
            StatusPanel.Visibility = Visibility.Visible;
        }

        private void HideStatusPanel()
        {
            StatusBoxExpandButton.Content = "";
            StatusPanel.Visibility = Visibility.Collapsed;
        }
    }
}
