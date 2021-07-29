﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using System.Net.Http;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;
using Microsoft.Toolkit.Uwp.Notifications;

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

            // IMPORTANT: Look at App.xaml.cs for handling toast activation
        }

        private async void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            string title = "Andrew sent you a picture";
            string content = "Check this out, The Enchantments!";
            string image = "https://picsum.photos/364/202?image=883";
            int conversationId = 5;

            // Construct the toast content and show it!
            new ToastContentBuilder()

                // Arguments that are returned when the user clicks the toast or a button
                .AddArgument("action", MyToastActions.ViewConversation)
                .AddArgument("conversationId", conversationId)

                // Visual content
                .AddText(title)
                .AddText(content)
                .AddInlineImage(new Uri(await DownloadImageToDisk(image)))
                .AddAppLogoOverride(new Uri(await DownloadImageToDisk("https://unsplash.it/64?image=1005")), ToastGenericAppLogoCrop.Circle)

                // Text box for typing a reply
                .AddInputTextBox("tbReply", "Type a reply")

                // Buttons
                .AddButton(new ToastButton()
                    .SetContent("Reply")
                    .AddArgument("action", MyToastActions.Reply)
                    .SetBackgroundActivation())

                .AddButton(new ToastButton()
                    .SetContent("Like")
                    .AddArgument("action", MyToastActions.Like)
                    .SetBackgroundActivation())

                .AddButton(new ToastButton()
                    .SetContent("View")
                    .AddArgument("action", MyToastActions.ViewImage)
                    .AddArgument("imageUrl", image))

                // And show the toast!
                .Show();
        }

        private static bool _hasPerformedCleanup;

        private static async Task<string> DownloadImageToDisk(string httpImage)
        {
            // Toasts can live for up to 3 days, so we cache images for up to 3 days.
            // Note that this is a very simple cache that doesn't account for space usage, so
            // this could easily consume a lot of space within the span of 3 days.
            try
            {
                if (ToastNotificationManagerCompat.CanUseHttpImages)
                {
                    return httpImage;
                }

                var directory = Directory.CreateDirectory(System.IO.Path.GetTempPath() + "WindowsNotifications.DesktopToasts.Images");

                if (!_hasPerformedCleanup)
                {
                    // First time we run, we'll perform cleanup of old images
                    _hasPerformedCleanup = true;

                    foreach (var d in directory.EnumerateDirectories())
                    {
                        if (d.CreationTimeUtc.Date < DateTime.UtcNow.Date.AddDays(-3))
                        {
                            d.Delete(true);
                        }
                    }
                }

                var dayDirectory = directory.CreateSubdirectory(DateTime.UtcNow.Day.ToString());
                string imagePath = dayDirectory.FullName + "\\" + (uint)httpImage.GetHashCode();

                if (File.Exists(imagePath))
                {
                    return imagePath;
                }

                HttpClient c = new HttpClient();
                using (var stream = await c.GetStreamAsync(httpImage))
                {
                    using (var fileStream = File.OpenWrite(imagePath))
                    {
                        stream.CopyTo(fileStream);
                    }
                }

                return imagePath;
            }
            catch
            {
                return string.Empty;
            }
        }

        internal void ShowConversation(int conversationId)
        {
            ContentBody.Content = new TextBlock()
            {
                Text = "You've just opened conversation " + conversationId,
                FontWeight = FontWeights.Bold
            };
        }

        internal void ShowImage(string imageUrl)
        {
            ContentBody.Content = new Image()
            {
                Source = new BitmapImage(new Uri(imageUrl))
            };
        }

        private void ButtonClearToasts_Click(object sender, RoutedEventArgs e)
        {
            ToastNotificationManagerCompat.History.Clear();
        }
    }
}
