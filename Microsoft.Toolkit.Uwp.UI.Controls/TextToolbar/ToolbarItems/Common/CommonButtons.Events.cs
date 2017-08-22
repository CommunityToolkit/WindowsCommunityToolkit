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
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.UI.Controls.TextToolbarButtons.Common
{
    /// <summary>
    /// Provides access to Generic Buttons that activate Formatter Methods
    /// </summary>
    public partial class CommonButtons
    {
        private void MakeBold(ToolbarButton button)
        {
            Model.Formatter.ButtonActions.FormatBold(button);
        }

        private void MakeItalics(ToolbarButton button)
        {
            Model.Formatter.ButtonActions.FormatItalics(button);
        }

        private void MakeStrike(ToolbarButton button)
        {
            Model.Formatter.ButtonActions.FormatStrikethrough(button);
        }

        private void MakeLink(ToolbarButton button)
        {
            Model.Formatter.ButtonActions.FormatLink(button, string.Empty, string.Empty, string.Empty);
        }

        private void MakeList(ToolbarButton button)
        {
            Model.Formatter.ButtonActions.FormatList(button);
        }

        private void MakeOList(ToolbarButton button)
        {
            Model.Formatter.ButtonActions.FormatOrderedList(button);
        }

        public async void OpenLinkCreator(ToolbarButton button)
        {
            var selection = button.Model.Editor.Document.Selection;

            var labelBox = new RichEditBox
            {
                PlaceholderText = Model.Labels.LabelLabel,
                Margin = new Thickness(0, 0, 0, 5),
                AcceptsReturn = false
            };
            var linkBox = new TextBox
            {
                PlaceholderText = Model.Labels.UrlLabel
            };

            labelBox.Document.SetDefaultCharacterFormat(selection.CharacterFormat);
            selection.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string Labeltext);
            labelBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, Labeltext);

            var result = await new ContentDialog
            {
                Title = Model.Labels.CreateLinkLabel,
                Content = new StackPanel
                {
                    Children =
                    {
                        labelBox,
                        linkBox
                    }
                },
                PrimaryButtonText = Model.Labels.OkLabel,
                SecondaryButtonText = Model.Labels.CancelLabel
            }.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string labelText;
                labelBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out labelText);

                string formattedlabelText;
                labelBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out formattedlabelText);

                Model.Formatter.ButtonActions.FormatLink(button, labelText.Trim(), formattedlabelText.Trim(), linkBox.Text.Trim());
            }
        }
    }
}