// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Windows.Storage.Streams;

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Class for connecting to Twitter.
    /// </summary>
    public class TwitterService
    {
        /// <summary>
        /// Private field for TwitterDataProvider.
        /// </summary>
        private TwitterDataProvider twitterDataProvider;

        /// <summary>
        /// Field for tracking oAuthTokens.
        /// </summary>
        private TwitterOAuthTokens tokens;

        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="TwitterService"/> class.
        /// </summary>
        public TwitterService()
        {
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static TwitterService instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static TwitterService Instance => instance ?? (instance = new TwitterService());

        /// <summary>
        /// Gets the current logged in user screen name.
        /// </summary>
        public string UserScreenName => Provider.UserScreenName;

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="consumerKey">Consumer key.</param>
        /// <param name="consumerSecret">Consumer secret.</param>
        /// <param name="callbackUri">Callback URI. Has to match callback URI defined at apps.twitter.com (can be arbitrary).</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string consumerKey, string consumerSecret, string callbackUri)
        {
            if (string.IsNullOrEmpty(consumerKey))
            {
                throw new ArgumentNullException(nameof(consumerKey));
            }

            if (string.IsNullOrEmpty(consumerSecret))
            {
                throw new ArgumentNullException(nameof(consumerSecret));
            }

            if (string.IsNullOrEmpty(callbackUri))
            {
                throw new ArgumentNullException(nameof(callbackUri));
            }

            var oAuthTokens = new TwitterOAuthTokens
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                CallbackUri = callbackUri
            };

            return Initialize(oAuthTokens);
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(TwitterOAuthTokens oAuthTokens)
        {
            if (oAuthTokens == null)
            {
                throw new ArgumentNullException(nameof(oAuthTokens));
            }

            tokens = oAuthTokens;
            isInitialized = true;

            twitterDataProvider = null;

            return true;
        }

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public TwitterDataProvider Provider
        {
            get
            {
                if (!isInitialized)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return twitterDataProvider ?? (twitterDataProvider = new TwitterDataProvider(tokens));
            }
        }

        /// <summary>
        /// Search for specific hash tag.
        /// </summary>
        /// <param name="hashTag">Hash tag.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <returns>Returns strongly typed list of results.</returns>
        public async Task<IEnumerable<Tweet>> SearchAsync(string hashTag, int maxRecords = 20)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.SearchAsync(hashTag, maxRecords, new TwitterSearchParser());
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await SearchAsync(hashTag, maxRecords);
            }

            return null;
        }

        /// <summary>
        /// Retrieve user data.
        /// </summary>
        /// <param name="screenName">User screen name or null for current logged user.</param>
        /// <returns>Returns user data.</returns>
        public async Task<TwitterUser> GetUserAsync(string screenName = null)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.GetUserAsync(screenName);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await GetUserAsync(screenName);
            }

            return null;
        }

        /// <summary>
        /// Retrieve user timeline data.
        /// </summary>
        /// <param name="screenName">User screen name.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <returns>Returns strongly typed list of results.</returns>
        public async Task<IEnumerable<Tweet>> GetUserTimeLineAsync(string screenName, int maxRecords = 20)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.GetUserTimeLineAsync(screenName, maxRecords, new TweetParser());
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await GetUserTimeLineAsync(screenName, maxRecords);
            }

            return null;
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">TwitterDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return. Up to a maximum of 200 per distinct request.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<Tweet>> RequestAsync(TwitterDataConfig config, int maxRecords = 20)
        {
            return await RequestAsync<Tweet>(config, maxRecords);
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <typeparam name="T">Model type expected back - e.g. Tweet.</typeparam>
        /// <param name="config">TwitterDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return. Up to a maximum of 200 per distinct request.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<T>> RequestAsync<T>(TwitterDataConfig config, int maxRecords = 20)
            where T : Toolkit.Parsers.SchemaBase
        {
            if (Provider.LoggedIn)
            {
                List<T> queryResults = new List<T>();

                var results = await Provider.LoadDataAsync<T>(config, maxRecords, 0, new TwitterParser<T>());

                foreach (var result in results)
                {
                    queryResults.Add(result);
                }

                return queryResults;
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await RequestAsync<T>(config, maxRecords);
            }

            return null;
        }

        /// <summary>
        /// Log user in to Twitter.
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        public Task<bool> LoginAsync()
        {
            return Provider.LoginAsync();
        }

        /// <summary>
        /// Log user out of Twitter.
        /// </summary>
        public void Logout()
        {
            Provider.Logout();
        }

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="message">Tweet message.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(string message, params IRandomAccessStream[] pictures)
        {
            return await TweetStatusAsync(new TwitterStatus { Message = message }, pictures);
        }

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="status">The tweet information.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(TwitterStatus status, params IRandomAccessStream[] pictures)
        {
            if (pictures.Length > 4)
            {
                throw new ArgumentOutOfRangeException(nameof(pictures));
            }

            if (Provider.LoggedIn)
            {
                return await Provider.TweetStatusAsync(status, pictures);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await TweetStatusAsync(status, pictures);
            }

            return false;
        }

        /// <summary>
        /// Open a connection to user's stream service
        /// </summary>
        /// <param name="callback">Method called each time a tweet arrives</param>
        /// <returns>Task</returns>
        public async Task StartUserStreamAsync(TwitterStreamCallbacks.TwitterStreamCallback callback)
        {
            if (Provider.LoggedIn)
            {
                await Provider.StartUserStreamAsync(new TwitterUserStreamParser(), callback);
                return;
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                await StartUserStreamAsync(callback);
            }
        }

        /// <summary>
        /// Close the connection to user's stream service
        /// </summary>
        public void StopUserStream()
        {
            Provider.StopStream();
        }
    }
}