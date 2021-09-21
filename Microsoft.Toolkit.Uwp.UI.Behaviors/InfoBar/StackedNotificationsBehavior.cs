// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.System;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// A behavior to add the stacked notification support to <see cref="InfoBar"/>.
    /// </summary>
    public class StackedNotificationsBehavior : BehaviorBase<InfoBar>
    {
        private readonly List<Notification> _stackedNotifications;
        private readonly DispatcherQueueTimer _dismissTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedNotificationsBehavior"/> class.
        /// </summary>
        public StackedNotificationsBehavior()
        {
            _stackedNotifications = new List<Notification>();

            var dispatcherQueue = DispatcherQueue.GetForCurrentThread();
            _dismissTimer = dispatcherQueue.CreateTimer();
            _dismissTimer.Tick += OnTimerTick;
        }

        /// <summary>
        /// Show <paramref name="notification"/>.
        /// </summary>
        /// <param name="notification">The notification to display.</param>
        public void Show(Notification notification)
        {
            _stackedNotifications.Add(notification);
            ShowNext();
        }

        /// <inheritdoc/>
        protected override bool Initialize()
        {
            AssociatedObject.Closed += OnInfoBarClosed;
            return true;
        }

        /// <inheritdoc/>
        protected override bool Uninitialize()
        {
            AssociatedObject.Closed -= OnInfoBarClosed;
            return true;
        }

        private void OnInfoBarClosed(InfoBar sender, InfoBarClosedEventArgs args)
        {
            // We display the next notification.
            ShowNext();
        }

        private void ShowNext()
        {
            if (AssociatedObject.IsOpen)
            {
                // One notification is already displayed. We wait for it to close
                return;
            }

            StopTimer();
            AssociatedObject.IsOpen = false;

            if (_stackedNotifications.Count == 0)
            {
                return;
            }

            var notificationToDisplay = _stackedNotifications[0];
            _stackedNotifications.RemoveAt(0);

            AssociatedObject.SetNotification(notificationToDisplay);
            AssociatedObject.IsOpen = true;

            StartTimer(notificationToDisplay.Duration);
        }

        private void StartTimer(TimeSpan duration)
        {
            if (duration == default)
            {
                return;
            }

            _dismissTimer.Interval = duration;
            _dismissTimer.Start();
        }

        private void StopTimer() => _dismissTimer.Stop();

        private void OnTimerTick(DispatcherQueueTimer sender, object args) => AssociatedObject.IsOpen = false;
    }
}
