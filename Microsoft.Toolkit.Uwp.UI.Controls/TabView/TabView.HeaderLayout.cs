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
                        var tvis = _tabItemsPresenter?.FindDescendants<TabViewItem>();
                        if (tvis != null)
                        {
                            var widthIterator = TabWidthProvider.ProvideWidth(tvis, tabs, available, this).GetEnumerator();

                            foreach (var tab in tvis)
                            {
                                if (widthIterator.MoveNext())
                                {
                                    var width = widthIterator.Current;
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
    }
}
