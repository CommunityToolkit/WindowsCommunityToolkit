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
using System;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    /// <summary>
    /// Strong typed Twitter schema.
    /// </summary>
    public class Tweet : SchemaBase
    {
        /// <summary>
        /// Gets or sets tweet text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets creation date and time.
        /// </summary>
        public DateTime CreationDateTime { get; set; }

        /// <summary>
        /// Gets or sets userId of Tweeter.
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        /// Gets or sets userName of Tweeter.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets user screen name of Tweeter.
        /// </summary>
        public string UserScreenName { get; set; }

        /// <summary>
        /// Gets or sets profile image of user.
        /// </summary>
        public string UserProfileImageUrl { get; set; }

        /// <summary>
        /// Gets or sets url of tweet.
        /// </summary>
        public string Url { get; set; }
    }
}
