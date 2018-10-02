// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// Message's Fields
    /// </summary>
    public enum MicrosoftGraphMessageFields
    {
        /// <summary>
        /// Indicates whether the message has attachments.
        /// </summary>
        Attachments,

        /// <summary>
        /// The Bcc: recipients for the message.
        /// </summary>
        BccRecipients,

        /// <summary>
        /// The body of the message.
        /// </summary>
        Body,

        /// <summary>
        /// The first 255 characters of the message body.
        /// </summary>
        BodyPreview,

        /// <summary>
        /// The categories associated with the message.
        /// </summary>
        Categories,

        /// <summary>
        /// The Cc: recipients for the message.
        /// </summary>
        CcRecipients,

        /// <summary>
        /// The version of the message.
        /// </summary>
        ChangeKey,

        /// <summary>
        /// The ID of the conversation the email belongs to.
        /// </summary>
        ConversationId,

        /// <summary>
        /// The date and time the message was created.
        /// </summary>
        CreatedDateTime,

        /// <summary>
        /// The mailbox owner and sender of the message.
        /// </summary>
        From,

        /// <summary>
        /// Indicates whether the message has attachments.
        /// </summary>
        HasAttachments,

        /// <summary>
        /// Id of the message
        /// </summary>
        Id,

        /// <summary>
        /// The importance of the message: Low, Normal, High.
        /// </summary>
        Importance,

        /// <summary>
        /// The classification of the message for the user, based on inferred relevance or importance, or on an explicit override. Possible values are: focused or other.
        /// </summary>
        InferenceClassification,

        /// <summary>
        /// The message ID in the format specified by RFC2822.
        /// </summary>
        InternetMessageId,

        /// <summary>
        /// Indicates whether a read receipt is requested for the message.
        /// </summary>
        IsDeliveryReceiptRequested,

        /// <summary>
        /// Indicates whether the message is a draft. A message is a draft if it hasn't been sent yet.
        /// </summary>
        IsDraft,

        /// <summary>
        /// Indicates whether the message has been read.
        /// </summary>
        IsRead,

        /// <summary>
        /// Indicates whether a read receipt is requested for the message.
        /// </summary>
        IsReadReceiptRequested,

        /// <summary>
        /// The date and time the message was last changed.
        /// </summary>
        LastModifiedDateTime,

        /// <summary>
        /// The unique identifier for the message's parent mailFolder.
        /// </summary>
        ParentFolderId,

        /// <summary>
        /// The date and time the message was received.
        /// </summary>
        ReceivedDateTime,

        /// <summary>
        /// The email addresses to use when replying.
        /// </summary>
        ReplyTo,

        /// <summary>
        /// The account that is actually used to generate the message.
        /// </summary>
        Sender,

        /// <summary>
        /// The date and time the message was sent.
        /// </summary>
        SentDateTime,

        /// <summary>
        /// The subject of the message.
        /// </summary>
        Subject,

        /// <summary>
        /// The To: recipients for the message.
        /// </summary>
        ToRecipients,

        /// <summary>
        /// The part of the body of the message that is unique to the current message.
        /// </summary>
        UniqueBody,

        /// <summary>
        /// The URL to open the message in Outlook Web App.
        /// </summary>
        WebLink
    }
}
