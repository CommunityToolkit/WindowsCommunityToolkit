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
        /// Identifies the <see cref="OptionsItemsSource"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsItemsSourceProperty = DependencyProperty.Register("OptionsItemsSource", typeof(object), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="OptionsItemTemplate"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsItemTemplateProperty = DependencyProperty.Register("OptionsItemTemplate", typeof(DataTemplate), typeof(HamburgerMenu), new PropertyMetadata(null));

        /// <summary>
        /// Identifies the <see cref="OptionsVisibility"/> dependency property.
        /// </summary>
        public static readonly DependencyProperty OptionsVisibilityProperty = DependencyProperty.Register("OptionsVisibility", typeof(Visibility), typeof(HamburgerMenu), new PropertyMetadata(Visibility.Visible));

        /// <summary>
        /// 	Gets or sets an object source used to generate the content of the options. 
        /// </summary>
        public object OptionsItemsSource
        {
            get { return GetValue(OptionsItemsSourceProperty); }
            set { SetValue(OptionsItemsSourceProperty, value); }
        }

        /// <summary>
        /// Gets or sets the DataTemplate used to display each item in the options. 
        /// </summary>
        public DataTemplate OptionsItemTemplate
        {
            get { return (DataTemplate)GetValue(OptionsItemTemplateProperty); }
            set { SetValue(OptionsItemTemplateProperty, value); }
        }

        /// <summary>
        /// Gets or sets options' visibility. 
        /// </summary>
        public Visibility OptionsVisibility
        {
            get { return (Visibility)GetValue(OptionsVisibilityProperty); }
            set { SetValue(OptionsVisibilityProperty, value); }
        }
    }
}
