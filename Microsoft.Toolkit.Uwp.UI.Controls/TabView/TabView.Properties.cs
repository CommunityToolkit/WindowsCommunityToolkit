using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TabView properties.
    /// </summary>
    public partial class TabView
    {
        /// <summary>
        /// Gets or sets the content to appear to the left or above the tab strip.
        /// </summary>
        public object LeftHeader
        {
            get { return (object)GetValue(LeftHeaderProperty); }
            set { SetValue(LeftHeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="LeftHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="LeftHeader"/> dependency property.</returns>
        public static readonly DependencyProperty LeftHeaderProperty =
            DependencyProperty.Register(nameof(LeftHeader), typeof(object), typeof(TabView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> for the <see cref="LeftHeader"/>.
        /// </summary>
        public DataTemplate LeftHeaderTemplate
        {
            get { return (DataTemplate)GetValue(LeftHeaderTemplateProperty); }
            set { SetValue(LeftHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="LeftHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="LeftHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty LeftHeaderTemplateProperty =
            DependencyProperty.Register(nameof(LeftHeaderTemplate), typeof(DataTemplate), typeof(TabView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the content to appear to the right or below the tab strip.
        /// </summary>
        public object RightHeader
        {
            get { return (object)GetValue(RightHeaderProperty); }
            set { SetValue(RightHeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RightHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="RightHeader"/> dependency property.</returns>
        public static readonly DependencyProperty RightHeaderProperty =
            DependencyProperty.Register(nameof(RightHeader), typeof(object), typeof(TabView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> for the <see cref="RightHeader"/>.
        /// </summary>
        public DataTemplate RightHeaderTemplate
        {
            get { return (DataTemplate)GetValue(RightHeaderTemplateProperty); }
            set { SetValue(RightHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="RightHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="RightHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty RightHeaderTemplateProperty =
            DependencyProperty.Register(nameof(RightHeaderTemplate), typeof(DataTemplate), typeof(TabView), new PropertyMetadata(null));

        /// <summary>
        /// Gets or sets a value indicating whether an 'Add' button appears for the user to add tabs themselves.
        /// </summary>
        public bool CanAddTabs
        {
            get { return (bool)GetValue(CanAddTabsProperty); }
            set { SetValue(CanAddTabsProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CanAddTabs"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="CanAddTabs"/> dependency property.</returns>
        public static readonly DependencyProperty CanAddTabsProperty =
            DependencyProperty.Register(nameof(CanAddTabs), typeof(bool), typeof(TabView), new PropertyMetadata(false));
    }
}
