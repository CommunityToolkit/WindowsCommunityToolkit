namespace Microsoft.Windows.Toolkit.Notifications
{
    using System;
    using System.Collections.Generic;

    /// <summary>
    /// Create your own custom actions, using controls like <see cref="ToastButton"/>, <see cref="ToastTextBox"/>, and <see cref="ToastSelectionBox"/>.
    /// </summary>
    public sealed class ToastActionsCustom : IToastActions
    {
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
            if (this.Buttons.Count > 5)
            {
                throw new InvalidOperationException("You have too many buttons. You can only have up to 5.");
            }
#endif

            var el = new Element_ToastActions();

            foreach (var input in this.Inputs)
            {
                el.Children.Add(ConvertToInputElement(input));
            }

            foreach (var button in this.Buttons)
            {
                el.Children.Add(ConvertToActionElement(button));
            }

#if ANNIVERSARY_UPDATE
            foreach (var item in ContextMenuItems)
                el.Children.Add(item.ConvertToElement());
#endif

            return el;
        }

        private static Element_ToastAction ConvertToActionElement(IToastButton button)
        {
            if (button is ToastButton)
            {
                return (button as ToastButton).ConvertToElement();
            }

            if (button is ToastButtonDismiss)
            {
                return (button as ToastButtonDismiss).ConvertToElement();
            }

            if (button is ToastButtonSnooze)
            {
                return (button as ToastButtonSnooze).ConvertToElement();
            }

            throw new NotImplementedException("Unknown button child: " + button.GetType());
        }

        private static Element_ToastInput ConvertToInputElement(IToastInput input)
        {
            if (input is ToastTextBox)
            {
                return (input as ToastTextBox).ConvertToElement();
            }

            if (input is ToastSelectionBox)
            {
                return (input as ToastSelectionBox).ConvertToElement();
            }

            throw new NotImplementedException("Unknown input child: " + input.GetType());
        }
    }
}