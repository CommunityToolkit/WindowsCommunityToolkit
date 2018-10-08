// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <inheritdoc/>
    [Bindable]
    public class EdgeTabWidthProvider : ITabWidthProvider
    {
        /// <inheritdoc/>
        public bool IsSelectedTabWidthDifferent => true;

        /// <inheritdoc/>
        public int MinimumWidth => 90;

        private const int StandardWidth = 200;

        /// <inheritdoc/>
        public IEnumerable<double> ProvideWidth(IEnumerable<TabViewItem> tabs, object items, double availableWidth, TabView parent)
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
