// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.ObjectModel;
using System.Net.Http;
using Microsoft.Toolkit.Parsers.Rss;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class RssParserPage : Page
    {
        public ObservableCollection<RssSchema> RSSFeed { get; } = new ObservableCollection<RssSchema>();

        public RssParserPage()
        {
            this.InitializeComponent();
            ParseRSS();
        }

        public string Url { get; set; } = "https://visualstudiomagazine.com/rss-feeds/news.aspx";

        public async void ParseRSS()
        {
            string feed = null;
            RSSFeed.Clear();

            using (var client = new HttpClient())
            {
                try
                {
                    feed = await client.GetStringAsync(Url);
                }
                catch
                {
                }
            }

            if (feed != null)
            {
                var parser = new RssParser();
                var rss = parser.Parse(feed);

                foreach (var element in rss)
                {
                    RSSFeed.Add(element);
                }
            }
        }

        private async void RSSList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RSSList.SelectedItem is RssSchema rssItem)
            {
                try
                {
                    await Launcher.LaunchUriAsync(new Uri(rssItem.FeedUrl));
                }
                catch
                {
                }
            }

            RSSList.SelectedItem = null;
        }
    }
}