using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Windows.Toolkit.UI.LayoutControls
{
    public sealed partial class HamburgerMenu
    {
        public event ItemClickEventHandler ItemClick;

        void HamburgerButton_Click(object sender, global::Windows.UI.Xaml.RoutedEventArgs e)
        {
            mainSplitView.IsPaneOpen = !mainSplitView.IsPaneOpen;
        }

        void ButtonsListView_ItemClick(object sender, ItemClickEventArgs e)
        {
            ItemClick?.Invoke(sender, e);
        }
    }
}
