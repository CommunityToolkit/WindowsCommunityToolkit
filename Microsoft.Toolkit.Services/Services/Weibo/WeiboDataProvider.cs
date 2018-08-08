// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if WINRT
using Microsoft.Toolkit.Services.PlatformSpecific.Uwp;
#endif

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Data Provider for connecting to Weibo service.
    /// </summary>
    public class WeiboDataProvider
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string BaseUrl = "https://api.weibo.com/2";
        private const string OAuthBaseUrl = "https://api.weibo.com/oauth2";

        private static HttpClient _client;

        /// <summary>
        /// Base Url for service.
        /// </summary>
        private readonly WeiboOAuthTokens _tokens;

        private readonly IAuthenticationBroker _authenticationBroker;
        private readonly IPasswordManager _passwordManager;
        private readonly IStorageManager _storageManager;

        /// <summary>
        /// Gets or sets logged in user information.
        /// </summary>
        public long? Uid { get; set; }

        /// <summary>
        /// Gets a value indicating whether the provider is already logged in
        /// </summary>
        public bool LoggedIn { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        /// <param name="authenticationBroker">Authentication result interface.</param>
        /// <param name="passwordManager">Platform password manager</param>
        /// <param name="storageManager">Platform storage provider</param>
        /// <param name="signatureManager">Platform signature manager</param>
        public WeiboDataProvider(WeiboOAuthTokens tokens, IAuthenticationBroker authenticationBroker, IPasswordManager passwordManager, IStorageManager storageManager)
        {
            _tokens = tokens;
            _authenticationBroker = authenticationBroker;
            _passwordManager = passwordManager;
            _storageManager = storageManager;
            if (_client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                _client = new HttpClient(handler);
            }
        }

#if WINRT
        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        public WeiboDataProvider(WeiboOAuthTokens tokens)
        {
            _tokens = tokens;
            _authenticationBroker = new UwpAuthenticationBroker();
            _passwordManager = new UwpPasswordManager();
            _storageManager = new UwpStorageManager();
            if (_client == null)
            {
                HttpClientHandler handler = new HttpClientHandler();
                handler.AutomaticDecompression = DecompressionMethods.GZip;
                _client = new HttpClient(handler);
            }
        }
#endif

        /// <summary>
        /// Retrieve user data.
        /// </summary>
        /// <param name="screenName">User screen name or null for current logged user</param>
        /// <returns>Returns user data.</returns>
        public async Task<WeiboUser> GetUserAsync(string screenName = null)
        {
            string rawResult = null;
            try
            {
                Uri uri;
                if (screenName == null)
                {
                    uri = new Uri($"{BaseUrl}/users/show.json?uid={Uid}");
                }
                else
                {
                    uri = new Uri($"{BaseUrl}/users/show.json?screen_name={screenName}");
                }

                WeiboOAuthRequest request = new WeiboOAuthRequest();
                rawResult = await request.ExecuteGetAsync(uri, _tokens);
                return JsonConvert.DeserializeObject<WeiboUser>(rawResult);
            }
            catch (UserNotFoundException)
            {
                throw new UserNotFoundException(screenName);
            }
            catch
            {
                if (!string.IsNullOrEmpty(rawResult))
                {
                    var error = JsonConvert.DeserializeObject<WeiboError>(rawResult);

                    throw new WeiboException { Error = error };
                }

                throw;
            }
        }

        /// <summary>
        /// Log user in to Weibo.
        /// </summary>
        /// <returns>Boolean indicating login success.</returns>
        public async Task<bool> LoginAsync()
        {
            var crendetials = _passwordManager.Get("Weibo");
            var uidString = _storageManager.Get("WeiboUid");
            if (long.TryParse(uidString, out var uid) && crendetials != null)
            {
                _tokens.AccessToken = crendetials.Password;
                Uid = uid;
                LoggedIn = true;
                return true;
            }

            string appKey = _tokens.AppKey;
            string redirectUri = _tokens.RedirectUri;
            string weiboUrl = $"{OAuthBaseUrl}/authorize?client_id={appKey}&redirect_uri={redirectUri}";

            Uri startUri = new Uri(weiboUrl);
            Uri endUri = new Uri(redirectUri);

            var result = await _authenticationBroker.Authenticate(startUri, endUri);

            switch (result.ResponseStatus)
            {
                case AuthenticationResultStatus.Success:
                    LoggedIn = true;
                    return await ExchangeRequestTokenForAccessTokenAsync(result.ResponseData);

                case AuthenticationResultStatus.ErrorHttp:
                    Debug.WriteLine("WAB failed, message={0}", result.ResponseErrorDetail.ToString());
                    LoggedIn = false;
                    return false;

                case AuthenticationResultStatus.UserCancel:
                    Debug.WriteLine("WAB user aborted.");
                    LoggedIn = false;
                    return false;
            }

            LoggedIn = false;
            return false;
        }

        /// <summary>
        /// Log user out of Weibo.
        /// </summary>
        public void Logout()
        {
            var credential = _passwordManager.Get("Weibo");
            if (credential != null)
            {
                _passwordManager.Remove("Weibo");
                _storageManager.Set("WeiboUid", null);
                Uid = null;
            }

            LoggedIn = false;
        }

        /// <summary>
        /// Extract and initialize access tokens.
        /// </summary>
        /// <param name="webAuthResultResponseData">WAB data containing appropriate tokens.</param>
        /// <returns>Success or failure.</returns>
        private async Task<bool> ExchangeRequestTokenForAccessTokenAsync(string webAuthResultResponseData)
        {
            string query = new Uri(webAuthResultResponseData, UriKind.Absolute).Query;
            string code = null;
            foreach (var keyValue in query.Split(new[] { '?', '&' }, StringSplitOptions.RemoveEmptyEntries))
            {
                if (keyValue.StartsWith("code=", StringComparison.OrdinalIgnoreCase))
                {
                    code = keyValue.Substring("code=".Length);
                    break;
                }
            }

            if (code == null)
            {
                return false;
            }

            string weiboUrl = $"{OAuthBaseUrl}/access_token";

            Dictionary<string, string> postData = new Dictionary<string, string>
            {
                ["client_id"] = _tokens.AppKey,
                ["client_secret"] = _tokens.AppSecret,
                ["grant_type"] = "authorization_code",
                ["code"] = code,
                ["redirect_uri"] = _tokens.RedirectUri
            };

            FormUrlEncodedContent postContent = new FormUrlEncodedContent(postData);

            string data = null;

            using (HttpResponseMessage response = await _client.PostAsync(weiboUrl, postContent).ConfigureAwait(false))
            {
                data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
            }

            JObject jObject = JObject.Parse(data);

            string accessToken = jObject["access_token"].ToObject<string>();
            long uid = jObject["uid"].ToObject<long>();

            Uid = uid;
            _tokens.AccessToken = accessToken;

            _passwordManager.Store("Weibo", new PasswordCredential { UserName = "AccessToken", Password = accessToken });
            _storageManager.Set("WeiboUid", uid.ToString());

            return true;
        }
    }
}