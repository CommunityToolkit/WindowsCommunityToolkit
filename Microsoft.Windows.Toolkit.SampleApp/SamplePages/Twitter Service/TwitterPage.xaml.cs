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
using Microsoft.Windows.Toolkit.Services.Twitter;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class TwitterPage
    {
        public TwitterPage()
        {
            InitializeComponent();

            ShareBox.Visibility = Visibility.Collapsed;
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            Shell.Current.DisplayWaitRing = true;
            TwitterService.Instance.Initialize(ConsumerKey.Text, ConsumerSecret.Text, CallbackUri.Text);
            if (!await TwitterService.Instance.LoginAsync())
            {
                ShareBox.Visibility = Visibility.Collapsed;
                Shell.Current.DisplayWaitRing = false;
                var error = new MessageDialog("Unable to log to Twitter");
                await error.ShowAsync();
                return;
            }

            ListView.ItemsSource = await TwitterService.Instance.GetUserTimeLineAsync("deltakosh", 50);

            ShareBox.Visibility = Visibility.Visible;

            var user = await TwitterService.Instance.GetUserAsync();
            ProfileImage.DataContext = user;
            Shell.Current.DisplayWaitRing = false;
        }

        private async void ShareButton_OnClick(object sender, RoutedEventArgs e)
        {
            await TwitterService.Instance.TweetStatusAsync(TweetText.Text);
        }
    }
}
