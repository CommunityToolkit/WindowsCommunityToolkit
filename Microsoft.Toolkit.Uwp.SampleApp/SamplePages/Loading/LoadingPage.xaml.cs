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

using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Microsoft.Toolkit.Uwp.UI.Animations;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class LoadingPage
    {
        public LoadingPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            AdaptiveGridViewControl.ItemsSource = await new Data.PhotosDataSource().GetItemsAsync();

            Shell.Current.RegisterNewCommand("Loading control with wait ring", async (sender, args) =>
            {
                LoadingContentControl.ContentTemplate = Resources["WaitListTemplate"] as DataTemplate;
                await ShowLoadingDialogAsync();
            });

            Shell.Current.RegisterNewCommand("Loading control with progressbar", async (sender, args) =>
            {
                LoadingContentControl.ContentTemplate = Resources["ProgressBarTemplate"] as DataTemplate;
                await ShowLoadingDialogAsync();
            });

            Shell.Current.RegisterNewCommand("Loading control with logo and bluring when requested", async (sender, args) =>
            {
                LoadingContentControl.ContentTemplate = Resources["LogoTemplate"] as DataTemplate;
                await LoadingContentControl.Blur(10, 100).StartAsync();
                await ShowLoadingDialogAsync();
            });

            base.OnNavigatedTo(e);
        }

        private async Task ShowLoadingDialogAsync()
        {
            LoadingControl.IsLoading = true;
            await Task.Delay(3000);
            LoadingControl.IsLoading = false;
        }
    }
}
