// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Twitter;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// TwitterDataProvider extensions for help methods using Uwp
    /// </summary>
    internal static class TwitterDataProviderExtensions
    {
        /// <summary>
        /// Tweets a status update.
        /// </summary>
        /// <param name="provider">The TwitterDataProvider.</param>
        /// <param name="tweet">Tweet text.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Success or failure.</returns>
        public static async Task<bool> TweetStatusAsync(this TwitterDataProvider provider, string tweet, params IRandomAccessStream[] pictures)
        {
            return await provider.TweetStatusAsync(new TwitterStatus { Message = tweet }, pictures);
        }

        /// <summary>
        /// Tweets a status update.
        /// </summary>
        /// <param name="provider">The TwitterDataProvider.</param>
        /// <param name="status">Tweet text.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Success or failure.</returns>
        public static async Task<bool> TweetStatusAsync(this TwitterDataProvider provider, TwitterStatus status, params IRandomAccessStream[] pictures)
        {
            var mediaIds = string.Empty;

            if (pictures != null && pictures.Length > 0)
            {
                var ids = new List<string>();
                foreach (var picture in pictures)
                {
                    ids.Add(await provider.UploadPictureAsync(picture));
                }

                mediaIds = "&media_ids=" + string.Join(",", ids);
            }

            var uri = new Uri($"{provider.BaseUrl}/statuses/update.json?{status.RequestParameters}{mediaIds}");

            TwitterOAuthRequest request = new TwitterOAuthRequest();
            await request.ExecutePostAsync(uri, provider.Tokens, provider.SigntureManager);

            return true;
        }

        /// <summary>
        /// Publish a picture to Twitter user's medias.
        /// </summary>
        /// <param name="provider">The TwitterDataProvider.</param>
        /// <param name="stream">Picture stream.</param>
        /// <returns>Media ID</returns>
        public static async Task<string> UploadPictureAsync(this TwitterDataProvider provider, IRandomAccessStream stream)
        {
            var uri = new Uri($"{provider.PublishUrl}/media/upload.json");

            // Get picture data
            var fileBytes = new byte[stream.Size];

            await stream.ReadAsync(fileBytes.AsBuffer(), (uint)stream.Size, InputStreamOptions.None);

            stream.Seek(0);

            string boundary = DateTime.Now.Ticks.ToString("x");

            TwitterOAuthRequest request = new TwitterOAuthRequest();
            return await request.ExecutePostMultipartAsync(uri, provider.Tokens, boundary, fileBytes, provider.SigntureManager);
        }
    }
}
