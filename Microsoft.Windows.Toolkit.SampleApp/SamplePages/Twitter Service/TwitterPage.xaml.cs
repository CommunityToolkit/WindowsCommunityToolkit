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
using Microsoft.Windows.Toolkit.Services.Facebook;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class TwitterPage
    {
        public TwitterPage()
        {
            InitializeComponent();

            QueryType.ItemsSource = new[] { "My feed", "My posts", "My tagged" };
            QueryType.SelectedIndex = 0;

            ShareBox.Visibility = Visibility.Collapsed;
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            FacebookService.Instance.Initialize(AppIDText.Text, FacebookService.Instance.WindowsStoreId);
            if (!await FacebookService.Instance.LoginAsync())
            {
                ShareBox.Visibility = Visibility.Collapsed;
                var error = new MessageDialog("Unable to log with Facebook");
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

            ListView.ItemsSource = await FacebookService.Instance.RequestAsync(config, 50);

            ShareBox.Visibility = Visibility.Visible;

            ProfileImage.DataContext = await FacebookService.Instance.GetUserPictureInfoAsync();
        }

        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            await FacebookService.Instance.PostToFeedAsync(TitleText.Text, DescriptionText.Text, "http://www.github.com/microsoft/uwptoolkit");
        }
    }
}
