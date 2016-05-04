using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.LayoutControls
{
    public sealed partial class HamburgerMenu
    {
        public static readonly DependencyProperty HamburgerWidthProperty = DependencyProperty.Register("HamburgerWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));
        public double HamburgerWidth
        {
            get { return (double)GetValue(HamburgerWidthProperty); }
            set { SetValue(HamburgerWidthProperty, value); }
        }

        public static readonly DependencyProperty HamburgerHeightProperty = DependencyProperty.Register("HamburgerHeight", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));
        public double HamburgerHeight
        {
            get { return (double)GetValue(HamburgerHeightProperty); }
            set { SetValue(HamburgerHeightProperty, value); }
        }

        public static readonly DependencyProperty HamburgerMarginProperty = DependencyProperty.Register("HamburgerMargin", typeof(Thickness), typeof(HamburgerMenu), new PropertyMetadata(null));
        public Thickness HamburgerMargin
        {
            get { return (Thickness)GetValue(HamburgerMarginProperty); }
            set { SetValue(HamburgerMarginProperty, value); }
        }

    }
}
