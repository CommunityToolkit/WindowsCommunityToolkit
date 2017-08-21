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
using Windows.Foundation.Collections;
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
    public partial class AdaptiveGridView : GridView
    {
        private bool _isLoaded;
        private ScrollMode _savedVerticalScrollMode;
        private ScrollMode _savedHorizontalScrollMode;
        private ScrollBarVisibility _savedVerticalScrollBarVisibility;
        private ScrollBarVisibility _savedHorizontalScrollBarVisibility;
        private Orientation _savedOrientation;
        private bool _needToRestoreScrollStates;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveGridView"/> class.
        /// </summary>
        public AdaptiveGridView()
        {
            IsTabStop = false;
            SizeChanged += OnSizeChanged;
            ItemClick += OnItemClick;
            Items.VectorChanged += ItemsOnVectorChanged;
            Loaded += OnLoaded;
            Unloaded += OnUnloaded;

            // Define ItemContainerStyle in code rather than using the DefaultStyle
            // to avoid having to define the entire style of a GridView. This can still
            // be set by the enduser to values of their chosing
            var style = new Style(typeof(GridViewItem));
            style.Setters.Add(new Setter(HorizontalContentAlignmentProperty, HorizontalAlignment.Stretch));
            style.Setters.Add(new Setter(VerticalContentAlignmentProperty, VerticalAlignment.Stretch));
            ItemContainerStyle = style;
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="obj">The element that's used to display the specified item.</param>
        /// <param name="item">The item to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject obj, object item)
        {
            base.PrepareContainerForItemOverride(obj, item);
            var element = obj as FrameworkElement;
            if (element != null)
            {
                var heightBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ItemHeight"),
                    Mode = BindingMode.TwoWay
                };

                var widthBinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath("ItemWidth"),
                    Mode = BindingMode.TwoWay
                };

                element.SetBinding(HeightProperty, heightBinding);
                element.SetBinding(WidthProperty, widthBinding);
            }
        }

        /// <summary>
        /// Calculates the width of the grid items.
        /// </summary>
        /// <param name="containerWidth">The width of the container control.</param>
        /// <returns>The calculated item width.</returns>
        protected virtual double CalculateItemWidth(double containerWidth)
        {
            double desiredWidth = double.IsNaN(DesiredWidth) ? containerWidth : DesiredWidth;

            var columns = CalculateColumns(containerWidth, desiredWidth);

            // If there's less items than there's columns, reduce the column count (if requested);
            if (Items != null && Items.Count > 0 && Items.Count < columns && StretchContentForSingleRow)
            {
                columns = Items.Count;
            }

            return (containerWidth / columns) - 5;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            OnOneRowModeEnabledChanged(this, OneRowModeEnabled);
        }

        private void ItemsOnVectorChanged(IObservableVector<object> sender, IVectorChangedEventArgs @event)
        {
            if (!double.IsNaN(ActualWidth))
            {
                // If the item count changes, check if more or less columns needs to be rendered,
                // in case we were having fewer items than columns.
                RecalculateLayout(ActualWidth);
            }
        }

        private void OnItemClick(object sender, ItemClickEventArgs e)
        {
            var cmd = ItemClickCommand;
            if (cmd != null)
            {
                if (cmd.CanExecute(e.ClickedItem))
                {
                    cmd.Execute(e.ClickedItem);
                }
            }
        }

        private void OnSizeChanged(object sender, SizeChangedEventArgs e)
        {
            // If the width of the internal list view changes, check if more or less columns needs to be rendered.
            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                RecalculateLayout(e.NewSize.Width);
            }
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = true;
            DetermineOneRowMode();
        }

        private void OnUnloaded(object sender, RoutedEventArgs e)
        {
            _isLoaded = false;
        }

        private void DetermineOneRowMode()
        {
            if (_isLoaded)
            {
                var itemsWrapGridPanel = ItemsPanelRoot as ItemsWrapGrid;

                if (OneRowModeEnabled)
                {
                    var b = new Binding()
                    {
                        Source = this,
                        Path = new PropertyPath("ItemHeight")
                    };

                    if (itemsWrapGridPanel != null)
                    {
                        _savedOrientation = itemsWrapGridPanel.Orientation;
                        itemsWrapGridPanel.Orientation = Orientation.Vertical;
                    }

                    SetBinding(MaxHeightProperty, b);

                    _savedHorizontalScrollMode = ScrollViewer.GetHorizontalScrollMode(this);
                    _savedVerticalScrollMode = ScrollViewer.GetVerticalScrollMode(this);
                    _savedHorizontalScrollBarVisibility = ScrollViewer.GetHorizontalScrollBarVisibility(this);
                    _savedVerticalScrollBarVisibility = ScrollViewer.GetVerticalScrollBarVisibility(this);
                    _needToRestoreScrollStates = true;

                    ScrollViewer.SetVerticalScrollMode(this, ScrollMode.Disabled);
                    ScrollViewer.SetVerticalScrollBarVisibility(this, ScrollBarVisibility.Hidden);
                    ScrollViewer.SetHorizontalScrollBarVisibility(this, ScrollBarVisibility.Visible);
                    ScrollViewer.SetHorizontalScrollMode(this, ScrollMode.Enabled);
                }
                else
                {
                    ClearValue(MaxHeightProperty);

                    if (!_needToRestoreScrollStates)
                    {
                        return;
                    }

                    _needToRestoreScrollStates = false;

                    if (itemsWrapGridPanel != null)
                    {
                        itemsWrapGridPanel.Orientation = _savedOrientation;
                    }

                    ScrollViewer.SetVerticalScrollMode(this, _savedVerticalScrollMode);
                    ScrollViewer.SetVerticalScrollBarVisibility(this, _savedVerticalScrollBarVisibility);
                    ScrollViewer.SetHorizontalScrollBarVisibility(this, _savedHorizontalScrollBarVisibility);
                    ScrollViewer.SetHorizontalScrollMode(this, _savedHorizontalScrollMode);
                }
            }
        }

        private void RecalculateLayout(double containerWidth)
        {
            var itemsPanel = ItemsPanelRoot as Panel;
            var panelMargin = itemsPanel != null ?
                              itemsPanel.Margin.Left + itemsPanel.Margin.Right :
                              0;

            // width should be the displayable width
            containerWidth = containerWidth - Padding.Left - Padding.Right - panelMargin;
            if (containerWidth > 0)
            {
                var newWidth = CalculateItemWidth(containerWidth);

                if (double.IsNaN(ItemWidth) || Math.Abs(newWidth - ItemWidth) > 1)
                {
                    ItemWidth = newWidth;
                }
            }
        }
    }
}