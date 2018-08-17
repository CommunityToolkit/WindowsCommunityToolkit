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
        /// <param name="item">data context of element clicked</param>
        public TabDraggedOutsideEventArgs(object item)
        {
            Item = item;
        }

        /// <summary>
        /// Gets or sets the Item/Data Context of the clicked item
        /// </summary>
        public object Item { get; set; }
    }
}
