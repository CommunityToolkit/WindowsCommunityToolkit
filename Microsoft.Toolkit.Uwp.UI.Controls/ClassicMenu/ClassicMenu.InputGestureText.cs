using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Classic Menu Control defines a menu of choices for users to invoke.
    /// </summary>
    public partial class ClassicMenu
    {
        /// <summary>
        /// Sets the text describing an input gesture that will call the command tied to the specified item.
        /// </summary>
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

        /// <summary>
        /// Gets InputGestureText attached property
        /// </summary>
        /// <param name="obj">Target MenuFlyoutItem</param>
        /// <returns>Input gesture text</returns>
        public static string GetInputGestureText(MenuFlyoutItem obj)
        {
            return (string)obj.GetValue(InputGestureTextProperty);
        }

        /// <summary>
        /// Sets InputGestureText attached property
        /// </summary>
        /// <param name="obj">Target MenuFlyoutItem</param>
        /// <param name="value">Input gesture text</param>
        public static void SetInputGestureText(MenuFlyoutItem obj, string value)
        {
            obj.SetValue(InputGestureTextProperty, value);
        }
    }
}
