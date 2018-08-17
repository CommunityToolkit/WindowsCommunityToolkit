// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Item Container for a <see cref="TabView"/>.
    /// </summary>
    public partial class TabViewItem : ListViewItem
    {
        public TabViewItem()
        {
            DefaultStyleKey = typeof(TabViewItem);
        }

        public override bool Equals(object obj)
        {
            return Header != null && obj != null && Header.GetHashCode() == obj.GetHashCode();
        }
    }
}
