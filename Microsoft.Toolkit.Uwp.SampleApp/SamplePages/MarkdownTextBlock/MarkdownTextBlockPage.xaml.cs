// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using Microsoft.Toolkit.Uwp.UI.Controls;
using Microsoft.Toolkit.Uwp.UI.Extensions;
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI;
using Windows.UI.Popups;
using Windows.UI.Text;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Documents;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class MarkdownTextBlockPage : Page, IXamlRenderListener
    {
        private TextBox unformattedText;
        private MarkdownTextBlock markdownText;

        public MarkdownTextBlockPage()
        {
            InitializeComponent();
            Shell.Current.ThemeChanged += Current_ThemeChanged;
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            unformattedText = control.FindChildByName("UnformattedText") as TextBox;

            markdownText = control.FindChildByName("MarkdownText") as MarkdownTextBlock;

            if (markdownText != null)
            {
                markdownText.RequestedTheme = Shell.Current.GetCurrentTheme();
                markdownText.LinkClicked += MarkdownText_LinkClicked;
                markdownText.ImageClicked += MarkdownText_ImageClicked;
                markdownText.CodeBlockResolving += MarkdownText_CodeBlockResolving;
            }

            SetInitalText("Loading text...");
            LoadData();
        }

        private void Current_ThemeChanged(object sender, Models.ThemeChangedArgs e)
        {
            if (e.CustomSet)
            {
                markdownText.RequestedTheme = e.Theme;
            }
        }

        private async void MarkdownText_ImageClicked(object sender, LinkClickedEventArgs e)
        {
            if (!Uri.TryCreate(e.Link, UriKind.Absolute, out Uri result))
            {
                await new MessageDialog("Masked relative Images needs to be manually handled.").ShowAsync();
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
        }

        private async void LoadData()
        {
            // Load the initial demo data from the file.
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
            if (unformattedText != null)
            {
                unformattedText.Text = text;
            }
        }

        private async void MarkdownText_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            if (!Uri.TryCreate(e.Link, UriKind.Absolute, out Uri result))
            {
                await new MessageDialog("Masked relative links needs to be manually handled.").ShowAsync();
            }
            else
            {
                await Launcher.LaunchUriAsync(new Uri(e.Link));
            }
        }

        // Custom Code Block Renderer
        private void MarkdownText_CodeBlockResolving(object sender, CodeBlockResolvingEventArgs e)
        {
            if (e.CodeLanguage == "CUSTOM")
            {
                e.Handled = true;
                e.InlineCollection.Add(new Run { Foreground = new SolidColorBrush(Colors.Red), Text = e.Text, FontWeight = FontWeights.Bold });
            }
        }
    }
}