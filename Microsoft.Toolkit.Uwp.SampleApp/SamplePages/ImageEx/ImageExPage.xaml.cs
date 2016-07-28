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
using Microsoft.Toolkit.Uwp.UI;

using Windows.UI.Xaml.Navigation;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ImageExPage
    {
        public ImageExPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            Shell.Current.RegisterNewCommand("Reset Cache", async (sender, args) =>
            {
                ImageExControl.ItemsSource = null;
                System.GC.Collect(); // Force GC to free file locks
                await ImageCache.ClearAsync();
            });

            Shell.Current.RegisterNewCommand("Reload content", (sender, args) =>
            {
                LoadData();
            });

            LoadData();
        }

        private void LoadData()
        {
            ImageExControl.ItemsSource = new Data.PhotosDataSource().GetItems(true);
        }
    }
}
