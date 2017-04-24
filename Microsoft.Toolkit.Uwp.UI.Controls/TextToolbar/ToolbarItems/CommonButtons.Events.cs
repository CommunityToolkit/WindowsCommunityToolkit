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

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons
{
    using System;
    using Windows.UI.Xaml;
    using Windows.UI.Xaml.Controls;

    /// <summary>
    /// Provides access to Generic Buttons that activate Formatter Methods
    /// </summary>
    public partial class CommonButtons
    {
        private void MakeBold(ToolbarButton button)
        {
            Model.Formatter.FormatBold(button);
        }

        private void MakeItalics(ToolbarButton button)
        {
            Model.Formatter.FormatItalics(button);
        }

        private void MakeStrike(ToolbarButton button)
        {
            Model.Formatter.FormatStrikethrough(button);
        }

        private void MakeLink(ToolbarButton button)
        {
            Model.Formatter.FormatLink(button, string.Empty, string.Empty);
        }

        private void MakeList(ToolbarButton button)
        {
            Model.Formatter.FormatList(button);
        }

        private void MakeOList(ToolbarButton button)
        {
            Model.Formatter.FormatOrderedList(button);
        }

        public async void OpenLinkCreater(ToolbarButton button)
        {
            var labelBox = new TextBox { PlaceholderText = Model.LabelLabel, Margin = new Thickness(0, 0, 0, 5) };
            var linkBox = new TextBox { PlaceholderText = Model.UrlLabel };

            var result = await new ContentDialog
            {
                Title = Model.CreateLinkLabel,
                Content = new StackPanel
                {
                    Children =
                    {
                        labelBox,
                        linkBox
                    }
                },
                PrimaryButtonText = Model.OkLabel,
                SecondaryButtonText = Model.CancelLabel
            }.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                Model.Formatter.FormatLink(button, labelBox.Text, linkBox.Text);
            }
        }
    }
}