// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Net.Http;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// Textbox Mask sample page
    /// </summary>
    public sealed partial class AnalyticsPage
    {
        private readonly HttpClient client;

        public AnalyticsPage()
        {
            InitializeComponent();
            client = new HttpClient();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            MarkdownTextBlockTextblock.Text = @"# Analytics


## Google analytics
*****

* [Main page](https://analytics.google.com)
* [NuGet](https://www.nuget.org/packages/UWP.SDKforGoogleAnalytics.Managed/)
* [Documentation, code and samples](https://github.com/dotnet/windows-sdk-for-google-analytics)
&nbsp;  
&nbsp;  


## HockeyApp
*****

* [Main page](http://hockeyapp.com/)
* [NuGet](https://www.nuget.org/packages/HockeySDK.UWP/)
&nbsp;  
&nbsp;  
";
        }

        private async void MarkdownTextBlockTextblock_OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", e.Link);
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
