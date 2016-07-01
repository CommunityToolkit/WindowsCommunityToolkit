// THIS CODE AND INFORMATION IS PROVIDED "AS IS" WITHOUT WARRANTY OF
// ANY KIND, EITHER EXPRESSED OR IMPLIED, INCLUDING BUT NOT LIMITED TO
// THE IMPLIED WARRANTIES OF MERCHANTABILITY AND/OR FITNESS FOR A
// PARTICULAR PURPOSE.
//
// Copyright (c) Microsoft Corporation. All rights reserved


using System.Collections.Generic;

namespace Microsoft.Windows.Toolkit.Notifications
{
    [NotificationXmlElement("actions")]
    internal sealed class Element_ToastActions
    {
        internal const ToastSystemCommand DEFAULT_SYSTEM_COMMAND = ToastSystemCommand.None;

        [NotificationXmlAttribute("hint-systemCommands", DEFAULT_SYSTEM_COMMAND)]
        public ToastSystemCommand SystemCommands { get; set; } = ToastSystemCommand.None;

        public IList<IElement_ToastActionsChild> Children { get; private set; } = new List<IElement_ToastActionsChild>();
    }

    internal interface IElement_ToastActionsChild { }

    internal enum ToastSystemCommand
    {
        None,
        SnoozeAndDismiss
    }
}