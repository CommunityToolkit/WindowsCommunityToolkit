// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
    /// If <see cref="ValidationMode"> is set to Forced then IsValid will be set according to whether the regex is valid, when TextBox lose focus and in case the textbox is invalid clear its value. </see>
    /// If <see cref="ValidationMode"> is set to Dynamic then IsValid will be set according to whether the regex is valid. If the newest charachter is invalid, only invalid character of the Textbox will be deleted.</see>
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
            var validationMode = (ValidationMode)textbox.GetValue(ValidationModeProperty);
            ValidateTextBox(textbox, validationMode == ValidationMode.Dynamic);
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
                if (!string.IsNullOrEmpty(textbox.Text))
                {
                    var validationModel = (ValidationMode)textbox.GetValue(ValidationModeProperty);
                    if (validationModel == ValidationMode.Forced)
                    {
                        textbox.Text = string.Empty;
                    }
                    else if (validationType != ValidationType.Email && validationType != ValidationType.PhoneNumber)
                    {
                        if (validationModel == ValidationMode.Dynamic)
                        {
                            int selectionStart = textbox.SelectionStart - 1;
                            textbox.Text = textbox.Text.Remove(selectionStart, 1);
                            textbox.SelectionStart = selectionStart;
                        }
                    }
                }
            }

            textbox.SetValue(IsValidProperty, regexMatch);
        }
    }
}
