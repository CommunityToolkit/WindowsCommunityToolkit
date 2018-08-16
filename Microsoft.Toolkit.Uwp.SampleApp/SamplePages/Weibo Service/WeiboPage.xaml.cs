// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.Weibo;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class WeiboPage
    {
        public WeiboPage()
        {
            InitializeComponent();

            // TODO

            AppKey.Text = "";
            AppSecret.Text = "";
            RedirectUri.Text = "";
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
                // TODO
                SampleController.Current.DisplayWaitRing = false;
                var error = new MessageDialog("Unable to log to Weibo");
                await error.ShowAsync();
                return;
            }

            // TODO

            HideCredentialsPanel();
            ShowSearchPanel();
            // TODO

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
                    WeiboService.Instance.Logout();
                    return;
                }
                else
                {
                    throw ex;
                }
            }

            ProfileImage.DataContext = user;
            ProfileImage.Visibility = Visibility.Visible;

            // TODO

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

        private void ShowSearchPanel()
        {
            // TODO
        }
    }
}