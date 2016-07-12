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

namespace Microsoft.Windows.Toolkit.Notifications
{
    /// <summary>
    /// One of <see cref="ToastButton"/>, <see cref="ToastButtonSnooze"/>, or <see cref="ToastButtonDismiss"/>.
    /// </summary>
    public interface IToastButton { }

    /// <summary>
    /// A button that the user can click on a Toast notification.
    /// </summary>
    public sealed class ToastButton : IToastButton
    {
        /// <summary>
        /// Initializes a Toast button with the required properties.
        /// </summary>
        /// <param name="content">The text to display on the button.</param>
        /// <param name="arguments">App-defined string of arguments that the app can later retrieve once it is activated when the user clicks the button.</param>
        public ToastButton(string content, string arguments)
        {
            if (content == null)
                throw new ArgumentNullException("content");

            if (arguments == null)
                throw new ArgumentNullException("arguments");

            Content = content;
            Arguments = arguments;
        }

        /// <summary>
        /// Required. The text to display on the button.
        /// </summary>
        public string Content { get; private set; }

        /// <summary>
        /// Required. App-defined string of arguments that the app can later retrieve once it is activated when the user clicks the button.
        /// </summary>
        public string Arguments { get; private set; }

        /// <summary>
        /// Controls what type of activation this button will use when clicked. Defaults to Foreground.
        /// </summary>
        public ToastActivationType ActivationType { get; set; } = ToastActivationType.Foreground;

        /// <summary>
        /// An optional image icon for the button to display (required for buttons adjacent to inputs like quick reply).
        /// </summary>
        public string ImageUri { get; set; }

        /// <summary>
        /// Specify the ID of an existing <see cref="ToastTextBox"/> in order to have this button display to the right of the input, achieving a quick reply scenario.
        /// </summary>
        public string TextBoxId { get; set; }

        internal Element_ToastAction ConvertToElement()
        {
            return new Element_ToastAction()
            {
                Content = Content,
                Arguments = Arguments,
                ActivationType = GetElementActivationType(),
                ImageUri = ImageUri,
                InputId = TextBoxId
            };
        }

        private Element_ToastActivationType GetElementActivationType()
        {
            switch (ActivationType)
            {
                case ToastActivationType.Foreground:
                    return Element_ToastActivationType.Foreground;

                case ToastActivationType.Background:
                    return Element_ToastActivationType.Background;

                case ToastActivationType.Protocol:
                    return Element_ToastActivationType.Protocol;

                default:
                    throw new NotImplementedException();
            }
        }
    }

    /// <summary>
    /// A button that, when clicked, is interpreted as a "dismiss" by the system, and the Toast is dismissed just like if the user swiped the Toast away.
    /// </summary>
    public sealed class ToastButtonDismiss : IToastButton
    {
        /// <summary>
        /// Custom text displayed on the button that overrides the default localized "Dismiss" text.
        /// </summary>
        public string CustomContent { get; private set; }

        /// <summary>
        /// Initializes a system-handled dismiss button that displays localized "Dismiss" text on the button.
        /// </summary>
        public ToastButtonDismiss() { }

        /// <summary>
        /// Constructs a system-handled dismiss button that displays your text on the button.
        /// </summary>
        /// <param name="customContent">The text you want displayed on the button.</param>
        public ToastButtonDismiss(string customContent)
        {
            if (customContent == null)
                throw new ArgumentNullException("customContent");

            CustomContent = customContent;
        }

        internal Element_ToastAction ConvertToElement()
        {
            return new Element_ToastAction()
            {
                Content = CustomContent == null ? "" : CustomContent, // If not using custom content, we need to provide empty string, otherwise Toast doesn't get displayed
                Arguments = "dismiss",
                ActivationType = Element_ToastActivationType.System
                // ImageUri is useless since Shell doesn't display it for system buttons
                // InputId is useless since dismiss button can't be placed to the right of text box (shell doesn't display it)
            };
        }
    }

    /// <summary>
    /// A system-handled snooze button that automatically handles snoozing of a Toast notification.
    /// </summary>
    public sealed class ToastButtonSnooze : IToastButton
    {
        /// <summary>
        /// Custom text displayed on the button that overrides the default localized "Snooze" text.
        /// </summary>
        public string CustomContent { get; private set; }

        /// <summary>
        /// Optionally specify the ID of an existing <see cref="ToastSelectionBox"/> in order to allow the user to pick a custom snooze time. The ID's of the <see cref="ToastSelectionBoxItem"/>s inside the selection box must represent the snooze interval in minutes. For example, if the user selects an item that has an ID of "120", then the notification will be snoozed for 2 hours. When the user clicks this button, if you specified a SelectionBoxId, the system will parse the ID of the selected item and snooze by that amount of minutes. If you didn't specify a SelectionBoxId, the system will snooze by the default system snooze time.
        /// </summary>
        public string SelectionBoxId { get; set; }

        /// <summary>
        /// Initializes a system-handled snooze button that displays localized "Snooze" text on the button and automatically handles snoozing. 
        /// </summary>
        public ToastButtonSnooze()
        {

        }

        /// <summary>
        /// Initializes a system-handled snooze button that displays your text on the button and automatically handles snoozing.
        /// </summary>
        /// <param name="customContent">The text you want displayed on the button.</param>
        public ToastButtonSnooze(string customContent)
        {
            if (customContent == null)
                throw new ArgumentNullException("customContent");

            CustomContent = customContent;
        }

        internal Element_ToastAction ConvertToElement()
        {
            return new Element_ToastAction()
            {
                Content = CustomContent == null ? "" : CustomContent, // If not using custom content, we need to provide empty string, otherwise Toast doesn't get displayed
                Arguments = "snooze",
                ActivationType = Element_ToastActivationType.System,
                InputId = SelectionBoxId
                // ImageUri is useless since Shell doesn't display it for system buttons
            };
        }
    }
}
