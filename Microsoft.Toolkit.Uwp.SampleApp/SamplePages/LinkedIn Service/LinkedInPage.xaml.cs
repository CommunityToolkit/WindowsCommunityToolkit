// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.Services.LinkedIn;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LinkedInPage : Page
    {
        public LinkedInPage()
        {
            InitializeComponent();
        }

        private async void ConnectButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(ClientId.Text) || string.IsNullOrEmpty(ClientSecret.Text) || string.IsNullOrEmpty(CallbackUri.Text))
            {
                return;
            }

            var oAuthTokens = new LinkedInOAuthTokens
            {
                ClientId = ClientId.Text,
                ClientSecret = ClientSecret.Text,
                CallbackUri = CallbackUri.Text
            };

            var succeeded = LinkedInService.Instance.Initialize(oAuthTokens, LinkedInPermissions.ReadBasicProfile | LinkedInPermissions.WriteShare);

            var loggedIn = await LinkedInService.Instance.LoginAsync();

            if (loggedIn)
            {
                var profile = await LinkedInService.Instance.GetUserProfileAsync();

                ProfileImage.DataContext = profile;
                ProfileImage.Visibility = Visibility.Visible;

                ShareBox.Visibility = Visibility.Visible;
            }
        }

        private async void ShareButton_Click(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(ShareText.Text))
            {
                return;
            }

            var response = await LinkedInService.Instance.ShareActivityAsync(ShareText.Text);

            var message = new MessageDialog("Share sent to LinkedIn");
            await message.ShowAsync();
        }

        private void ShareExpandButton_Click(object sender, RoutedEventArgs e)
        {
            if (SharePanel.Visibility == Visibility.Visible)
            {
                HideSharePanel();
            }
            else
            {
                ShowSharePanel();
            }
        }

        private void ShowSharePanel()
        {
            ShareExpandButton.Content = "";
            SharePanel.Visibility = Visibility.Visible;
        }

        private void HideSharePanel()
        {
            ShareExpandButton.Content = "";
            SharePanel.Visibility = Visibility.Collapsed;
        }
    }
}
