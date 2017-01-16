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

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Textbox regex extension helps developer to validate a textbox with a regex using the Regex property, IsValid property is updated with the regex validation result, if ValidationMode is Normal only IsValid property is setted if ValidationMode is Forced and the input is not valid the textbox text will be cleared
    /// </summary>
    public partial class TextBoxRegexEx
    {
        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty RegexProperty = DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(TextBoxRegexEx), new PropertyMetadata(null, RegexPropertyOnChange));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid", typeof(bool), typeof(TextBoxRegexEx), new PropertyMetadata(false));

        /// <summary>
        /// 
        /// </summary>
        public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.RegisterAttached("ValidationMode", typeof(ValidationMode), typeof(TextBoxRegexEx), new PropertyMetadata(ValidationMode.Normal));

        public static string GetRegex(DependencyObject obj)
        {
            return (string)obj.GetValue(RegexProperty);
        }

        public static void SetRegex(DependencyObject obj, string value)
        {
            obj.SetValue(RegexProperty, value);
        }

        public static bool GetIsValid(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValidProperty);
        }

        public static void SetValidationMode(DependencyObject obj, string value)
        {
            obj.SetValue(ValidationModeProperty, value);
        }

        public static ValidationMode GetValidationMode(DependencyObject obj)
        {
            return (ValidationMode)obj.GetValue(ValidationModeProperty);
        }
    }
}