// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Item Container for a <see cref="TabView"/>.
    /// </summary>
    public partial class TabViewItem
    {
        /// <summary>
        /// Gets or sets the header content for the tab.
        /// </summary>
        public object Header
        {
            get { return (object)GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Header"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="Header"/> dependency property.</returns>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(nameof(Header), typeof(object), typeof(TabViewItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the icon to appear in the tab header.
        /// </summary>
        public IconElement Icon
        {
            get { return (IconElement)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Icon"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="Icon"/> dependency property.</returns>
        public static readonly DependencyProperty IconProperty =
            DependencyProperty.Register(nameof(Icon), typeof(IconElement), typeof(TabViewItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the template to override for the tab header.
        /// </summary>
        public DataTemplate HeaderTemplate
        {
            get { return (DataTemplate)GetValue(HeaderTemplateProperty); }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="HeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="HeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(nameof(HeaderTemplate), typeof(DataTemplate), typeof(TabViewItem), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether the tab can be closed by the user with the close button.
        /// </summary>
        public bool IsClosable
        {
            get { return (bool)GetValue(IsClosableProperty); }
            set { SetValue(IsClosableProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsClosable"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="IsClosable"/> dependency property.</returns>
        public static readonly DependencyProperty IsClosableProperty =
            DependencyProperty.Register(nameof(IsClosable), typeof(bool), typeof(TabViewItem), new PropertyMetadata(null));
    }
}
