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
using Microsoft.Toolkit.Uwp.Services.Bing;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class BingPage
    {
        public BingPage()
        {
            InitializeComponent();
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!ConnectionHelper.IsInternetAvailable)
            {
                var dialog = new MessageDialog("Internet connection not detected. Please try again later.");
                await dialog.ShowAsync();

                return;
            }

            if (string.IsNullOrEmpty(SearchText.Text))
            {
                return;
            }

            Shell.Current.DisplayWaitRing = true;
            var searchConfig = new BingSearchConfig
            {
                Country = BingCountry.UnitedStates,
                Query = SearchText.Text
            };

            ListView.ItemsSource = await BingService.Instance.RequestAsync(searchConfig, 50);
            Shell.Current.DisplayWaitRing = false;
        }
    }
}
