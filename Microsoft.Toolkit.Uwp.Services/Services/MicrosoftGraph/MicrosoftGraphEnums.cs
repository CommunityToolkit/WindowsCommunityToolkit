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

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
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
            /// Uses the Azure Active Directory Endpoint
            /// </summary>
            V1,

            /// <summary>
            /// Uses the converged EndPoint in order to authenticate either a Microsoft Account or a Work or School Account
            /// </summary>
            V2
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
            /// Use the name to order the items
            /// </summary>
            Name
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
