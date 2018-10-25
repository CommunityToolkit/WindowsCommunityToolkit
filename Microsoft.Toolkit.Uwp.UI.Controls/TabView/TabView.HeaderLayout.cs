using Microsoft.Toolkit.Uwp.UI.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView methods related to calculating the width of the <see cref="TabViewItem"/> Headers.
    /// </summary>
    public partial class TabView
    {
        private static double GetInitialActualWidth(TabViewItem obj)
        {
            return (double)obj.GetValue(InitialActualWidthProperty);
        }

        private static void SetInitialActualWidth(TabViewItem obj, double value)
        {
            obj.SetValue(InitialActualWidthProperty, value);
        }

        // Using a DependencyProperty as the backing store for MyProperty.  This enables animation, styling, binding, etc...
        private static readonly DependencyProperty InitialActualWidthProperty =
            DependencyProperty.RegisterAttached("InitialActualWidth", typeof(double), typeof(TabView), new PropertyMetadata(0.0));

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
                if (tabc != null)
                {
                    var available = ActualWidth - taken;
                    var required = 0.0;
                    var mintabwidth = double.MaxValue;

                    if (available > 0)
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
                                case TabWidthMode.Actual:
                                    width = ProvideActualWidth(tab, item, available);
                                    break;
                                case TabWidthMode.Equal:
                                    width = ProvideEqualWidth(tab, item, available);
                                    break;
                                case TabWidthMode.Compact:
                                    width = ProvideCompactWidth(tab, item, available);
                                    break;
                            }

                            if (width > 0)
                            {
                                tab.Width = width;
                                required += width;
                            }
                            else
                            {
                                tab.Width = double.NaN;
                                required += tab.ActualWidth;
                            }
                        }
                    }

                    if (available > mintabwidth)
                    {
                        // Constrain the column based on our required and available space
                        tabc.MaxWidth = available;
                    }

                    //// TODO: If it's less, should we move the selected tab to only be the one shown by default?

                    if (available <= mintabwidth && mintabwidth < double.MaxValue)
                    {
                        tabc.Width = new GridLength(mintabwidth);
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

        private double ProvideActualWidth(TabViewItem tab, object item, double availableWidth)
        {
            var tabclosesize = tab.FindDescendantByName("CloseButtonContainer")?.Width;

            var size = GetInitialActualWidth(tab);

            if (size <= double.Epsilon && tab.ActualWidth > tab.MinWidth)
            {
                SetInitialActualWidth(tab, tab.ActualWidth);
                size = tab.ActualWidth;
            }

            // If it's selected leave extra room for close button and add right-margin.
            return (tab.IsClosable == true ? (tabclosesize.HasValue ? tabclosesize.Value : 0) : 0) + size;
        }

        private double ProvideEqualWidth(TabViewItem tab, object item, double availableWidth)
        {
            if (double.IsNaN(SelectedTabWidth))
            {
                if (Items.Count() <= 1)
                {
                    return availableWidth;
                }

                return Math.Max(tab.MinWidth, availableWidth / Items.Count());
            }
            else if (Items.Count() <= 1)
            {
                // Default case of a single tab, make it full size.
                return Math.Min(SelectedTabWidth, availableWidth);
            }
            else
            {
                var width = (availableWidth - SelectedTabWidth) / (Items.Count() - 1);

                // Constrain between 90 and 200
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
