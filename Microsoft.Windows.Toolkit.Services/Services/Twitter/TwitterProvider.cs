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
    /// Generated class for connecting to underlying service data provider.
    /// Developer may modify the OAuth settings in the GetProvider() method after generation, but be aware that these will get overwritten if re-adding the Connected Service instance for this provider.
    /// </summary>
    public class TwitterProvider : IOAuthDataService<TwitterDataProvider, TwitterSchema, TwitterDataConfig, TwitterOAuthTokens>
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
        /// Initializes a new instance of the <see cref="TwitterProvider"/> class.
        /// Default private constructor.
        /// </summary>
        private TwitterProvider()
        {
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static TwitterProvider instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static TwitterProvider Instance => instance ?? (instance = new TwitterProvider());

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

            this.tokens = oAuthTokens;
            isInitialized = true;

            return true;
        }

        /// <summary>
        /// Returns a reference to an instance of the underlying data provider.
        /// </summary>
        /// <returns>TwitterDataProvider instance.</returns>
        public TwitterDataProvider GetProvider()
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("Provider not initialized.");
            }

            return twitterDataProvider ?? (twitterDataProvider = new TwitterDataProvider(tokens));
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">TwitterDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<TwitterSchema>> RequestAsync(TwitterDataConfig config, int maxRecords = 20)
        {
            List<TwitterSchema> queryResults = new List<TwitterSchema>();

            var results = await GetProvider().LoadDataAsync(config, maxRecords);

            foreach (var result in results)
            {
                queryResults.Add(result);
            }

            return queryResults;
        }

        /// <summary>
        /// Not currently supported for this service provider.
        /// </summary>
        /// <returns>Returns success or failure of login attempt.</returns>
        public async Task<bool> LoginAsync()
        {
            var provider = GetProvider();

            return await provider.LoginAsync();
        }

        /// <summary>
        /// Not supported for Twitter Provider.
        /// </summary>
        /// <param name="requiredPermissions">Not supported.</param>
        /// <returns>Returns success or failure of login request.</returns>
        public Task<bool> LoginAsync(List<string> requiredPermissions)
        {
            throw new NotImplementedException();
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
        /// <param name="link">Link parameter is not used.</param>
        /// <param name="description">Description parameter is not used.</param>
        /// <returns>Returns success or failure of post request.</returns>
        public async Task<bool> PostToFeedAsync(string title, string link = "", string description = "")
        {
            var provider = GetProvider();

            return await provider.TweetStatus(title);
        }
    }
}
