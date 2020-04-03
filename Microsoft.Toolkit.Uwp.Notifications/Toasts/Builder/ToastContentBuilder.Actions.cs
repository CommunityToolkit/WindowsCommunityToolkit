// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.Notifications
{
#if !WINRT
#pragma warning disable SA1008
#pragma warning disable SA1009
    /// <summary>
    /// Builder class used to create <see cref="ToastContent"/>
    /// </summary>
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

        /// <summary>
        /// Add a button to the current toast.
        /// </summary>
        /// <param name="content">Text to display on the button.</param>
        /// <param name="activationType">Type of activation this button will use when clicked. Defaults to Foreground.</param>
        /// <param name="arguments">App-defined string of arguments that the app can later retrieve once it is activated when the user clicks the button.</param>
        /// <param name="imageUri">Optional image icon for the button to display (required for buttons adjacent to inputs like quick reply).</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
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

        /// <summary>
        /// Add a button to the current toast.
        /// </summary>
        /// <param name="button">An instance of class that implement <see cref="IToastButton"/> for the button that will be used on the toast.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
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

        /// <summary>
        /// Add an button to the toast that will be display to the right of the input text box, achieving a quick reply scenario.
        /// </summary>
        /// <param name="textBoxId">ID of an existing <see cref="ToastTextBox"/> in order to have this button display to the right of the input, achieving a quick reply scenario.</param>
        /// <param name="content">Text to display on the button.</param>
        /// <param name="activationType">Type of activation this button will use when clicked. Defaults to Foreground.</param>
        /// <param name="arguments">App-defined string of arguments that the app can later retrieve once it is activated when the user clicks the button.</param>
        /// <param name="imageUri">An optional image icon for the button to display (required for buttons adjacent to inputs like quick reply)</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddButton(string textBoxId, string content, ToastActivationType activationType, string arguments, Uri imageUri = default(Uri))
        {
            // Add new button
            ToastButton button = new ToastButton(content, arguments)
            {
                ActivationType = activationType,
                TextBoxId = textBoxId
            };

            if (imageUri != default(Uri))
            {
                button.ImageUri = imageUri.OriginalString;
            }

            return AddButton(button);
        }

        /// <summary>
        /// Add an input text box that the user can type into.
        /// </summary>
        /// <param name="id">Required ID property so that developers can retrieve user input once the app is activated.</param>
        /// <param name="placeHolderContent">Placeholder text to be displayed on the text box when the user hasn't typed any text yet.</param>
        /// <param name="title">Title text to display above the text box.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
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

        /// <summary>
        /// Add a combo box / drop-down menu that contain options for user to select.
        /// </summary>
        /// <param name="id">Required ID property used so that developers can retrieve user input once the app is activated.</param>
        /// <param name="choices">List of choices that will be available for user to select.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddComboBox(string id, params (string comboBoxItemId, string comboBoxItemContent)[] choices)
        {
            return AddComboBox(id, default(string), choices);
        }

        /// <summary>
        /// Add a combo box / drop-down menu that contain options for user to select.
        /// </summary>
        /// <param name="id">Required ID property used so that developers can retrieve user input once the app is activated.</param>
        /// <param name="defaultSelectionBoxItemId">Sets which item is selected by default, and refers to the Id property of <see cref="ToastSelectionBoxItem"/>. If you do not provide this or null, the default selection will be empty (user sees nothing).</param>
        /// <param name="choices">List of choices that will be available for user to select.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddComboBox(string id, string defaultSelectionBoxItemId, params (string comboBoxItemId, string comboBoxItemContent)[] choices)
        {
            return AddComboBox(id, default(string), defaultSelectionBoxItemId, choices);
        }

        /// <summary>
        /// Add a combo box / drop-down menu that contain options for user to select.
        /// </summary>
        /// <param name="id">Required ID property used so that developers can retrieve user input once the app is activated.</param>
        /// <param name="title">Title text to display above the Combo Box.</param>
        /// <param name="defaultSelectionBoxItemId">Sets which item is selected by default, and refers to the Id property of <see cref="ToastSelectionBoxItem"/>. If you do not provide this or null, the default selection will be empty (user sees nothing).</param>
        /// <param name="choices">List of choices that will be available for user to select.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddComboBox(string id, string title, string defaultSelectionBoxItemId, params (string comboBoxItemId, string comboBoxItemContent)[] choices)
        {
            return AddComboBox(id, title, defaultSelectionBoxItemId, choices as IEnumerable<(string, string)>);
        }

        /// <summary>
        /// Add a combo box / drop-down menu that contain options for user to select.
        /// </summary>
        /// <param name="id">Required ID property used so that developers can retrieve user input once the app is activated.</param>
        /// <param name="title">Title text to display above the Combo Box.</param>
        /// <param name="defaultSelectionBoxItemId">Sets which item is selected by default, and refers to the Id property of <see cref="ToastSelectionBoxItem"/>. If you do not provide this or null, the default selection will be empty (user sees nothing).</param>
        /// <param name="choices">List of choices that will be available for user to select.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddComboBox(string id, string title, string defaultSelectionBoxItemId, IEnumerable<(string comboBoxItemId, string comboBoxItemContent)> choices)
        {
            var box = new ToastSelectionBox(id);

            if (defaultSelectionBoxItemId != default(string))
            {
                box.DefaultSelectionBoxItemId = defaultSelectionBoxItemId;
            }

            if (title != default(string))
            {
                box.Title = title;
            }

            for (int i = 0; i < choices.Count(); i++)
            {
                var (comboBoxItemId, comboBoxItemContent) = choices.ElementAt(i);
                box.Items.Add(new ToastSelectionBoxItem(comboBoxItemId, comboBoxItemContent));
            }

            return AddToastInput(box);
        }

        /// <summary>
        /// Add an input option to the Toast.
        /// </summary>
        /// <param name="input">An instance of a class that implement <see cref="IToastInput"/> that will be used on the toast.</param>
        /// <returns>The current instance of <see cref="ToastContentBuilder"/></returns>
        public ToastContentBuilder AddToastInput(IToastInput input)
        {
            InputList.Add(input);

            return this;
        }
    }
#pragma warning restore SA1008
#pragma warning restore SA1009
#endif
}
