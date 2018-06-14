// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// EventArgs used for the <see cref="HamburgerMenu"/> ItemInvoked event
    /// </summary>
    [Obsolete("The HamburgerMenuItemInvokedEventArgs will be removed alongside the HamburgerMenu in a future major release. Please use the NavigationView control available in the Fall Creators Update")]
    public class HamburgerMenuItemInvokedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the invoked item
        /// </summary>
        public object InvokedItem { get; internal set; }

        /// <summary>
        /// Gets a value indicating whether the invoked item is an options item
        /// </summary>
        public bool IsItemOptions { get; internal set; }
    }
}
