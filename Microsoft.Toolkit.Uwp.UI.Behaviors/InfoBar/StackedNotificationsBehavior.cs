// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls.Primitives;

namespace Microsoft.Toolkit.Uwp.UI.Behaviors
{
    /// <summary>
    /// A behavior to add the stacked notification support to <see cref="InfoBar"/>.
    /// </summary>
    public class StackedNotificationsBehavior : BehaviorBase<InfoBar>
    {
        private readonly LinkedList<Notification> _stackedNotifications;
        private readonly DispatcherQueueTimer _dismissTimer;
        private Notification _currentNotification;
        private bool _initialIconVisible;
        private object _initialContent;
        private DataTemplate _initialContentTemplate;
        private ButtonBase _initialActionButton;

        /// <summary>
        /// Initializes a new instance of the <see cref="StackedNotificationsBehavior"/> class.
        /// </summary>
        public StackedNotificationsBehavior()
        {
            _stackedNotifications = new LinkedList<Notification>();

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
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            _stackedNotifications.AddLast(notification);
            ShowNext();
        }

        /// <summary>
        /// Remove the <paramref name="notification"/>.
        /// If the notification is displayed, it will be closed.
        /// If the notification is still in the queue, it will be removed.
        /// </summary>
        /// <param name="notification">The notification to remove.</param>
        public void Remove(Notification notification)
        {
            if (notification is null)
            {
                throw new ArgumentNullException(nameof(notification));
            }

            if (notification == _currentNotification)
            {
                // We close the notification. This will trigger the display of the next one.
                // See OnInfoBarClosed.
                AssociatedObject.IsOpen = false;
                return;
            }

            _stackedNotifications.Remove(notification);
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
            RestoreOverridenProperties();

            if (_stackedNotifications.Count == 0)
            {
                _currentNotification = null;
                return;
            }

            var notificationToDisplay = _stackedNotifications.First.Value;
            _stackedNotifications.RemoveFirst();

            _currentNotification = notificationToDisplay;
            SetNotification(notificationToDisplay);
            AssociatedObject.IsOpen = true;

            StartTimer(notificationToDisplay.Duration);
        }

        private void SetNotification(Notification notification)
        {
            AssociatedObject.Title = notification.Title;
            AssociatedObject.Message = notification.Message;
            AssociatedObject.Severity = notification.Severity;

            if (notification.Overrides.HasFlag(NotificationOverrides.Icon))
            {
                _initialIconVisible = AssociatedObject.IsIconVisible;
                AssociatedObject.IsIconVisible = notification.IsIconVisible;
            }

            if (notification.Overrides.HasFlag(NotificationOverrides.Content))
            {
                _initialContent = AssociatedObject.Content;
                AssociatedObject.Content = notification.Content;
            }

            if (notification.Overrides.HasFlag(NotificationOverrides.ContentTemplate))
            {
                _initialContentTemplate = AssociatedObject.ContentTemplate;
                AssociatedObject.ContentTemplate = notification.ContentTemplate;
            }

            if (notification.Overrides.HasFlag(NotificationOverrides.ActionButton))
            {
                _initialActionButton = AssociatedObject.ActionButton;
                AssociatedObject.ActionButton = notification.ActionButton;
            }
        }

        private void RestoreOverridenProperties()
        {
            if (_currentNotification is null)
            {
                return;
            }

            if (_currentNotification.Overrides.HasFlag(NotificationOverrides.Icon))
            {
                AssociatedObject.IsIconVisible = _initialIconVisible;
            }

            if (_currentNotification.Overrides.HasFlag(NotificationOverrides.Content))
            {
                AssociatedObject.Content = _initialContent;
            }

            if (_currentNotification.Overrides.HasFlag(NotificationOverrides.ContentTemplate))
            {
                AssociatedObject.ContentTemplate = _initialContentTemplate;
            }

            if (_currentNotification.Overrides.HasFlag(NotificationOverrides.ActionButton))
            {
                AssociatedObject.ActionButton = _initialActionButton;
            }
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
