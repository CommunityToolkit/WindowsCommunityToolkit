// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
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

        private string GetHtmlMessage()
        {
            return @"Recently, we released the Windows Anniversary Update and a new <a href='https://developer.microsoft.com/en-us/windows/downloads'>Windows Software Developer Kit (SDK) for Windows 10</a> containing tools, app templates, platform controls, Windows Runtime APIs, emulators and much more, to help create innovative and compelling Universal Windows apps.</p><p>Today, we are introducing the open-source <a href='http://aka.ms/uwptoolkit'>UWP Community Toolkit</a>, a new project that enables the developer community to collaborate and contribute new capabilities on top of the SDK.</p><p>We designed the toolkit with these goals in mind:</p><p style='padding-left: 60px'><strong>1. Simplified app development</strong>: The toolkit includes new capabilities (helper functions, custom controls and app services) that simplify or demonstrate common developer tasks. Where possible, our goal is to allow app developers to get started with just one line of code.<br><strong>2. Open-Source</strong>: The toolkit (source code, issues and roadmap) will be developed as an open-source project. We welcome contributions from the .NET developer community.<br><strong>3. Alignment with SDK</strong>: The feedback from the community on this project will be reflected in future versions of the Windows SDK for Windows 10.</p>";
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
                // await MicrosoftGraphService.Instance.User.Message.SendEmailAsync("Introducing the UWP Community Toolkit", GetHtmlMessage(), BodyType.Html, toRecipients, ccRecipients);
            }
            catch (Microsoft.Graph.ServiceException ex)
            {
                await DisplayAuthorizationErrorMessage(ex, "Send mail as user");
            }

            await DisplayMessageAsync("Succeeded!");
        }

        private async Task DisplayMessageAsync(string message)
        {
            MessageDialog msg = new MessageDialog(message);
            await msg.ShowAsync();
        }

        private async Task DisplayAuthorizationErrorMessage(Microsoft.Graph.ServiceException ex, string additionalMessage)
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

            await error.ShowAsync();
        }
    }
}
