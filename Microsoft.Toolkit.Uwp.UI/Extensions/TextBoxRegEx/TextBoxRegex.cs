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

using System.Diagnostics;
using System.Text.RegularExpressions;
using Microsoft.Toolkit.Extensions;
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
        private static void TextBoxRegexPropertyOnChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textbox = sender as TextBox;

            if (textbox == null)
            {
                return;
            }

            ValidateTextBox(textbox, false);

            textbox.Loaded -= Textbox_Loaded;
            textbox.LostFocus -= Textbox_LostFocus;
            textbox.TextChanged -= Textbox_TextChanged;
            textbox.Loaded += Textbox_Loaded;
            textbox.LostFocus += Textbox_LostFocus;
            textbox.TextChanged += Textbox_TextChanged;
        }

        private static void Textbox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var textbox = (TextBox)sender;
            ValidateTextBox(textbox, false);
        }

        private static void Textbox_Loaded(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            ValidateTextBox(textbox);
        }

        private static void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            ValidateTextBox(textbox);
        }

        private static void ValidateTextBox(TextBox textbox, bool force = true)
        {
            var validationType = (ValidationType)textbox.GetValue(ValidationTypeProperty);
            string regex;
            bool regexMatch = false;
            switch (validationType)
            {
                default:
                    regex = textbox.GetValue(RegexProperty) as string;
                    if (string.IsNullOrWhiteSpace(regex))
                    {
                        Debug.WriteLine("Regex property can't be null or empty when custom mode is selected");
                        return;
                    }

                    regexMatch = Regex.IsMatch(textbox.Text, regex);
                    break;
                case ValidationType.Decimal:
                    regexMatch = textbox.Text.IsDecimal();
                    break;
                case ValidationType.Email:
                    regexMatch = textbox.Text.IsEmail();
                    break;
                case ValidationType.Number:
                    regexMatch = textbox.Text.IsNumeric();
                    break;
                case ValidationType.PhoneNumber:
                    regexMatch = textbox.Text.IsPhoneNumber();
                    break;
                case ValidationType.Characters:
                    regexMatch = textbox.Text.IsCharacterString();
                    break;
            }

            if (!regexMatch && force)
            {
                var validationModel = (ValidationMode)textbox.GetValue(ValidationModeProperty);
                if (validationModel == ValidationMode.Forced)
                {
                    textbox.Text = string.Empty;
                }
            }

            textbox.SetValue(IsValidProperty, regexMatch);
        }
    }
}
