using Windows.UI.Xaml;

namespace Microsoft.Windows.Toolkit.UI.Controls
{
    using global::Windows.UI.Xaml.Controls;

    /// <summary>
    /// The HamburgerMenu is based on a SplitView control. By default it contains a HamburgerButton and a ListView to display menu items.
    /// </summary>
    public partial class HamburgerMenu
    {
        /// <summary>
        /// Identifies the <see cref="HamburgerWidth"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HamburgerWidthProperty = DependencyProperty.Register(nameof(HamburgerWidth), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));

        /// <summary>
        /// Identifies the <see cref="HamburgerHeight"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HamburgerHeightProperty = DependencyProperty.Register(nameof(HamburgerHeight), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));

        /// <summary>
        /// Identifies the <see cref="HamburgerMargin"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HamburgerMarginProperty = DependencyProperty.Register(nameof(HamburgerMargin), typeof(Thickness), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="HamburgerFontSize"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HamburgerFontSizeProperty = DependencyProperty.Register(nameof(HamburgerFontSize), typeof(double), typeof(HamburgerMenu), new PropertyMetadata(18.0));

        /// <summary>
        /// Identifies the <see cref="HamburgerIcon"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty HamburgerIconProperty = DependencyProperty.Register(nameof(HamburgerIcon), typeof(IconElement), typeof(HamburgerMenu), new PropertyMetadata(default(IconElement)));

        /// <summary>
        /// Gets or sets the hamburger icon.
        /// </summary>
        public IconElement HamburgerIcon
        {
            get { return (IconElement)GetValue(HamburgerIconProperty); }
            set { SetValue(HamburgerIconProperty, value); }
        }

        /// <summary>
        /// Gets or sets main button's width
        /// </summary>
        public double HamburgerWidth
        {
            get { return (double)GetValue(HamburgerWidthProperty); }
            set { SetValue(HamburgerWidthProperty, value); }
        }

        /// <summary>
        /// Gets or sets main button's height
        /// </summary>
        public double HamburgerHeight
        {
            get { return (double)GetValue(HamburgerHeightProperty); }
            set { SetValue(HamburgerHeightProperty, value); }
        }

        /// <summary>
        /// Gets or sets main button's margin
        /// </summary>
        public Thickness HamburgerMargin
        {
            get { return (Thickness)GetValue(HamburgerMarginProperty); }
            set { SetValue(HamburgerMarginProperty, value); }
        }

        /// <summary>
        /// Gets or sets main button's font size
        /// </summary>
        public Thickness HamburgerFontSize
        {
            get { return (Thickness)GetValue(HamburgerFontSizeProperty); }
            set { SetValue(HamburgerFontSizeProperty, value); }
        }
    }
}
