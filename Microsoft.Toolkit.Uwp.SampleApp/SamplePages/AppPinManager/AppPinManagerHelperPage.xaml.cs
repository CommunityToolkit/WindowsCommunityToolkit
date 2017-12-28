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
using Microsoft.Toolkit.Uwp.Helpers;
using Windows.ApplicationModel;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class AppPinManagerHelperPage : Page
    {
        public AppPinManagerHelperPage()
        {
            this.InitializeComponent();
        }

        private async void SpecificAppSMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            var appList = (await Package.Current.GetAppListEntriesAsync())[0];
            var pinResult = await AppPinManager.PinSpecificAppToStartMenuAsync(appList);
            StatusMessage.Text = "SpecificApp in StartMenu : " + pinResult.ToString();
        }

        private async void UserSpecificAppSMenuButton_OnClick(object sender, RoutedEventArgs e)
        {
            var userInfo = await User.FindAllAsync();
            if (userInfo.Count > 0)
            {
                var appList = (await Package.Current.GetAppListEntriesAsync())[0];
                var pinResult = await AppPinManager.PinUserSpecificAppToStartMenuAsync(userInfo[0], appList);
                StatusMessage.Text = "User SpecificApp in StartMenu : " + pinResult.ToString();
            }
        }

        private async void CurrentAppTBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var pinResult = await AppPinManager.PinCurrentAppToTaskBarAsync();
            StatusMessage.Text = "Current App in TaskBar : " + pinResult.ToString();
        }

        private async void SpecificAppTBarButton_OnClick(object sender, RoutedEventArgs e)
        {
            var appList = (await Package.Current.GetAppListEntriesAsync())[0];
            var pinResult = await AppPinManager.PinSpecificAppToTaskBarAsync(appList);
            StatusMessage.Text = "Specific App in TaskBar : " + pinResult.ToString();
        }
    }
}
