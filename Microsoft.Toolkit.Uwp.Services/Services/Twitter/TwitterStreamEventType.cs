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

using System.Runtime.Serialization;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Describes the type of event that has occured on twitter
    /// </summary>
    public enum TwitterStreamEventType
    {
        /// <summary>
        /// The event is an unknown type
        /// </summary>
        Unknown,

        /// <summary>
        /// The source user has blocked the target user.
        /// </summary>
        [EnumMember(Value = "block")]
        Block,

        /// <summary>
        /// The source user has unblocked the target user.
        /// </summary>
        [EnumMemberAttribute(Value = "unblock")]
        Unblock,

        /// <summary>
        /// The source user has favorited the target users tweet.
        /// </summary>
        [EnumMemberAttribute(Value = "favorite")]
        Favorite,

        /// <summary>
        /// The source user has unfaovorited the target users tweet.
        /// </summary>
        [EnumMemberAttribute(Value = "unfavorite")]
        Unfavorite,

        /// <summary>
        /// The source user has followed the target user.
        /// </summary>
        [EnumMemberAttribute(Value = "follow")]
        Follow,

        /// <summary>
        /// The source user has unfollowed the target user.
        /// </summary>
        [EnumMemberAttribute(Value = "unfollow")]
        Unfollow,

        /// <summary>
        /// The source user has added the target user to the a list.
        /// </summary>
        [EnumMemberAttribute(Value = "list_member_added")]
        ListMemberAdded,

        /// <summary>
        /// The source user has removed the target user from a list.
        /// </summary>
        [EnumMemberAttribute(Value = "list_member_removed")]
        ListMemberRemoved,

        /// <summary>
        /// The source user has subscribed to a list.
        /// </summary>
        [EnumMemberAttribute(Value = "list_user_subscribed")]
        ListUserSubscribed,

        /// <summary>
        /// The source user has unsubscribed from a list.
        /// </summary>
        [EnumMemberAttribute(Value = "list_user_unsubscribed")]
        ListUserUnsubscribed,

        /// <summary>
        /// The source user created a list.
        /// </summary>
        [EnumMemberAttribute(Value = "list_created")]
        ListCreated,

        /// <summary>
        /// The source user update a lists properties.
        /// </summary>
        [EnumMemberAttribute(Value = "list_updated")]
        ListUpdated,

        /// <summary>
        /// The source user deleted a list.
        /// </summary>
        [EnumMemberAttribute(Value = "list_destroyed")]
        ListDestroyed,

        /// <summary>
        /// The source users profile was updated.
        /// </summary>
        [EnumMemberAttribute(Value = "user_update")]
        UserUpdated,

        /// <summary>
        /// The source users profile was updated.
        /// </summary>
        [EnumMemberAttribute(Value = "access_revoked")]
        AccessRevoked,

        /// <summary>
        /// The source users tweet was quoted.
        /// </summary>
        [EnumMemberAttribute(Value = "quoted_tweet")]
        QuotedTweet
    }
}
