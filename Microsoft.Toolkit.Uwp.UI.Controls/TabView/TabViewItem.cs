// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Item Container for a <see cref="TabView"/>.
    /// </summary>
    public partial class TabViewItem : ListViewItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TabViewItem"/> class.
        /// </summary>
        public TabViewItem()
        {
            DefaultStyleKey = typeof(TabViewItem);
        }
    }
}
