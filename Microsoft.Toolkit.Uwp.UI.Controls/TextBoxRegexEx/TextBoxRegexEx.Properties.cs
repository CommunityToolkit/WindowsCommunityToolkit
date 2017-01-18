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
        /// The regex expression that will be validated on the textbox
        /// </summary>
        public static readonly DependencyProperty RegexProperty = DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(TextBoxRegexEx), new PropertyMetadata(null, TextBoxRegexExPropertyOnChange));

        /// <summary>
        /// Gets the result of the textbox validation agains the Regex property
        /// </summary>
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid", typeof(bool), typeof(TextBoxRegexEx), new PropertyMetadata(false));

        /// <summary>
        /// The validation mode of the validation extension (Normal, Forced)
        /// </summary>
        public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.RegisterAttached("ValidationMode", typeof(ValidationMode), typeof(TextBoxRegexEx), new PropertyMetadata(ValidationMode.Normal, TextBoxRegexExPropertyOnChange));

        /// <summary>
        /// The validation types are predefined validation that can be used directly
        /// </summary>
        public static readonly DependencyProperty ValidationTypeProperty = DependencyProperty.RegisterAttached("ValidationType", typeof(ValidationType), typeof(TextBoxRegexEx), new PropertyMetadata(ValidationType.Custom, TextBoxRegexExPropertyOnChange));

        /// <summary>
        /// Gets the Regex property
        /// </summary>
        /// <param name="obj">TextBox Control</param>
        /// <returns>Regex Value</returns>
        public static string GetRegex(DependencyObject obj)
        {
            return (string)obj.GetValue(RegexProperty);
        }

        /// <summary>
        /// Set Regex property
        /// </summary>
        /// <param name="obj">TextBox control</param>
        /// <param name="value">Regex value</param>
        public static void SetRegex(DependencyObject obj, string value)
        {
            obj.SetValue(RegexProperty, value);
        }

        /// <summary>
        /// Gets the Regex property
        /// </summary>
        /// <param name="obj">TextBox Control</param>
        /// <returns>Regex Value</returns>
        public static bool GetIsValid(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValidProperty);
        }

        /// <summary>
        /// Set IsValid property
        /// </summary>
        /// <param name="obj">TextBox control</param>
        /// <param name="value">IsValid value</param>
        public static void SetIsValid(DependencyObject obj, bool value)
        {
            obj.SetValue(ValidationModeProperty, value);
        }

        /// <summary>
        /// Gets ValidationMode property
        /// </summary>
        /// <param name="obj">TextBox Control</param>
        /// <returns>ValidationMode Value</returns>
        public static ValidationMode GetValidationMode(DependencyObject obj)
        {
            return (ValidationMode)obj.GetValue(ValidationModeProperty);
        }

        /// <summary>
        /// Set ValidationMode property
        /// </summary>
        /// <param name="obj">TextBox control</param>
        /// <param name="value">ValidationMode value</param>
        public static void SetValidationMode(DependencyObject obj, ValidationMode value)
        {
            obj.SetValue(ValidationModeProperty, value);
        }

        /// <summary>
        /// Gets ValidationType property
        /// </summary>
        /// <param name="obj">TextBox Control</param>
        /// <returns>ValidationType Value</returns>
        public static ValidationType GetValidationType(DependencyObject obj)
        {
            return (ValidationType)obj.GetValue(ValidationTypeProperty);
        }

        /// <summary>
        /// Set ValidationType property
        /// </summary>
        /// <param name="obj">TextBox control</param>
        /// <param name="value">ValidationType value</param>
        public static void SetValidationType(DependencyObject obj, ValidationType value)
        {
            obj.SetValue(ValidationTypeProperty, value);
        }
    }
}