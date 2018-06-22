using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Markup;
using System.Windows.Media;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Win32.UI.Controls.WPF
{
    [ContentProperty(nameof(Content))]
    public class InkToolbarCustomToolButton : InkToolbarToolButton
    {
        public InkToolbarCustomToolButton()
            : this("Windows.UI.Xaml.Controls.InkToolbarCustomToolButton")
        {
        }

        public InkToolbarCustomToolButton(string childType)
           : base(childType)
        {
            Bind(nameof(ConfigurationContent), ConfigurationContentProperty, global::Windows.UI.Xaml.Controls.InkToolbarCustomToolButton.ConfigurationContentProperty);
        }

        public static DependencyProperty ConfigurationContentProperty { get; } = DependencyProperty.Register(nameof(ConfigurationContent), typeof(UIElement), typeof(InkToolbarCustomToolButton));

        public UIElement ConfigurationContent { get => (UIElement)GetValue(ConfigurationContentProperty); set => SetValue(ConfigurationContentProperty, value); }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Content)]
        public object Content { get => UwpControl.Content; set => UwpControl.Content = value; }
    }
}
