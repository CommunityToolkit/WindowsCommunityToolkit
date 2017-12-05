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
        private const string DecimalRegex = "^-?[0-9]{1,28}([.,][0-9]{1,28})?$";
        private const string EmailRegex = @"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,})+)$";
        private const string NumberRegex = "^-?[0-9]{1,9}$";
        private const string PhoneNumberRegex = @"^\s*\+?\s*([0-9][\s-]*){9,}$";
        private const string CharactersRegex = "^[A-Za-z]+$";

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
            switch (validationType)
            {
                default:
                    regex = textbox.GetValue(RegexProperty) as string;
                    if (string.IsNullOrWhiteSpace(regex))
                    {
                        Debug.WriteLine("Regex property can't be null or empty when custom mode is selected");
                        return;
                    }

                    break;
                case ValidationType.Decimal:
                    regex = DecimalRegex;
                    break;
                case ValidationType.Email:
                    regex = EmailRegex;
                    break;
                case ValidationType.Number:
                    regex = NumberRegex;
                    break;
                case ValidationType.PhoneNumber:
                    regex = PhoneNumberRegex;
                    break;
                case ValidationType.Characters:
                    regex = CharactersRegex;
                    break;
            }

            if (Regex.IsMatch(textbox.Text, regex))
            {
                textbox.SetValue(IsValidProperty, true);
            }
            else
            {
                if (force)
                {
                    var validationModel = (ValidationMode)textbox.GetValue(ValidationModeProperty);
                    if (validationModel == ValidationMode.Forced)
                    {
                        textbox.Text = string.Empty;
                    }
                }

                textbox.SetValue(IsValidProperty, false);
            }
        }
    }
}
