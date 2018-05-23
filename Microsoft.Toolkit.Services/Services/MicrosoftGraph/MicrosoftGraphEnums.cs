// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// MicrosoftGraphService enums
    /// </summary>
    public class MicrosoftGraphEnums
    {
        /// <summary>
        /// Specifies the version of the authorization endpoint to use.
        /// </summary>
        public enum AuthenticationModel
        {
            /// <summary>
            /// Uses the converged EndPoint in order to authenticate either a Microsoft Account or a Work or School Account
            /// </summary>
            V2,

            /// <summary>
            /// Uses the Azure Active Directory Endpoint
            /// </summary>
            V1
        }

        /// <summary>
        /// Specifies the size of the items's thumbnail
        /// </summary>
        public enum ThumbnailSize
        {
            /// <summary>
            /// Large
            /// </summary>
            Large,

            /// <summary>
            /// Medium
            /// </summary>
            Medium,

            /// <summary>
            /// Small
            /// </summary>
            Small
        }

        /// <summary>
        /// Specifies the services to use.
        /// </summary>
        [Flags]
        public enum ServicesToInitialize
        {
            /// <summary>
            /// OneDrive (Reserved for future use)
            /// </summary>
            OneDrive = 0x01,

            /// <summary>
            /// Message
            /// </summary>
            Message = 0x02,

            /// <summary>
            /// user Profile
            /// </summary>
            UserProfile = 0x04,

            /// <summary>
            /// Event
            /// </summary>
            Event = 0x08
        }

        /// <summary>
        /// Specifies in wich order to sort the file or folder
        /// </summary>
        public enum OrderBy
        {
            /// <summary>
            /// Do nothing
            /// </summary>
            None,

            /// <summary>
            /// Use the name to order the items with "A" first
            /// </summary>
            Name,

            /// <summary>
            /// Use the name to order the items, with "A" first, new form
            /// </summary>
            NameAsc = Name,

            /// <summary>
            /// Use the name to order the items, with "Z" first
            /// </summary>
            NameDesc,

            /// <summary>
            /// Use the size to order the items, with the smallest first
            /// </summary>
            Size,

            /// <summary>
            /// Use the size to order the items, with the smallest first, new form
            /// </summary>
            SizeAsc = Size,

            /// <summary>
            /// Use the size to order the items, with the largest first
            /// </summary>
            SizeDesc,

            /// <summary>
            /// Use the date to order the items, with the oldest first
            /// </summary>
            Date,

            /// <summary>
            /// Use the date to order the items, with the oldest first, new form
            /// </summary>
            DateAsc = Date,

            /// <summary>
            /// Use the date to order the items, with the newest first
            /// </summary>
            DateDesc
        }

        /// <summary>
        /// Specifies what to do if a file or folder with the specified name already exists
        /// in the current folder when you copy, move, or rename a file or folder.
        /// </summary>
        public enum OneDriveItemNameCollisionOption
        {
            /// <summary>
            /// Raise an exception if the file or folder already exists
            /// </summary>
            Fail,

            /// <summary>
            /// Replace the existing item if the file or folder already exists.
            /// </summary>
            Replace,

            /// <summary>
            /// Rename the existing item.
            /// </summary>
            Rename
        }
    }
}
