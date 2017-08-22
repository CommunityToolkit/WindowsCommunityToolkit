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
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class LoadingPage : IXamlRenderListener
    {
        private AdaptiveGridView adaptiveGridViewControl;
        private Loading loadingControl;
        private ContentControl loadingContentControl;
        private ResourceDictionary resources;

        public LoadingPage()
        {
            InitializeComponent();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            adaptiveGridViewControl = control.FindChildByName("AdaptiveGridViewControl") as AdaptiveGridView;
            loadingControl = control.FindDescendantByName("LoadingControl") as Loading;
            loadingContentControl = control.FindChildByName("LoadingContentControl") as ContentControl;
            resources = control.Resources;

            if (adaptiveGridViewControl != null)
            {
                adaptiveGridViewControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
            }
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Loading control with wait ring", async (sender, args) =>
            {
                if (loadingContentControl != null)
                {
                    loadingContentControl.ContentTemplate = resources["WaitListTemplate"] as DataTemplate;
                    await ShowLoadingDialogAsync();
                }
            });

            Shell.Current.RegisterNewCommand("Loading control with progressbar", async (sender, args) =>
            {
                if (loadingContentControl != null)
                {
                    loadingContentControl.ContentTemplate = resources["ProgressBarTemplate"] as DataTemplate;
                    await ShowLoadingDialogAsync();
                }
            });

            Shell.Current.RegisterNewCommand("Loading control with logo and bluring when requested", async (sender, args) =>
            {
                if (loadingContentControl != null)
                {
                    loadingContentControl.ContentTemplate = resources["LogoTemplate"] as DataTemplate;
                    await loadingContentControl.Blur(2, 100).StartAsync();
                    await ShowLoadingDialogAsync();
                    await loadingContentControl.Blur(0, 0).StartAsync();
                }
            });
        }

        private async Task ShowLoadingDialogAsync()
        {
            loadingControl.IsLoading = true;
            await Task.Delay(3000);
            loadingControl.IsLoading = false;
        }
    }
}
