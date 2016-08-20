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

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    using System;
    using System.Windows.Input;

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
    public sealed partial class AdaptiveGridView
    {
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
            DependencyProperty.Register(nameof(ItemHeight), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(0D));

        /// <summary>
        /// Identifies the <see cref="OneRowModeEnabled"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OneRowModeEnabledProperty =
            DependencyProperty.Register(nameof(OneRowModeEnabled), typeof(bool), typeof(AdaptiveGridView), new PropertyMetadata(false, (o, e) => { OnOneRowModeEnabledChanged(o, e.NewValue); }));

        /// <summary>
        /// Identifies the <see cref="VerticalScrollMode"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty VerticalScrollModeProperty =
            DependencyProperty.Register(nameof(VerticalScrollMode), typeof(ScrollMode), typeof(AdaptiveGridView), new PropertyMetadata(ScrollMode.Auto));

        /// <summary>
        /// Identifies the <see cref="ItemWidth"/> dependency property.
        /// </summary>
        private static readonly DependencyProperty ItemWidthProperty =
            DependencyProperty.Register(nameof(ItemWidth), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(0D));

        /// <summary>
        /// Identifies the <see cref="DesiredWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty DesiredWidthProperty =
            DependencyProperty.Register(nameof(DesiredWidth), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(0D, DesiredWidthChanged));

        /// <summary>
        /// Identifies the <see cref="ItemAspectRatio"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty ItemAspectRatioProperty =
            DependencyProperty.Register(nameof(ItemAspectRatio), typeof(double), typeof(AdaptiveGridView), new PropertyMetadata(0D, ItemAspectRatioChanged));

        private static void OnOneRowModeEnabledChanged(DependencyObject d, object newValue)
        {
            var self = d as AdaptiveGridView;

            if (self._isInitialized && self._listView != null)
            {
                var oneRowMode = (bool)newValue;

                if (oneRowMode)
                {
                    var b = new Binding()
                    {
                        Source = self,
                        Path = new PropertyPath(nameof(ItemHeight))
                    };

                    self._listView.SetBinding(GridView.MaxHeightProperty, b);
                    ScrollViewer.SetVerticalScrollMode(self._listView, ScrollMode.Disabled);

                    // Scroll to the top of the viewport
                    var scroller = self._listView.FindDescendant<ScrollViewer>();
                    scroller?.ChangeView(horizontalOffset: null, verticalOffset: 0, zoomFactor: null, disableAnimation: true);
                }
                else
                {
                    var b = new Binding
                    {
                        Source = self,
                        Path = new PropertyPath(nameof(VerticalScrollMode))
                    };

                    self._listView.ClearValue(GridView.MaxHeightProperty);
                    self._listView.SetBinding(ScrollViewer.VerticalScrollModeProperty, b);
                }
            }
        }

        private static void DesiredWidthChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as AdaptiveGridView;
            if (self._isInitialized && self._listView != null)
            {
                self.RecalculateLayout(self._listView.ActualWidth);
            }
        }

        private static void ItemAspectRatioChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var self = d as AdaptiveGridView;
            if (self._isInitialized && self._listView != null && self._templateProxy != null)
            {
                var newRatio = (double)e.NewValue;
                if (newRatio == 0)
                {
                    var b = new Binding()
                    {
                        Source = self,
                        Path = new PropertyPath(nameof(ItemHeight))
                    };

                    self._templateProxy.SetBinding(FrameworkElement.HeightProperty, b);
                }
                else
                {
                    self._templateProxy.ClearValue(FrameworkElement.HeightProperty);
                }

                self.RecalculateLayout(self._listView.ActualWidth);
            }
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
        /// Gets or sets the desired aspect ratio of each item
        /// </summary>
        /// <value>The ratio of item width divided by item height, or 0 for no fixed aspect ratio.</value>
        /// <example>Use an aspect ratio of 1.0 to make square items.</example>
        /// <example>Use the Golden Ratio of 1.618 to make items that are wider than they are tall.</example>
        /// <example>Use an aspect ratio of 0 to set your own value in <c ref="ItemHeight"/>.</example>
        public double ItemAspectRatio
        {
            get { return (double)GetValue(ItemAspectRatioProperty); }
            set { SetValue(ItemAspectRatioProperty, value); }
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
        /// Gets or sets the height of each item in the grid.
        /// </summary>
        /// <value>The height of the item.</value>
        public double ItemHeight
        {
            get { return (double)GetValue(ItemHeightProperty); }
            set { SetValue(ItemHeightProperty, value); }
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

        private ScrollMode VerticalScrollMode
        {
            get { return (ScrollMode)GetValue(VerticalScrollModeProperty); }
            set { SetValue(VerticalScrollModeProperty, value); }
        }

        private double ItemWidth
        {
            get { return (double)GetValue(ItemWidthProperty); }
            set { SetValue(ItemWidthProperty, value); }
        }

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
