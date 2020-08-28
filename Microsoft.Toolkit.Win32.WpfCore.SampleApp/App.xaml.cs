// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using Microsoft.Toolkit.Uwp.Notifications;

namespace Microsoft.Toolkit.Win32.WpfCore.SampleApp
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            // Listen to toast notification activations
            ToastNotificationManagerCompat.OnActivated += this.ToastNotificationManagerCompat_OnActivated;

            if (!ToastNotificationManagerCompat.WasCurrentProcessToastActivated())
            {
                new MainWindow().Show();
            }
        }

        private void ToastNotificationManagerCompat_OnActivated(ToastNotificationActivatedEventArgsCompat e)
        {
            Dispatcher.Invoke(() =>
            {
                var args = e.Argument;
                var userInputCount = e.UserInput.Count;
                MessageBox.Show("Activated!");
            });
        }
    }
}
