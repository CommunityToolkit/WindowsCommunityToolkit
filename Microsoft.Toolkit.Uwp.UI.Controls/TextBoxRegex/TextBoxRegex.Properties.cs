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
    /// TextBoxRegex allows text validation using a regular expression.
    /// </summary>
    /// <remarks>
    /// If<see cref="ValidationMode"> is set to Normal then IsValid will be set according to either the regex is valid.</see>
    /// If<see cref="ValidationMode"> is set to Forced and the input is not valid the TextBox text will be cleared.</see>
    /// </remarks>
    public partial class TextBoxRegex
    {
        /// <summary>
        /// The regex expression that will be validated on the textbox
        /// </summary>
        public static readonly DependencyProperty RegexProperty = DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(TextBoxRegex), new PropertyMetadata(null, TextBoxRegexPropertyOnChange));

        /// <summary>
        /// Gets the result of the TextBoxRegex validation again the Regex property
        /// </summary>
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid", typeof(bool), typeof(TextBoxRegex), new PropertyMetadata(false));

        /// <summary>
        /// The validation mode of the validation extension (Normal, Forced)
        /// </summary>
        public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.RegisterAttached("ValidationMode", typeof(ValidationMode), typeof(TextBoxRegex), new PropertyMetadata(ValidationMode.Normal, TextBoxRegexPropertyOnChange));

        /// <summary>
        /// The validation types are predefined validation that can be used directly
        /// </summary>
        public static readonly DependencyProperty ValidationTypeProperty = DependencyProperty.RegisterAttached("ValidationType", typeof(ValidationType), typeof(TextBoxRegex), new PropertyMetadata(ValidationType.Custom, TextBoxRegexPropertyOnChange));

        /// <summary>
        /// Gets the Regex property
        /// </summary>
        /// <param name="obj">TextBox to get Regex property from.</param>
        /// <returns>The regular expression assigned to the TextBox</returns>
        public static string GetRegex(DependencyObject obj)
        {
            return (string)obj.GetValue(RegexProperty);
        }

        /// <summary>
        /// Set Regex property
        /// </summary>
        /// <param name="obj">The TextBox to set the regular expression on</param>
        /// <param name="value">Regex value</param>
        public static void SetRegex(DependencyObject obj, string value)
        {
            obj.SetValue(RegexProperty, value);
        }

        /// <summary>
        /// Gets a value indicating if the Text is valid according to the Regex property.
        /// </summary>
        /// <param name="obj">TextBox to be validated.</param>
        /// <returns>TextBox regular expression validation result</returns>
        public static bool GetIsValid(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsValidProperty);
        }

        /// <summary>
        /// Sets the IsValid property.
        /// </summary>
        /// <param name="obj">TextBox to be assigned the property</param>
        /// <param name="value">A value indicating if the Text is valid according to the Regex property.</param>
        public static void SetIsValid(DependencyObject obj, bool value)
        {
            obj.SetValue(ValidationModeProperty, value);
        }

        /// <summary>
        /// Gets <see cref="ValidationMode"/> property
        /// </summary>
        /// <param name="obj">TextBox to get the <see cref="ValidationMode"/> from</param>
        /// <returns>TextBox <see cref="ValidationMode"/> value</returns>
        public static ValidationMode GetValidationMode(DependencyObject obj)
        {
            return (ValidationMode)obj.GetValue(ValidationModeProperty);
        }

        /// <summary>
        /// Set <see cref="ValidationMode"/> property
        /// </summary>
        /// <param name="obj">TextBox to set the <see cref="ValidationMode"/> on.</param>
        /// <param name="value">TextBox <see cref="ValidationMode"/> value</param>
        public static void SetValidationMode(DependencyObject obj, ValidationMode value)
        {
            obj.SetValue(ValidationModeProperty, value);
        }

        /// <summary>
        /// Gets <see cref="ValidationType"/> property
        /// </summary>
        /// <param name="obj">TextBox to get <see cref="ValidationType"/> from.</param>
        /// <returns>TextBox <see cref="ValidationType"/> Value</returns>
        public static ValidationType GetValidationType(DependencyObject obj)
        {
            return (ValidationType)obj.GetValue(ValidationTypeProperty);
        }

        /// <summary>
        /// Set <see cref="ValidationType"/> property
        /// </summary>
        /// <param name="obj">TextBox to set the <see cref="ValidationType"/> on.</param>
        /// <param name="value">TextBox <see cref="ValidationType"/> value</param>
        public static void SetValidationType(DependencyObject obj, ValidationType value)
        {
            obj.SetValue(ValidationTypeProperty, value);
        }
    }
}