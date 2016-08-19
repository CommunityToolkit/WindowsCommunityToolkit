﻿// ******************************************************************
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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
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
    [TemplatePart(Name = "ListView", Type = typeof(ListViewBase))]
    public sealed partial class AdaptiveGridView : Control
    {
        private int _columns;
        private bool _isInitialized;
        private ListViewBase _listView;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdaptiveGridView"/> class.
        /// </summary>
        public AdaptiveGridView()
        {
            IsTabStop = false;
            DefaultStyleKey = typeof(AdaptiveGridView);
        }

        private void RecalculateLayout(double containerWidth)
        {
            if (containerWidth == 0 || DesiredWidth == 0)
            {
                return;
            }

            _columns = CalculateColumns(containerWidth, DesiredWidth);

            // If there's less items than there's columns, reduce the column count;
            if (_listView != null && _listView.Items != null
                && _listView.Items.Count > 0 && _listView.Items.Count < _columns)
            {
                _columns = _listView.Items.Count;
            }

            ItemWidth = (containerWidth / _columns) - 5;
        }

        /// <summary>
        /// Invoked whenever application code or internal processes (such as a rebuilding layout pass) call
        /// ApplyTemplate. In simplest terms, this means the method is called just before a UI element displays
        /// in your app. Override this method to influence the default post-template logic of a class.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();
            if (_listView != null)
            {
                _listView.SizeChanged -= ListView_SizeChanged;
                _listView.ItemClick -= ListView_ItemClick;
                _listView.Items.VectorChanged -= ListViewItems_VectorChanged;
                _listView = null;
            }

            _listView = GetTemplateChild("ListView") as ListViewBase;
            if (_listView != null)
            {
                _listView.SizeChanged += ListView_SizeChanged;
                _listView.ItemClick += ListView_ItemClick;
                _listView.Items.VectorChanged += ListViewItems_VectorChanged;
                _listView.SelectionChanged += ListView_SelectionChanged;
            }

            _isInitialized = true;
            OnOneRowModeEnabledChanged(this, OneRowModeEnabled);
            InitializeBindings();
        }

        private void InitializeBindings()
        {
            // Set bindings from base control.
            var selectedItemBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("SelectedItem"),
                Mode = BindingMode.TwoWay
            };

            var isItemClickEnabledBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("IsItemClickEnabled"),
                Mode = BindingMode.TwoWay
            };

            var selectionModeBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("SelectionMode"),
                Mode = BindingMode.TwoWay
            };

            var selectionIndexBinding = new Binding()
            {
                Source = this,
                Path = new PropertyPath("SelectedIndex"),
                Mode = BindingMode.TwoWay
            };

            _listView.SetBinding(ListViewBase.SelectionModeProperty, selectionModeBinding);
            _listView.SetBinding(ListViewBase.IsItemClickEnabledProperty, isItemClickEnabledBinding);
            _listView.SetBinding(Selector.SelectedItemProperty, selectedItemBinding);
            _listView.SetBinding(Selector.SelectedIndexProperty, selectionIndexBinding);
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            _selectedItems = _listView.SelectedItems;
            SelectionChanged?.Invoke(this, e);
        }

        private void ListViewItems_VectorChanged(Windows.Foundation.Collections.IObservableVector<object> sender, Windows.Foundation.Collections.IVectorChangedEventArgs @event)
        {
            if (_listView != null && !double.IsNaN(_listView.ActualWidth))
            {
                // If the item count changes, check if more or less columns needs to be rendered,
                // in case we were having fewer items than columns.
                RecalculateLayout(_listView.ActualWidth);
            }
        }

        private void ListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            var cmd = ItemClickCommand;
            if (cmd != null)
            {
                if (cmd.CanExecute(e.ClickedItem))
                {
                    cmd.Execute(e.ClickedItem);
                }
            }

            ItemClick?.Invoke(this, e);
        }

        private void ListView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // If the width of the internal list view changes, check if more or less columns needs to be rendered.
            if (e.PreviousSize.Width != e.NewSize.Width)
            {
                RecalculateLayout(e.NewSize.Width);
            }
        }
    }
}
