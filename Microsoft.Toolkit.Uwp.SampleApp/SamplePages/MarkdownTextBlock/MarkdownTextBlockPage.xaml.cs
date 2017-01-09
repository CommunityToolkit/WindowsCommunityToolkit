using System;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class MarkdownTextBlockPage : Page
    {
        public MarkdownTextBlockPage()
        {
            this.InitializeComponent();
            SetInitalText("Loading text...");
            LoadData();
        }

        private async void LoadData()
        {
            // Load the inital demo data from the file.
            try
            {
                string initalMarkdownText = await FileIO.ReadTextAsync(await Package.Current.InstalledLocation.GetFileAsync("SamplePages\\MarkdownTextBlock\\InitialContent.md"));
                SetInitalText(initalMarkdownText);
            }
            catch (Exception)
            {
                SetInitalText("**Error Loading Content.**");
            }
        }

        private void SetInitalText(string text)
        {
            ui_unformattedText.Text = text;
            ui_markdownText.Markdown = text;
        }

        private async void MarkdownText_OnMarkdownLinkTapped(object sender, UI.Controls.OnMarkdownLinkTappedArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
