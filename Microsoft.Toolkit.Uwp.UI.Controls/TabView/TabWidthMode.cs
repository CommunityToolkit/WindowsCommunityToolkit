// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Possible modes for how to layout a <see cref="TabViewItem"/> Header's Width in the <see cref="TabView"/>.
    /// </summary>
    public enum TabWidthMode
    {
        /// <summary>
        /// Each tab header takes up as much space as it needs.  This is similar to how WPF and Visual Studio Code behave.
        /// Suggest to keep <see cref="TabView.IsCloseButtonOverlay"/> set to false.
        /// <see cref="TabView.SelectedTabWidth"/> is ignored.
        /// In this scenario, tab width behavior is effectively turned off.  This can be useful when using custom styling or a custom panel for layout of <see cref="TabViewItem"/> as well.
        /// </summary>
        Actual,

        /// <summary>
        /// Each tab header will use the minimal space set by <see cref="FrameworkElement.MinWidth"/> on the <see cref="TabViewItem"/>.
        /// Suggest to set the <see cref="TabView.SelectedTabWidth"/> to show more content for the selected item.
        /// </summary>
        Compact,

        /// <summary>
        /// Each tab header will fill to fit the available space.  If <see cref="TabView.SelectedTabWidth"/> is set, that will be used as a Maximum Width.
        /// This is similar to how Microsoft Edge behaves when used with the <see cref="TabView.SelectedTabWidth"/>.
        /// Suggest to set <see cref="TabView.IsCloseButtonOverlay"/> to true.
        /// Suggest to set <see cref="TabView.SelectedTabWidth"/> to 200 and the TabViewItemHeaderMinWidth Resource to 90.
        /// </summary>
        Equal,
    }
}
