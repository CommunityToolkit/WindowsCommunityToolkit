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
