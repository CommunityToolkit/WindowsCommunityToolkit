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

using Microsoft.Toolkit.Uwp.UI.Controls.Design.Common;
using Microsoft.Windows.Design;
//using Microsoft.Windows.Design.Features;
using Microsoft.Windows.Design.Metadata;
//using Microsoft.Windows.Design.Model;
using System.ComponentModel;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Design
{
    //internal class DataGridDefaults : DefaultInitializer
    //{
    //    public override void InitializeDefaults(ModelItem item)
    //    {
    //        item.Properties[nameof(DataGrid.MyProperty)].SetValue(<value other than runtime default>);
    //    }
    //}

    internal class DataGridMetadata : AttributeTableBuilder
    {
        public DataGridMetadata() : base()
        {
            AddCallback(typeof(DataGrid),
                b =>
                {
                    //b.AddCustomAttributes(new FeatureAttribute(typeof(DataGridDefaults)));
                    b.AddCustomAttributes(nameof(DataGrid.AlternatingRowBackground), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.AreRowDetailsFrozen), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.AreRowGroupHeadersFrozen), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.AutoGenerateColumns), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.CanUserReorderColumns), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.CanUserResizeColumns), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.CanUserSortColumns), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.CellStyle), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.ClipboardCopyMode), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGrid.ColumnHeaderHeight), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.ColumnHeaderStyle), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.Columns), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.ColumnWidth), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.CurrentColumn), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGrid.DragIndicatorStyle), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGrid.DropLocationIndicatorStyle), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGrid.FrozenColumnCount), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.GridLinesVisibility), new CategoryAttribute(Properties.Resources.CategoryGridLines));
                    b.AddCustomAttributes(nameof(DataGrid.HeadersVisibility), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.HorizontalGridLinesBrush), new CategoryAttribute(Properties.Resources.CategoryGridLines));
                    b.AddCustomAttributes(nameof(DataGrid.HorizontalScrollBarVisibility), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(nameof(DataGrid.IsReadOnly), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGrid.IsValid), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGrid.ItemsSource), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.MaxColumnWidth), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.MinColumnWidth), new CategoryAttribute(Properties.Resources.CategoryColumns));
                    b.AddCustomAttributes(nameof(DataGrid.RowBackground), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.RowDetailsTemplate), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.RowDetailsVisibilityMode), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.RowGroupHeaderPropertyNameAlternative), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.RowGroupHeaderStyles), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.RowHeaderStyle), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.RowHeaderWidth), new CategoryAttribute(Properties.Resources.CategoryHeaders));
                    b.AddCustomAttributes(nameof(DataGrid.RowHeight), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.RowStyle), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.SelectedIndex), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(DataGrid.SelectedItem), new CategoryAttribute(Properties.Resources.CategoryCommon));
                    b.AddCustomAttributes(nameof(DataGrid.SelectedItems), new CategoryAttribute(Properties.Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(DataGrid.SelectionMode), new CategoryAttribute(Properties.Resources.CategoryRows));
                    b.AddCustomAttributes(nameof(DataGrid.VerticalGridLinesBrush), new CategoryAttribute(Properties.Resources.CategoryGridLines));
                    b.AddCustomAttributes(nameof(DataGrid.VerticalScrollBarVisibility), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(new ToolboxCategoryAttribute(ToolboxCategoryPaths.Toolkit, false));
                });

            AddCallback(typeof(DataGridColumn),
                b =>
                {
                    b.AddCustomAttributes(nameof(DataGridColumn.CanUserResize), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(nameof(DataGridColumn.CanUserSort), new CategoryAttribute(Properties.Resources.CategorySort));
                    b.AddCustomAttributes(nameof(DataGridColumn.Header), new CategoryAttribute(Properties.Resources.CategoryHeader));
                    b.AddCustomAttributes(nameof(DataGridColumn.HeaderStyle), new CategoryAttribute(Properties.Resources.CategoryHeader));
                    b.AddCustomAttributes(nameof(DataGridColumn.MaxWidth), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(nameof(DataGridColumn.MinWidth), new CategoryAttribute(Properties.Resources.CategoryLayout));
                    b.AddCustomAttributes(nameof(DataGridColumn.SortDirection), new CategoryAttribute(Properties.Resources.CategorySort));
                    b.AddCustomAttributes(nameof(DataGridColumn.Visibility), new CategoryAttribute(Properties.Resources.CategoryAppearance));
                    b.AddCustomAttributes(nameof(DataGridColumn.Width), new CategoryAttribute(Properties.Resources.CategoryLayout));
                });

            AddCallback(typeof(DataGridBoundColumn),
                b =>
                {
                    b.AddCustomAttributes(nameof(DataGridBoundColumn.Binding), new CategoryAttribute(Properties.Resources.CategoryCellBinding));
                });

            AddCallback(typeof(DataGridTextColumn),
                b =>
                {
                    b.AddCustomAttributes(nameof(DataGridTextColumn.FontFamily), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGridTextColumn.FontSize), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGridTextColumn.FontStyle), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGridTextColumn.FontWeight), new CategoryAttribute(Properties.Resources.CategoryText));
                    b.AddCustomAttributes(nameof(DataGridTextColumn.Foreground), new CategoryAttribute(Properties.Resources.CategoryText));
                });

            AddCallback(typeof(DataGridCheckBoxColumn),
                b =>
                {
                    b.AddCustomAttributes(nameof(DataGridCheckBoxColumn.IsThreeState), new CategoryAttribute(Properties.Resources.CategoryCommon));
                });

            AddCallback(typeof(DataGridTemplateColumn),
                b =>
                {
                    b.AddCustomAttributes(nameof(DataGridTemplateColumn.CellEditingTemplate), new CategoryAttribute(Properties.Resources.CategoryCellTemplate));
                    b.AddCustomAttributes(nameof(DataGridTemplateColumn.CellTemplate), new CategoryAttribute(Properties.Resources.CategoryCellTemplate));
                });
        }
    }
}
