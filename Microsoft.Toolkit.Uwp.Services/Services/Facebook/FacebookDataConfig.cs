// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Facebook
{
    /// <summary>
    /// Configuration object for specifying richer query information.
    /// </summary>
    public class FacebookDataConfig
    {
        /// <summary>
        /// Gets a predefined config to get user feed. The feed of posts (including status updates) and links published by this person, or by others on this person's profile
        /// </summary>
        public static FacebookDataConfig MyFeed => new FacebookDataConfig { Query = "/me/feed" };

        /// <summary>
        /// Gets a predefined config to show only the posts that were published by this person
        /// </summary>
        public static FacebookDataConfig MyPosts => new FacebookDataConfig { Query = "/me/posts" };

        /// <summary>
        /// Gets a predefined config to show only the posts that this person was tagged in
        /// </summary>
        public static FacebookDataConfig MyTagged => new FacebookDataConfig { Query = "/me/tagged" };

        /// <summary>
        /// Gets or sets the query string for filtering service results.
        /// </summary>
        public string Query { get; set; }
    }
}
