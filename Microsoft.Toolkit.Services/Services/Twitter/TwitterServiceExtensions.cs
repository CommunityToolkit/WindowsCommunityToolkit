// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Internal;
using Microsoft.Toolkit.Services.Twitter;
#if WINRT
using Windows.Storage.Streams;
#endif

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// Twitter service extensions to use for Uwp
    /// </summary>
    public static class TwitterServiceExtensions
    {
        /// <summary>
        /// Initialize underlying provider with relevent token information for Uwp.
        /// </summary>
        /// <param name="service">The TwitterService</param>
        /// <param name="consumerKey">Consumer key.</param>
        /// <param name="consumerSecret">Consumer secret.</param>
        /// <param name="callbackUri">Callback URI. Has to match callback URI defined at apps.twitter.com (can be arbitrary).</param>
        /// <returns>Success or failure.</returns>

        #if WINRT
        public static bool Initialize(this TwitterService service, string consumerKey, string consumerSecret, string callbackUri)
        {
            return service.Initialize(consumerKey, consumerSecret, callbackUri, new UwpAuthenticationBroker(), new UWpPasswordManager(), new UwpStorageManager(), new UwpSignatureManager());
        }
        #endif

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="service">The TwitterService.</param>
        /// <param name="message">Tweet message.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Returns success or failure of post request.</returns>
        #if WINRT
        public static async Task<bool> TweetStatusAsync(this TwitterService service, string message, params IRandomAccessStream[] pictures)
        {
            return await service.TweetStatusAsync(new TwitterStatus { Message = message }, pictures);
        }
        #endif

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="service">The TwitterService.</param>
        /// <param name="status">The tweet information.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Returns success or failure of post request.</returns>
        #if WINRT
        public static async Task<bool> TweetStatusAsync(this TwitterService service, TwitterStatus status, params IRandomAccessStream[] pictures)
        {
            if (pictures.Length > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(pictures));
            }

            if (service.Provider.LoggedIn)
            {
                return await service.Provider.TweetStatusAsync(status, pictures);
            }

            var isLoggedIn = await service.LoginAsync();
            if (isLoggedIn)
            {
                return await service.TweetStatusAsync(status, pictures);
            }

            return false;
        }
        #endif
    }
}
