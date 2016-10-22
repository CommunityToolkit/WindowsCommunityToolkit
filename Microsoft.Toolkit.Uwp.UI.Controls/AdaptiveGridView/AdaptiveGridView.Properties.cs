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

using System.Collections.Generic;
using System.Windows.Input;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The AdaptiveGridView control allows to present information within a Grid View perfectly adjusting the
    /// total display available space. It reacts to changes in the layout as well as the content so it can adapt
    /// to different form factors automatically.
    /// </summary>
    /// <remarks>
    /// The number and the width of items are calculated based on the
    /// screen resolution in order to fully leverage the available screen space. The property ItemsHeight define
    /// the items fixed height and the property DesiredWidth sets the minimum width for the elements to add a
    /// new column.</remarks>
    public partial class AdaptiveGridView
    {
        /// <summary>
        /// Identifies the <see cref="SelectedIndex"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register(nameof(SelectedIndex), typeof(int), typeof(AdaptiveGridView), new PropertyMetadata(-1));

        /// <summary>
        /// Identifies the <see cref="SelectedItem"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemProperty =
            DependencyProperty.Register(nameof(SelectedItem), typeof(object), typeof(AdaptiveGridView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SelectedItems"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectedItemsProperty =
            DependencyProperty.Register(nameof(SelectedItems), typeof(IList<object>), typeof(AdaptiveGridView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="SelectionMode"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty SelectionModeProperty =
            DependencyProperty.Register(nameof(SelectionMode), typeof(ListViewSelectionMode), typeof(AdaptiveGridView), new PropertyMetadata(ListViewSelectionMode.None));

        /// <summary>
        /// Identifies the <see cref="IsItemClickEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty IsItemClickEnabledProperty =
            DependencyProperty.Register(nameof(IsItemClickEnabled), typeof(bool), typeof(AdaptiveGridView), new PropertyMetadata(true));

        /// <summary>
        /// Identifies the <see cref="ItemClickCommand"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemClickCommandProperty =
            DependencyProperty.Register(nameof(ItemClickCommand), typeof(ICommand), typeof(AdaptiveGridView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty =
            DependencyProperty.Register(nameof(ItemsSource), typeof(object), typeof(AdaptiveGridView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty =
            DependencyProperty.Register(nameof(ItemTemplate), typeof(DataTemplate), typeof(AdaptiveGridView), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="ItemHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemHeightProperty =
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the <see cref="OneRowModeEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OneRowModeEnabledProperty =
            DependencyProperty.Register(nameof(OneRowModeEnabled), typeof(bool), typeof(AdaptiveGridView), new PropertyMetadata(false, (o, e) => { OnOneRowModeEnabledChanged(o, e.NewValue); }));

        /// <summary>
        /// Identifies the <see cref="ItemWidth"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(double.NaN));

        /// <summary>
        /// Identifies the <see cref="DesiredWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.Register(nameof(DesiredWidth), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(double.NaN, DesiredWidthChanged));

        private static void OnOneRowModeEnabledChanged(DependencyObject d, object newValue)
        {
            var self = d as AdaptiveGridView;

            if ((bool)newValue)
            {
                if (self._isInitialized)
                {
                    var b = new Binding()
                    {
                        Source = self,
                        Path = new PropertyPath("ItemHeight")
                    };

                    self._listView.SetBinding(GridView.MaxHeightProperty, b);
                    ScrollViewer.SetVerticalScrollMode(self, ScrollMode.Disabled);
                    ScrollViewer.SetVerticalScrollBarVisibility(self, ScrollBarVisibility.Hidden);
                }
            }
        }

        private static void DesiredWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as AdaptiveGridView;
            if (self._isInitialized)
            {
                self.RecalculateLayout(self._listView.ActualWidth);
            }
        }

        /// <summary>
        /// Gets or sets the index of the selected object.
        /// </summary>
        /// <value>The index of the selected item in the collection. Default is -1 when initialized.</value>
        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        /// <summary>
        /// Gets or sets the desired width of each item
        /// </summary>
        /// <value>The width of the desired.</value>
        public double DesiredWidth
        {
            get { return (double)GetValue(DesiredWidthProperty); }
            set { SetValue(DesiredWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets the command to execute when an item is clicked.
        /// </summary>
        /// <value>The item click command.</value>
        public ICommand ItemClickCommand
        {
            get { return (ICommand)GetValue(ItemClickCommandProperty); }
            set { SetValue(ItemClickCommandProperty, value); }
        }

        /// <summary>
        /// Gets the selected multiple objects in the collection.
        /// </summary>
        /// <value>The object that is used to store selected multiple items.</value>
        public IList<object> SelectedItems => _listView.SelectedItems;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether the items are clickable or not.
        /// </summary>
        /// <value>Default is false.</value>
        public bool IsItemClickEnabled
        {
            get { return (bool)GetValue(IsItemClickEnabledProperty); }
            set { SetValue(IsItemClickEnabledProperty, value); }
        }

        /// <summary>
        /// Gets or sets the SelectionMode property of ListViewBase.
        /// </summary>
        /// <value>Default is None.</value>
        public ListViewSelectionMode SelectionMode
        {
            get { return (ListViewSelectionMode)GetValue(SelectionModeProperty); }
            set { SetValue(SelectionModeProperty, value); }
        }

        /// <summary>
        /// Gets or sets the height of each item in the grid.
        /// </summary>
        /// <value>The height of the item.</value>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets the single selected object when the SelectionMode is set to Single.
        /// </summary>
        /// <value>Stores the single selected item. Default is null.</value>
        public object SelectedItem
        {
            get { return (object)GetValue(SelectedItemProperty); }
            set { SetValue(SelectedItemProperty, value); }
        }

        /// <summary>
        /// Gets or sets an object source used to generate the content of the grid.
        /// </summary>
        /// <value>The object that is used to generate the content of the ItemsControl. The default is null</value>
        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each item.
        /// </summary>
        /// <value>The template that specifies the visualization of the data objects. The default is null.</value>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether only one row should be displayed.
        /// </summary>
        /// <value><c>true</c> if only one row is displayed; otherwise, <c>false</c>.</value>
        public bool OneRowModeEnabled
        {
            get { return (bool)GetValue(OneRowModeEnabledProperty); }
            set { SetValue(OneRowModeEnabledProperty, value); }
        }

        /// <summary>
        /// Event raised when an item is clicked
        /// </summary>
        public event ItemClickEventHandler ItemClick;

        /// <summary>
        /// Event raised when an item is added or removed to/from the collection.
        /// </summary>
        public event SelectionChangedEventHandler SelectionChanged;

        private double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the AdaptiveGridView instance is the active view in its owning SemanticZoom.
        /// </summary>
        public bool IsActiveView { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the AdaptiveGridView instance is the zoomed-in view in its owning SemanticZoom.
        /// </summary>
        public bool IsZoomedInView { get; set; }

        /// <summary>
        /// Gets or sets the SemanticZoom instance that hosts the AdaptiveGridView.
        /// </summary>
        public SemanticZoom SemanticZoomOwner { get; set; }

        private static int CalculateColumns(double containerWidth, double itemWidth)
        {
            var columns = (int)(containerWidth / itemWidth);
            if (columns == 0)
            {
                columns = 1;
            }

            return columns;
        }
    }
}
