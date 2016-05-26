using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
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

        private void HamburgerButton_Click(object sender, global::Windows.UI.Xaml.RoutedEventArgs e)
        {
            IsPaneOpen = !IsPaneOpen;
        }

        private void ButtonsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_optionsListView != null)
            {
                _optionsListView.SelectedIndex = -1;
            }
            ItemClick?.Invoke(sender, e);
        }

        private void OptionsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            if (_buttonsListView != null)
            {
                _buttonsListView.SelectedIndex = -1;
            }
            OptionsItemClick?.Invoke(sender, e);
        }
    }
}
