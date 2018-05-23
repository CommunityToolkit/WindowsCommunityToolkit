// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// Event's Fields
    /// </summary>
    public enum MicrosoftGraphEventFields
    {
        /// <summary>
        /// The collection of attendees for the event.
        /// </summary>
        Attendees,

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
        /// The version of the message.
        /// </summary>
        ChangeKey,

        /// <summary>
        /// The date and time the message was created.
        /// </summary>
        CreatedDateTime,

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
        /// Set to true if the event lasts all day.
        /// </summary>
        IsAllDay,

        /// <summary>
        /// Set to true if the event has been canceled.
        /// </summary>
        IsCancelled,

        /// <summary>
        /// Set to true if the message sender is also the organizer.
        /// </summary>
        IsOrganizer,

        /// <summary>
        /// Set to true if an alert is set to remind the user of the event.
        /// </summary>
        IsReminderOn,

        /// <summary>
        /// The Timestamp type represents date and time information.
        /// </summary>
        LastModifiedDateTime,

        /// <summary>
        /// The location of the event.
        /// </summary>
        Location,

        /// <summary>
        /// A URL for an online meeting.
        /// </summary>
        OnlineMeetingUrl,

        /// <summary>
        /// The organizer of the event.
        /// </summary>
        Organizer,

        /// <summary>
        /// The end time zone that was set when the event was created.
        /// </summary>
        OriginalEndTimeZone,

        /// <summary>
        /// The Timestamp type represents date and time information.
        /// </summary>
        OriginalStart,

        /// <summary>
        /// The start time zone that was set when the event was created.
        /// </summary>
        OriginalStartTimeZone,

        /// <summary>
        /// The recurrence pattern for the event.
        /// </summary>
        Recurrence,

        /// <summary>
        /// The number of minutes before the event start time that the reminder alert occurs.
        /// </summary>
        ReminderMinutesBeforeStart,

        /// <summary>
        /// Set to true if the sender would like a response when the event is accepted or declined.
        /// </summary>
        ResponseRequested,

        /// <summary>
        /// Indicates the type of response sent in response to an event message.
        /// </summary>
        ResponseStatus,

        /// <summary>
        /// Possible values are: Normal, Personal, Private, Confidential.
        /// </summary>
        Sensitivity,

        /// <summary>
        /// The categories assigned to the item.
        /// </summary>
        SeriesMasterId,

        /// <summary>
        /// The status to show: Free = 0, Tentative = 1, Busy = 2, Oof = 3, WorkingElsewhere = 4, Unknown = -1.
        /// </summary>
        ShowAs,

        /// <summary>
        /// The date, time, and time zone that the event starts.
        /// </summary>
        Start,

        /// <summary>
        /// The subject of the message.
        /// </summary>
        Subject,

        /// <summary>
        /// The event type: SingleInstance = 0, Occurrence = 1, Exception = 2, SeriesMaster = 3.
        /// </summary>
        Type,

        /// <summary>
        /// The URL to open the message in Outlook Web App.
        /// </summary>
        WebLink
    }
}
