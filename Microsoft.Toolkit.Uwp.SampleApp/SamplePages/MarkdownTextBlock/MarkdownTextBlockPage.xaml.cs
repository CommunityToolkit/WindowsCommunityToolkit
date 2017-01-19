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
using Windows.ApplicationModel;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    public sealed partial class MarkdownTextBlockPage : Page
    {
        public MarkdownTextBlockPage()
        {
            InitializeComponent();
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
            UiUnformattedText.Text = text;
        }

        private async void MarkdownText_OnLinkClicked(object sender, UI.Controls.LinkClickedEventArgs e)
        {
            await Launcher.LaunchUriAsync(new Uri(e.Link));
        }
    }
}
