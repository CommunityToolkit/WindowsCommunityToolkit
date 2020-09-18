using System;
using System.Collections.Generic;
using System.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace FlexPanelTest
{
    partial class EnumPicker : ComboBox
    {
        public static readonly DependencyProperty EnumTypeProperty =
            DependencyProperty.Register("EnumType", typeof(Type), typeof(EnumPicker), new PropertyMetadata(null, OnEnumTypeChanged));

        private static void OnEnumTypeChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (d is EnumPicker picker)
            {
                if (e.OldValue != null)
                {
                    picker.ItemsSource = null;
                }
                if (e.NewValue != null)
                {
                    if (!((Type)e.NewValue).IsEnum)
                        throw new ArgumentException("EnumPicker: EnumType property must be enumeration type");
                    picker.ItemsSource = Enum.GetValues((Type)e.NewValue);
                }
            }
        }

        public Type EnumType
        {
            set => SetValue(EnumTypeProperty, value);
            get => (Type)GetValue(EnumTypeProperty);
        }

        
    }
}
