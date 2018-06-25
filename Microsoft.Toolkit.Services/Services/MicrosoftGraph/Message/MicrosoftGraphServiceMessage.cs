// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    ///  Class using Office 365 Microsoft Graph Messages API
    /// </summary>
    public class MicrosoftGraphServiceMessage
    {
        private const string OrderBy = "receivedDateTime desc";

        /// <summary>
        /// GraphServiceClient instance
        /// </summary>
        private GraphServiceClient _graphProvider;

        /// <summary>
        /// Store the request for the next page of messages
        /// </summary>
        private IUserMessagesCollectionRequest _nextPageRequest;

        /// <summary>
        /// Store the connected user's profile
        /// </summary>
        private Graph.User _currentUser = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphServiceMessage"/> class.
        /// </summary>
        /// <param name="graphClientProvider">Instance of GraphClientService class</param>
        /// <param name="currentConnectedUser">Instance of Graph.User class</param>
        public MicrosoftGraphServiceMessage(GraphServiceClient graphClientProvider, User currentConnectedUser)
        {
            _graphProvider = graphClientProvider;
            _currentUser = currentConnectedUser;
        }

        /// <summary>
        /// retrieve the next page of messages
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <returns>the next collection of messages or null if there are no more messages</returns>
        public async Task<IUserMessagesCollectionPage> NextPageEmailsAsync(CancellationToken cancellationToken)
        {
            if (_nextPageRequest != null)
            {
                var messages = await _nextPageRequest.GetAsync(cancellationToken);
                _nextPageRequest = messages.NextPageRequest;
                return messages;
            }

            // no more messages
            return null;
        }

        /// <summary>
        /// retrieve the next page of messages
        /// </summary>
        /// <returns>the next collection of messages or null if there are no more messages</returns>
        public Task<IUserMessagesCollectionPage> NextPageEmailsAsync()
        {
            return NextPageEmailsAsync(CancellationToken.None);
        }

        /// <summary>
        /// Retrieve current connected user's emails.
        /// <para>(default=10)</para>
        /// <para>Permission Scope: Mail.Read (Read user mail)</para>
        /// </summary>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the messages</returns>
        public Task<IUserMessagesCollectionPage> GetEmailsAsync(int top = 10, MicrosoftGraphMessageFields[] selectFields = null)
        {
            return GetEmailsAsync(CancellationToken.None, top, selectFields);
        }

        /// <summary>
        /// Retrieve current connected user's emails.
        /// <para>(default=10)</para>
        /// <para>Permission Scope : Mail.Read (Read user mail)</para>
        /// </summary>
        /// <param name="cancellationToken">The <see cref="CancellationToken"/> for the request.</param>
        /// <param name="top">The number of items to return in a result set.</param>
        /// <param name="selectFields">array of fields Microsoft Graph has to include in the response.</param>
        /// <returns>a Collection of Pages containing the messages</returns>
        public async Task<IUserMessagesCollectionPage> GetEmailsAsync(CancellationToken cancellationToken, int top = 10, MicrosoftGraphMessageFields[] selectFields = null)
        {
            IUserMessagesCollectionPage messages = null;
            if (selectFields == null)
            {
                messages = await _graphProvider.Me.Messages.Request().Top(top).OrderBy(OrderBy).GetAsync(cancellationToken);
            }
            else
            {
                string selectedProperties = MicrosoftGraphHelper.BuildString<MicrosoftGraphMessageFields>(selectFields);
                messages = await _graphProvider.Me.Messages.Request().Top(top).OrderBy(OrderBy).Select(selectedProperties).GetAsync(cancellationToken);
            }

            _nextPageRequest = messages.NextPageRequest;
            return messages;
        }

        /// <summary>
        /// Send a message
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
        public Task SendEmailAsync(CancellationToken cancellationToken, string subject, string content, BodyType contentType, string[] to, string[] cc = null, Importance importance = Importance.Normal)
        {
            if (_currentUser == null)
            {
                throw new ServiceException(new Error { Message = "No user connected", Code = "NoUserConnected", ThrowSite = "Windows Community Toolkit" });
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

            var userBuilder = _graphProvider.Users[_currentUser.Id];
            return userBuilder.SendMail(coreMessage, false).Request().PostAsync(cancellationToken);
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
        public Task SendEmailAsync(string subject, string content, BodyType contentType, string[] to, string[] cc = null, Importance importance = Importance.Normal)
        {
            return SendEmailAsync(CancellationToken.None, subject, content, contentType, to, cc, importance);
        }
    }
}
