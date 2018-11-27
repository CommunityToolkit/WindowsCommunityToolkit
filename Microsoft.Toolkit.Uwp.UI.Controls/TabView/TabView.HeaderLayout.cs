// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView methods related to calculating the width of the <see cref="TabViewItem"/> Headers.
    /// </summary>
    public partial class TabView
    {
        // Attached property for storing widths of tabs if set by other means during layout.
        private static double GetOriginalWidth(TabViewItem obj)
        {
            return (double)obj.GetValue(OriginalWidthProperty);
        }

        private static void SetOriginalWidth(TabViewItem obj, double value)
        {
            obj.SetValue(OriginalWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty OriginalWidthProperty =
            DependencyProperty.RegisterAttached("OriginalWidth", typeof(double), typeof(TabView), new PropertyMetadata(null));

        private static void OnLayoutEffectingPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs args)
        {
            var tabview = sender as TabView;
            if (tabview != null && tabview._hasLoaded)
            {
                tabview.TabView_SizeChanged(tabview, null);
            }
        }

        private void TabView_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            // We need to do this calculation here in Size Changed as the
            // Columns don't have their Actual Size calculated in Measure or Arrange.
            if (_hasLoaded && _tabViewContainer != null)
            {
                // Look for our special columns to calculate size of other 'stuff'
                var taken = _tabViewContainer.ColumnDefinitions.Sum(cd => GetIgnoreColumn(cd) ? 0 : cd.ActualWidth);

                // Get the column we want to work on for available space
                var tabc = _tabViewContainer.ColumnDefinitions.FirstOrDefault(cd => GetConstrainColumn(cd));
                if (tabc != null)
                {
                    var available = ActualWidth - taken;
                    var required = 0.0;
                    var mintabwidth = double.MaxValue;

                    if (TabWidthBehavior == TabWidthMode.Actual)
                    {
                        if (_tabScroller != null)
                        {
                            // If we have a scroll container, get its size.
                            required = _tabScroller.ExtentWidth;
                        }

                        // Restore original widths
                        foreach (var item in Items)
                        {
                            var tab = ContainerFromItem(item) as TabViewItem;
                            if (tab == null)
                            {
                                continue; // container not generated yet
                            }

                            if (tab.ReadLocalValue(OriginalWidthProperty) != DependencyProperty.UnsetValue)
                            {
                                tab.Width = GetOriginalWidth(tab);
                            }
                        }
                    }
                    else if (available > 0)
                    {
                        // Calculate the width for each tab from the provider and determine how much space they take.
                        foreach (var item in Items)
                        {
                            var tab = ContainerFromItem(item) as TabViewItem;
                            if (tab == null)
                            {
                                continue; // container not generated yet
                            }

                            mintabwidth = Math.Min(mintabwidth, tab.MinWidth);

                            double width = double.NaN;

                            switch (TabWidthBehavior)
                            {
                                case TabWidthMode.Equal:
                                    width = ProvideEqualWidth(tab, item, available);
                                    break;
                                case TabWidthMode.Compact:
                                    width = ProvideCompactWidth(tab, item, available);
                                    break;
                            }

                            if (tab.ReadLocalValue(OriginalWidthProperty) == DependencyProperty.UnsetValue)
                            {
                                SetOriginalWidth(tab, tab.Width);
                            }

                            if (width > double.Epsilon)
                            {
                                tab.Width = width;
                                required += Math.Max(Math.Min(width, tab.MaxWidth), tab.MinWidth);
                            }
                            else
                            {
                                tab.Width = GetOriginalWidth(tab);
                                required += tab.ActualWidth;
                            }
                        }
                    }
                    else
                    {
                        // Fix negative bounds.
                        available = 0.0;

                        // Still need to determine a 'minimum' width (if available)
                        // TODO: Consolidate this logic with above better?
                        foreach (var item in Items)
                        {
                            var tab = ContainerFromItem(item) as TabViewItem;
                            if (tab == null)
                            {
                                continue; // container not generated yet
                            }

                            mintabwidth = Math.Min(mintabwidth, tab.MinWidth);
                        }
                    }

                    if (!(mintabwidth < double.MaxValue))
                    {
                        mintabwidth = 0.0; // No Containers, no visual, 0 size.
                    }

                    if (available > mintabwidth)
                    {
                        // Constrain the column based on our required and available space
                        tabc.MaxWidth = available;
                    }

                    //// TODO: If it's less, should we move the selected tab to only be the one shown by default?

                    if (available <= mintabwidth || Math.Abs(available - mintabwidth) < double.Epsilon)
                    {
                        tabc.Width = new GridLength(mintabwidth);
                    }
                    else if (required >= available)
                    {
                        // Fix size as we don't have enough space for all the tabs.
                        tabc.Width = new GridLength(available);
                    }
                    else
                    {
                        // We haven't filled up our space, so we want to expand to take as much as needed.
                        tabc.Width = GridLength.Auto;
                    }
                }
            }
        }

        private double ProvideEqualWidth(TabViewItem tab, object item, double availableWidth)
        {
            if (double.IsNaN(SelectedTabWidth))
            {
                if (Items.Count <= 1)
                {
                    return availableWidth;
                }

                return Math.Max(tab.MinWidth, availableWidth / Items.Count);
            }
            else if (Items.Count() <= 1)
            {
                // Default case of a single tab, make it full size.
                return Math.Min(SelectedTabWidth, availableWidth);
            }
            else
            {
                var width = (availableWidth - SelectedTabWidth) / (Items.Count - 1);

                // Constrain between Min and Selected (Max)
                if (width < tab.MinWidth)
                {
                    width = tab.MinWidth;
                }
                else if (width > SelectedTabWidth)
                {
                    width = SelectedTabWidth;
                }

                // If it's selected make it full size, otherwise whatever the size should be.
                return tab.IsSelected
                    ? Math.Min(SelectedTabWidth, availableWidth)
                    : width;
            }
        }

        private double ProvideCompactWidth(TabViewItem tab, object item, double availableWidth)
        {
            // If we're selected and have a value for that, then just return that.
            if (tab.IsSelected && !double.IsNaN(SelectedTabWidth))
            {
                return SelectedTabWidth;
            }

            // Otherwise use min size.
            return tab.MinWidth;
        }
    }
}
