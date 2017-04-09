using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Input;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    public partial class ClassicMenu
    {
        public static readonly DependencyProperty InputGestureTextProperty = DependencyProperty.RegisterAttached("InputGestureText", typeof(string), typeof(MenuFlyoutItem), new PropertyMetadata(null, InputGestureTextChanged));

        private static void InputGestureTextChanged(DependencyObject obj, DependencyPropertyChangedEventArgs e)
        {
            var inputGestureValue = e.NewValue as string;
            if (string.IsNullOrEmpty(inputGestureValue))
            {
                return;
            }

            inputGestureValue = inputGestureValue.ToUpper();

            if (MenuItemInputGestureCache.ContainsKey(inputGestureValue))
            {
                return;
            }

            var menuItem = (MenuFlyoutItem)obj;
            MenuItemInputGestureCache.Add(inputGestureValue.ToUpper(), menuItem);
        }

        public static string GetInputGestureText(MenuFlyoutItem obj)
        {
            return (string)obj.GetValue(InputGestureTextProperty);
        }

        public static void SetInputGestureText(MenuFlyoutItem obj, string value)
        {
            obj.SetValue(InputGestureTextProperty, value);
        }
    }
}
