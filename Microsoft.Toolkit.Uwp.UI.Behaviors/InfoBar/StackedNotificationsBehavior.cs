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
        private readonly Queue<Notification> _stackedNotifications;
        private readonly DispatcherQueueTimer _dismissTimer;
        private Notification _currentNotification;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedNotificationsBehavior"/> class.
        /// </summary>
        public StackedNotificationsBehavior()
        {
            _stackedNotifications = new Queue<Notification>();

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
            _stackedNotifications.Enqueue(notification);
            ShowNext();
        }

        /// <inheritdoc/>
        protected override bool Initialize()
        {
            AssociatedObject.Closed += OnInfoBarClosed;
            AssociatedObject.PointerEntered += OnPointerEntered;
            AssociatedObject.PointerExited += OnPointedExited;
            return true;
        }

        /// <inheritdoc/>
        protected override bool Uninitialize()
        {
            AssociatedObject.Closed -= OnInfoBarClosed;
            AssociatedObject.PointerEntered -= OnPointerEntered;
            AssociatedObject.PointerExited -= OnPointedExited;
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

            if (!_stackedNotifications.TryDequeue(out var notificationToDisplay))
            {
                _currentNotification = null;
                return;
            }

            _currentNotification = notificationToDisplay;
            AssociatedObject.SetNotification(notificationToDisplay);
            AssociatedObject.IsOpen = true;

            StartTimer(notificationToDisplay.Duration);
        }

        private void StartTimer(TimeSpan? duration)
        {
            if (duration is null)
            {
                return;
            }

            _dismissTimer.Interval = duration.Value;
            _dismissTimer.Start();
        }

        private void StopTimer() => _dismissTimer.Stop();

        private void OnTimerTick(DispatcherQueueTimer sender, object args) => AssociatedObject.IsOpen = false;

        private void OnPointedExited(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) => StartTimer(_currentNotification?.Duration);

        private void OnPointerEntered(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e) => StopTimer();
    }
}
