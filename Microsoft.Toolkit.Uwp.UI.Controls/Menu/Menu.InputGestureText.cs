// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Menu Control defines a menu of choices for users to invoke.
    /// </summary>
    public partial class Menu
    {
        /// <summary>
        /// Sets the text describing an input gesture that will call the command tied to the specified item.
        /// </summary>
        public static readonly DependencyProperty InputGestureTextProperty = DependencyProperty.RegisterAttached("InputGestureText", typeof(string), typeof(DependencyObject), new PropertyMetadata(null, InputGestureTextChanged));

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

            MenuItemInputGestureCache.Add(inputGestureValue.ToUpper(), obj);
        }

        /// <summary>
        /// Gets InputGestureText attached property
        /// </summary>
        /// <param name="obj">Target MenuFlyoutItem</param>
        /// <returns>Input gesture text</returns>
        public static string GetInputGestureText(DependencyObject obj)
        {
            return (string)obj.GetValue(InputGestureTextProperty);
        }

        /// <summary>
        /// Sets InputGestureText attached property
        /// </summary>
        /// <param name="obj">Target MenuFlyoutItem</param>
        /// <param name="value">Input gesture text</param>
        public static void SetInputGestureText(DependencyObject obj, string value)
        {
            obj.SetValue(InputGestureTextProperty, value);
        }
    }
}
