// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.Collections.Generic;
using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// The HamburgerMenu is based on a SplitView control. By default it contains a HamburgerButton and a ListView to display menu items.
    /// </summary>
    public partial class HamburgerMenu
    {
        /// <summary>
        /// Event raised when an item is clicked
        /// </summary>
        public event ItemClickEventHandler ItemClick;

        /// <summary>
        /// Event raised when an options' item is clicked
        /// </summary>
        public event ItemClickEventHandler OptionsItemClick;

        /// <summary>
        /// Event raised when an item is invoked
        /// </summary>
        public event EventHandler<HamburgerMenuItemInvokedEventArgs> ItemInvoked;

        private void HamburgerButton_Click(object sender, RoutedEventArgs e)
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private void ButtonsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_optionsListView != null)
            {
                _optionsListView.SelectedIndex = -1;
            }

            ItemClick?.Invoke(this, e);
            ItemInvoked?.Invoke(this, new HamburgerMenuItemInvokedEventArgs() { InvokedItem = e.ClickedItem, IsItemOptions = false });
        }

        private void OptionsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_buttonsListView != null)
            {
                _buttonsListView.SelectedIndex = -1;
            }

            OptionsItemClick?.Invoke(this, e);
            ItemInvoked?.Invoke(this, new HamburgerMenuItemInvokedEventArgs() { InvokedItem = e.ClickedItem, IsItemOptions = true });
        }

        private void NavigationViewItemInvoked(NavigationView sender, NavigationViewItemInvokedEventArgs args)
        {
            if (args.IsSettingsInvoked && _settingsObject != null)
            {
                ItemInvoked?.Invoke(this, new HamburgerMenuItemInvokedEventArgs { InvokedItem = _settingsObject });
            }
            else
            {
                var options = OptionsItemsSource as IEnumerable<object>;
                var isOption = options != null && options.Contains(args.InvokedItem);

                ItemInvoked?.Invoke(this, new HamburgerMenuItemInvokedEventArgs() { InvokedItem = args.InvokedItem, IsItemOptions = isOption });
            }
        }
    }
}
