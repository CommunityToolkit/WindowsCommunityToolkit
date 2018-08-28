// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView is a control for displaying a set of tabs and their content.
    /// </summary>
    [TemplatePart(Name = TABCONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = TABADDBUTTON_NAME, Type = typeof(Button))]
    public partial class TabView : ListViewBase
    {
        private const string TABCONTENTPRESENTER_NAME = "TabContentPresenter";
        private const string TABADDBUTTON_NAME = "AddTabButton";

        private ContentPresenter _tabContentPresenter;
        private Button _tabAddButton;

        /// <summary>
        /// Occurs when a tab is dragged by the user outside of the <see cref="TabView"/>.  Generally, this paradigm is used to create a new-window with the torn-off tab.
        /// The creation and handling of the new-window is left to the app's developer.
        /// </summary>
        public event EventHandler<TabDraggedOutsideEventArgs> TabDraggedOutside;

        /// <summary>
        /// Occurs when a tab's Close button is clicked.  Set <see cref="TabClosingEventArgs.Cancel"/> to true to prevent automatic Tab Closure.
        /// </summary>
        public event EventHandler<TabClosingEventArgs> TabClosing;

        /// <summary>
        /// Occurs when a the Add button is clicked (visible when <see cref="CanAddTabs"/> is true).
        /// </summary>
        public event EventHandler AddTab;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class.
        /// </summary>
        public TabView()
        {
            this.DefaultStyleKey = typeof(TabView);
        }

        /// <inheritdoc/>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabViewItem();
        }

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabViewItem;
        }

        /// <inheritdoc/>
        protected override Size MeasureOverride(Size availableSize)
        {
            return base.MeasureOverride(availableSize);
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            _tabContentPresenter = GetTemplateChild(TABCONTENTPRESENTER_NAME) as ContentPresenter;
            _tabAddButton = GetTemplateChild(TABADDBUTTON_NAME) as Button;

            DragLeave += TabPresenter_DragLeave;
            DragItemsCompleted += TabPresenter_DragItemsCompleted;

            if (_tabContentPresenter != null)
            {
                SelectionChanged += TabView_SelectionChanged;
            }

            if (_tabAddButton != null)
            {
                _tabAddButton.Click += AddTabButton_Click;
            }
        }

        private void AddTabButton_Click(object sender, RoutedEventArgs e)
        {
            AddTab?.Invoke(this, new EventArgs());
        }

        private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedItem == null)
            {
                _tabContentPresenter.Content = null;
                _tabContentPresenter.ContentTemplate = null;
            }
            else
            {
                var container = ContainerFromItem(SelectedItem) as TabViewItem;
                if (container != null)
                {
                    _tabContentPresenter.Content = container.Content;
                    _tabContentPresenter.ContentTemplate = container.ContentTemplate;
                }
            }
        }

        /// <inheritdoc/>
        protected override void OnItemsChanged(object e)
        {
            base.OnItemsChanged(e);
        }

        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var tvi = element as TabViewItem;

            tvi.Loaded += TabViewItem_Loaded;

            base.PrepareContainerForItemOverride(element, item);
        }

        private void TabViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            var tvi = sender as TabViewItem;

            var btn = tvi.FindDescendantByName("CloseButton") as Button;
            if (btn != null)
            {
                btn.Click += CloseButton_Clicked;
            }
        }

        private void CloseButton_Clicked(object sender, RoutedEventArgs e)
        {
            var tvi = (sender as FrameworkElement).FindAscendant<TabViewItem>();

            if (tvi != null)
            {
                var args = new TabClosingEventArgs(tvi);
                TabClosing?.Invoke(this, args);

                if (!args.Cancel)
                {
                    if (ItemsSource != null)
                    {
                        (ItemsSource as IList).Remove(tvi);
                    }
                    else if (Items != null)
                    {
                        Items.Remove(tvi);
                    }
                }
            }
        }

        private void TabPresenter_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            // args.DropResult == None when outside of area (i.e. create new window), otherwise move
            // how behave across windows?
            ////Debug.WriteLine("[TabView][DragItemsCompleted] " + (args.Items.FirstOrDefault() as TabViewItem)?.Content?.ToString());

            if (args.DropResult == DataPackageOperation.None)
            {
                TabDraggedOutside?.Invoke(this, new TabDraggedOutsideEventArgs(args.Items.FirstOrDefault()));
            }
        }

        private void TabPresenter_DragLeave(object sender, DragEventArgs e)
        {
            ////Debug.WriteLine("[TabView][DragLeave] " + e.ToString());
        }
    }
}
