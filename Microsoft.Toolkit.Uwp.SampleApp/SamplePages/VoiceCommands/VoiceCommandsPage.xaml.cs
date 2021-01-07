using System;
using Microsoft.Toolkit.Uwp.UI.Behaviors;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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
            _ = new MessageDialog("TODO: What can I Say").ShowAsync();
        }
    }
}
