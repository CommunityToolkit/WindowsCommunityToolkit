using Windows.Foundation.Metadata;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    [Deprecated("The Blade class has been replaced with the BladeItem class. Please use that going forward", DeprecationType.Deprecate, 1)]
    public class Blade : BladeItem
    {
        /// <summary>
        /// Gets or sets the visual content of this blade
        /// </summary>
        [Deprecated("This property has been replaced with the Content property of the control. It is no longer required to place content within the Element property.", DeprecationType.Deprecate, 1)]
        public UIElement Element
        {
            get { return (UIElement)GetValue(ElementProperty); }
            set { SetValue(ElementProperty, value); }
        }

        /// <summary>
        /// Identifies the <see cref="Element"/> dependency property.
        /// </summary>
        [Deprecated("This property has been replaced with the Content property of the control. It is no longer required to place content within the Element property.", DeprecationType.Deprecate, 1)]
        public static readonly DependencyProperty ElementProperty = DependencyProperty.Register(nameof(Element), typeof(UIElement), typeof(Blade), new PropertyMetadata(null, OnElementChanged));

        private static void OnElementChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var blade = (Blade) d;
            blade.Content = e.NewValue;
        }
    }
}
