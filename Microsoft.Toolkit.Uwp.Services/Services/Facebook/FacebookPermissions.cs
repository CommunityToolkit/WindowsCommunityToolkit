// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// List of user related data permissions
    /// </summary>
    [Flags]
    public enum FacebookPermissions
    {
        /// <summary>
        /// Public profile
        /// </summary>
        PublicProfile = 1,

        /// <summary>
        /// Email
        /// </summary>
        Email = 2,

        /// <summary>
        /// Publish actions
        /// </summary>
        PublishActions = 4,

        /// <summary>
        /// About me
        /// </summary>
        UserAboutMe = 8,

        /// <summary>
        /// Birthday
        /// </summary>
        UserBirthday = 16,

        /// <summary>
        /// Education history
        /// </summary>
        UserEducationHistory = 32,

        /// <summary>
        /// Friends
        /// </summary>
        UserFriends = 64,

        /// <summary>
        /// Games activity
        /// </summary>
        UserGamesActivity = 128,

        /// <summary>
        /// Hometown
        /// </summary>
        UserHometown = 256,

        /// <summary>
        /// Likes
        /// </summary>
        UserLikes = 512,

        /// <summary>
        /// Location
        /// </summary>
        UserLocation = 1024,

        /// <summary>
        /// Photos
        /// </summary>
        UserPhotos = 2048,

        /// <summary>
        /// Posts
        /// </summary>
        UserPosts = 4096,

        /// <summary>
        /// Relationship details
        /// </summary>
        UserRelationshipDetails = 8192,

        /// <summary>
        /// Relationships
        /// </summary>
        UserRelationships = 16384,

        /// <summary>
        /// Religion and politics
        /// </summary>
        UserReligionPolitics = 32768,

        /// <summary>
        /// Status
        /// </summary>
        UserStatus = 65536,

        /// <summary>
        /// Tagged places
        /// </summary>
        UserTaggedPlaces = 131072,

        /// <summary>
        /// Videos
        /// </summary>
        UserVideos = 262144,

        /// <summary>
        /// Website
        /// </summary>
        UserWebsite = 524288,

        /// <summary>
        /// WorkHistory
        /// </summary>
        UserWorkHistory = 1048576
    }
}
