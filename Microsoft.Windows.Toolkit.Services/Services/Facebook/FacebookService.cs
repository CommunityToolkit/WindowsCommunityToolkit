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
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Windows.Toolkit.Services.Core;
using Microsoft.Windows.Toolkit.Services.Services.Facebook;
using Newtonsoft.Json;
using Windows.Foundation.Collections;
using Windows.Security.Authentication.Web;
using Windows.UI.Popups;
using winsdkfb;
using winsdkfb.Graph;

namespace Microsoft.Windows.Toolkit.Services.Facebook
{
    /// <summary>
    /// Class for connecting to underlying service data provider.
    /// Developer may modify the OAuth settings in the constructor after generation, but be aware that these will get overwritten if re-adding the Connected Service instance for this provider.
    /// </summary>
    public class FacebookService : IOAuthDataService<FBSession, FacebookSchema, FacebookDataConfig, FacebookOAuthTokens>
    {
        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool isInitialized;

        /// <summary>
        /// Reference to required Facebook requiredPermissions for the app.
        /// </summary>
        private FBPermissions permissions;

        /// <summary>
        /// Reference to paginated array object for handling multiple pages of returned service list data.
        /// </summary>
        private FBPaginatedArray paginatedArray;

        /// <summary>
        /// Strongly typed list of service query data.
        /// </summary>
        private List<FacebookSchema> queryResults;

        /// <summary>
        /// Gets a Windows Store ID that can be used during development time
        /// </summary>
        public string DevelopmentTimeWindowsStoreId => WebAuthenticationBroker.GetCurrentApplicationCallbackUri().ToString();

