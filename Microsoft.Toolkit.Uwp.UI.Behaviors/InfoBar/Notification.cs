// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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
        /// Gets or sets the duration of the notification.
        /// Use <see cref="TimeSpan.MinValue"/> for a persistent notification.
        /// </summary>
        public TimeSpan Duration { get; set; }
    }
}
