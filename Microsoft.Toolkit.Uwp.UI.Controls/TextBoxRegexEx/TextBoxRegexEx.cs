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

using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Textbox regex extension helps developer to validate a textbox with a regex using the Regex property, IsValid property is updated with the regex validation result, if ValidationMode is Normal only IsValid property is setted if ValidationMode is Forced and the input is not valid the textbox text will be cleared
    /// </summary>
    public partial class TextBoxRegexEx
    {
        private static void RegexPropertyOnChange(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            var textbox = sender as TextBox;

            var regex = textbox?.GetValue(RegexProperty) as string;
            if (string.IsNullOrWhiteSpace(regex))
            {
                return;
            }

            ValidateTextBox(textbox, regex);
            textbox.LostFocus -= Textbox_LostFocus;
            textbox.LostFocus += Textbox_LostFocus;
        }

        private static void Textbox_LostFocus(object sender, RoutedEventArgs e)
        {
            var textbox = (TextBox)sender;
            var regex = textbox.GetValue(RegexProperty) as string;
            if (string.IsNullOrWhiteSpace(regex))
            {
                return;
            }

            ValidateTextBox(textbox, regex);
        }

        private static void ValidateTextBox(TextBox textbox, string regex)
        {
            if (Regex.IsMatch(textbox.Text, regex))
            {
                textbox.SetValue(IsValidProperty, true);
            }
            else
            {
                var validationModel = (ValidationMode)textbox.GetValue(ValidationModeProperty);
                if (validationModel == ValidationMode.Forced)
                {
                    textbox.Text = string.Empty;
                }

                textbox.SetValue(IsValidProperty, false);
            }
        }
    }
}