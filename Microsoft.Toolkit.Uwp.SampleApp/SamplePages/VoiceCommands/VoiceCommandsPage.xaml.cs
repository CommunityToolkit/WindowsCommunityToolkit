// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class VoiceCommandsPage : Page
    {
        public VoiceCommandsPage()
        {
            this.InitializeComponent();
            this.Loaded += this.VoiceCommandsPage_Loaded;
        }

        private async void VoiceCommandsPage_Loaded(object sender, RoutedEventArgs e)
        {
            if (VoiceCommandTrigger.SpeechRecognizer is null)
            {
                VoiceCommandTrigger.SpeechRecognizer = await WindowsMediaSpeechRecognizer.CreateAsync(Window.Current);
            }
        }

        private void ButtonAdd_Click(object sender, RoutedEventArgs e)
        {
            MoveItem(listBoxAvailable, listBoxSelected);
        }

        private void MoveItem(ListBox from, ListBox to)
        {
            var selectedIndex = from.SelectedIndex;
            var selectedItem = from.SelectedItem;
            if (selectedIndex > -1)
            {
                from.Items.RemoveAt(selectedIndex);
                from.SelectedIndex = Math.Min(selectedIndex, from.Items.Count - 1);
                to.Items.Add(selectedItem);
                to.SelectedIndex = to.Items.Count - 1;
            }
        }

        private void ButtonDelete_Click(object sender, RoutedEventArgs e)
        {
            MoveItem(listBoxSelected, listBoxAvailable);
        }

        private void ButtonAppend_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrWhiteSpace(textBoxExtraItem.Text))
            {
                listBoxSelected.Items.Add(new ListBoxItem
                {
                    Content = textBoxExtraItem.Text
                });
                listBoxSelected.SelectedIndex = listBoxSelected.Items.Count - 1;
                textBoxExtraItem.Text = string.Empty;
            }
        }

        public void WhatCanISay()
        {
            string content = "You can speak the following voice commands: \r\n" +
                             "- What can I say\r\n" +
                             "- Help \r\n" +
                             "- Add\r\n" +
                             "- Delete\r\n" +
                             "- Move avaiable up\r\n" +
                             "- Move avaiable down\r\n" +
                             "- Move avaiable to First\r\n" +
                             "- Move avaiable to Last\r\n" +
                             "- Move selected up\r\n" +
                             "- Move selected down\r\n" +
                             "- Move selected to First\r\n" +
                             "- Move selected to Last";
            _ = new MessageDialog(content, "What can I Say").ShowAsync();
        }

        private void ToggleListning_Toggled(object sender, RoutedEventArgs e)
        {
            if (triggerListning is object)
            {
                triggerListning.Text = toggleListning.IsOn ? "Voice Off" : "Voice On";
                actionListning.Value = !toggleListning.IsOn;
            }
        }
    }
}
