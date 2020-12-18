using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// Attached properties to support attaching custom pipelines to UI elements.
    /// </summary>
    public static class UIElementExtensions
    {
        /// <summary>
        /// Identifies the Visual XAML attached property.
        /// </summary>
        public static readonly DependencyProperty VisualProperty = DependencyProperty.RegisterAttached(
            "Visual",
            typeof(PipelineVisual),
            typeof(UIElementExtensions),
            new PropertyMetadata(null, OnVisualPropertyChanged));

        /// <summary>
        /// Gets the value of <see cref="VisualProperty"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to get the value for.</param>
        /// <returns>The retrieved <see cref="PipelineVisual"/> item.</returns>
        public static PipelineVisual GetVisual(UIElement element)
        {
            return (PipelineVisual)element.GetValue(VisualProperty);
        }

        /// <summary>
        /// Sets the value of <see cref="VisualProperty"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> to set the value for.</param>
        /// <param name="value">The <see cref="PipelineVisual"/> value to set.</param>
        public static void SetVisual(UIElement element, PipelineVisual value)
        {
            element.SetValue(VisualProperty, value);
        }

        /// <summary>
        /// Callback to apply the visual for <see cref="VisualProperty"/>.
        /// </summary>
        /// <param name="d">The target object the property was changed for.</param>
        /// <param name="e">The <see cref="DependencyPropertyChangedEventArgs"/> instance for the current event.</param>
        private static async void OnVisualPropertyChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var element = (UIElement)d;

            await ((PipelineVisual)e.NewValue).GetPipeline().AttachAsync(element, element);
        }
    }
}
