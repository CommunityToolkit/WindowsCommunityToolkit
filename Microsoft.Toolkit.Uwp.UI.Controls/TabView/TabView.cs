// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;
using System.Linq;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView is a control for displaying a set of tabs and their content.
    /// </summary>
    [TemplatePart(Name = LISTVIEW_NAME, Type = typeof(ListViewBase))]
    public class TabView : ListViewBase
    {
        private const string LISTVIEW_NAME = "TabListPanel";

        private ListViewBase _tabListPanel;

        /// <summary>
        /// Occurs when a tab is dragged by the user outside of the <see cref="TabView"/>.  Generally, this paradigm is used to create a new-window with the torn-off tab.
        /// The creation and handling of the new-window is left to the app's developer.
        /// </summary>
        public event EventHandler<TabDraggedOutsideEventArgs> TabDraggedOutside;

        public TabView()
        {
            this.DefaultStyleKey = typeof(TabView);
        }

        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabViewItem();
        }

        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabViewItem;
        }

        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            //_tabListPanel = GetTemplateChild(LISTVIEW_NAME) as ListViewBase;

            //if (_tabListPanel != null)
            //{
            //    // TODO: Clean this up...
            //    if (Items.Count > 0)
            //    {
            //        _tabListPanel.ItemsSource = Items;
            //    }
            //    else
            //    {
            //        _tabListPanel.ItemsSource = ItemsSource;
            //    }
            //    // TODO: Save Token
            //    RegisterPropertyChangedCallback(ItemsSourceProperty, OnItemsSourcePropertyChanged);

            //    _tabListPanel.DragLeave += _tabListPanel_DragLeave;
            //    _tabListPanel.DragItemsCompleted += _tabListPanel_DragItemsCompleted;
            //}
        }

        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
        }

        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);
        }

        private void _tabListPanel_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            // args.DropResult == None when outside of area (i.e. create new window), otherwise move
            // how behave across windows?
            Debug.WriteLine("[TabView][DragItemsCompleted] " + (args.Items.FirstOrDefault() as TabViewItem)?.Content?.ToString());

            if (args.DropResult == DataPackageOperation.None)
            {
                TabDraggedOutside?.Invoke(this, new TabDraggedOutsideEventArgs(args.Items.FirstOrDefault()));
            }
        }

        private void _tabListPanel_DragLeave(object sender, DragEventArgs e)
        {
            Debug.WriteLine("[TabView][DragLeave] " + e.ToString());
        }

        private void OnItemsSourcePropertyChanged(DependencyObject sender, DependencyProperty dp)
        {
            _tabListPanel.ItemsSource = ItemsSource;
        }
    }
}
