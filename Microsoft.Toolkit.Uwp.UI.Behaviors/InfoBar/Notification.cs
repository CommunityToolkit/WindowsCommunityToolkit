// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// The content of a notification to display in <see cref="StackedNotificationsBehavior"/>.
    /// The <see cref="Title"/>, <see cref="Message"/>, <see cref="Duration"/> and <see cref="Severity"/> values will
    /// always be applied to the targeted <see cref="InfoBar"/>.
    /// The <see cref="IsIconVisible"/>, <see cref="Content"/>, <see cref="ContentTemplate"/> and <see cref="ActionButton"/> values
    /// will be applied only if set.
    /// </summary>
    public class Notification
    {
        private NotificationOverrides _overrides;
        private bool _isIconVisible;
        private object _content;
        private DataTemplate _contentTemplate;
        private ButtonBase _actionButton;

        /// <summary>
        /// Gets or sets the notification title.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the notification message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the duration of the notification.
        /// Set to null for persistent notification.
        /// </summary>
        public TimeSpan? Duration { get; set; }

        /// <summary>
        /// Gets or sets the type of the <see cref="InfoBar"/> to apply consistent status color, icon,
        /// and assistive technology settings dependent on the criticality of the notification.
        /// </summary>
        public InfoBarSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the icon is visible or not.
        /// True if the icon is visible; otherwise, false. The default is true.
        /// </summary>
        public bool IsIconVisible
        {
            get => _isIconVisible;
            set
            {
                _isIconVisible = value;
                _overrides |= NotificationOverrides.Icon;
            }
        }

        /// <summary>
        /// Gets or sets the XAML Content that is displayed below the title and message in
        ///  the InfoBar.
        /// </summary>
        public object Content
        {
            get => _content;
            set
            {
                _content = value;
                _overrides |= NotificationOverrides.Content;
            }
        }

        /// <summary>
        /// Gets or sets the data template for the <see cref="Content"/>.
        /// </summary>
        public DataTemplate ContentTemplate
        {
            get => _contentTemplate;
            set
            {
                _contentTemplate = value;
                _overrides |= NotificationOverrides.ContentTemplate;
            }
        }

        /// <summary>
        /// Gets or sets the action button of the InfoBar.
        /// </summary>
        public ButtonBase ActionButton
        {
            get => _actionButton;
            set
            {
                _actionButton = value;
                _overrides |= NotificationOverrides.ActionButton;
            }
        }

        internal NotificationOverrides Overrides => _overrides;
    }

    /// <summary>
    /// The overrides which should be set on the notification.
    /// </summary>
    [Flags]
    internal enum NotificationOverrides
    {
        None,
        Icon,
        Content,
        ContentTemplate,
        ActionButton,
    }
}
