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
    /// A selection box item (an item that the user can select from the drop down list).
    /// </summary>
    public sealed class ToastSelectionBoxItem
    {
        /// <summary>
        /// Constructs a new Toast SelectionBoxItem with the required elements.
        /// </summary>
        /// <param name="id">Developer-provided ID that the developer uses later to retrieve input when the Toast is interacted with.</param>
        /// <param name="content">String that is displayed on the selection item. This is what the user sees.</param>
        public ToastSelectionBoxItem(string id, string content)
        {
            if (id == null)
                throw new ArgumentNullException("id");

            if (content == null)
                throw new ArgumentNullException("content");

            Id = id;
            Content = content;
        }

        /// <summary>
        /// The ID property is required, and is used so that developers can retrieve user input once the app is activated.
        /// </summary>
        public string Id { get; private set; }

        /// <summary>
        /// The Content property is required, and is a string that is displayed on the selection item.
        /// </summary>
        public string Content { get; private set; }

        internal Element_ToastSelection ConvertToElement()
        {
            return new Element_ToastSelection()
            {
                Id = Id,
                Content = Content
            };
        }
    }


}