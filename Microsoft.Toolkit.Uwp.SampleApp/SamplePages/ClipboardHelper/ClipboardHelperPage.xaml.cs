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
            ClipboardHelper.SetHtml(CopyHtmlTextBox.Text);
            await new MessageDialog("copy finished").ShowAsync();
        }

        private async void CopyTextButton_Click(object sender, RoutedEventArgs e)
        {
            ClipboardHelper.SetText(CopyTextTextBox.Text);
            await new MessageDialog("copy finished").ShowAsync();
        }

        private async void PasteHtmlButton_Click(object sender, RoutedEventArgs e)
        {
            var html = await ClipboardHelper.GetHtmlAsync();
            if (string.IsNullOrEmpty(html))
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
            if (string.IsNullOrEmpty(text))
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