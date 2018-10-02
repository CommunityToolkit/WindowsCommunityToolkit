// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        /// <summary>
        /// Opens a <see cref="ContentDialog"/> for the user to enter a URL
        /// </summary>
        /// <param name="button">The <see cref="ToolbarButton"/> invoked</param>
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

            CheckBox relativeBox = null;

            var contentPanel = new StackPanel
            {
                Children =
                    {
                        labelBox,
                        linkBox
                    }
            };

            if (Model.UseURIChecker)
            {
                relativeBox = new CheckBox
                {
                    Content = Model.Labels.RelativeLabel
                };
                contentPanel.Children.Add(relativeBox);
            }

            labelBox.Document.SetDefaultCharacterFormat(selection.CharacterFormat);
            selection.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string Labeltext);
            labelBox.Document.SetText(Windows.UI.Text.TextSetOptions.FormatRtf, Labeltext);

            var result = await new ContentDialog
            {
                Title = Model.Labels.CreateLinkLabel,
                Content = contentPanel,
                PrimaryButtonText = Model.Labels.OkLabel,
                SecondaryButtonText = Model.Labels.CancelLabel
            }.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                labelBox.Document.GetText(Windows.UI.Text.TextGetOptions.None, out string labelText);
                labelBox.Document.GetText(Windows.UI.Text.TextGetOptions.FormatRtf, out string formattedlabelText);

                string linkText = linkBox.Text.Trim();

                if (string.IsNullOrWhiteSpace(linkText))
                {
                    ShowContentDialog(Model.Labels.WarningLabel, Model.Labels.LinkInvalidLabel, Model.Labels.OkLabel);
                    return;
                }

                if (Model.UseURIChecker && !string.IsNullOrWhiteSpace(linkText))
                {
                    var wellFormed = Uri.IsWellFormedUriString(linkText, relativeBox?.IsChecked == true ? UriKind.RelativeOrAbsolute : UriKind.Absolute);
                    if (!wellFormed)
                    {
                        ShowContentDialog(Model.Labels.WarningLabel, Model.Labels.LinkInvalidLabel, Model.Labels.OkLabel);
                        return;
                    }
                }

                Model.Formatter.ButtonActions.FormatLink(button, labelText.Trim(), formattedlabelText.Trim(), linkText);
            }
        }

        /// <summary>
        /// Opens a <see cref="ContentDialog"/> to notify the user about empty and whitespace inputs.
        /// </summary>
        /// <param name="title">The <see cref="string"/> </param>
        /// <param name="content">The <see cref="string"/> of the ContentDialog</param>
        /// <param name="primaryButtonText">The <see cref="string"/> content of the primary button</param>
        private async void ShowContentDialog(string title, string content, string primaryButtonText)
        {
            await new ContentDialog
            {
                Title = title,
                Content = content,
                PrimaryButtonText = primaryButtonText
            }.ShowAsync();
        }
    }
}