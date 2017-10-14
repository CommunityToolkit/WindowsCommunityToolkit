using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public class HamburgerMenuNavViewItemStyleSelector : StyleSelector
    {
        public Style MenuItemStyle { get; set; }

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
