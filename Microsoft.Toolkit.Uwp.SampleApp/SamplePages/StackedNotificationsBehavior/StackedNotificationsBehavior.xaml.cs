// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Behaviors;
using System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class StackedNotificationsBehavior : Page
    {
        private Notification _lastNotification;

        public StackedNotificationsBehavior()
        {
            InitializeComponent();
            Load();
        }

        private static string GetRandomText()
        {
            var random = new Random();
            var result = random.Next(1, 4);

            switch (result)
            {
                case 1: return "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Donec sollicitudin bibendum enim at tincidunt. Praesent egestas ipsum ligula, nec tincidunt lacus semper non.";
                case 2: return "Pellentesque in risus eget leo rhoncus ultricies nec id ante.";
                case 3: default: return "Sed quis nisi quis nunc condimentum varius id consectetur metus. Duis mauris sapien, commodo eget erat ac, efficitur iaculis magna. Morbi eu velit nec massa pharetra cursus. Fusce non quam egestas leo finibus interdum eu ac massa. Quisque nec justo leo. Aenean scelerisque placerat ultrices. Sed accumsan lorem at arcu commodo tristique.";
            }
        }

        private void Load()
        {
            SampleController.Current.RegisterNewCommand(
                "Show information notification",
                (s, a) =>
                {
                    _lastNotification = new Notification
                    {
                        Title = $"Notification {DateTimeOffset.Now}",
                        Message = GetRandomText(),
                        Duration = GetDuration(),
                        Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Informational,
                    };
                    stackedNotificationBehavior.Show(_lastNotification);
                });
            SampleController.Current.RegisterNewCommand(
                "Show error notification",
                (s, a) =>
                {
                    _lastNotification = new Notification
                    {
                        Title = $"Notification {DateTimeOffset.Now}",
                        Message = GetRandomText(),
                        Duration = GetDuration(),
                        Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Error,
                    };
                    stackedNotificationBehavior.Show(_lastNotification);
                });
            SampleController.Current.RegisterNewCommand(
                "Show notification with action",
                (s, a) =>
                {
                    _lastNotification = new Notification
                    {
                        Title = $"Notification {DateTimeOffset.Now}",
                        Message = GetRandomText(),
                        Duration = GetDuration(),
                        Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Warning,
                        ActionButton = new Button { Content = "Action" }
                    };
                    stackedNotificationBehavior.Show(_lastNotification);
                });
            SampleController.Current.RegisterNewCommand(
                "Show notification with custom content",
                (s, a) =>
                {
                    _lastNotification = new Notification
                    {
                        Title = $"Notification {DateTimeOffset.Now}",
                        Message = GetRandomText(),
                        Duration = GetDuration(),
                        Severity = Microsoft.UI.Xaml.Controls.InfoBarSeverity.Warning,
                        Content = new TextBlock { Text = "Custom content" }
                    };
                    stackedNotificationBehavior.Show(_lastNotification);
                });
            SampleController.Current.RegisterNewCommand(
                "Remove last notification",
                (s, a) =>
                {
                    if (_lastNotification is null)
                    {
                        return;
                    }

                    stackedNotificationBehavior.Remove(_lastNotification);
                });
        }

        private TimeSpan? GetDuration()
        {
            if(!int.TryParse(NotificationDurationTextBox.Text, out var duration) || duration <= 0)
            {
                return null;
            }

            return TimeSpan.FromMilliseconds(duration);
        }
    }
}
