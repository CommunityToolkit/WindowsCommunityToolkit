using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

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

        /// <summary>
        /// Gets or sets the implementer of <see cref="ITabWidthProvider"/> interface to provide widths of tabs for <see cref="TabView"/>.
        /// </summary>
        public ITabWidthProvider TabWidthProvider
        {
            get { return (ITabWidthProvider)GetValue(TabWidthProviderProperty); }
            set { SetValue(TabWidthProviderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabWidthProvider"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabWidthProvider"/> dependency property.</returns>
        public static readonly DependencyProperty TabWidthProviderProperty =
            DependencyProperty.Register(nameof(TabWidthProvider), typeof(ITabWidthProvider), typeof(TabView), new PropertyMetadata(new EdgeTabWidthProvider()));

        public static bool GetIgnoreColumn(ColumnDefinition obj)
        {
            return (bool)obj.GetValue(IgnoreColumnProperty);
        }

        public static void SetIgnoreColumn(ColumnDefinition obj, bool value)
        {
            obj.SetValue(IgnoreColumnProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IgnoreColumnProperty"/> attached property.
        /// </summary>
        /// <returns>The identifier for the <see cref="CanAddTabs"/> dependency property.</returns>
        public static readonly DependencyProperty IgnoreColumnProperty =
            DependencyProperty.RegisterAttached("IgnoreColumn", typeof(bool), typeof(TabView), new PropertyMetadata(false));

        public static bool GetConstrainColumn(ColumnDefinition obj)
        {
            return (bool)obj.GetValue(ConstrainColumnProperty);
        }

        public static void SetConstrainColumn(ColumnDefinition obj, bool value)
        {
            obj.SetValue(ConstrainColumnProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ConstrainColumnProperty"/> attached property.
        /// </summary>
        /// <returns>The identifier for the <see cref="CanAddTabs"/> dependency property.</returns>
        public static readonly DependencyProperty ConstrainColumnProperty =
            DependencyProperty.RegisterAttached("ConstrainColumn", typeof(bool), typeof(TabView), new PropertyMetadata(false));
    }
}
