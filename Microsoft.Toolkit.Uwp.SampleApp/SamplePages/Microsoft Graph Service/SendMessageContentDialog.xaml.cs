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
using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Uwp.Services.MicrosoftGraph;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Microsoft.Toolkit.Uwp.SampleApp.SamplePages
{
    /// <summary>
    /// ContentDialog that shows how to Send an email
    /// </summary>
    public sealed partial class SendMessageContentDialog : ContentDialog
    {
        public SendMessageContentDialog()
        {
            InitializeComponent();
        }

        private void ContentDialog_PrimaryButtonClick(ContentDialog sender, ContentDialogButtonClickEventArgs args)
        {
        }

        private async void AppBarButton_Click(object sender, RoutedEventArgs e)
        {
            char separator = ';';
            if (string.IsNullOrEmpty(TxtTo.Text.Trim()))
            {
                await DisplayMessageAsync("Need at least one email address");
                return;
            }

            string[] toRecipients = TxtTo.Text.Split(separator);
            string[] ccRecipients = null;
            if (!string.IsNullOrEmpty(TxtCc.Text.Trim()))
            {
                ccRecipients = TxtCc.Text.Split(separator);
            }

            string subject = TxtSubject.Text;
            string content;
            richEditBoxContent.Document.GetText(Windows.UI.Text.TextGetOptions.None, out content);
            try
            {
                await MicrosoftGraphService.Instance.User.Message.SendEmailAsync(subject, content, BodyType.Text, toRecipients, ccRecipients, Importance.Normal);

                // Sending a second message in html format
                // await MicrosoftGraphService.Instance.User.Message.SendEmailAsync("Introducing the Windows Community Toolkit", GetHtmlMessage(), BodyType.Html, toRecipients, ccRecipients);
            }
            catch (ServiceException ex)
            {
                await DisplayAuthorizationErrorMessageAsync(ex, "Send mail as user");
            }

            await DisplayMessageAsync("Succeeded!");
        }

        private Task DisplayMessageAsync(string message)
        {
            MessageDialog msg = new MessageDialog(message);
            return msg.ShowAsync().AsTask();
        }

        private Task DisplayAuthorizationErrorMessageAsync(ServiceException ex, string additionalMessage)
        {
            MessageDialog error = null;
            if (ex.Error.Code.Equals("ErrorAccessDenied"))
            {
                error = new MessageDialog($"{ex.Error.Code}\nCheck in Azure Active Directory portal the '{additionalMessage}' Delegated Permissions");
            }
            else
            {
                error = new MessageDialog(ex.Error.Message);
            }

            return error.ShowAsync().AsTask();
        }
    }
}
