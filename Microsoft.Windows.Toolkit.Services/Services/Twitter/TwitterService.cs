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
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Windows.Toolkit.Services.Core;

namespace Microsoft.Windows.Toolkit.Services.Twitter
{
    /// <summary>
    /// Class for connecting to Twitter.
    /// </summary>
    public class TwitterService : IOAuthDataService<TwitterDataProvider, Tweet, TwitterDataConfig, TwitterOAuthTokens>
    {
        /// <summary>
        /// Private singleton field for TwitterDataProvider.
        /// </summary>
        private static TwitterDataProvider twitterDataProvider;

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
        /// Default private constructor.
        /// </summary>
        private TwitterService()
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
        public async Task<IEnumerable<Tweet>> SearchAsync(string hashTag, int maxRecords)
        {
            return await Provider.SearchAsync(hashTag, maxRecords, new TwitterSearchParser());
        }

        /// <summary>
        /// Retrieve user timeline data.
        /// </summary>
        /// <param name="screenName">User screen name.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <returns>Returns strongly typed list of results.</returns>
        public async Task<IEnumerable<Tweet>> GetUserTimeLineAsync(string screenName, int maxRecords)
        {
            return await Provider.GetUserTimeLineAsync(screenName, maxRecords, new TwitterTimelineParser());
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">TwitterDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<Tweet>> RequestAsync(TwitterDataConfig config, int maxRecords = 20)
        {
            List<Tweet> queryResults = new List<Tweet>();

            var results = await Provider.LoadDataAsync(config, maxRecords);

            foreach (var result in results)
            {
                queryResults.Add(result);
            }

            return queryResults;
        }

        /// <summary>
        /// Log user in to Twitter.
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync()
        {
            return await Provider.LoginAsync();
        }

        /// <summary>
        /// Not currently supported for this service provider.
        /// </summary>
        /// <returns>Task to support await of async call.</returns>
        public Task LogoutAsync()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Post a Tweet.
        /// </summary>
        /// <param name="title">Tweet message.</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> TweetStatusAsync(string title)
        {
            return await Provider.TweetStatus(title);
        }
    }
}
