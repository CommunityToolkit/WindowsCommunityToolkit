using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// Provides attached dependency properties that allow a <see cref="UIElement"/>
    /// to conditionally enable or disable its tooltip.
    /// </summary>
    public class ConditionalToolTip : DependencyObject
    {
        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for enabling or disabling the tooltip for a <see cref="UIElement"/>.
        /// </summary>
        public static readonly DependencyProperty IsToolTipEnabledProperty = DependencyProperty.RegisterAttached(
            "IsToolTipEnabled",
            typeof(bool),
            typeof(ConditionalToolTip),
            new PropertyMetadata(true, OnIsToolTipEnabledChanged));

        /// <summary>
        /// Attached <see cref="DependencyProperty"/> for binding for the content to display in the tooltip for a <see cref="UIElement"/>.
        /// </summary>
        public static readonly DependencyProperty ToolTipContentProperty = DependencyProperty.RegisterAttached(
            "ToolTipContent",
            typeof(object),
            typeof(ConditionalToolTip),
            new PropertyMetadata(null, OnContentChanged));

        /// <summary>
        /// Sets the bool determining if the tooltip is enabled for the specified <see cref="UIElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> from which to set the associated IsToolTipEnabled value.</param>
        /// <param name="value">The bool value to assign.</param>
        public static void SetIsToolTipEnabled(UIElement element, bool value) => element.SetValue(IsToolTipEnabledProperty, value);

        /// <summary>
        /// Gets the bool determing if the tooltip is enabled for the specified <see cref="UIElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> from which to get the associated IsToolTipEnabled value.</param>
        /// <returns>True if the conditional tooltip is enabled. False, otherwise.</returns>
        public static bool GetIsToolTipEnabled(UIElement element) => (bool)element.GetValue(IsToolTipEnabledProperty);

        /// <summary>
        /// Sets the content of the conditional tooltip associated with the specified <see cref="UIElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> from which to set the associated conditional tooltip content.</param>
        /// <param name="value">The content to assign.</param>
        public static void SetToolTipContent(UIElement element, object value) => element.SetValue(ToolTipContentProperty, value);

        /// <summary>
        /// Gets the content of the conditional tooltip associated with the specified <see cref="UIElement"/>.
        /// </summary>
        /// <param name="element">The <see cref="UIElement"/> from which to get the associated conditional tooltip content.</param>
        /// <returns>The conditional tooltip's content.</returns>
        public static object GetToolTipContent(UIElement element) => (object)element.GetValue(ToolTipContentProperty);

        private static void OnIsToolTipEnabledChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element)
            {
                ToolTipService.SetToolTip(d, e.NewValue is true
                    ? GetToolTipContent(element)
                    : null);
            }
        }

        private static void OnContentChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is UIElement element && GetIsToolTipEnabled(element))
            {
                ToolTipService.SetToolTip(d, e.NewValue);
            }
        }
    }
}
