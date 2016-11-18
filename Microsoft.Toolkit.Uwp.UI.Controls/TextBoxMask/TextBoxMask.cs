using System.Collections.Generic;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// Text Mask allows a user to more easily enter fixed width text in TextBox control
    /// where you would like them to enter the data in a certain format
    /// </summary>
    public partial class TextBoxMask
    {
        private const char DefaultPlaceHolder = '_';
        private static readonly KeyValuePair<char, string> AlphaCharacterRepresentation = new KeyValuePair<char, string>('a', "[A-Za-z]");
        private static readonly KeyValuePair<char, string> NumericCharacterRepresentation = new KeyValuePair<char, string>('9', "[0-9]");
        private static readonly KeyValuePair<char, string> AlphaNumericRepresentation = new KeyValuePair<char, string>('*', "[A-Za-z0-9]");

        // TODO handle Control Z cases
        private static void OnMaskChanged(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var textbox = d as TextBox;
            if (textbox == null)
            {
                return;
            }

            // incase no value is provided us it as normal textbox
            var value = e.NewValue as string;
            if (string.IsNullOrWhiteSpace(value))
            {
                return;
            }

            var placeHolder = (char?)textbox.GetValue(PlaceHolderProperty);
            if (!placeHolder.HasValue)
            {
                return;
            }

            var representationDictionary = textbox.GetValue(RepresentationDictionaryProperty) as Dictionary<char, string>;
            if (representationDictionary == null)
            {
                representationDictionary = new Dictionary<char, string>();
                representationDictionary.Add(AlphaCharacterRepresentation.Key, AlphaCharacterRepresentation.Value);
                representationDictionary.Add(NumericCharacterRepresentation.Key, NumericCharacterRepresentation.Value);
                representationDictionary.Add(AlphaNumericRepresentation.Key, AlphaNumericRepresentation.Value);
            }

            // TODO: insert generic custom representation
            textbox.SetValue(RepresentationDictionaryProperty, representationDictionary);

            string displayText = value.Replace(AlphaCharacterRepresentation.Key, placeHolder.Value).
                                Replace(NumericCharacterRepresentation.Key, placeHolder.Value).
                                Replace(AlphaNumericRepresentation.Key, placeHolder.Value);
            textbox.Text = displayText;
            textbox.TextChanging += Textbox_TextChanging;
            textbox.SelectionChanged += Textbox_SelectionChanged;
            textbox.SetValue(OldTextProperty, displayText);
        }

        private static void Textbox_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var textbox = sender as TextBox;
            if (textbox == null)
            {
                return;
            }

            textbox.SetValue(OldSelectionStartProperty, textbox.SelectionStart);
            textbox.SetValue(OldSelectionLengthProperty, textbox.SelectionLength);
        }

        private static void Textbox_TextChanging(TextBox textbox, TextBoxTextChangingEventArgs args)
        {
            var mask = textbox.GetValue(MaskProperty) as string;
            var representationDictionary = textbox.GetValue(RepresentationDictionaryProperty) as Dictionary<char, string>;
            var placeHolder = (char?)textbox.GetValue(PlaceHolderProperty);
            var oldText = textbox.GetValue(OldTextProperty) as string;
            var oldSelectionStart = (int)textbox.GetValue(OldSelectionStartProperty);
            var oldSelectionLength = (int)textbox.GetValue(OldSelectionLengthProperty);
            if (string.IsNullOrWhiteSpace(mask) ||
                representationDictionary == null ||
                !placeHolder.HasValue || oldText == null)
            {
                return;
            }

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

            var selectedCharacter = textbox.SelectionStart > 0 ?
                textbox.Text[textbox.SelectionStart - 1] :
                placeHolder.Value;

            var textArray = oldText.ToCharArray();
            var newSelectionIndex = oldSelectionStart - deleteBackspaceIndex;
            var singleOrMultiSelectionIndex = oldSelectionLength == 0 ? oldSelectionLength + 1 : oldSelectionLength;
            for (int i = newSelectionIndex; i < (oldSelectionStart - deleteBackspaceIndex + singleOrMultiSelectionIndex); i++)
            {
                var maskChar = mask[i];

                // If dynamic character a,9,* or custom
                if (representationDictionary.ContainsKey(maskChar))
                {
                    if (isDeleteOrBackspace)
                    {
                        textArray[i] = placeHolder.Value;
                        continue;
                    }

                    var pattern = representationDictionary[maskChar];
                    if (Regex.IsMatch(selectedCharacter.ToString(), pattern))
                    {
                        textArray[i] = selectedCharacter;

                        // updating text box new index
                        // TODO apply this change in selection
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
                            textArray[i] = placeHolder.Value;
                        }
                    }
                }

                // if fixed character
                else
                {
                    textArray[i] = oldText[i];
                }
            }

            textbox.Text = new string(textArray);
            textbox.SetValue(OldTextProperty, textbox.Text);
            textbox.SelectionStart = newSelectionIndex;
        }
    }
}