        /// <summary>
        /// Initializes a new instance of the <see cref="FacebookService"/> class.
        /// Default private constructor.
        /// </summary>
        private FacebookService()
        {
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(FacebookOAuthTokens oAuthTokens)
        {
            if (oAuthTokens == null)
            {
                throw new ArgumentNullException(nameof(oAuthTokens));
            }

            return Initialize(oAuthTokens.AppId, oAuthTokens.WindowsStoreId);
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="appId">Application ID (Provided by Facebook developer site)</param>
        /// <param name="windowsStoreId">Windows Store SID</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appId, string windowsStoreId)
        {
            if (string.IsNullOrEmpty(appId))
            {
                throw new ArgumentNullException(nameof(appId));
            }

            if (string.IsNullOrEmpty(windowsStoreId))
            {
                throw new ArgumentNullException(nameof(windowsStoreId));
            }

            isInitialized = true;

            var provider = GetProvider();

            provider.FBAppId = appId;
            provider.WinAppId = windowsStoreId;

            return true;
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static FacebookService instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static FacebookService Instance => instance ?? (instance = new FacebookService());

        /// <summary>
        /// Returns a reference to an instance of the underlying data provider.
        /// </summary>
        /// <returns>FBSession instance.</returns>
        public FBSession GetProvider()
        {
            if (!isInitialized)
            {
                throw new InvalidOperationException("Provider not initialized.");
            }

            return FBSession.ActiveSession;
        }

        /// <summary>
        /// Log in to the underlying service instance.
        /// </summary>
        /// <returns>Returns success or failure of login.</returns>
        /// <remarks>public_profile and user_posts permissions will be used by default.</remarks>
        public async Task<bool> LoginAsync()
        {
            return await LoginAsync(FacebookPermissions.PublicProfile | FacebookPermissions.UserPosts);
        }

        /// <summary>
        /// Login with set of required requiredPermissions.
        /// </summary>
        /// <param name="requiredPermissions">List of required required permissions.</param>
        /// <returns>Success or failure.</returns>
        public async Task<bool> LoginAsync(FacebookPermissions requiredPermissions)
        {
            var permissionsStringList = new List<string>();

            foreach (FacebookPermissions value in Enum.GetValues(typeof(FacebookPermissions)))
            {
                if ((requiredPermissions & value) != 0)
                {
                    var name = value.ToString();
                    var finalName = new StringBuilder();

                    foreach (var c in name)
                    {
                        if (char.IsUpper(c))
                        {
                            if (finalName.Length > 0)
                            {
                                finalName.Append('_');
                            }

                            finalName.Append(char.ToLower(c));
                        }
                        else
                        {
                            finalName.Append(c);
                        }
                    }

                    permissionsStringList.Add(finalName.ToString());
                }
            }

            return await LoginAsync(permissionsStringList);
        }

        /// <summary>
        /// Login with set of required requiredPermissions.
        /// </summary>
        /// <param name="requiredPermissions">List of required required permissions.</param>
        /// <returns>Success or failure.</returns>
        public async Task<bool> LoginAsync(List<string> requiredPermissions)
        {
            permissions = new FBPermissions(requiredPermissions);

            var provider = GetProvider();

            if (provider != null)
            {
                var result = await provider.LoginAsync(permissions, SessionLoginBehavior.WebAuth);

                if (result.Succeeded)
                {
                    return true;
                }

                if (result.ErrorInfo != null)
                {
                    Debug.WriteLine(string.Format("Error logging in: {0}", result.ErrorInfo.Message));
                }

                return false;
            }

            Debug.WriteLine("Error logging in - no Active session found");
            return false;
        }

        /// <summary>
        /// Log out of the underlying service instance.
        /// </summary>
        /// <returns>Task to support await of async call.</returns>
        public async Task LogoutAsync()
        {
            var provider = GetProvider();

            await provider.LogoutAsync();
        }

        /// <summary>
        /// Request list data from service provider based upon a given config / query.
        /// </summary>
        /// <param name="config">TwitterDataConfig instance.</param>
        /// <param name="maxRecords">Upper limit of records to return.</param>
        /// <returns>Strongly typed list of data returned from the service.</returns>
        public async Task<List<FacebookSchema>> RequestAsync(FacebookDataConfig config, int maxRecords = 20)
        {
            var provider = GetProvider();

            if (provider.LoggedIn)
            {
                queryResults = new List<FacebookSchema>();

                PropertySet propertySet = new PropertySet { { "fields", "id,message,from,created_time,link,full_picture" } };

                var factory = new FBJsonClassFactory(JsonConvert.DeserializeObject<FacebookSchema>);

                paginatedArray = new FBPaginatedArray(config.Query, propertySet, factory);

                var result = await paginatedArray.FirstAsync();

                if (result.Succeeded)
                {
                    IReadOnlyList<object> results = (IReadOnlyList<object>)result.Object;

                    await ProcessResultsAsync(results, maxRecords);

                    return queryResults;
                }

                throw new Exception(result.ErrorInfo?.Message);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await RequestAsync(config, maxRecords);
            }

            return null;
        }

        /// <summary>
        /// Enables posting data to the timeline of the underlying service provider instance.
        /// </summary>
        /// <param name="title">Title of the post.</param>
        /// <param name="link">Link contained as part of the post.</param>
        /// <param name="description">Description of the post.</param>
        /// <returns>Task to support await of async call.</returns>
        public async Task<bool> PostToFeedAsync(string title, string link, string description)
        {
            var provider = GetProvider();

            if (provider.LoggedIn)
            {
                var parameters = new PropertySet { { "title", title }, { "link", link }, { "description", description } };

                var result = await provider.ShowFeedDialogAsync(parameters);

                if (result.Succeeded)
                {
                    await new MessageDialog("Posted.").ShowAsync();
                    return true;
                }

                Debug.WriteLine(string.Format("Could not post. {0}", result.ErrorInfo?.ErrorUserMessage));
                return false;
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await PostToFeedAsync(title, link, description);
            }

            return false;
        }

        /// <summary>
        /// Helper method to process pages of results from underlying service instance.
        /// </summary>
        /// <param name="results">List of results to process.</param>
        /// <param name="maxRecords">Total upper limit of records to process.</param>
        /// <returns>Task to support await of async call.</returns>
        private async Task ProcessResultsAsync(IReadOnlyList<object> results, int maxRecords)
        {
            foreach (FacebookSchema result in results)
            {
                if (queryResults.Count < maxRecords)
                {
                    queryResults.Add(result);
                }
            }

            if (paginatedArray.HasNext && queryResults.Count < maxRecords)
            {
                var nextResult = await paginatedArray.NextAsync();
                if (nextResult.Succeeded)
                {
                    IReadOnlyList<object> nextResults = (IReadOnlyList<object>)nextResult.Object;
                    await ProcessResultsAsync(nextResults, maxRecords);
                }
            }
        }
    }
}
