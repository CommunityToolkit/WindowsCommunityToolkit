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
        public int MinimumWidth => 90;

        private const int StandardWidth = 200;

        /// <inheritdoc/>
        public IEnumerable<double> ProvideWidth(IEnumerable<TabViewItem> tabs, object items, double availableWidth)
        {
            if (tabs.Count() <= 1)
            {
                // Default case of a single tab, make it full size.
                yield return Math.Min(StandardWidth, availableWidth);
            }
            else
            {
                var width = (availableWidth - StandardWidth) / (tabs.Count() - 1);

                // Constrain between 90 and 200
                if (width < MinimumWidth)
                {
                    width = MinimumWidth;
                }
                else if (width > StandardWidth)
                {
                    width = StandardWidth;
                }

                foreach (var tab in tabs)
                {
                    // If it's selected make it full size, otherwise whatever the size should be.
                    yield return tab.IsSelected
                        ? Math.Min(StandardWidth, availableWidth)
                        : width;
                }
            }
        }
    }
}
