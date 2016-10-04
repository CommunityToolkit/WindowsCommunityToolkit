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
using System.Linq;
using Microsoft.Toolkit.Uwp.Services.Bing;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class BingPage
    {
        public BingPage()
        {
            InitializeComponent();

            QueryType.ItemsSource = new[] { "Bing Search", "Bing News" };
            QueryType.SelectedIndex = 0;
            Countries.ItemsSource = Enum.GetValues(typeof(BingCountry)).Cast<BingCountry>().ToList();
            Countries.SelectedItem = BingCountry.UnitedStates;
            Languages.ItemsSource = Enum.GetValues(typeof(BingLanguage)).Cast<BingLanguage>().ToList();
            Languages.SelectedItem = BingLanguage.English;
        }

        private async void SearchButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!await Tools.CheckInternetConnectionAsync())
            {
                return;
            }

            if (string.IsNullOrEmpty(SearchText.Text))
            {
                return;
            }

            BingCountry country = (BingCountry)(Countries?.SelectedItem ?? BingCountry.UnitedStates);
            BingLanguage language = (BingLanguage)(Languages?.SelectedItem ?? BingLanguage.English);

            BingQueryType queryType;
            switch (QueryType.SelectedIndex)
            {
                case 1:
                    queryType = BingQueryType.News;
                    break;
                default:
                    queryType = BingQueryType.Search;
                    break;
            }

            Shell.Current.DisplayWaitRing = true;
            var searchConfig = new BingSearchConfig
            {
                Country = country,
                Language = language,
                Query = SearchText.Text,
                QueryType = queryType
            };

            ListView.ItemsSource = await BingService.Instance.RequestAsync(searchConfig, 50);
            Shell.Current.DisplayWaitRing = false;
        }
    }
}
