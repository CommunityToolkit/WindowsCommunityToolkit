// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.UI.Animations;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class LoadingPage : IXamlRenderListener
    {
        private ScrollViewer scrollViewerControl;
        private AdaptiveGridView adaptiveGridViewControl;
        private Loading loadingControl;
        private ContentControl loadingContentControl;
        private ResourceDictionary resources;

        public LoadingPage()
        {
            InitializeComponent();
            Load();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            scrollViewerControl = control.FindChildByName("ScrollViewerControl") as ScrollViewer;
            adaptiveGridViewControl = control.FindChildByName("AdaptiveGridViewControl") as AdaptiveGridView;
            loadingControl = control.FindDescendantByName("LoadingControl") as Loading;
            loadingContentControl = control.FindChildByName("LoadingContentControl") as ContentControl;
            resources = control.Resources;

            if (adaptiveGridViewControl != null)
            {
                adaptiveGridViewControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();
            }
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand("Loading control with wait ring", async (sender, args) =>
            {
                if (loadingContentControl != null)
                {
                    loadingContentControl.ContentTemplate = resources["WaitListTemplate"] as DataTemplate;
                    await ShowLoadingDialogAsync();
                }
            });

            SampleController.Current.RegisterNewCommand("Loading control with progressbar", async (sender, args) =>
            {
                if (loadingContentControl != null)
                {
                    loadingContentControl.ContentTemplate = resources["ProgressBarTemplate"] as DataTemplate;
                    await ShowLoadingDialogAsync();
                }
            });

            SampleController.Current.RegisterNewCommand("Loading control with logo and blurring when requested", async (sender, args) =>
            {
                if (loadingContentControl != null)
                {
                    loadingContentControl.ContentTemplate = resources["LogoTemplate"] as DataTemplate;

                    // We need to blur the ScrollViewer hosting the main content of the page and not the
                    // root control, otherwise the popup with the icon would be blurred as well, as it
                    // would be in the same visual tree as the control whose visual is being blurred.
                    await scrollViewerControl.Blur(2, 100).StartAsync();
                    await ShowLoadingDialogAsync();
                    await scrollViewerControl.Blur(0, 0).StartAsync();
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
