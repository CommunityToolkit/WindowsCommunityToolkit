// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// <see cref="StyleSelector"/> to be used with <see cref="NavigationView"/>
    /// HamburgerMenuNavViewItemStyleSelector is used by the <see cref="HamburgerMenu"/>
    /// </summary>
    [Obsolete("The HamburgerMenuNavViewItemStyleSelector will be removed alongside the HamburgerMenu in a future major release. Please use the NavigationView control available in the Fall Creators Update")]
    public class HamburgerMenuNavViewItemStyleSelector : StyleSelector
    {
        /// <summary>
        /// Gets or sets the <see cref="Style"/> to be set if the container is a <see cref="NavigationViewItem"/>
        /// </summary>
        public Style MenuItemStyle { get; set; }

        /// <inheritdoc/>
        protected override Style SelectStyleCore(object item, DependencyObject container)
        {
            if (container is NavigationViewItem)
            {
                return MenuItemStyle;
            }
            else
            {
                return null;
            }
        }
    }
}
