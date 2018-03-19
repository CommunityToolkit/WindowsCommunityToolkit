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
