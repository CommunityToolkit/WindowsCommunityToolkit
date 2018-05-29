// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.UI.Controls
{
    /// <summary>
    /// In App Notification defines a control to show local notification in the app.
    /// </summary>
    public partial class InAppNotification
    {
        /// <summary>
        /// Event raised when the notification is opening
        /// </summary>
        public event InAppNotificationOpeningEventHandler Opening;

        /// <summary>
        /// Event raised when the notification is opened
        /// </summary>
        public event EventHandler Opened;

        /// <summary>
        /// Event raised when the notification is closing
        /// </summary>
        public event InAppNotificationClosingEventHandler Closing;

        /// <summary>
        /// Event raised when the notification is closed
        /// </summary>
        public event InAppNotificationClosedEventHandler Closed;

        private void DismissButton_Click(object sender, RoutedEventArgs e)
        {
            Dismiss(InAppNotificationDismissKind.User);
        }

        private void DismissTimer_Tick(object sender, object e)
        {
            Dismiss(InAppNotificationDismissKind.Timeout);
            _dismissTimer.Stop();
        }

        private void OpenAnimationTimer_Tick(object sender, object e)
        {
            _animationTimer.Stop();
            Opened?.Invoke(this, EventArgs.Empty);
            _animationTimer.Tick -= OpenAnimationTimer_Tick;
        }

        private void DismissAnimationTimer_Tick(object sender, object e)
        {
            _animationTimer.Stop();
            Closed?.Invoke(this, new InAppNotificationClosedEventArgs(_lastDismissKind));
            _animationTimer.Tick -= DismissAnimationTimer_Tick;
        }
    }
}
