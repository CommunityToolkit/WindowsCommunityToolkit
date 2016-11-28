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
        private const string DefaultPlaceHolder = "_";
        private static readonly KeyValuePair<char, string> AlphaCharacterRepresentation = new KeyValuePair<char, string>('a', "[A-Za-z]");
        private static readonly KeyValuePair<char, string> NumericCharacterRepresentation = new KeyValuePair<char, string>('9', "[0-9]");
        private static readonly KeyValuePair<char, string> AlphaNumericRepresentation = new KeyValuePair<char, string>('*', "[A-Za-z0-9]");

        private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InitTextBoxMask(d, e);
        }

        private static void OnPlaceHolderChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InitTextBoxMask(d, e);
        }

        private static void OnCustomMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            InitTextBoxMask(d, e);
        }

        private static void InitTextBoxMask(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;

            if (textbox == null)
            {
                return;
            }

            textbox.TextChanging -= Textbox_TextChanging;
            textbox.SelectionChanged -= Textbox_SelectionChanged;
            textbox.Paste -= Textbox_Paste;

            var regexMask = textbox.GetValue(RegexMaskProperty) as string;
            if (!string.IsNullOrWhiteSpace(regexMask))
            {
                throw new ArgumentException("RegexMask property can't be used with Mask property");
            }

            // incase no value is provided us it as normal textbox
            var mask = textbox.GetValue(MaskProperty) as string;
            if (string.IsNullOrWhiteSpace(mask))
            {
                return;
            }

            var placeHolderValue = textbox.GetValue(PlaceHolderProperty) as string;
            if (string.IsNullOrEmpty(placeHolderValue))
            {
                throw new ArgumentException("PlaceHolder can't be null or empty");
            }

            var placeHolder = placeHolderValue[0];

            var representationDictionary = new Dictionary<char, string>();
            representationDictionary.Add(AlphaCharacterRepresentation.Key, AlphaCharacterRepresentation.Value);
            representationDictionary.Add(NumericCharacterRepresentation.Key, NumericCharacterRepresentation.Value);
            representationDictionary.Add(AlphaNumericRepresentation.Key, AlphaNumericRepresentation.Value);

            var customDictionaryValue = textbox.GetValue(CustomMaskProperty) as string;
            if (!string.IsNullOrWhiteSpace(customDictionaryValue))
            {
                var customRoles = customDictionaryValue.Split(',');
                foreach (var role in customRoles)
                {
                    var roleValues = role.Split(':');
                    if (roleValues.Length != 2)
                    {
                        throw new ArgumentException("Invalid CustomMask property");
                    }

                    var keyValue = roleValues[0];
                    var value = roleValues[1];
                    char key;

                    // an exception should be throw if the regex is not valid
                    Regex.Match(string.Empty, value);
                    if (!char.TryParse(keyValue, out key))
                    {
                        throw new ArgumentException("Invalid CustomMask property, please validate the mask key");
                    }

                    representationDictionary.Add(key, value);
                }
            }

            textbox.SetValue(RepresentationDictionaryProperty, representationDictionary);

            var displayText = mask;
            foreach (var key in representationDictionary.Keys)
            {
                displayText = displayText.Replace(key, placeHolder);
            }

            textbox.Text = displayText;
            textbox.TextChanging += Textbox_TextChanging;
            textbox.SelectionChanged += Textbox_SelectionChanged;
            textbox.Paste += Textbox_Paste;
            textbox.SetValue(OldTextProperty, displayText);
        }

        private static async void Textbox_Paste(object sender, TextControlPasteEventArgs e)
        {
            e.Handled = true;
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
            var representationDictionary = textbox?.GetValue(RepresentationDictionaryProperty) as Dictionary<char, string>;
            if (string.IsNullOrWhiteSpace(mask) ||
                representationDictionary == null)
            {
                return;
            }

            // to update the textbox text without triggering TextChanging text
            textbox.TextChanging -= Textbox_TextChanging;

            var oldSelectionStart = (int)textbox.GetValue(OldSelectionStartProperty);
            var maxLength = pasteText.Length < mask.Length ? pasteText.Length : mask.Length;
            var textArray = textbox.Text.ToCharArray();

            for (int i = oldSelectionStart; i < oldSelectionStart + maxLength; i++)
            {
                var maskChar = mask[i];
                var selectedChar = pasteText[i - oldSelectionStart];

                // If dynamic character a,9,* or custom
                if (representationDictionary.ContainsKey(maskChar))
                {
                    var pattern = representationDictionary[maskChar];
                    if (Regex.IsMatch(selectedChar.ToString(), pattern))
                    {
                        textArray[i] = selectedChar;
                    }
                }
            }

            textbox.Text = new string(textArray);
            textbox.SetValue(OldTextProperty, textbox.Text);
            textbox.SelectionStart = oldSelectionStart + maxLength;
            textbox.TextChanging += Textbox_TextChanging;
        }

        private static void Textbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            textbox?.SetValue(OldSelectionStartProperty, textbox.SelectionStart);
            textbox?.SetValue(OldSelectionLengthProperty, textbox.SelectionLength);
        }

        private static void Textbox_TextChanging(TextBox textbox, TextBoxTextChangingEventArgs args)
        {
            var mask = textbox.GetValue(MaskProperty) as string;
            var representationDictionary = textbox.GetValue(RepresentationDictionaryProperty) as Dictionary<char, string>;
            var placeHolderValue = textbox?.GetValue(PlaceHolderProperty) as string;
            var oldText = textbox.GetValue(OldTextProperty) as string;
            var oldSelectionStart = (int)textbox.GetValue(OldSelectionStartProperty);
            var oldSelectionLength = (int)textbox.GetValue(OldSelectionLengthProperty);
            if (string.IsNullOrWhiteSpace(mask) ||
                representationDictionary == null ||
                string.IsNullOrEmpty(placeHolderValue) ||
                oldText == null)
            {
                return;
            }

            var placeHolder = placeHolderValue[0];
            var isDeleteOrBackspace = false;
            var deleteBackspaceIndex = 0;

            // Delete or backspace is triggered
            // if the new length is less than or equal the old text - the old selection length then a delete or backspace is triggered with or without selection and no characters is added
            if (textbox.Text.Length < oldText.Length
                && textbox.Text.Length <= oldText.Length - oldSelectionLength)
            {
                isDeleteOrBackspace = true;
                if (oldSelectionLength == 0)
                {
                    // backspace else delete
                    if (oldSelectionStart != textbox.SelectionStart)
                    {
                        deleteBackspaceIndex++;
                    }
                }
            }

            // case adding data at the end of the textbox
            if (oldSelectionStart >= oldText.Length && !isDeleteOrBackspace)
            {
                textbox.Text = oldText;
                if (oldText.Length >= 0)
                {
                    textbox.SelectionStart = oldText.Length;
                }

                return;
            }

            var selectedChar = textbox.SelectionStart > 0 ?
                textbox.Text[textbox.SelectionStart - 1] :
                placeHolder;

            var textArray = oldText.ToCharArray();

            // detect if backspace or delete is triggered to handle the right removed character
            var newSelectionIndex = oldSelectionStart - deleteBackspaceIndex;

            // for handling single key click add +1 to match length for selection =1
            var singleOrMultiSelectionIndex = oldSelectionLength == 0 ? oldSelectionLength + 1 : oldSelectionLength;
            for (int i = newSelectionIndex; i < (oldSelectionStart - deleteBackspaceIndex + singleOrMultiSelectionIndex); i++)
            {
                var maskChar = mask[i];

                // If dynamic character a,9,* or custom
                if (representationDictionary.ContainsKey(maskChar))
                {
                    if (isDeleteOrBackspace)
                    {
                        textArray[i] = placeHolder;
                        continue;
                    }

                    var pattern = representationDictionary[maskChar];
                    if (Regex.IsMatch(selectedChar.ToString(), pattern))
                    {
                        textArray[i] = selectedChar;

                        // updating text box new index
                        newSelectionIndex++;
                    }

                    // character doesn't match the pattern get the old character
                    else
                    {
                        // if single press don't change
                        if (oldSelectionLength == 0)
                        {
                            textArray[i] = oldText[i];
                        }

                        // if change in selection reset to default place holder instead of keeping the old valid to be clear for the user
                        else
                        {
                            textArray[i] = placeHolder;
                        }
                    }
                }

                // if fixed character
                else
                {
                    textArray[i] = oldText[i];

                    // updating text box new index
                    newSelectionIndex++;
                }
            }

            textbox.Text = new string(textArray);
            textbox.SetValue(OldTextProperty, textbox.Text);

            // without selection
            if (oldSelectionLength == 0)
            {
                textbox.SelectionStart = newSelectionIndex;
            }
            else
            {
                // we can't handle both selection direction because there is no property to detect which direction the selection was
                // so considering the most common direction from left to right and position the index based on it
                textbox.SelectionStart = oldSelectionStart + oldSelectionLength;
            }
        }
    }
}
