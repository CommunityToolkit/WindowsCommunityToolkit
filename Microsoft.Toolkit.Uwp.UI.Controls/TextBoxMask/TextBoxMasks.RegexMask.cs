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

using System;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TextBox mask property allows a user to more easily enter fixed width text in TextBox control
    /// where you would like them to enter the data in a certain format
    /// </summary>
    public partial class TextBoxMasks
    {
        private static void InitRegexMask(TextBox textbox)
        {
            var mask = textbox.GetValue(MaskProperty) as string;
            if (string.IsNullOrWhiteSpace(mask))
            {
                return;
            }

            // an exception should be throw if the regex is not valid
            Regex.Match(string.Empty, mask);

            textbox.TextChanging += Textbox_TextChanging_RegexMask;
            textbox.Paste += Textbox_Paste_RegexMask;
            textbox.SelectionChanged += Textbox_SelectionChanged;
            textbox.SetValue(OldTextProperty, textbox.Text);
        }

        private static async void Textbox_Paste_RegexMask(object sender, TextControlPasteEventArgs e)
        {
            DataPackageView dataPackageView = Clipboard.GetContent();
            if (!dataPackageView.Contains(StandardDataFormats.Text))
            {
                return;
            }

            var pasteText = await dataPackageView.GetTextAsync();
            if (string.IsNullOrWhiteSpace(pasteText))
            {
                return;
            }

            var textbox = sender as TextBox;
            var mask = textbox?.GetValue(MaskProperty) as string;
            if (string.IsNullOrWhiteSpace(mask))
            {
                return;
            }

            // to update the textbox text without triggering TextChanging text
            if (!Regex.IsMatch(pasteText, mask))
            {
                e.Handled = true;
            }

            // Resubscribe to the event
            textbox.SetValue(OldTextProperty, textbox.Text);
        }

        private static void Textbox_TextChanging_RegexMask(TextBox textbox, TextBoxTextChangingEventArgs args)
        {
            var mask = textbox.GetValue(MaskProperty) as string;
            var oldText = textbox.GetValue(OldTextProperty) as string;
            var oldSelectionStart = (int)textbox.GetValue(OldSelectionStartProperty);
            if (string.IsNullOrWhiteSpace(mask) ||
                oldText == null)
            {
                return;
            }

            if (textbox.SelectionStart > 0)
            {
                var selectedChar = textbox.Text[textbox.SelectionStart - 1];

                // checking regex over all textbox text is a tough operation so we check only the selected char
                if (!Regex.IsMatch(selectedChar.ToString(), mask))
                {
                    textbox.Text = oldText;
                    textbox.SelectionStart = oldSelectionStart;
                    return;
                }
            }

            textbox.SetValue(OldTextProperty, textbox.Text);
        }
    }
}
