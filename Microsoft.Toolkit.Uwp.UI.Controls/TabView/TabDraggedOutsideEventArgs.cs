// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// A class used by the <see cref="TabView"/> TabDraggedOutside Event
    /// </summary>
    public class TabDraggedOutsideEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabDraggedOutsideEventArgs"/> class.
        /// </summary>
        /// <param name="item">data context of element dragged</param>
        /// <param name="tab"><see cref="TabViewItem"/> container being dragged.</param>
        public TabDraggedOutsideEventArgs(object item, TabViewItem tab)
        {
            Item = item;
            Tab = tab;
        }

        /// <summary>
        /// Gets or sets the Item/Data Context of the item being dragged outside of the <see cref="TabView"/>.
        /// </summary>
        public object Item { get; set; }

        /// <summary>
        /// Gets the Tab being dragged outside of the <see cref="TabView"/>.
        /// </summary>
        public TabViewItem Tab { get; private set; }
    }
}
