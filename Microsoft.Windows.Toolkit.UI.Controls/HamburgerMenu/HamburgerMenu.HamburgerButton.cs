using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    /// <summary>
    /// The HamburgerMenu is based on a SplitView control. By default it contains a HamburgerButton and a ListView to display menu items.
    /// </summary>
    public partial class HamburgerMenu
    {
        /// <summary>
        /// Dependency property associated with HamburgerWidth
        /// </summary>
        public static readonly DependencyProperty HamburgerWidthProperty = DependencyProperty.Register("HamburgerWidth", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));
        /// <summary>
        /// Define main button's width
        /// </summary>
        public double HamburgerWidth
        {
            get { return (double)GetValue(HamburgerWidthProperty); }
            set { SetValue(HamburgerWidthProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with HamburgerHeight
        /// </summary>
        public static readonly DependencyProperty HamburgerHeightProperty = DependencyProperty.Register("HamburgerHeight", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));
        /// <summary>
        /// Define main button's height
        /// </summary>
        public double HamburgerHeight
        {
            get { return (double)GetValue(HamburgerHeightProperty); }
            set { SetValue(HamburgerHeightProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with HamburgerMargin
        /// </summary>
        public static readonly DependencyProperty HamburgerMarginProperty = DependencyProperty.Register("HamburgerMargin", typeof(Thickness), typeof(HamburgerMenu), new PropertyMetadata(null));
        /// <summary>
        /// Define main button's margin
        /// </summary>
        public Thickness HamburgerMargin
        {
            get { return (Thickness)GetValue(HamburgerMarginProperty); }
            set { SetValue(HamburgerMarginProperty, value); }
        }

    }
}
