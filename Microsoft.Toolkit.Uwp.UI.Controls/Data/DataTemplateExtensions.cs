using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Data
{
    /// <summary>
    /// Implements the Hierarchy attached property of type <see cref="HierarchicalDataTemplate"/>
    /// that is used in hierarchical controls such as <see cref="TreeView"/>
    /// to define the bindings and templates used to visualize the hierarchy.
    /// </summary>
    public static class DataTemplateExtensions
    {
        /// <summary>
        /// Hierarchy Attached Dependency Property
        /// </summary>
        public static readonly DependencyProperty HierarchyProperty =
            DependencyProperty.RegisterAttached(
                "Hierarchy",
                typeof(HierarchicalDataTemplate),
                typeof(DataTemplateExtensions),
                new PropertyMetadata(null, OnHierarchyChanged));

        /// <summary>
        /// Gets the Hierarchy property. This dependency property
        /// indicates the hierarchical template extensions to use for data-bound hierarchical controls.
        /// </summary>
        /// <returns></returns>
        public static HierarchicalDataTemplate GetHierarchy(DependencyObject d)
        {
            return (HierarchicalDataTemplate)d.GetValue(HierarchyProperty);
        }

        /// <summary>
        /// Sets the Hierarchy property. This dependency property
        /// indicates the hierarchical template extensions to use for data-bound hierarchical controls.
        /// </summary>
        public static void SetHierarchy(DependencyObject d, HierarchicalDataTemplate value)
        {
            d.SetValue(HierarchyProperty, value);
        }

        /// <summary>
        /// Handles changes to the Hierarchy property.
        /// </summary>
        /// <param name="d">
        /// The <see cref="DependencyObject"/> on which
        /// the property has changed value.
        /// </param>
        /// <param name="e">
        /// Event data that is issued by any event that
        /// tracks changes to the effective value of this property.
        /// </param>
        private static void OnHierarchyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            HierarchicalDataTemplate oldHierarchy = (HierarchicalDataTemplate)e.OldValue;
            HierarchicalDataTemplate newHierarchy = (HierarchicalDataTemplate)d.GetValue(HierarchyProperty);
        }
    }
}
