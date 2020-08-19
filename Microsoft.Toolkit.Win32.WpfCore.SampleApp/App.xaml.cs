using System;
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
            // Register the app to support toast notifications
            DesktopNotificationManagerCompat.RegisterApplication(
                aumid: "Microsoft.Toolkit.Win32.WpfCore",
                displayName: "Toolkit Win32 WPF Core Sample App",
                iconPath: "C:\\icon.png");

            // And listen to toast notification activations
            DesktopNotificationManagerCompat.OnActivated += this.DesktopNotificationManagerCompat_OnActivated;

            if (!DesktopNotificationManagerCompat.WasProcessToastActivated())
            {
                new MainWindow().Show();
            }
        }

        private void DesktopNotificationManagerCompat_OnActivated(DesktopNotificationActivatedEventArgs e)
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
