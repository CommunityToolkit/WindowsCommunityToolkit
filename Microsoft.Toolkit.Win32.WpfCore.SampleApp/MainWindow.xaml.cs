// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Windows;
using Microsoft.Toolkit.Uwp.Notifications;
using Windows.UI.Notifications;

namespace Microsoft.Toolkit.Win32.WpfCore.SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ButtonShowToast_Click(object sender, RoutedEventArgs e)
        {
            var content = new ToastContentBuilder()
                .AddText("Hello from WPF!")
                .GetToastContent();

            var notif = new ToastNotification(content.GetXml());

            DesktopNotificationManagerCompat.CreateToastNotifier().Show(notif);
        }
    }
}
