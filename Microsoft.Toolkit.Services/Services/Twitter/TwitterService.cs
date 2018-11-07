// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;

#if WINRT
using Microsoft.Toolkit.Services.PlatformSpecific.Uwp;
using Windows.Storage.Streams;
#endif

#if NET462
using Microsoft.Toolkit.Services.PlatformSpecific.NetFramework;
#endif

namespace Microsoft.Toolkit.Services.Twitter
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

        private IPasswordManager passwordManager;
        private IStorageManager storageManager;
        private IAuthenticationBroker authenticationBroker;
        private ISignatureManager signatureManager;

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
        /// <param name="authenticationBroker">Authentication result interface.</param>
        /// <param name="passwordManager">Password Manager interface, store the password.</param>
        /// <param name="storageManager">Storage Manager interface</param>
        /// <param name="signatureManager">Signature manager to sign the OAuth request</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string consumerKey, string consumerSecret, string callbackUri, IAuthenticationBroker authenticationBroker, IPasswordManager passwordManager, IStorageManager storageManager, ISignatureManager signatureManager)
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

            if (authenticationBroker == null)
            {
                throw new ArgumentException(nameof(authenticationBroker));
            }

            if (passwordManager == null)
            {
                throw new ArgumentException(nameof(passwordManager));
            }

            if (storageManager == null)
            {
                throw new ArgumentException(nameof(storageManager));
            }

            if (signatureManager == null)
            {
                throw new ArgumentException(nameof(signatureManager));
            }

            var oAuthTokens = new TwitterOAuthTokens
            {
                ConsumerKey = consumerKey,
                ConsumerSecret = consumerSecret,
                CallbackUri = callbackUri
            };

            return Initialize(oAuthTokens, authenticationBroker, passwordManager, storageManager, signatureManager);
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <param name="authenticationBroker">Authentication result interface.</param>
        /// <param name="passwordManager">Password Manager interface, store the password.</param>
        /// <param name="storageManager">Storage Manager interface</param>
        /// <param name="signatureManager">Signature manager to sign the OAuth request</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(TwitterOAuthTokens oAuthTokens, IAuthenticationBroker authenticationBroker, IPasswordManager passwordManager, IStorageManager storageManager, ISignatureManager signatureManager)
        {
            tokens = oAuthTokens ?? throw new ArgumentNullException(nameof(oAuthTokens));
            this.authenticationBroker = authenticationBroker ?? throw new ArgumentNullException(nameof(authenticationBroker));
            this.passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
            this.storageManager = storageManager ?? throw new ArgumentNullException(nameof(storageManager));
            this.signatureManager = signatureManager ?? throw new ArgumentNullException(nameof(signatureManager));

            isInitialized = true;

            twitterDataProvider = null;

            return true;
        }

#if WINRT
        /// <summary>
        /// Initialize underlying provider with relevent token information for Uwp.
        /// </summary>
        /// <param name="consumerKey">Consumer key.</param>
        /// <param name="consumerSecret">Consumer secret.</param>
        /// <param name="callbackUri">Callback URI. Has to match callback URI defined at apps.twitter.com (can be arbitrary).</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string consumerKey, string consumerSecret, string callbackUri)
        {
            return Initialize(consumerKey, consumerSecret, callbackUri, new UwpAuthenticationBroker(), new UwpPasswordManager(), new UwpStorageManager(), new UwpSignatureManager());
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(TwitterOAuthTokens oAuthTokens)
        {
            return Initialize(oAuthTokens, new UwpAuthenticationBroker(), new UwpPasswordManager(), new UwpStorageManager(), new UwpSignatureManager());
        }
#endif

#if NET462
        /// <summary>
        /// Initialize underlying provider with relevent token information for Uwp.
        /// </summary>
        /// <param name="consumerKey">Consumer key.</param>
        /// <param name="consumerSecret">Consumer secret.</param>
        /// <param name="callbackUri">Callback URI. Has to match callback URI defined at apps.twitter.com (can be arbitrary).</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string consumerKey, string consumerSecret, string callbackUri)
        {
            return Initialize(consumerKey, consumerSecret, callbackUri, new NetFrameworkAuthenticationBroker(), new NetFrameworkPasswordManager(), new NetFrameworkStorageManager(), new NetFrameworkSignatureManager());
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(TwitterOAuthTokens oAuthTokens)
        {
            return Initialize(oAuthTokens, new NetFrameworkAuthenticationBroker(), new NetFrameworkPasswordManager(), new NetFrameworkStorageManager(), new NetFrameworkSignatureManager());
        }
#endif

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

                return twitterDataProvider ?? (twitterDataProvider = new TwitterDataProvider(tokens, authenticationBroker, passwordManager, storageManager, signatureManager));
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
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="message">Tweet message.</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(string message)
        {
            return await TweetStatusAsync(new TwitterStatus { Message = message });
        }

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="message">Tweet message.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(string message, params Stream[] pictures)
        {
            return await TweetStatusAsync(new TwitterStatus { Message = message }, pictures);
        }

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="status">The tweet information.</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(TwitterStatus status)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.TweetStatusAsync(status);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await TweetStatusAsync(status);
            }

            return false;
        }

        /// <summary>
        /// Post a Tweet with associated pictures.
        /// </summary>
        /// <param name="status">The tweet information.</param>
        /// <param name="pictures">Pictures to attach to the tweet (up to 4).</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(TwitterStatus status, params Stream[] pictures)
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
        [Obsolete("Logout is deprecated, please use LogoutAsync instead.", true)]
        public void Logout()
        {
            Provider.Logout();
        }

        /// <summary>
        /// Log user out of Twitter.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task LogoutAsync()
        {
           return Provider.LogoutAsync();
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