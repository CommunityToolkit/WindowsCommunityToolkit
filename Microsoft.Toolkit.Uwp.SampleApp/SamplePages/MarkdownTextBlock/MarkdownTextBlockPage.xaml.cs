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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class MarkdownTextBlockPage : Page, IXamlRenderListener
    {
        private TextBox unformattedText;

        public MarkdownTextBlockPage()
        {
            InitializeComponent();
        }

        public void OnXamlRendered(FrameworkElement control)
        {
            unformattedText = control.FindChildByName("UnformattedText") as TextBox;

            var markdownText = control.FindChildByName("MarkdownText") as MarkdownTextBlock;

            if (markdownText != null)
            {
                markdownText.LinkClicked += MarkdownText_LinkClicked;
            }

            SetInitalText("Loading text...");
            LoadData();
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

        private async void MarkdownText_LinkClicked(object sender, UI.Controls.LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
