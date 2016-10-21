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
using System.Linq;
using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.SampleApp.Models;
using Windows.UI.Popups;
using Windows.UI.Xaml.Navigation;
using Windows.UI.Xaml.Data;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class AdaptiveGridViewPage
    {
        public AdaptiveGridViewPage()
        {
            InitializeComponent();
        }

        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            var propertyDesc = e.Parameter as PropertyDescriptor;

            if (propertyDesc != null)
            {
                DataContext = propertyDesc.Expando;
            }

            await GetItemsForDisplay();

            AdaptiveGridViewControl.ItemClick += AdaptiveGridViewControl_ItemClick;
            AdaptiveGridViewControl.SelectionChanged += AdaptiveGridViewControl_SelectionChanged;
        }

        private void AdaptiveGridViewControl_SelectionChanged(object sender, Windows.UI.Xaml.Controls.SelectionChangedEventArgs e)
        {
            SelectedItemCountTextBlock.Text = AdaptiveGridViewControl.SelectedItems.Any()
                ? $"You have selected {AdaptiveGridViewControl.SelectedItems.Count} items."
                : "You haven't selected any items";
        }

        private async void AdaptiveGridViewControl_ItemClick(object sender, Windows.UI.Xaml.Controls.ItemClickEventArgs e)
        {
            if (e.ClickedItem != null)
            {
                await new MessageDialog($"You clicked {(e.ClickedItem as PhotoDataItem).Title}", "Item Clicked").ShowAsync();
            }
        }

        private async void CheckBox_Checked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await GetItemsForDisplay();
        }

        private async Task GetItemsForDisplay()
        {
            if (ShowGroupedItems.IsChecked.GetValueOrDefault())
            {
                ItemsViewSource.IsSourceGrouped = true;
                ItemsViewSource.Source = await new Data.PhotosDataSource().GetGroupedItemsAsync();

            }
            else
            {
                ItemsViewSource.IsSourceGrouped = false;
                ItemsViewSource.Source = await new Data.PhotosDataSource().GetItemsAsync();
            }
        }

        private async void CheckBox_Unchecked(object sender, Windows.UI.Xaml.RoutedEventArgs e)
        {
            await GetItemsForDisplay();
        }
    }
}
