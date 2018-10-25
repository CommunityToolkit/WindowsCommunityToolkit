// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;
#if WINRT
using Microsoft.Toolkit.Services.PlatformSpecific.Uwp;
using Windows.Storage.Streams;
#endif

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Class for connecting to Weibo.
    /// </summary>
    public class WeiboService
    {
        /// <summary>
        /// Private field for WeiboDataProvider.
        /// </summary>
        private WeiboDataProvider _weiboDataProvider;

        /// <summary>
        /// Field for tracking oAuthTokens.
        /// </summary>
        private WeiboOAuthTokens _tokens;

        private IPasswordManager _passwordManager;
        private IStorageManager _storageManager;
        private IAuthenticationBroker _authenticationBroker;

        /// <summary>
        /// Field for tracking initialization status.
        /// </summary>
        private bool _isInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboService"/> class.
        /// </summary>
        public WeiboService()
        {
        }

        /// <summary>
        /// Private singleton field.
        /// </summary>
        private static WeiboService _instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static WeiboService Instance => _instance ?? (_instance = new WeiboService());

        /// <summary>
        /// Gets the current logged in user id.
        /// </summary>
        public long? Uid => Provider.Uid;

        /// <summary>
        /// Initialize underlying provider with relevant token information.
        /// </summary>
        /// <param name="appKey">App key.</param>
        /// <param name="appSecret">App secret.</param>
        /// <param name="redirectUri">Redirect URI. Has to match redirect URI defined at open.weibo.com/apps (can be arbitrary).</param>
        /// <param name="authenticationBroker">Authentication result interface.</param>
        /// <param name="passwordManager">Password Manager interface, store the password.</param>
        /// <param name="storageManager">Storage Manager interface</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appKey, string appSecret, string redirectUri, IAuthenticationBroker authenticationBroker, IPasswordManager passwordManager, IStorageManager storageManager)
        {
            if (string.IsNullOrEmpty(appKey))
            {
                throw new ArgumentNullException(nameof(appKey));
            }

            if (string.IsNullOrEmpty(appSecret))
            {
                throw new ArgumentNullException(nameof(appSecret));
            }

            if (string.IsNullOrEmpty(redirectUri))
            {
                throw new ArgumentNullException(nameof(redirectUri));
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

            var oAuthTokens = new WeiboOAuthTokens
            {
                AppKey = appKey,
                AppSecret = appSecret,
                RedirectUri = redirectUri
            };

            return Initialize(oAuthTokens, authenticationBroker, passwordManager, storageManager);
        }

        /// <summary>
        /// Initialize underlying provider with relevant token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <param name="authenticationBroker">Authentication result interface.</param>
        /// <param name="passwordManager">Password Manager interface, store the password.</param>
        /// <param name="storageManager">Storage Manager interface</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(WeiboOAuthTokens oAuthTokens, IAuthenticationBroker authenticationBroker, IPasswordManager passwordManager, IStorageManager storageManager)
        {
            _tokens = oAuthTokens ?? throw new ArgumentNullException(nameof(oAuthTokens));
            this._authenticationBroker = authenticationBroker ?? throw new ArgumentNullException(nameof(authenticationBroker));
            this._passwordManager = passwordManager ?? throw new ArgumentNullException(nameof(passwordManager));
            this._storageManager = storageManager ?? throw new ArgumentNullException(nameof(storageManager));

            _isInitialized = true;

            _weiboDataProvider = null;

            return true;
        }

#if WINRT
        /// <summary>
        /// Initialize underlying provider with relevent token information for Uwp.
        /// </summary>
        /// <param name="appKey">App key.</param>
        /// <param name="appSecret">App secret.</param>
        /// <param name="redirectUri">Redirect URI. Has to match redirect URI defined at open.weibo.com/apps (can be arbitrary).</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(string appKey, string appSecret, string redirectUri)
        {
            return Initialize(appKey, appSecret, redirectUri, new UwpAuthenticationBroker(), new UwpPasswordManager(), new UwpStorageManager());
        }

        /// <summary>
        /// Initialize underlying provider with relevent token information.
        /// </summary>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <returns>Success or failure.</returns>
        public bool Initialize(WeiboOAuthTokens oAuthTokens)
        {
            return Initialize(oAuthTokens, new UwpAuthenticationBroker(), new UwpPasswordManager(), new UwpStorageManager());
        }
#endif

        /// <summary>
        /// Gets a reference to an instance of the underlying data provider.
        /// </summary>
        public WeiboDataProvider Provider
        {
            get
            {
                if (!_isInitialized)
                {
                    throw new InvalidOperationException("Provider not initialized.");
                }

                return _weiboDataProvider ?? (_weiboDataProvider = new WeiboDataProvider(_tokens, _authenticationBroker, _passwordManager, _storageManager));
            }
        }

        /// <summary>
        /// Log user in to Weibo.
        /// </summary>
        /// <returns>Returns success of failure of login attempt.</returns>
        public Task<bool> LoginAsync()
        {
            return Provider.LoginAsync();
        }

        /// <summary>
        /// Log user out of Weibo.
        /// </summary>
        [Obsolete("Logout is deprecated, please use LogoutAsync instead.", true)]
        public void Logout()
        {
            Provider.Logout();
        }

        /// <summary>
        /// Log user out of Weibo.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public Task LogoutAsync()
        {
            return Provider.LogoutAsync();
        }

        /// <summary>
        /// Retrieve user data.
        /// </summary>
        /// <param name="screenName">User screen name or null for current logged user.</param>
        /// <returns>Returns user data.</returns>
        public async Task<WeiboUser> GetUserAsync(string screenName = null)
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
        public async Task<IEnumerable<WeiboStatus>> GetUserTimeLineAsync(string screenName, int maxRecords = 20)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.GetUserTimeLineAsync(screenName, maxRecords, new WeiboStatusParser());
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await GetUserTimeLineAsync(screenName, maxRecords);
            }

            return null;
        }

        /// <summary>
        /// Post a status.
        /// Due to the restriction by Weibo API, your status must include a url which starts with "http"/"https".
        /// You can add a url to the list of secure domains in the basic information section on your Weibo application page.
        /// </summary>
        /// <param name="status">The status information.</param>
        /// <returns>Returns the published weibo status.</returns>
        public async Task<WeiboStatus> PostStatusAsync(string status)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.PostStatusAsync(status);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await PostStatusAsync(status);
            }

            return null;
        }

        /// <summary>
        /// Post a status with a picture.
        /// Due to the restriction by Weibo API, your status must include a url which starts with "http"/"https".
        /// You can add a url to the list of secure domains in the basic information section on your Weibo application page.
        /// </summary>
        /// <param name="status">The status information.</param>
        /// <param name="picture">Picture to attach to the status.</param>
        /// <returns>Returns the published weibo status.</returns>
        public async Task<WeiboStatus> PostStatusAsync(string status, Stream picture)
        {
            if (Provider.LoggedIn)
            {
                return await Provider.PostStatusAsync(status, picture);
            }

            var isLoggedIn = await LoginAsync();
            if (isLoggedIn)
            {
                return await PostStatusAsync(status, picture);
            }

            return null;
        }
    }
}