using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class ClipboardHelperPage
    {
        public ClipboardHelperPage()
        {
            InitializeComponent();
        }

        private async void CopyHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            ClipboardHelper.SetRawHtml(CopyHtmlTextBox.Text);
            await new MessageDialog("copy finished").ShowAsync();
        }

        private async void CopyTextButton_Click(object sender, RoutedEventArgs e)
        {
            ClipboardHelper.SetText(CopyTextTextBox.Text);
            await new MessageDialog("copy finished").ShowAsync();
        }

        private async void PasteHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            var html = await ClipboardHelper.GetRawHtmlAsync();
            if (html != null)
            {
                PasteHtmlTextBlock.Text = html;
            }
            else
            {
                await new MessageDialog("no html in clipboard").ShowAsync();
            }
        }

        private async void PasteTextButton_Click(object sender, RoutedEventArgs e)
        {
            var text = await ClipboardHelper.GetTextAsync();
            if (text != null)
            {
                PasteTextTextBlock.Text = text;
            }
            else
            {
                await new MessageDialog("no text in clipboard").ShowAsync();
            }
        }
    }
}