using System.Diagnostics;
using Microsoft.Toolkit.Uwp.UI.Controls.Data;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Represents a control that contains multiple items and has a header.
    /// </summary>
    [StyleTypedProperty(Property = "ItemContainerStyle", StyleTargetType = typeof(ContentPresenter))]
    public partial class HeaderedItemsControl : ItemsControl
    {
        /// <summary>
        /// Gets or sets a value indicating whether the Header property has been
        /// set to the item of an ItemsControl.
        /// </summary>
        internal bool HeaderIsItem { get; set; }

        /// <summary>
        /// Gets or sets the item that labels the control.
        /// </summary>
        /// <value>
        /// The item that labels the control. The default value is null.
        /// </value>
        public object Header
        {
            get { return GetValue(HeaderProperty); }
            set { SetValue(HeaderProperty, value); }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />
        /// dependency property.
        /// </value>
        /// <remarks>
        /// Note: WPF defines this property via a call to AddOwner of
        /// HeaderedContentControl's HeaderProperty.
        /// </remarks>
        public static readonly DependencyProperty HeaderProperty =
            DependencyProperty.Register(
                "Header",
                typeof(object),
                typeof(HeaderedItemsControl),
                new PropertyMetadata(null, OnHeaderPropertyChanged));

        /// <summary>
        /// HeaderProperty property changed handler.
        /// </summary>
        /// <param name="d">
        /// HeaderedItemsControl that changed its Header.
        /// </param>
        /// <param name="e">Event arguments.</param>
        private static void OnHeaderPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedItemsControl source = d as HeaderedItemsControl;
            Debug.Assert(source != null, "source != null");
            source.OnHeaderChanged(e.OldValue, e.NewValue);
        }

        /// <summary>
        /// Gets or sets a data template that is used to display the contents of
        /// the control's header.
        /// </summary>
        /// <value>
        /// Gets or sets a data template that is used to display the contents of
        /// the control's header. The default is null.
        /// </value>
        public DataTemplate HeaderTemplate
        {
            get { return GetValue(HeaderTemplateProperty) as DataTemplate; }
            set { SetValue(HeaderTemplateProperty, value); }
        }

        /// <summary>
        /// Identifies the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" />
        /// dependency property.
        /// </summary>
        /// <value>
        /// The identifier for the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" />
        /// dependency property.
        /// </value>
        /// <remarks>
        /// Note: WPF defines this property via a call to AddOwner of
        /// HeaderedContentControl's HeaderTemplateProperty.
        /// </remarks>
        public static readonly DependencyProperty HeaderTemplateProperty =
            DependencyProperty.Register(
                "HeaderTemplate",
                typeof(DataTemplate),
                typeof(HeaderedItemsControl),
                new PropertyMetadata(null, OnHeaderTemplatePropertyChanged));

        /// <summary>
        /// HeaderTemplateProperty property changed handler.
        /// </summary>
        /// <param name="d">
        /// HeaderedItemsControl that changed its HeaderTemplate.
        /// </param>
        /// <param name="e">Event arguments.</param>
        private static void OnHeaderTemplatePropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HeaderedItemsControl source = d as HeaderedItemsControl;
            DataTemplate oldHeaderTemplate = e.OldValue as DataTemplate;
            DataTemplate newHeaderTemplate = e.NewValue as DataTemplate;
            Debug.Assert(source != null, "source != null");
            source.OnHeaderTemplateChanged(oldHeaderTemplate, newHeaderTemplate);
        }

        protected override void OnItemContainerStyleChanged(Style oldItemContainerStyle, Style newItemContainerStyle)
        {
            ItemsControlHelper.UpdateItemContainerStyle(newItemContainerStyle);
        }

        /// <summary>
        /// Gets the ItemsControlHelper that is associated with this control.
        /// </summary>
        internal ItemsControlHelper ItemsControlHelper { get; private set; }

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="HeaderedItemsControl"/> class.
        /// </summary>
        public HeaderedItemsControl()
        {
            DefaultStyleKey = typeof(HeaderedItemsControl);
            ItemsControlHelper = new ItemsControlHelper(this);
        }

        /// <summary>
        /// Called when the value of the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />
        /// property changes.
        /// </summary>
        /// <param name="oldHeader">
        /// The old value of the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />
        /// property.
        /// </param>
        /// <param name="newHeader">
        /// The new value of the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.Header" />
        /// property.
        /// </param>
        protected virtual void OnHeaderChanged(object oldHeader, object newHeader)
        {
        }

        /// <summary>
        /// Called when the value of the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" />
        /// property changes.
        /// </summary>
        /// <param name="oldHeaderTemplate">
        /// The old value of the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" />
        /// property.
        /// </param>
        /// <param name="newHeaderTemplate">
        /// The new value of the
        /// <see cref="P:System.Windows.Controls.HeaderedItemsControl.HeaderTemplate" />
        /// property.
        /// </param>
        protected virtual void OnHeaderTemplateChanged(DataTemplate oldHeaderTemplate, DataTemplate newHeaderTemplate)
        {
        }

        /// <summary>
        /// Builds the visual tree for the
        /// <see cref="T:System.Windows.Controls.HeaderedItemsControl" /> when a
        /// new template is applied.
        /// </summary>
        protected override void OnApplyTemplate()
        {
            ItemsControlHelper.OnApplyTemplate();
            base.OnApplyTemplate();
        }

        /// <summary>
        /// Prepares the specified element to display the specified item.
        /// </summary>
        /// <param name="element">
        /// The container element used to display the specified item.
        /// </param>
        /// <param name="item">The content to display.</param>
        protected override void PrepareContainerForItemOverride(DependencyObject element, object item)
        {
            ItemsControlHelper.PrepareContainerForItemOverride(element, ItemContainerStyle);
            PrepareHeaderedItemsControlContainerForItemOverride(element, item, this, ItemContainerStyle);

            base.PrepareContainerForItemOverride(element, item);
        }

        /// <summary>
        /// Prepares the specified container to display the specified item.
        /// </summary>
        /// <param name="element">
        /// Container element used to display the specified item.
        /// </param>
        /// <param name="item">Specified item to display.</param>
        /// <param name="parent">The parent ItemsControl.</param>
        /// <param name="parentItemContainerStyle">
        /// The ItemContainerStyle for the parent ItemsControl.
        /// </param>
        internal static void PrepareHeaderedItemsControlContainerForItemOverride(DependencyObject element, object item, ItemsControl parent, Style parentItemContainerStyle)
        {
            HeaderedItemsControl headeredItemsControl = element as HeaderedItemsControl;
            if (headeredItemsControl != null)
            {
                PrepareHeaderedItemsControlContainer(headeredItemsControl, item, parent, parentItemContainerStyle);
            }
        }

        /// <summary>
        /// Prepare a PrepareHeaderedItemsControlContainer container for an
        /// item.
        /// </summary>
        /// <param name="control">Container to prepare.</param>
        /// <param name="item">Item to be placed in the container.</param>
        /// <param name="parentItemsControl">The parent ItemsControl.</param>
        /// <param name="parentItemContainerStyle">
        /// The ItemContainerStyle for the parent ItemsControl.
        /// </param>
        private static void PrepareHeaderedItemsControlContainer(HeaderedItemsControl control, object item, ItemsControl parentItemsControl, Style parentItemContainerStyle)
        {
            if (control == item)
            {
                return;
            }

            // Copy the ItemsControl properties from parent to child
            DataTemplate parentItemTemplate = parentItemsControl.ItemTemplate;
            if (parentItemTemplate != null)
            {
                control.SetValue(ItemTemplateProperty, parentItemTemplate);
            }

            if (parentItemContainerStyle != null && HasDefaultValue(control, ItemContainerStyleProperty))
            {
                control.SetValue(ItemContainerStyleProperty, parentItemContainerStyle);
            }

            // Copy the Header properties from parent to child
            if (control.HeaderIsItem || HasDefaultValue(control, HeaderProperty))
            {
                control.Header = item;
                control.HeaderIsItem = true;
            }

            if (parentItemTemplate != null)
            {
                control.SetValue(HeaderTemplateProperty, parentItemTemplate);
            }

            if (parentItemContainerStyle != null && control.Style == null)
            {
                control.SetValue(StyleProperty, parentItemContainerStyle);
            }

            // Note: this is where we would apply the HeaderTemplateSelector
            // (if implemented) or attempt to lookup the implicit template
            // for the type of the item if the headerTemplate were null.

            // Setup a hierarchical template
            DataTemplate headerTemplate = parentItemTemplate;
            if (headerTemplate == null)
            {
                return;
            }

            HierarchicalDataTemplate hierarchy = DataTemplateExtensions.GetHierarchy(headerTemplate);

            if (hierarchy?.ItemsSource != null && HasDefaultValue(control, ItemsSourceProperty))
            {
                control.SetBinding(
                    ItemsSourceProperty,
                    new Binding
                    {
                        Converter = hierarchy.ItemsSource.Converter,
                        ConverterLanguage = hierarchy.ItemsSource.ConverterLanguage,
                        ConverterParameter = hierarchy.ItemsSource.ConverterParameter,
                        Mode = hierarchy.ItemsSource.Mode,
                        Path = hierarchy.ItemsSource.Path,
                        Source = control.Header
                    });
            }

            if (hierarchy != null && hierarchy.IsItemTemplateSet && control.ItemTemplate == parentItemTemplate)
            {
                control.ClearValue(ItemTemplateProperty);
                if (hierarchy.ItemTemplate != null)
                {
                    control.ItemTemplate = hierarchy.ItemTemplate;
                }
            }

            if (hierarchy != null && hierarchy.IsItemContainerStyleSet && control.ItemContainerStyle == parentItemContainerStyle)
            {
                control.ClearValue(ItemContainerStyleProperty);
                if (hierarchy.ItemContainerStyle != null)
                {
                    control.ItemContainerStyle = hierarchy.ItemContainerStyle;
                }
            }
        }

        /// <summary>
        /// Check whether a control has the default value for a property.
        /// </summary>
        /// <param name="control">The control to check.</param>
        /// <param name="property">The property to check.</param>
        /// <returns>
        /// True if the property has the default value; false otherwise.
        /// </returns>
        private static bool HasDefaultValue(Control control, DependencyProperty property)
        {
            Debug.Assert(control != null, "control should not be null!");
            Debug.Assert(property != null, "property should not be null!");
            return control.ReadLocalValue(property) == DependencyProperty.UnsetValue;
        }
    }
}