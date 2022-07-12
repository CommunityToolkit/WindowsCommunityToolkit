// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

namespace Microsoft.Toolkit.Uwp.Notifications
{
    internal sealed class Element_ToastActions : IHaveXmlName, IHaveXmlNamedProperties, IHaveXmlChildren
    {
        internal const ToastSystemCommand DEFAULT_SYSTEM_COMMAND = ToastSystemCommand.None;

        public ToastSystemCommand SystemCommands { get; set; } = ToastSystemCommand.None;

        public IList<IElement_ToastActionsChild> Children { get; private set; } = new List<IElement_ToastActionsChild>();

        /// <inheritdoc/>
        string IHaveXmlName.Name => "actions";

        /// <inheritdoc/>
        IEnumerable<object> IHaveXmlChildren.Children => Children;

        /// <inheritdoc/>
        IEnumerable<KeyValuePair<string, object>> IHaveXmlNamedProperties.EnumerateNamedProperties()
        {
            if (SystemCommands != DEFAULT_SYSTEM_COMMAND)
            {
                yield return new("hint-systemCommands", SystemCommands);
            }
        }
    }

    internal interface IElement_ToastActionsChild
    {
    }

    internal enum ToastSystemCommand
    {
        None,
        SnoozeAndDismiss
    }
}