// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Toolkit.Parsers;
using Microsoft.Toolkit.Services.Core;
using Microsoft.Toolkit.Services.OAuth;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if WINRT
using System.Runtime.InteropServices.WindowsRuntime;
using Microsoft.Toolkit.Services.PlatformSpecific.Uwp;
using Windows.Storage.Streams;
#endif

#if NET462
using Microsoft.Toolkit.Services.PlatformSpecific.NetFramework;
#endif

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Data Provider for connecting to Weibo service.
    /// </summary>
    public class WeiboDataProvider : Toolkit.Services.DataProviderBase<WeiboDataConfig, Toolkit.Parsers.SchemaBase>
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string BaseUrl = "https://api.weibo.com/2";
        private const string OAuthBaseUrl = "https://api.weibo.com/oauth2";
        private const string PasswordKey = "Weibo";
        private const string StorageKey = "WeiboUid";

        private static HttpClient _client;

        /// <summary>
        /// Base Url for service.
        /// </summary>
        private readonly WeiboOAuthTokens _tokens;
        private readonly IAuthenticationBroker _authenticationBroker;
        private readonly IPasswordManager _passwordManager;
        private readonly IStorageManager _storageManager;

        /// <summary>
        /// Gets if logged in user information.
        /// </summary>
        public long? Uid { get; private set; }

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
        public WeiboDataProvider(WeiboOAuthTokens tokens, IAuthenticationBroker authenticationBroker, IPasswordManager passwordManager, IStorageManager storageManager)
        {
            if (string.IsNullOrEmpty(tokens.AppSecret))
            {
                throw new ArgumentException("Missing app secret");
            }

            if (string.IsNullOrEmpty(tokens.AppKey))
            {
                throw new ArgumentException("Missing app key");
            }

            if (string.IsNullOrEmpty(tokens.RedirectUri))
            {
                throw new ArgumentException("Missing redirect uri");
            }

            _tokens = tokens;
            _authenticationBroker = authenticationBroker ?? throw new ArgumentException("Invalid AuthenticationBroker");
            _passwordManager = passwordManager ?? throw new ArgumentException("Invalid PasswordManager");
            _storageManager = storageManager ?? throw new ArgumentException("Invalid StorageManager");
            if (_client == null)
            {
                HttpClientHandler handler = new HttpClientHandler { AutomaticDecompression = DecompressionMethods.GZip };
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
            : this(tokens, new UwpAuthenticationBroker(), new UwpPasswordManager(), new UwpStorageManager())
        {
        }
#endif

#if NET462
        /// <summary>
        /// Initializes a new instance of the <see cref="WeiboDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        public WeiboDataProvider(WeiboOAuthTokens tokens)
            : this(tokens, new NetFrameworkAuthenticationBroker(), new NetFrameworkPasswordManager(), new NetFrameworkStorageManager())
        {
        }
#endif

        /// <summary>
        /// Log user in to Weibo.
        /// </summary>
        /// <returns>Boolean indicating login success.</returns>
        public async Task<bool> LoginAsync()
        {
            var credentials = _passwordManager.Get(PasswordKey);
            string uidString = await _storageManager.GetAsync(StorageKey);
            if (long.TryParse(uidString, out var uid) && credentials != null)
            {
                _tokens.AccessToken = credentials.Password;
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
                    LoggedIn = false;
                    return false;

                case AuthenticationResultStatus.UserCancel:
                    LoggedIn = false;
                    return false;
            }

            LoggedIn = false;
            return false;
        }

        /// <summary>
        /// Log user out of Weibo.
        /// </summary>
        [Obsolete("Logout is deprecated, please use LogoutAsync instead.", true)]
        public void Logout()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LogoutAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        /// <summary>
        /// Log user out of Weibo.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LogoutAsync()
        {
            var credential = _passwordManager.Get(PasswordKey);

            if (credential != null)
            {
                _passwordManager.Remove(PasswordKey);
                await _storageManager.SetAsync(StorageKey, null);
            }

            Uid = null;
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
            if (string.IsNullOrEmpty(accessToken))
            {
                throw new NullReferenceException("The accessToken is null.");
            }

            long uid = jObject["uid"].ToObject<long>();

            Uid = uid;
            _tokens.AccessToken = accessToken;

            _passwordManager.Store(PasswordKey, new PasswordCredential { UserName = "AccessToken", Password = accessToken });
            await _storageManager.SetAsync(StorageKey, uid.ToString());

            return true;
        }

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
                    uri = new Uri($"{BaseUrl}/users/show.json?screen_name={OAuthEncoder.UrlEncode(screenName)}");
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
        /// Retrieve user timeline data with specific parser.
        /// </summary>
        /// <typeparam name="TSchema">Strong type for results.</typeparam>
        /// <param name="screenName">User screen name.</param>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="parser">Specific results parser.</param>
        /// <returns>Returns strongly typed list of results.</returns>
        public async Task<IEnumerable<TSchema>> GetUserTimeLineAsync<TSchema>(string screenName, int maxRecords, Toolkit.Parsers.IParser<TSchema> parser)
            where TSchema : Toolkit.Parsers.SchemaBase
        {
            string rawResult = null;
            try
            {
                var uri = new Uri($"{BaseUrl}/statuses/user_timeline.json?screen_name={OAuthEncoder.UrlEncode(screenName)}&count={maxRecords}");

                WeiboOAuthRequest request = new WeiboOAuthRequest();
                rawResult = await request.ExecuteGetAsync(uri, _tokens);

                var result = parser.Parse(rawResult);
                return result
                        .Take(maxRecords)
                        .ToList();
            }
            catch (UserNotFoundException)
            {
                throw new UserNotFoundException(screenName);
            }
            catch
            {
                if (!string.IsNullOrEmpty(rawResult))
                {
                    var errors = JsonConvert.DeserializeObject<WeiboError>(rawResult);

                    throw new WeiboException { Error = errors };
                }

                throw;
            }
        }

        /// <summary>
        /// Get home time line data.
        /// </summary>
        /// <typeparam name="TSchema">Strong typed result.</typeparam>
        /// <param name="maxRecords">Upper record limit.</param>
        /// <param name="parser">Specific result parser.</param>
        /// <returns>Return strong typed list of results.</returns>
        private async Task<IEnumerable<TSchema>> GetHomeTimeLineAsync<TSchema>(int maxRecords, Toolkit.Parsers.IParser<TSchema> parser)
            where TSchema : Toolkit.Parsers.SchemaBase
        {
            var uri = new Uri($"{BaseUrl}/statuses/home_timeline.json?count={maxRecords}");

            WeiboOAuthRequest request = new WeiboOAuthRequest();
            var rawResult = await request.ExecuteGetAsync(uri, _tokens);

            return parser.Parse(rawResult);
        }

        /// <summary>
        /// Wrapper around REST API for making data request.
        /// </summary>
        /// <typeparam name="TSchema">Schema to use</typeparam>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper limit for records returned.</param>
        /// <param name="pageIndex">The zero-based index of the page that corresponds to the items to retrieve.</param>
        /// <param name="parser">IParser implementation for interpreting results.</param>
        /// <returns>Strongly typed list of results.</returns>
        protected override async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(WeiboDataConfig config, int maxRecords, int pageIndex, IParser<TSchema> parser)
        {
            IEnumerable<TSchema> items;
            switch (config.QueryType)
            {
                case WeiboQueryType.User:
                    items = await GetUserTimeLineAsync(config.Query, maxRecords, parser);
                    break;

                case WeiboQueryType.Home:
                default:
                    items = await GetHomeTimeLineAsync(maxRecords, parser);
                    break;
            }

            return items;
        }

        /// <summary>
        /// Check validity of configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        protected override void ValidateConfig(WeiboDataConfig config)
        {
            if (config?.Query == null && config?.QueryType != WeiboQueryType.Home)
            {
                throw new ConfigParameterNullException(nameof(config.Query));
            }

            if (_tokens == null)
            {
                throw new ConfigParameterNullException(nameof(_tokens));
            }

            if (string.IsNullOrEmpty(_tokens.AppKey))
            {
                throw new OAuthKeysNotPresentException(nameof(_tokens.AppKey));
            }

            if (string.IsNullOrEmpty(_tokens.AppSecret))
            {
                throw new OAuthKeysNotPresentException(nameof(_tokens.AppSecret));
            }
        }

        /// <summary>
        /// Returns parser implementation for specified configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        /// <returns>Strongly typed parser.</returns>
        protected override IParser<SchemaBase> GetDefaultParser(WeiboDataConfig config)
        {
            if (config == null)
            {
                throw new ConfigNullException();
            }

            switch (config.QueryType)
            {
                case WeiboQueryType.Home:
                case WeiboQueryType.User:
                    return new WeiboParser<Toolkit.Parsers.SchemaBase>();

                default:
                    return new WeiboParser<Toolkit.Parsers.SchemaBase>();
            }
        }

        /// <summary>
        /// Posts a status update.
        /// </summary>
        /// <param name="status">Status text.</param>
        /// <param name="picture">Picture to attach to the status.</param>
        /// <returns>Success or failure.</returns>
        public async Task<WeiboStatus> PostStatusAsync(string status, Stream picture = null)
        {
            var uri = new Uri($"{BaseUrl}/statuses/share.json");

            WeiboOAuthRequest request = new WeiboOAuthRequest();

            if (picture == null)
            {
                return await request.ExecutePostAsync(uri, _tokens, status);
            }
            else
            {
                byte[] fileBytes;

                using (var ms = new MemoryStream())
                {
                    await picture.CopyToAsync(ms);
                    fileBytes = ms.ToArray();
                }

                return await request.ExecutePostMultipartAsync(uri, _tokens, status, fileBytes);
            }
        }
    }
}
