using System;
using System.Windows;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    public class InkToolbarCustomPenButton : InkToolbarPenButton
    {
        public InkToolbarCustomPenButton()
            : this("Windows.UI.Xaml.Controls.InkToolbarCustomPenButton")
        {
        }

        public InkToolbarCustomPenButton(string typeName)
           : base(typeName)
        {
            Bind(nameof(CustomPen), CustomPenProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.CustomPenProperty);
            Bind(nameof(ConfigurationContent), ConfigurationContentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomPenButton.ConfigurationContentProperty);
        }

        public static DependencyProperty ConfigurationContentProperty { get; } = DependencyProperty.Register(nameof(ConfigurationContent), typeof(UIElement), typeof(InkToolbarCustomPenButton));

        public static DependencyProperty CustomPenProperty { get; } = DependencyProperty.Register(nameof(InkToolbarCustomPen), typeof(UIElement), typeof(InkToolbarCustomPenButton));

        public InkToolbarCustomPen CustomPen { get => (InkToolbarCustomPen)GetValue(CustomPenProperty); set => SetValue(CustomPenProperty, value); }

        public UIElement ConfigurationContent { get => (UIElement)GetValue(ConfigurationContentProperty); set => SetValue(ConfigurationContentProperty, value); }
    }
}
