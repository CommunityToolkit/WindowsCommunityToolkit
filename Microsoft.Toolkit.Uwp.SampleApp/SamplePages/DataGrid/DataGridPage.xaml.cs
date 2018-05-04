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

using Microsoft.Toolkit.Uwp.SampleApp.Data;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class DataGridPage : Page, IXamlRenderListener
    {
        private DataGrid dataGrid;
        private AppBarButton groupButton;
        private MenuFlyoutItem rankLowItem;
        private MenuFlyoutItem rankHighItem;
        private MenuFlyoutItem heightLowItem;
        private MenuFlyoutItem heightHighItem;
        private DataGridDataSource viewModel = new DataGridDataSource();
        private DataGridSortDirection? previousSortDirection = null;

        public DataGridPage()
        {
            InitializeComponent();
        }

        public async void OnXamlRendered(FrameworkElement control)
        {
            dataGrid = control.FindDescendantByName("dataGrid") as DataGrid;
            dataGrid.ItemsSource = await viewModel.GetDataAsync();
            dataGrid.Sorting += DataGrid_Sorting;
            dataGrid.LoadingRowGroup += DataGrid_LoadingRowGroup;

            groupButton = control.FindDescendantByName("groupButton") as AppBarButton;
            groupButton.Click += GroupButton_Click;

            rankLowItem = control.FindName("rankLow") as MenuFlyoutItem;
            rankLowItem.Click += RankLowItem_Click;
            rankHighItem = control.FindName("rankHigh") as MenuFlyoutItem;
            rankHighItem.Click += RankHigh_Click;
            heightLowItem = control.FindName("heightLow") as MenuFlyoutItem;
            heightLowItem.Click += HeightLow_Click;
            heightHighItem = control.FindName("heightHigh") as MenuFlyoutItem;
            heightHighItem.Click += HeightHigh_Click;
        }

        private void RankLowItem_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = viewModel.FilterData(DataGridDataSource.FilterOptions.Rank_Low);
        }

        private void DataGrid_LoadingRowGroup(object sender, DataGridRowGroupHeaderEventArgs e)
        {
            ICollectionViewGroup group = e.RowGroupHeader.CollectionViewGroup;
            DataGridDataItem item = group.GroupItems[0] as DataGridDataItem;
            e.RowGroupHeader.PropertyValue = item.Range;
        }

        private void GroupButton_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = viewModel.GroupData().View;
        }

        private void DataGrid_Sorting(object sender, DataGridColumnEventArgs e)
        {
            string previousSortedColumn = viewModel.CachedSortedColumn;
            if (previousSortedColumn != string.Empty)
            {
                foreach (DataGridColumn c in dataGrid.Columns)
                {
                    if (c.Tag.ToString() == previousSortedColumn)
                    {
                        if (previousSortedColumn != e.Column.Tag.ToString())
                        {
                            c.SortDirection = null;
                            previousSortDirection = null;
                        }
                    }
                }
            }

            if ((e.Column.SortDirection == null || e.Column.SortDirection == DataGridSortDirection.Ascending)
                && previousSortDirection != DataGridSortDirection.Ascending)
            {
                dataGrid.ItemsSource = viewModel.SortData(e.Column.Tag.ToString(), true);
                e.Column.SortDirection = DataGridSortDirection.Ascending;
                previousSortDirection = DataGridSortDirection.Ascending;
            }
            else
            {
                dataGrid.ItemsSource = viewModel.SortData(e.Column.Tag.ToString(), false);
                e.Column.SortDirection = DataGridSortDirection.Descending;
                previousSortDirection = DataGridSortDirection.Descending;
            }
        }

        private void RankHigh_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = viewModel.FilterData(DataGridDataSource.FilterOptions.Rank_High);
        }

        private void HeightLow_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = viewModel.FilterData(DataGridDataSource.FilterOptions.Height_Low);
        }

        private void HeightHigh_Click(object sender, RoutedEventArgs e)
        {
            dataGrid.ItemsSource = viewModel.FilterData(DataGridDataSource.FilterOptions.Height_High);
        }
    }
}
