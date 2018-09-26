using System;
using System.Collections.Generic;
using System.Text;

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
    public partial class ToastContentBuilder
    {
        private IToastActions Actions
        {
            get
            {
                if (Content.Actions == null)
                {
                    Content.Actions = new ToastActionsCustom();
                }

                return Content.Actions;
            }
        }

        private IList<IToastButton> ButtonList
        {
            get
            {
                return ((ToastActionsCustom)Actions).Buttons;
            }
        }

        private IList<IToastInput> InputList
        {
            get
            {
                return ((ToastActionsCustom)Actions).Inputs;
            }
        }

        public ToastContentBuilder AddButton(string content, ToastActivationType activationType, string arguments, Uri imageUri = default(Uri))
        {
            // Add new button
            ToastButton button = new ToastButton(content, arguments)
            {
                ActivationType = activationType
            };

            if (imageUri != default(Uri))
            {
                button.ImageUri = imageUri.OriginalString;
            }

            return AddButton(button);
        }

        public ToastContentBuilder AddButton(IToastButton button)
        {
            // List has max 5 buttons
            if (ButtonList.Count == 5)
            {
                throw new InvalidOperationException("A toast can't have more than 5 buttons");
            }

            ButtonList.Add(button);

            return this;
        }

        public ToastContentBuilder AddInputTextBoxButton(string textBoxId, string content, ToastActivationType activationType, string arguments, Uri imageUri)
        {
            // Add new button
            ToastButton button = new ToastButton(content, arguments)
            {
                ActivationType = activationType,
                TextBoxId = textBoxId,
                ImageUri = imageUri.OriginalString
            };

            return AddButton(button);
        }

        public ToastContentBuilder AddInputTextBox(string id, string placeHolderContent = default(string), string title = default(string))
        {
            var inputTextBox = new ToastTextBox(id);

            if (placeHolderContent != default(string))
            {
                inputTextBox.PlaceholderContent = placeHolderContent;
            }

            if (title != default(string))
            {
                inputTextBox.Title = title;
            }

            return AddToastInput(inputTextBox);
        }

        public ToastContentBuilder AddComboBox(string id, string defaultSelectionBoxItemId, params (string comboBoxItemId, string comboBoxItemContent)[] choices)
        {
            return AddComboBox(id, null, defaultSelectionBoxItemId, choices);
        }

        public ToastContentBuilder AddComboBox(string id, string title, string defaultSelectionBoxItemId, params (string comboBoxItemId, string comboBoxItemContent)[] choices)
        {
            var box = new ToastSelectionBox(id);
            box.DefaultSelectionBoxItemId = defaultSelectionBoxItemId;
            box.Title = title;

            for (int i = 0; i < choices.Length; i++)
            {
                var choice = choices[i];
                box.Items.Add(new ToastSelectionBoxItem(choice.comboBoxItemId, choice.comboBoxItemContent));
            }

            return AddToastInput(box);
        }

        public ToastContentBuilder AddToastInput(IToastInput input)
        {
            InputList.Add(input);

            return this;
        }
    }

#endif
}
