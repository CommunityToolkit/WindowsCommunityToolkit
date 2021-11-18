// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// The content of a notification to display in <see cref="StackedNotificationsBehavior"/>
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
        /// Gets or sets the type of the <see cref="InfoBar"/> to apply consistent status color, icon,
        /// and assistive technology settings dependent on the criticality of the notification.
        /// </summary>
        public InfoBarSeverity Severity { get; set; }

        /// <summary>
        /// Gets or sets a value that indicates whether the icon is visible in the InfoBar.
        /// true if the icon is visible; otherwise, false. The default is true.
        /// </summary>
        public bool IsIconVisible { get; set; } = true;

        /// <summary>
        /// Gets or sets the XAML Content that is displayed below the title and message in
        ///  the InfoBar.
        /// </summary>
        public object Content { get; set; }

        /// <summary>
        /// Gets or sets the data template for the <see cref="Content"/>.
        /// </summary>
        public DataTemplate ContentTemplate { get; set; }

        /// <summary>
        /// Gets or sets the action button of the InfoBar.
        /// </summary>
        public ButtonBase ActionButton { get; set; }

        /// <summary>
        /// Gets or sets the duration of the notification.
        /// Set to null for persistent notification.
        /// </summary>
        public TimeSpan? Duration { get; set; }
    }
}
