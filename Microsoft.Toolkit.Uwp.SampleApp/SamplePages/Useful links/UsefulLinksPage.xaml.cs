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
using Microsoft.Toolkit.Uwp.UI.Controls;
using Windows.System;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class UsefulLinksPage
    {
        private static HttpHelper _httpHelper = null;

        static UsefulLinksPage()
        {
            _httpHelper = new HttpHelper(1);
        }

        public UsefulLinksPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.DisplayWaitRing = true;
            try
            {
                using (var request = new HttpHelperRequest(new Uri("https://raw.githubusercontent.com/Microsoft/UWPCommunityToolkit/dev/githubresources/content/links.md")))
                {
                    using (var response = await _httpHelper.SendRequestAsync(request))
                    {
                        if (response.Success)
                        {
                            MarkdownTextBlockTextblock.Text = await response.Content.ReadAsStringAsync();
                        }
                    }
                }
            }
            catch (Exception exception)
            {
                MarkdownTextBlockTextblock.Text = "Unable to download content: " + exception.Message;
                TrackingManager.TrackException(exception);
            }

            Shell.Current.DisplayWaitRing = false;
        }

        private async void MarkdownTextBlockTextblock_OnLinkClicked(object sender, LinkClickedEventArgs e)
        {
            TrackingManager.TrackEvent("Link", e.Link);
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
