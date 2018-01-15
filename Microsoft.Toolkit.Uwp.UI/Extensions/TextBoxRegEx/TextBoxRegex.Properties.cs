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

namespace Microsoft.Toolkit.Uwp.UI.Extensions
{
    /// <summary>
    /// TextBoxRegex allows text validation using a regular expression.
    /// </summary>
    /// <remarks>
    /// If <see cref="ValidationMode"> is set to Normal then IsValid will be set according to whether the regex is valid.</see>
    /// If <see cref="ValidationMode"> is set to Forced and the input is not valid the TextBox text will be cleared.</see>
    /// </remarks>
    public partial class TextBoxRegex
    {
        /// <summary>
        /// Identifies the Regex attached dependency property.
        /// </summary>
        public static readonly DependencyProperty RegexProperty = DependencyProperty.RegisterAttached("Regex", typeof(string), typeof(TextBoxRegex), new PropertyMetadata(null, TextBoxRegexPropertyOnChange));

        /// <summary>
        /// Identifies the IsValid attached dependency property.
        /// </summary>
        public static readonly DependencyProperty IsValidProperty = DependencyProperty.RegisterAttached("IsValid", typeof(bool), typeof(TextBoxRegex), new PropertyMetadata(false));

        /// <summary>
        /// Identifies the ValidationMode attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationModeProperty = DependencyProperty.RegisterAttached("ValidationMode", typeof(ValidationMode), typeof(TextBoxRegex), new PropertyMetadata(ValidationMode.Normal, TextBoxRegexPropertyOnChange));

        /// <summary>
        /// Identifies the ValidationType attached dependency property.
        /// </summary>
        public static readonly DependencyProperty ValidationTypeProperty = DependencyProperty.RegisterAttached("ValidationType", typeof(ValidationType), typeof(TextBoxRegex), new PropertyMetadata(ValidationType.Custom, TextBoxRegexPropertyOnChange));

        /// <summary>
        /// Gets the value of the TextBoxRegex.Regex XAML attached property from the specified TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to get Regex property from.</param>
        /// <returns>The regular expression assigned to the TextBox</returns>
        public static string GetRegex(TextBox textBox)
        {
            return (string)textBox.GetValue(RegexProperty);
        }

        /// <summary>
        /// Sets the value of the TextBoxRegex.Regex XAML attached property for a target TextBox.
        /// </summary>
        /// <param name="textBox">The TextBox to set the regular expression on</param>
        /// <param name="value">Regex value</param>
        public static void SetRegex(TextBox textBox, string value)
        {
            textBox.SetValue(RegexProperty, value);
        }

        /// <summary>
        /// Gets the value of the TextBoxRegex.IsValid XAML attached property from the specified TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to be validated.</param>
        /// <returns>TextBox regular expression validation result</returns>
        public static bool GetIsValid(TextBox textBox)
        {
            return (bool)textBox.GetValue(IsValidProperty);
        }

        /// <summary>
        /// Sets the value of the TextBoxRegex.IsValid XAML attached property for a target TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to be assigned the property</param>
        /// <param name="value">A value indicating if the Text is valid according to the Regex property.</param>
        public static void SetIsValid(TextBox textBox, bool value)
        {
            textBox.SetValue(ValidationModeProperty, value);
        }

        /// <summary>
        /// Gets the value of the TextBoxRegex.ValidationMode XAML attached property from the specified TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to get the <see cref="ValidationMode"/> from</param>
        /// <returns>TextBox <see cref="ValidationMode"/> value</returns>
        public static ValidationMode GetValidationMode(TextBox textBox)
        {
            return (ValidationMode)textBox.GetValue(ValidationModeProperty);
        }

        /// <summary>
        /// Sets the value of the TextBoxRegex.ValidationMode XAML attached property for a target TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to set the <see cref="ValidationMode"/> on.</param>
        /// <param name="value">TextBox <see cref="ValidationMode"/> value</param>
        public static void SetValidationMode(TextBox textBox, ValidationMode value)
        {
            textBox.SetValue(ValidationModeProperty, value);
        }

        /// <summary>
        /// Gets the value of the TextBoxRegex.ValidationType XAML attached property from the specified TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to get <see cref="ValidationType"/> from.</param>
        /// <returns>TextBox <see cref="ValidationType"/> Value</returns>
        public static ValidationType GetValidationType(TextBox textBox)
        {
            return (ValidationType)textBox.GetValue(ValidationTypeProperty);
        }

        /// <summary>
        /// Sets the value of the TextBoxRegex.ValidationType XAML attached property for a target TextBox.
        /// </summary>
        /// <param name="textBox">TextBox to set the <see cref="ValidationType"/> on.</param>
        /// <param name="value">TextBox <see cref="ValidationType"/> value</param>
        public static void SetValidationType(TextBox textBox, ValidationType value)
        {
            textBox.SetValue(ValidationTypeProperty, value);
        }
    }
}