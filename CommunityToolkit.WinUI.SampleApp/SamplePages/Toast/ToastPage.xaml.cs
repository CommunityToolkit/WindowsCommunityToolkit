// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using CommunityToolkit.WinUI.Notifications;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;

namespace CommunityToolkit.WinUI.SampleApp.SamplePages
{
    public sealed partial class ToastPage : Page
    {
        public ToastPage()
        {
            InitializeComponent();
        }

        private void ButtonPopToast_Click(object sender, RoutedEventArgs e)
        {
            PopToast();
        }

#pragma warning disable SA1008 // Parenthesis spacing
#pragma warning disable SA1117 // Parameters must be on same line or separate lines

        private void PopToast()
        {
            new ToastContentBuilder()
                .SetToastScenario(ToastScenario.Reminder)
                .AddArgument("action", "viewEvent")
                .AddArgument("eventId", 1983)
                .AddText("Adaptive Tiles Meeting")
                .AddText("Conf Room 2001 / Building 135")
                .AddText("10:00 AM - 10:30 AM")
                .AddComboBox("snoozeTime", "15", ("1", "1 minute"),
                                                 ("15", "15 minutes"),
                                                 ("60", "1 hour"),
                                                 ("240", "4 hours"),
                                                 ("1440", "1 day"))
                .AddButton(new ToastButton()
                    .SetSnoozeActivation("snoozeTime"))
                .AddButton(new ToastButton()
                    .SetDismissActivation())
                .Show();
        }

#pragma warning restore SA1008
#pragma warning restore SA1117
    }
}