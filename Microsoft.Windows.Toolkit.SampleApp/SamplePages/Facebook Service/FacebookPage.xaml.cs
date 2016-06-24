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

using Microsoft.Windows.Toolkit.Services.Facebook;
using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.SampleApp.SamplePages
{
    public sealed partial class FacebookPage
    {
        public FacebookPage()
        {
            InitializeComponent();
        }

        private async void ConnectButton_OnClick(object sender, RoutedEventArgs e)
        {
            FacebookService.Instance.Initialize(AppIDText.Text, FacebookService.Instance.DevelopmentTimeWindowsStoreId);
            await FacebookService.Instance.LoginAsync();

            ListView.ItemsSource = await FacebookService.Instance.RequestAsync(FacebookDataConfig.MyPosts, 50);
        }
    }
}
