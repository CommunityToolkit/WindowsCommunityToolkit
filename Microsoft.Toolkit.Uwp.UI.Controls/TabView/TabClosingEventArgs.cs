// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Event arguments for <see cref="TabView.TabClosing"/> event.
    /// </summary>
    public class TabClosingEventArgs : EventArgs
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabClosingEventArgs"/> class.
        /// </summary>
        /// <param name="item">Item being closed.</param>
        /// <param name="tab">TabViewItem container being closed.</param>
        public TabClosingEventArgs(object item, TabViewItem tab)
        {
            Item = item;
            Tab = tab;
        }

        /// <summary>
        /// Gets the Item being closed.
        /// </summary>
        public object Item { get; private set; }

        /// <summary>
        /// Gets the Tab being closed.
        /// </summary>
        public TabViewItem Tab { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether the notification should be closed.
        /// </summary>
        public bool Cancel { get; set; }
    }
}