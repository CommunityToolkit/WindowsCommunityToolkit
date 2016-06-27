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
using System.Globalization;
using Microsoft.Windows.Toolkit.Services.Core;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    /// <summary>
    /// Twitter Parser.
    /// </summary>
    internal static class TwitterParser
    {
        /// <summary>
        /// Parse Twitter timeline item into strong type.
        /// </summary>
        /// <param name="item">TwitterTimelineItem item.</param>
        /// <returns>Strong typed object.</returns>
        public static Tweet Parse(this TwitterTimelineItem item)
        {
            Tweet tweet = new Tweet
            {
                InternalID = item.Id,
                Text = item.Text.DecodeHtml(),
                CreationDateTime = TryParse(item.CreatedAt)
            };

            if (item.User == null)
            {
                tweet.UserId = string.Empty;
                tweet.UserName = string.Empty;
                tweet.UserScreenName = string.Empty;
                tweet.UserProfileImageUrl = string.Empty;
                tweet.Url = string.Empty;
            }
            else
            {
                tweet.UserId = item.User.Id;
                tweet.UserName = item.User.Name.DecodeHtml();
                tweet.UserScreenName = string.Concat("@", item.User.ScreenName.DecodeHtml());
                tweet.UserProfileImageUrl = item.User.ProfileImageUrl;
                tweet.Url = string.Format("https://twitter.com/{0}/status/{1}", item.User.ScreenName, item.Id);
                if (!string.IsNullOrEmpty(tweet.UserProfileImageUrl))
                {
                    tweet.UserProfileImageUrl = tweet.UserProfileImageUrl.Replace("_normal", string.Empty);
                }
            }

            return tweet;
        }

        /// <summary>
        /// Parse string into strong DateTime type.
        /// </summary>
        /// <param name="dateTime">DateTime string.</param>
        /// <returns>Strong typed DateTime.</returns>
        private static DateTime TryParse(string dateTime)
        {
            DateTime dt;
            if (!DateTime.TryParseExact(dateTime, "ddd MMM dd HH:mm:ss zzzz yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out dt))
            {
                dt = DateTime.Today;
            }

            return dt;
        }
    }
}
