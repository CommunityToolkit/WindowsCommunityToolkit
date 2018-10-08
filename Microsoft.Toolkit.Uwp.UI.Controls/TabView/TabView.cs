// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel.DataTransfer;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView is a control for displaying a set of tabs and their content.
    /// </summary>
    [TemplatePart(Name = TABCONTENTPRESENTER_NAME, Type = typeof(ContentPresenter))]
    [TemplatePart(Name = TABVIEWCONTAINER_NAME, Type = typeof(Grid))]
    [TemplatePart(Name = TABITEMSPRESENTER_NAME, Type = typeof(ItemsPresenter))]
    [TemplatePart(Name = TABSCROLLVIEWER_NAME, Type = typeof(ScrollViewer))]
    [TemplatePart(Name = TABADDBUTTON_NAME, Type = typeof(ButtonBase))]
    [TemplatePart(Name = TABSCROLLBACKBUTTON_NAME, Type = typeof(ButtonBase))]
    [TemplatePart(Name = TABSCROLLFORWARDBUTTON_NAME, Type = typeof(ButtonBase))]
    public partial class TabView : ListViewBase
    {
        private const int SCROLL_AMOUNT = 50; // TODO: Should this come from TabWidthProvider?

        private const string TABCONTENTPRESENTER_NAME = "TabContentPresenter";
        private const string TABVIEWCONTAINER_NAME = "TabViewContainer";
        private const string TABITEMSPRESENTER_NAME = "TabsItemsPresenter";
        private const string TABSCROLLVIEWER_NAME = "TabsScrollViewer";
        private const string TABADDBUTTON_NAME = "AddTabButton";
        private const string TABSCROLLBACKBUTTON_NAME = "ScrollBackButton";
        private const string TABSCROLLFORWARDBUTTON_NAME = "ScrollForwardButton";

        private ContentPresenter _tabContentPresenter;
        private Grid _tabViewContainer;
        private ItemsPresenter _tabItemsPresenter;
        private ScrollViewer _tabScroller;
        private ButtonBase _tabAddButton;
        private ButtonBase _tabScrollBackButton;
        private ButtonBase _tabScrollForwardButton;

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
        /// Occurs when a the Add button is clicked (visible when <see cref="IsAddTabButtonVisible"/> is true).
        /// </summary>
        public event EventHandler AddTabButtonClick;

        /// <summary>
        /// Initializes a new instance of the <see cref="TabView"/> class.
        /// </summary>
        public TabView()
        {
            this.DefaultStyleKey = typeof(TabView);

            RegisterPropertyChangedCallback(ItemsSourceProperty, ItemsSource_PropertyChanged);

            Loaded += TabView_Loaded;
        }

        private async void TabView_Loaded(object sender, RoutedEventArgs e)
        {
            Loaded -= TabView_Loaded;

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
            {
                // Need to set a tab's selection on load, otherwise ListView resets to null.
                SetSelection();

                // Need to set the contentpresenter's content here for embedded XAML TabViewItems, otherwise never loads, platform issue?
                if (_tabContentPresenter.Content == null)
                {
                    _tabContentPresenter.Content = (ContainerFromItem(SelectedItem) as TabViewItem)?.Content;
                }

                // Need to 'refresh' the ContentPresenter for ItemsSource Databound scenarios for it to render... :(
                _tabContentPresenter.UpdateLayout();
            });
        }

        /// <inheritdoc/>
        protected override void ClearContainerForItemOverride(DependencyObject element, object item)
        {
            base.ClearContainerForItemOverride(element, item);
        }

        /// <inheritdoc/>
        protected override DependencyObject GetContainerForItemOverride()
        {
            return new TabViewItem()
            {
                HeaderTemplate = ItemHeaderTemplate
            };
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
            _tabViewContainer = GetTemplateChild(TABVIEWCONTAINER_NAME) as Grid;
            _tabItemsPresenter = GetTemplateChild(TABITEMSPRESENTER_NAME) as ItemsPresenter;
            _tabScroller = GetTemplateChild(TABSCROLLVIEWER_NAME) as ScrollViewer;
            _tabAddButton = GetTemplateChild(TABADDBUTTON_NAME) as ButtonBase;

            DragLeave += TabPresenter_DragLeave;
            DragItemsCompleted += TabPresenter_DragItemsCompleted;
            SizeChanged += TabView_SizeChanged;

            if (_tabItemsPresenter != null)
            {
                _tabItemsPresenter.SizeChanged -= TabView_SizeChanged;
                _tabItemsPresenter.SizeChanged += TabView_SizeChanged;
            }

            if (_tabContentPresenter != null)
            {
                SelectionChanged -= TabView_SelectionChanged;
                SelectionChanged += TabView_SelectionChanged;
            }

            if (_tabAddButton != null)
            {
                _tabAddButton.Click -= AddTabButton_Click;
                _tabAddButton.Click += AddTabButton_Click;
            }

            if (_tabScroller != null)
            {
                _tabScroller.Loaded -= ScrollViewer_Loaded;
                _tabScroller.Loaded += ScrollViewer_Loaded;
            }
        }

        private void ScrollViewer_Loaded(object sender, RoutedEventArgs e)
        {
            _tabScroller.Loaded -= ScrollViewer_Loaded;

            _tabScrollBackButton = _tabScroller.FindDescendantByName(TABSCROLLBACKBUTTON_NAME) as ButtonBase;
            _tabScrollForwardButton = _tabScroller.FindDescendantByName(TABSCROLLFORWARDBUTTON_NAME) as ButtonBase;

            if (_tabScrollBackButton != null)
            {
                _tabScrollBackButton.Click -= ScrollTabBackButton_Click;
                _tabScrollBackButton.Click += ScrollTabBackButton_Click;
            }

            if (_tabScrollForwardButton != null)
            {
                _tabScrollForwardButton.Click -= ScrollTabForwardButton_Click;
                _tabScrollForwardButton.Click += ScrollTabForwardButton_Click;
            }
        }

        private void ScrollTabBackButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tabScroller != null)
            {
                _tabScroller.ChangeView(Math.Max(0, _tabScroller.HorizontalOffset - SCROLL_AMOUNT), null, null);
            }
        }

        private void ScrollTabForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (_tabScroller != null)
            {
                _tabScroller.ChangeView(Math.Min(_tabScroller.ScrollableWidth, _tabScroller.HorizontalOffset + SCROLL_AMOUNT), null, null);
            }
        }

        private void TabView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // We need to do this calculation here in Size Changed as the
            // Columns don't have their Actual Size calculated in Measure or Arrange.
            if (_tabViewContainer != null)
            {
                // Look for our special columns to calculate size of other 'stuff'
                var taken = _tabViewContainer.ColumnDefinitions.Sum(cd => GetIgnoreColumn(cd) ? 0 : cd.ActualWidth);

                // Get the column we want to work on for available space
                var tabc = _tabViewContainer.ColumnDefinitions.FirstOrDefault(cd => GetConstrainColumn(cd));
                var tabs = GetTabSource();

                if (tabc != null && tabs != null)
                {
                    var available = ActualWidth - taken;
                    var required = 0.0;

                    if (available > 0)
                    {
                        // Calculate the width for each tab from the provider and determine how much space they take.
                        var tvis = _tabItemsPresenter.FindDescendants<TabViewItem>();
                        var widthIterator = TabWidthProvider.ProvideWidth(tvis, tabs, available, this).GetEnumerator();

                        foreach (var tab in tvis)
                        {
                            if (widthIterator.MoveNext())
                            {
                                var width = widthIterator.Current;
                                if (width > 0)
                                {
                                    tab.Width = Math.Max(width, 0);
                                }
                                else
                                {
                                    tab.Width = double.NaN;
                                }
                                required += width;
                            }
                        }
                    }

                    if (available > TabWidthProvider.MinimumWidth)
                    {
                        // Constrain the column based on our required and available space
                        tabc.MaxWidth = available;
                    }

                    //// TODO: If it's less, should we move the selected tab to only be the one shown by default?

                    if (available <= TabWidthProvider.MinimumWidth)
                    {
                        tabc.Width = new GridLength(TabWidthProvider.MinimumWidth);
                    }
                    else if (required >= available)
                    {
                        // Fix size as we don't have enough space for all the tabs.
                        tabc.Width = new GridLength(tabc.MaxWidth);
                    }
                    else
                    {
                        // We haven't filled up our space, so we want to expand to take as much as needed.
                        tabc.Width = GridLength.Auto;
                    }
                }
            }
        }

        private void AddTabButton_Click(object sender, RoutedEventArgs e)
        {
            AddTabButtonClick?.Invoke(this, new EventArgs());
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

            // If our width can be effected by the selection, need to run algorithm.
            if (TabWidthProvider != null && TabWidthProvider.IsSelectedTabWidthDifferent)
            {
                TabView_SizeChanged(sender, null);
            }
        }

        /// <inheritdoc/>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            var tvi = element as TabViewItem;

            tvi.Loaded += TabViewItem_Loaded;

            if (tvi.Header == null)
            {
                tvi.Header = item;
            }

            if (tvi.HeaderTemplate == null)
            {
                tvi.HeaderTemplate = ItemHeaderTemplate;
            }

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
                var item = ItemFromContainer(tvi);

                var args = new TabClosingEventArgs(item, tvi);
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
            else
            {
                // If dragging the active tab, there's an issue with the CP blanking.
                #pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
                Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Low, () =>
                {
                    // Need to set the contentpresenter's content here for embedded XAML TabViewItems, otherwise never loads, platform issue?
                    if (_tabContentPresenter.Content == null)
                    {
                        _tabContentPresenter.Content = (ContainerFromItem(SelectedItem) as TabViewItem)?.Content;
                    }

                    // Need to 'refresh' the ContentPresenter for ItemsSource Databound scenarios for it to render... :(
                    _tabContentPresenter.UpdateLayout();
                });
                #pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            }
        }

        private void TabPresenter_DragLeave(object sender, DragEventArgs e)
        {
            ////Debug.WriteLine("[TabView][DragLeave] " + e.ToString());
        }
    }
}
