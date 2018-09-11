using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Interface for a provider to determine <see cref="TabViewItem"/> widths for the <see cref="TabView"/>.
    /// </summary>
    public interface ITabWidthProvider
    {
        /// <summary>
        /// Gets a value indicating whether the selected tab's width can be different.
        /// </summary>
        bool IsSelectedTabWidthDifferent { get; }

        /// <summary>
        /// Provide a set of widths for all the visible tabs in the collection.
        /// </summary>
        /// <param name="tabs">Collection of <see cref="TabViewItem"/>.</param>
        /// <param name="items">Collection of items from the Items or ItemsSource of the <see cref="TabView"/>.</param>
        /// <param name="availableWidth">Maximum size available to display tabs.</param>
        /// <returns>Set of widths for each tab.</returns>
        IEnumerable<double> ProvideWidth(IEnumerable<TabViewItem> tabs, ICollection<object> items, double availableWidth);
    }
}
