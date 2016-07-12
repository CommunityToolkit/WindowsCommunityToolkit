// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved

using System;

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// A text box control on the Toast that a user can type text into.
    /// </summary>
    public sealed class ToastTextBox : IToastInput
    {
        /// <summary>
        /// Initializes a new Toast TextBox input control with the required elements.
        /// </summary>
        /// <param name="id">Developer-provided ID that the developer uses later to retrieve input when the Toast is interacted with.</param>
        public ToastTextBox(string id)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            Id = id;
        }

        /// <summary>
        /// The ID property is required, and is used so that developers can retrieve user input once the app is activated.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// Title text to display above the text box.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Placeholder text to be displayed on the text box when the user hasn't typed any text yet.
        /// </summary>
        public string PlaceholderContent { get; set; }

        /// <summary>
        /// The initial text to place in the text box. Leave this null for a blank text box.
        /// </summary>
        public string DefaultInput { get; set; }

        internal Element_ToastInput ConvertToElement()
        {
            return new Element_ToastInput()
            {
                Id = Id,
                Type = ToastInputType.Text,
                DefaultInput = DefaultInput,
                PlaceholderContent = PlaceholderContent,
                Title = Title
            };
        }
    }


}