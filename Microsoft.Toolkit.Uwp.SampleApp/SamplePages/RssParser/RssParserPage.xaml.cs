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
                catch { }
            }

            RSSList.SelectedItem = null;
        }
    }
}