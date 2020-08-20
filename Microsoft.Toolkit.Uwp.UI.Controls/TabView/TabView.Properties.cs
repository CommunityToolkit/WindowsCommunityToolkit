// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        public object TabStartHeader
        {
            get { return (object)GetValue(TabStartHeaderProperty); }
            set { SetValue(TabStartHeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabStartHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabStartHeader"/> dependency property.</returns>
        public static readonly DependencyProperty TabStartHeaderProperty =
            DependencyProperty.Register(nameof(TabStartHeader), typeof(object), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> for the <see cref="TabStartHeader"/>.
        /// </summary>
        public DataTemplate TabStartHeaderTemplate
        {
            get { return (DataTemplate)GetValue(TabStartHeaderTemplateProperty); }
            set { SetValue(TabStartHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabStartHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabStartHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty TabStartHeaderTemplateProperty =
            DependencyProperty.Register(nameof(TabStartHeaderTemplate), typeof(DataTemplate), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the content to appear next to the tab strip.
        /// </summary>
        public object TabActionHeader
        {
            get { return (object)GetValue(TabActionHeaderProperty); }
            set { SetValue(TabActionHeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabActionHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabActionHeader"/> dependency property.</returns>
        public static readonly DependencyProperty TabActionHeaderProperty =
            DependencyProperty.Register(nameof(TabActionHeader), typeof(object), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> for the <see cref="TabActionHeader"/>.
        /// </summary>
        public DataTemplate TabActionHeaderTemplate
        {
            get { return (DataTemplate)GetValue(TabActionHeaderTemplateProperty); }
            set { SetValue(TabActionHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabActionHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabActionHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty TabActionHeaderTemplateProperty =
            DependencyProperty.Register(nameof(TabActionHeaderTemplate), typeof(DataTemplate), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the content to appear to the right or below the tab strip.
        /// </summary>
        public object TabEndHeader
        {
            get { return (object)GetValue(TabEndHeaderProperty); }
            set { SetValue(TabEndHeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabEndHeader"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabEndHeader"/> dependency property.</returns>
        public static readonly DependencyProperty TabEndHeaderProperty =
            DependencyProperty.Register(nameof(TabEndHeader), typeof(object), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the <see cref="DataTemplate"/> for the <see cref="TabEndHeader"/>.
        /// </summary>
        public DataTemplate TabEndHeaderTemplate
        {
            get { return (DataTemplate)GetValue(TabEndHeaderTemplateProperty); }
            set { SetValue(TabEndHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabEndHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabEndHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty TabEndHeaderTemplateProperty =
            DependencyProperty.Register(nameof(TabEndHeaderTemplate), typeof(DataTemplate), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the default <see cref="DataTemplate"/> for the <see cref="TabViewItem.HeaderTemplate"/>.
        /// </summary>
        public DataTemplate ItemHeaderTemplate
        {
            get { return (DataTemplate)GetValue(ItemHeaderTemplateProperty); }
            set { SetValue(ItemHeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="ItemHeaderTemplate"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabStartHeaderTemplate"/> dependency property.</returns>
        public static readonly DependencyProperty ItemHeaderTemplateProperty =
            DependencyProperty.Register(nameof(ItemHeaderTemplate), typeof(DataTemplate), typeof(TabView), new PropertyMetadata(null, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether by default a Tab can be closed or not if no value to <see cref="TabViewItem.IsClosable"/> is provided.
        /// </summary>
        public bool CanCloseTabs
        {
            get { return (bool)GetValue(CanCloseTabsProperty); }
            set { SetValue(CanCloseTabsProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="CanCloseTabs"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="CanCloseTabs"/> dependency property.</returns>
        public static readonly DependencyProperty CanCloseTabsProperty =
            DependencyProperty.Register(nameof(CanCloseTabs), typeof(bool), typeof(TabView), new PropertyMetadata(false, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating whether a <see cref="TabViewItem"/> Close Button should be included in layout calculations.
        /// </summary>
        public bool IsCloseButtonOverlay
        {
            get { return (bool)GetValue(IsCloseButtonOverlayProperty); }
            set { SetValue(IsCloseButtonOverlayProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="IsCloseButtonOverlay"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="IsCloseButtonOverlay"/> dependency property.</returns>
        public static readonly DependencyProperty IsCloseButtonOverlayProperty =
            DependencyProperty.Register(nameof(IsCloseButtonOverlay), typeof(bool), typeof(TabView), new PropertyMetadata(false, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets a value indicating the size of the selected tab.  By default this is set to Auto and the selected tab size doesn't change.
        /// </summary>
        public double SelectedTabWidth
        {
            get { return (double)GetValue(SelectedTabWidthProperty); }
            set { SetValue(SelectedTabWidthProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="SelectedTabWidth"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="SelectedTabWidth"/> dependency property.</returns>
        public static readonly DependencyProperty SelectedTabWidthProperty =
            DependencyProperty.Register(nameof(SelectedTabWidth), typeof(double), typeof(TabView), new PropertyMetadata(double.NaN, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets or sets the current <see cref="TabWidthMode"/> which determins how tab headers' width behave.
        /// </summary>
        public TabWidthMode TabWidthBehavior
        {
            get { return (TabWidthMode)GetValue(TabWidthBehaviorProperty); }
            set { SetValue(TabWidthBehaviorProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="TabWidthBehavior"/> dependency property.
        /// </summary>
        /// <returns>The identifier for the <see cref="TabWidthBehavior"/> dependency property.</returns>
        public static readonly DependencyProperty TabWidthBehaviorProperty =
            DependencyProperty.Register(nameof(TabWidthBehavior), typeof(TabWidthMode), typeof(TabView), new PropertyMetadata(TabWidthMode.Actual, OnLayoutEffectingPropertyChanged));

        /// <summary>
        /// Gets the attached property value to indicate if this grid column should be ignored when calculating header sizes.
        /// </summary>
        /// <param name="obj">Grid Column.</param>
        /// <returns>Boolean indicating if this column is ignored by TabViewHeader logic.</returns>
        public static bool GetIgnoreColumn(ColumnDefinition obj)
        {
            return (bool)obj.GetValue(IgnoreColumnProperty);
        }

        /// <summary>
        /// Sets the attached property value for <see cref="IgnoreColumnProperty"/>
        /// </summary>
        /// <param name="obj">Grid Column.</param>
        /// <param name="value">Boolean value</param>
        public static void SetIgnoreColumn(ColumnDefinition obj, bool value)
        {
            obj.SetValue(IgnoreColumnProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="IgnoreColumnProperty"/> attached property.
        /// </summary>
        /// <returns>The identifier for the IgnoreColumn attached property.</returns>
        public static readonly DependencyProperty IgnoreColumnProperty =
            DependencyProperty.RegisterAttached("IgnoreColumn", typeof(bool), typeof(TabView), new PropertyMetadata(false));

        /// <summary>
        /// Gets the attached value indicating this column should be restricted for the <see cref="TabViewItem"/> headers.
        /// </summary>
        /// <param name="obj">Grid Column.</param>
        /// <returns>True if this column should be constrained.</returns>
        public static bool GetConstrainColumn(ColumnDefinition obj)
        {
            return (bool)obj.GetValue(ConstrainColumnProperty);
        }

        /// <summary>
        /// Sets the attached property value for the <see cref="ConstrainColumnProperty"/>
        /// </summary>
        /// <param name="obj">Grid Column.</param>
        /// <param name="value">Boolean value.</param>
        public static void SetConstrainColumn(ColumnDefinition obj, bool value)
        {
            obj.SetValue(ConstrainColumnProperty, value);
        }

        /// <summary>
        /// Identifies the <see cref="ConstrainColumnProperty"/> attached property.
        /// </summary>
        /// <returns>The identifier for the ConstrainColumn attached property.</returns>
        public static readonly DependencyProperty ConstrainColumnProperty =
            DependencyProperty.RegisterAttached("ConstrainColumn", typeof(bool), typeof(TabView), new PropertyMetadata(false));
    }
}
