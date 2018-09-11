using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <inheritdoc/>
    public class EdgeTabWidthProvider : ITabWidthProvider
    {
        /// <inheritdoc/>
        public bool IsSelectedTabWidthDifferent => true;

        /// <inheritdoc/>
        public IEnumerable<double> ProvideWidth(IEnumerable<TabViewItem> tabs, ICollection<object> items, double availableWidth)
        {
            if (tabs.Count() <= 1)
            {
                // Default case of a single tab, make it full size.
                yield return 200;
            }
            else
            {
                var width = (availableWidth - 200) / (tabs.Count() - 1);

                // Constrain between 90 and 200
                if (width < 90)
                {
                    width = 90;
                }
                else if (width > 200)
                {
                    width = 200;
                }

                foreach (var tab in tabs)
                {
                    // If it's selected make it full size, otherwise whatever the size should be.
                    yield return tab.IsSelected ? 200 : width;
                }
            }
        }
    }
}
