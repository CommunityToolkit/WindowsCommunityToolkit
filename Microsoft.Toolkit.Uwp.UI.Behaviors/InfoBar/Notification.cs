// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;
using System;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// The content of a notification to display in <see cref="StackedNotificationsBehavior"/>.
    /// All the values will be applied to the targeted <see cref="Microsoft.UI.Xaml.Controls.InfoBar"/>.
    /// </summary>
    public class Notification
    {
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
    }
}
