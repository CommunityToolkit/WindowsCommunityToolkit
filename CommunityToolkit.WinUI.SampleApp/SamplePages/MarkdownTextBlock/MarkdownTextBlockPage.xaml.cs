// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using CommunityToolkit.WinUI.UI;
using CommunityToolkit.WinUI.UI.Controls;
using Microsoft.UI;
using Microsoft.UI.Text;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;
using Windows.System;

namespace CommunityToolkit.WinUI.SampleApp.SamplePages
{
    public sealed partial class MarkdownTextBlockPage : Page, IXamlRenderListener
    {
        private TextBox unformattedText;
        private MarkdownTextBlock markdownText;

        public MarkdownTextBlockPage()
        {
            InitializeComponent();
            SampleController.Current.ThemeChanged += Current_ThemeChanged;
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            unformattedText = control.FindChild("UnformattedText") as TextBox;

            markdownText = control.FindChild("MarkdownText") as MarkdownTextBlock;

            if (markdownText != null)
            {
                markdownText.RequestedTheme = SampleController.Current.GetCurrentTheme();
                markdownText.LinkClicked -= MarkdownText_LinkClicked;
                markdownText.LinkClicked += MarkdownText_LinkClicked;
                markdownText.ImageClicked -= MarkdownText_ImageClicked;
                markdownText.ImageClicked += MarkdownText_ImageClicked;
                markdownText.CodeBlockResolving -= MarkdownText_CodeBlockResolving;
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
            if (!Uri.IsWellFormedUriString(e.Link, UriKind.Absolute))
            {
                await new ContentDialog
                {
                    Title = "Windows Community Toolkit Sample App",
                    Content = "Masked relative Images needs to be manually handled.",
                    CloseButtonText = "Close",
                    XamlRoot = XamlRoot
                }.ShowAsync();
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
                using (var jsonStream = await Samples.LoadLocalFile("SamplePages/MarkdownTextBlock/InitialContent.md"))
                {
                    using (var streamreader = new StreamReader(jsonStream))
                    {
                        string initalMarkdownText = await streamreader.ReadToEndAsync();
                        SetInitalText(initalMarkdownText);
                    }
                }
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
            if (!Uri.IsWellFormedUriString(e.Link, UriKind.Absolute))
            {
                await new ContentDialog
                {
                    Title = "Windows Community Toolkit Sample App",
                    Content = "Masked relative links needs to be manually handled.",
                    CloseButtonText = "Close",
                    XamlRoot = XamlRoot
                }.ShowAsync();
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