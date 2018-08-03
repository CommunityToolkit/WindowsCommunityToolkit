// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Services.Bing;
using Windows.UI.Core;
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
            CntryList.ItemsSource = Enum.GetValues(typeof(BingCountry)).Cast<BingCountry>().ToList();
            CntryList.SelectedItem = BingCountry.UnitedStates;
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

            BingCountry country = (BingCountry)(CntryList?.SelectedItem ?? BingCountry.UnitedStates);
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

            var searchConfig = new BingSearchConfig
            {
                Country = country,
                Language = language,
                Query = SearchText.Text,
                QueryType = queryType
            };

            // Gets an instance of BingService that is able to load search results incrementally.
            var collection = BingService.GetAsIncrementalLoading(searchConfig, 50);
            collection.OnStartLoading = async () => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SampleController.Current.DisplayWaitRing = true; });
            collection.OnEndLoading = async () => await Dispatcher.RunAsync(CoreDispatcherPriority.Normal, () => { SampleController.Current.DisplayWaitRing = false; });

            ListView.ItemsSource = collection;
        }
    }
}
