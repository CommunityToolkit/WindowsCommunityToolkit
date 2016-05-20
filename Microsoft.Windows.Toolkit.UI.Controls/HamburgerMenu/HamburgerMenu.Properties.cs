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
    public partial class HamburgerMenu
    {
        /// <summary>
        /// Dependency property associated with OpenPaneLength
        /// </summary>
        public static readonly DependencyProperty OpenPaneLengthProperty = DependencyProperty.Register("OpenPaneLength", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(240.0));

        /// <summary>
        /// Gets or sets the width of the pane when it's fully expanded.
        /// </summary>
        public double OpenPaneLength
        {
            get { return (double)GetValue(OpenPaneLengthProperty); }
            set { SetValue(OpenPaneLengthProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with PanePlacement
        /// </summary>
        public static readonly DependencyProperty PanePlacementProperty = DependencyProperty.Register("PanePlacement", typeof(SplitViewPanePlacement), typeof(HamburgerMenu), new PropertyMetadata(SplitViewPanePlacement.Left));
        /// <summary>
        /// Gets or sets a value that specifies whether the pane is shown on the right or left side of the control.
        /// </summary>
        public SplitViewPanePlacement PanePlacement
        {
            get { return (SplitViewPanePlacement)GetValue(PanePlacementProperty); }
            set { SetValue(PanePlacementProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with DisplayMode
        /// </summary>
        public static readonly DependencyProperty DisplayModeProperty = DependencyProperty.Register("DisplayMode", typeof(SplitViewDisplayMode), typeof(HamburgerMenu), new PropertyMetadata(SplitViewDisplayMode.CompactInline));
        /// <summary>
        /// Gets of sets a value that specifies how the pane and content areas are shown.
        /// </summary>
        public SplitViewDisplayMode DisplayMode
        {
            get { return (SplitViewDisplayMode)GetValue(DisplayModeProperty); }
            set { SetValue(DisplayModeProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with CompactPaneLength
        /// </summary>
        public static readonly DependencyProperty CompactPaneLengthProperty = DependencyProperty.Register("CompactPaneLength", typeof(double), typeof(HamburgerMenu), new PropertyMetadata(48.0));
        /// <summary>
        /// Gets or sets the width of the pane in its compact display mode.
        /// </summary>
        public double CompactPaneLength
        {
            get { return (double)GetValue(CompactPaneLengthProperty); }
            set { SetValue(CompactPaneLengthProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with PaneBackground
        /// </summary>
        public static readonly DependencyProperty PaneBackgroundProperty = DependencyProperty.Register("PaneBackground", typeof(Brush), typeof(HamburgerMenu), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the Brush to apply to the background of the Pane area of the control.
        /// </summary>
        public Brush PaneBackground
        {
            get { return (Brush)GetValue(PaneBackgroundProperty); }
            set { SetValue(PaneBackgroundProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with IsPaneOpen
        /// </summary>
        public static readonly DependencyProperty IsPaneOpenProperty = DependencyProperty.Register("IsPaneOpen", typeof(bool), typeof(HamburgerMenu), new PropertyMetadata(false));
        /// <summary>
        /// Gets or sets a value that specifies whether the pane is expanded to its full width.
        /// </summary>
        public bool IsPaneOpen
        {
            get { return (bool)GetValue(IsPaneOpenProperty); }
            set { SetValue(IsPaneOpenProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with ItemsSource
        /// </summary>
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register("ItemsSource", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));
        /// <summary>
        /// 	Gets or sets an object source used to generate the content of the menu. 
        /// </summary>
        public object ItemsSource
        {
            get { return GetValue(ItemsSourceProperty); }
            set { SetValue(ItemsSourceProperty, value); }
        }

        /// <summary>
        /// Dependency property associated with ItemTemplate
        /// </summary>
        public static readonly DependencyProperty ItemTemplateProperty = DependencyProperty.Register("ItemTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));
        /// <summary>
        /// Gets or sets the DataTemplate used to display each item. 
        /// </summary>
        public DataTemplate ItemTemplate
        {
            get { return (DataTemplate)GetValue(ItemTemplateProperty); }
            set { SetValue(ItemTemplateProperty, value); }
        }

    }
}
