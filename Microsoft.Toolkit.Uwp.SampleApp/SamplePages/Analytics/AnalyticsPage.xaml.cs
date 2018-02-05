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
