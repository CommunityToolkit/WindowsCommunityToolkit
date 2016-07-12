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

namespace Microsoft.Windows.Toolkit.Notifications
{

    /// <summary>
    /// Automatically constructs a selection box for snooze intervals, and snooze/dismiss buttons, all automatically localized, and snoozing logic is automatically handled by the system.
    /// </summary>
    public sealed class ToastActionsSnoozeAndDismiss : IToastActions
    {
        /// <summary>
        /// Automatically constructs a selection box for snooze intervals, and snooze/dismiss buttons, all automatically localized, and snoozing logic is automatically handled by the system.
        /// </summary>
        public ToastActionsSnoozeAndDismiss() { }

#if ANNIVERSARY_UPDATE
        /// <summary>
        /// New in RS1: Custom context menu items, providing additional actions when the user right clicks the Toast notification. You can only have up to 5 items.
        /// </summary>
        public IList<ToastContextMenuItem> ContextMenuItems { get; private set; } = new List<ToastContextMenuItem>();
#endif

        internal Element_ToastActions ConvertToElement()
        {
#if ANNIVERSARY_UPDATE
            if (ContextMenuItems.Count > 5)
                throw new InvalidOperationException("You have too many context menu items. You can only have up to 5.");
#endif

            var el = new Element_ToastActions()
            {
                SystemCommands = ToastSystemCommand.SnoozeAndDismiss
            };

#if ANNIVERSARY_UPDATE
            foreach (var item in ContextMenuItems)
                el.Children.Add(item.ConvertToElement());
#endif

            return el;
        }
    }

    /// <summary>
    /// Create your own custom actions, using controls like <see cref="ToastButton"/>, <see cref="ToastTextBox"/>, and <see cref="ToastSelectionBox"/>.
    /// </summary>
    public sealed class ToastActionsCustom : IToastActions
    {
        /// <summary>
        /// Initializes a new instance of custom actions, which can use controls like <see cref="ToastButton"/>, <see cref="ToastTextBox"/>, and <see cref="ToastSelectionBox"/>.
        /// </summary>
        public ToastActionsCustom() { }

        /// <summary>
        /// Inputs like <see cref="ToastTextBox"/> and <see cref="ToastSelectionBox"/> can be added to the Toast. Only up to 5 inputs can be added; after that, an exception is thrown.
        /// </summary>
        public IList<IToastInput> Inputs { get; private set; } = new LimitedList<IToastInput>(5);

        /// <summary>
        /// Buttons are displayed after all the inputs (or adjacent to inputs if used as quick reply buttons). Only up to 5 buttons can be added (or fewer if you are also including context menu items). After that, an exception is thrown. You can add <see cref="ToastButton"/>, <see cref="ToastButtonSnooze"/>, or <see cref="ToastButtonDismiss"/>
        /// </summary>
        public IList<IToastButton> Buttons { get; private set; } = new LimitedList<IToastButton>(5);

#if ANNIVERSARY_UPDATE
        /// <summary>
        /// New in RS1: Custom context menu items, providing additional actions when the user right clicks the Toast notification. You can only have up to 5 buttons and context menu items *combined*. Thus, if you have one context menu item, you can only have four buttons, etc.
        /// </summary>
        public IList<ToastContextMenuItem> ContextMenuItems { get; private set; } = new List<ToastContextMenuItem>();
#endif

        internal Element_ToastActions ConvertToElement()
        {
#if ANNIVERSARY_UPDATE
            if (Buttons.Count + ContextMenuItems.Count > 5)
                throw new InvalidOperationException("You have too many buttons/context menu items. You can only have up to 5 total.");
#else
            if (Buttons.Count > 5)
                throw new InvalidOperationException("You have too many buttons. You can only have up to 5.");
#endif

            var el = new Element_ToastActions();

            foreach (var input in Inputs)
                el.Children.Add(ConvertToInputElement(input));

            foreach (var button in Buttons)
                el.Children.Add(ConvertToActionElement(button));

#if ANNIVERSARY_UPDATE
            foreach (var item in ContextMenuItems)
                el.Children.Add(item.ConvertToElement());
#endif

            return el;
        }

        private static Element_ToastAction ConvertToActionElement(IToastButton button)
        {
            if (button is ToastButton)
                return (button as ToastButton).ConvertToElement();

            else if (button is ToastButtonDismiss)
                return (button as ToastButtonDismiss).ConvertToElement();

            else if (button is ToastButtonSnooze)
                return (button as ToastButtonSnooze).ConvertToElement();

            throw new NotImplementedException("Unknown button child: " + button.GetType());
        }

        private static Element_ToastInput ConvertToInputElement(IToastInput input)
        {
            if (input is ToastTextBox)
                return (input as ToastTextBox).ConvertToElement();

            else if (input is ToastSelectionBox)
                return (input as ToastSelectionBox).ConvertToElement();

            throw new NotImplementedException("Unknown input child: " + input.GetType());
        }
    }

    /// <summary>
    /// An input element on a Toast notification. One of <see cref="ToastTextBox"/> or <see cref="ToastSelectionBox"/>.
    /// </summary>
    public interface IToastInput { }

    /// <summary>
    /// Actions to display on a Toast notification. One of <see cref="ToastActionsCustom"/> or <see cref="ToastActionsSnoozeAndDismiss"/>.
    /// </summary>
    public interface IToastActions
    {
#if ANNIVERSARY_UPDATE
        /// <summary>
        /// New in RS1: Custom context menu items, providing additional actions when the user right clicks the Toast notification.
        /// </summary>
        IList<ToastContextMenuItem> ContextMenuItems { get; }
#endif
    }
}
