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
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Windows.ApplicationModel.DataTransfer;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// TextBox Mask property allows a user to more easily enter fixed width text in TextBox control
    /// where you would like them to enter the data in a certain format
    /// </summary>
    public partial class TextBoxMasks
    {
        private static void OnRegexMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;

            if (textbox == null)
            {
                return;
            }

            textbox.TextChanging -= Textbox_TextChanging_RegexMask;
            textbox.Paste -= Textbox_Paste_RegexMask;

            var mask = textbox.GetValue(MaskProperty) as string;
            if (!string.IsNullOrWhiteSpace(mask))
            {
                throw new ArgumentException("Mask property can't be used with RegexMask property");
            }

            var regexMask = textbox.GetValue(RegexMaskProperty) as string;
            if (string.IsNullOrWhiteSpace(regexMask))
            {
                return;
            }

            // an exception should be throw if the regex is not valid
            Regex.Match(string.Empty, regexMask);

            textbox.TextChanging += Textbox_TextChanging_RegexMask;
            textbox.Paste += Textbox_Paste_RegexMask;
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
            var regexMask = textbox?.GetValue(RegexMaskProperty) as string;
            if (string.IsNullOrWhiteSpace(regexMask))
            {
                return;
            }

            // to update the textbox text without triggering TextChanging text
            if (!Regex.IsMatch(pasteText, regexMask))
            {
                e.Handled = true;
            }

            // Resubscribe to the event
            textbox.SetValue(OldTextProperty, textbox.Text);
        }

        private static void Textbox_TextChanging_RegexMask(TextBox textbox, TextBoxTextChangingEventArgs args)
        {
            var regexMask = textbox.GetValue(RegexMaskProperty) as string;
            var oldText = textbox.GetValue(OldTextProperty) as string;
            if (string.IsNullOrWhiteSpace(regexMask) ||
                oldText == null)
            {
                return;
            }

            // in this event regex ismatch shouldn't called on a big text so we need to apply it only on the difference because matching the whole text make the application don't respond to inputs.
            if (!Regex.IsMatch(textbox.Text, regexMask) && !string.IsNullOrEmpty(textbox.Text))
            {
                var oldSelectionStart = textbox.SelectionStart;
                textbox.Text = oldText;
                textbox.SelectionStart = oldSelectionStart;
            }

            textbox.SetValue(OldTextProperty, textbox.Text);
        }
    }
}
