// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView is a control for displaying a set of tabs and their content.
    /// </summary>
    [Obsolete("Please migrate to the TabView control from WinUI, this control will be removed in a future release. https://aka.ms/winui")]
    [TemplatePart(Name = TabContentPresenterName, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = TabViewContainerName, Type = typeof(Grid))]
    [TemplatePart(Name = TabsItemsPresenterName, Type = typeof(ItemsPresenter))]
    [TemplatePart(Name = TabsScrollViewerName, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = TabsScrollBackButtonName, Type = typeof(ButtonBase))]
    [TemplatePart(Name = TabsScrollForwardButtonName, Type = typeof(ButtonBase))]
    public partial class TabView : ListViewBase
    {
        private const int ScrollAmount = 50; // TODO: Should this be based on TabWidthMode

        private const string TabContentPresenterName = "TabContentPresenter";
        private const string TabViewContainerName = "TabViewContainer";
        private const string TabsItemsPresenterName = "TabsItemsPresenter";
        private const string TabsScrollViewerName = "ScrollViewer";
        private const string TabsScrollBackButtonName = "ScrollBackButton";
        private const string TabsScrollForwardButtonName = "ScrollForwardButton";

        private ContentPresenter _tabContentPresenter;
        private Grid _tabViewContainer;
        private ItemsPresenter _tabItemsPresenter;
        private ScrollViewer _tabScroller;
        private ButtonBase _tabScrollBackButton;
        private ButtonBase _tabScrollForwardButton;

        private bool _hasLoaded;
        private bool _isDragging;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class.
        /// </summary>
        public TabView()
        {
            DefaultStyleKey = typeof(TabView);

            // Container Generation Hooks
            RegisterPropertyChangedCallback(ItemsSourceProperty, ItemsSource_PropertyChanged);
            ItemContainerGenerator.ItemsChanged += ItemContainerGenerator_ItemsChanged;

            // Drag and Layout Hooks
            DragItemsStarting += TabView_DragItemsStarting;
            DragItemsCompleted += TabView_DragItemsCompleted;
            SizeChanged += TabView_SizeChanged;

            // Selection Hook
            SelectionChanged += TabView_SelectionChanged;
        }

        /// <summary>
        /// Occurs when a tab is dragged by the user outside of the <see cref="TabView"/>.  Generally, this paradigm is used to create a new-window with the torn-off tab.
        /// The creation and handling of the new-window is left to the app's developer.
        /// </summary>
        public event EventHandler<TabDraggedOutsideEventArgs> TabDraggedOutside;

        /// <summary>
        /// Occurs when a tab's Close button is clicked.  Set <see cref="CancelEventArgs.Cancel"/> to true to prevent automatic Tab Closure.
        /// </summary>
        public event EventHandler<TabClosingEventArgs> TabClosing;

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride() => new TabViewItem();

        /// <inheritdoc/>
        protected override bool IsItemItsOwnContainerOverride(object item)
        {
            return item is TabViewItem;
        }

        /// <inheritdoc/>
        protected override void OnApplyTemplate()
        {
            base.OnApplyTemplate();

            if (_tabItemsPresenter != null)
            {
                _tabItemsPresenter.SizeChanged -= TabView_SizeChanged;
            }

            if (_tabScroller != null)
            {
                _tabScroller.Loaded -= ScrollViewer_Loaded;
            }

            _tabContentPresenter = GetTemplateChild(TabContentPresenterName) as ContentPresenter;
            _tabViewContainer = GetTemplateChild(TabViewContainerName) as Grid;
            _tabItemsPresenter = GetTemplateChild(TabsItemsPresenterName) as ItemsPresenter;
            _tabScroller = GetTemplateChild(TabsScrollViewerName) as ScrollViewer;

            if (_tabItemsPresenter != null)
            {
                _tabItemsPresenter.SizeChanged += TabView_SizeChanged;
            }

            if (_tabScroller != null)
            {
                _tabScroller.Loaded += ScrollViewer_Loaded;
            }
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _tabScroller.Loaded -= ScrollViewer_Loaded;

            if (_tabScrollBackButton != null)
            {
                _tabScrollBackButton.Click -= ScrollTabBackButton_Click;
            }

            if (_tabScrollForwardButton != null)
            {
                _tabScrollForwardButton.Click -= ScrollTabForwardButton_Click;
            }

            _tabScrollBackButton = _tabScroller.FindDescendantByName(TabsScrollBackButtonName) as ButtonBase;
            _tabScrollForwardButton = _tabScroller.FindDescendantByName(TabsScrollForwardButtonName) as ButtonBase;

            if (_tabScrollBackButton != null)
            {
                _tabScrollBackButton.Click += ScrollTabBackButton_Click;
            }

            if (_tabScrollForwardButton != null)
            {
                _tabScrollForwardButton.Click += ScrollTabForwardButton_Click;
            }
        }

        private void ScrollTabBackButton_Click(object sender, RoutedEventArgs e)
        {
            _tabScroller.ChangeView(Math.Max(0, _tabScroller.HorizontalOffset - ScrollAmount), null, null);
        }

        private void ScrollTabForwardButton_Click(object sender, RoutedEventArgs e)
        {
            _tabScroller.ChangeView(Math.Min(_tabScroller.ScrollableWidth, _tabScroller.HorizontalOffset + ScrollAmount), null, null);
        }

        private void TabView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (_isDragging)
            {
                // Skip if we're dragging, we'll reset when we're done.
                return;
            }

            if (_tabContentPresenter != null)
            {
                if (SelectedItem == null)
                {
                    _tabContentPresenter.Content = null;
                    _tabContentPresenter.ContentTemplate = null;
                }
                else
                {
                    if (ContainerFromItem(SelectedItem) is TabViewItem container)
                    {
                        _tabContentPresenter.Content = container.Content;
                        _tabContentPresenter.ContentTemplate = container.ContentTemplate;
                    }
                }
            }

            // If our width can be effected by the selection, need to run algorithm.
            if (!double.IsNaN(SelectedTabWidth))
            {
                TabView_SizeChanged(sender, null);
            }
        }

        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            base.PrepareContainerForItemOverride(element, item);

            var tabitem = element as TabViewItem;

            tabitem.Loaded -= TabViewItem_Loaded;
            tabitem.Closing -= TabViewItem_Closing;
            tabitem.Loaded += TabViewItem_Loaded;
            tabitem.Closing += TabViewItem_Closing;

            if (tabitem.Header == null)
            {
                tabitem.Header = item;
            }

            if (tabitem.HeaderTemplate == null)
            {
                var headertemplatebinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(ItemHeaderTemplate)),
                    Mode = BindingMode.OneWay
                };
                tabitem.SetBinding(TabViewItem.HeaderTemplateProperty, headertemplatebinding);
            }

            if (tabitem.IsClosable != true && tabitem.ReadLocalValue(TabViewItem.IsClosableProperty) == DependencyProperty.UnsetValue)
            {
                var iscloseablebinding = new Binding()
                {
                    Source = this,
                    Path = new PropertyPath(nameof(CanCloseTabs)),
                    Mode = BindingMode.OneWay,
                };
                tabitem.SetBinding(TabViewItem.IsClosableProperty, iscloseablebinding);
            }
        }

        private void TabViewItem_Loaded(object sender, RoutedEventArgs e)
        {
            var tabitem = sender as TabViewItem;

            tabitem.Loaded -= TabViewItem_Loaded;

            // Only need to do this once.
            if (!_hasLoaded)
            {
                _hasLoaded = true;

                // Need to set a tab's selection on load, otherwise ListView resets to null.
                SetInitialSelection();

                // Need to make sure ContentPresenter is set to content based on selection.
                TabView_SelectionChanged(this, null);

                // Need to make sure we've registered our removal method.
                ItemsSource_PropertyChanged(this, null);

                // Make sure we complete layout now.
                TabView_SizeChanged(this, null);
            }
        }

        private void TabViewItem_Closing(object sender, TabClosingEventArgs e)
        {
            var item = ItemFromContainer(e.Tab);

            var args = new TabClosingEventArgs(item, e.Tab);
            TabClosing?.Invoke(this, args);

            if (!args.Cancel)
            {
                if (ItemsSource != null)
                {
                    _removeItemsSourceMethod?.Invoke(ItemsSource, new object[] { item });
                }
                else
                {
                    Items.Remove(item);
                }
            }
        }

        private void TabView_DragItemsStarting(object sender, DragItemsStartingEventArgs e)
        {
            // Keep track of drag so we don't modify content until done.
            _isDragging = true;
        }

        private void TabView_DragItemsCompleted(ListViewBase sender, DragItemsCompletedEventArgs args)
        {
            _isDragging = false;

            // args.DropResult == None when outside of area (e.g. create new window)
            if (args.DropResult == DataPackageOperation.None)
            {
                var item = args.Items.FirstOrDefault();
                var tab = ContainerFromItem(item) as TabViewItem;

                if (tab == null && item is FrameworkElement fe)
                {
                    tab = fe.FindParent<TabViewItem>();
                }

                if (tab == null)
                {
                    // We still don't have a TabViewItem, most likely is a static TabViewItem in the template being dragged and not selected.
                    // This is a fallback scenario for static tabs.
                    // Note: This can be wrong if two TabViewItems share the exact same Content (i.e. a string), this should be unlikely in any practical scenario.
                    for (int i = 0; i < Items.Count; i++)
                    {
                        var tabItem = ContainerFromIndex(i) as TabViewItem;
                        if (ReferenceEquals(tabItem.Content, item))
                        {
                            tab = tabItem;
                            break;
                        }
                    }
                }

                TabDraggedOutside?.Invoke(this, new TabDraggedOutsideEventArgs(item, tab));
            }
            else
            {
                // If dragging the active tab, there's an issue with the CP blanking.
                TabView_SelectionChanged(this, null);
            }
        }
    }
}
