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

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    using System.Threading;
    using System.Threading.Tasks;
    using Graph;
    using System.Collections.Generic;
    using System;

    /// <summary>
    ///  Class using Office 365 Microsoft Graph Messages API
    /// </summary>
    public partial class MicrosoftGraphService
    {
        private const string OrderBy = "receivedDateTime desc";

        /// <summary>
        /// Retrieve user"s emails.
        /// <para>(default=10)</para>
        /// <para>Permission Scope: Mail.Read (Read user mail)</para>
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the messages</returns>
        public Task<IUserMessagesCollectionPage> GetUserMessagesAsync(int top = 10, MicrosoftGraphMessageFields[] selectFields = null)
        {
            return this.GetUserMessagesAsync(CancellationToken.None, top, selectFields);
        }

        /// <summary>
        /// Retrieve user"s emails.
        /// <para>(default=10)</para>
        /// <para>Permission Scope : Mail.Read (Read user mail)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the messages</returns>
        public Task<IUserMessagesCollectionPage> GetUserMessagesAsync(CancellationToken cancellationToken, int top = 10, MicrosoftGraphMessageFields[] selectFields = null)
        {
            if (selectFields == null)
            {
                return graphServiceClient.Me.Messages.Request().Top(top).OrderBy(OrderBy).GetAsync(cancellationToken);
            }

            string selectedProperties = MicrosoftGraphHelper.BuildString<MicrosoftGraphMessageFields>(selectFields);

            return graphServiceClient.Me.Messages.Request().Top(top).OrderBy(OrderBy).Select(selectedProperties).GetAsync();
        }

        /// <summary>
        /// Send an message
        /// <para> Permission Scope: (Send mail as user)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="subject">The subject of the message.</param>
        /// <param name="content">The text or HTML content.</param>
        /// <param name="contentType">The content type: Text = 0, HTML = 1</param>
        /// <param name="to">The To recipients for the message.</param>
        /// <param name="cc">The Cc recipients for the message.</param>
        /// <param name="importance">The importance of the message: Low = 0, Normal = 1, High = 2.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SendMessageAsync(CancellationToken cancellationToken, string subject, string content, BodyType contentType, string[] to, string[] cc = null, Importance importance = Importance.Normal)
        {
            if (currentConnectedUser == null)
            {
                throw new ServiceException(new Error { Message = "No user connected", Code = "NoUserConnected", ThrowSite = "UWP Community Toolkit" });
            }

            List<Recipient> ccRecipients = null;

            if (to == null)
            {
                throw new ArgumentNullException(nameof(to));
            }

            ItemBody body = new ItemBody
            {
                Content = content,
                ContentType = contentType
            };
            List<Recipient> toRecipients = new List<Recipient>();
            to.CopyTo(toRecipients);

            if (cc != null)
            {
                ccRecipients = new List<Recipient>();
                cc.CopyTo(ccRecipients);
            }

            Message coreMessage = new Message
            {
                Subject = subject,
                Body = body,
                Importance = importance,
                ToRecipients = toRecipients,
                CcRecipients = ccRecipients,
                BccRecipients = null,
                IsDeliveryReceiptRequested = false,

            };

            var userBuilder = graphServiceClient.Users[currentConnectedUser.Id];
            return userBuilder.SendMail(coreMessage,false).Request().PostAsync(cancellationToken);
        }

        /// <summary>
        /// Send an message
        /// <para> Permission Scope: (Send mail as user)</para>
        /// </summary>
        /// <param name="subject">The subject of the message.</param>
        /// <param name="content">The text or HTML content.</param>
        /// <param name="contentType">The content type: Text = 0, HTML = 1</param>
        /// <param name="to">The To recipients for the message.</param>
        /// <param name="cc">The Cc recipients for the message.</param>
        /// <param name="importance">The importance of the message: Low = 0, Normal = 1, High = 2.</param>
        /// <returns><see cref="Task"/> representing the asynchronous operation.</returns>
        public Task SendMessageAsync(string subject, string content, BodyType contentType, string[] to, string[] cc = null, Importance importance = Importance.Normal)
        {
            return this.SendMessageAsync(CancellationToken.None, subject, content, contentType, to, cc, importance);
        }
    }
}
