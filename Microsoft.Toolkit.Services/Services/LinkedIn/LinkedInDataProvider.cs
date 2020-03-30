// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;
using Newtonsoft.Json.Linq;

#if WINRT
using Microsoft.Toolkit.Services.PlatformSpecific.Uwp;
#endif

#if NET462
using Microsoft.Toolkit.Services.PlatformSpecific.NetFramework;
#endif

namespace Microsoft.Toolkit.Services.LinkedIn
{
    /// <summary>
    /// Data Provider for connecting to LinkedIn service.
    /// </summary>
    public class LinkedInDataProvider
    {
        private const string _oAuthBaseUrl = "https://www.linkedin.com/uas/oauth2/";
        private const string _baseUrl = "https://api.linkedin.com/v1";

        private static HttpClient client = new HttpClient();

        /// <summary>
        /// Gets or sets logged in user information.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// Gets a value indicating whether the provider is already logged in
        /// </summary>
        public bool LoggedIn { get; private set; }

        /// <summary>
        /// Gets or sets requiredPermissions property.
        /// </summary>
        public LinkedInPermissions RequiredPermissions { get; set; }

        private readonly IAuthenticationBroker _authentication;
        private readonly IPasswordManager _passwordManager;
        private readonly IStorageManager _storageManager;

        /// <summary>
        /// Gets or sets tokens property.
        /// </summary>
        public LinkedInOAuthTokens Tokens { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedInDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        /// <param name="requiredPermissions">Required permissions for the session.</param>
        /// <param name="authentication">Authentication result interface.</param>
        /// <param name="passwordManager">Password Manager interface, store the password.</param>
        /// <param name="storageManager">Storage Manager interface.</param>
        public LinkedInDataProvider(LinkedInOAuthTokens tokens, LinkedInPermissions requiredPermissions, IAuthenticationBroker authentication, IPasswordManager passwordManager, IStorageManager storageManager)
        {
            if (string.IsNullOrEmpty(tokens.ClientSecret))
            {
                throw new ArgumentException("Missing client secret key");
            }

            if (string.IsNullOrEmpty(tokens.ClientId))
            {
                throw new ArgumentException("Missing client ID");
            }

            if (string.IsNullOrEmpty(tokens.CallbackUri))
            {
                throw new ArgumentException("Missing callback uri");
            }

            if (!Enum.IsDefined(typeof(LinkedInPermissions), requiredPermissions))
            {
                throw new ArgumentException("Error retrieving required permissions");
            }

            Tokens = tokens;
            RequiredPermissions = requiredPermissions;
            _authentication = authentication ?? throw new ArgumentException("Invalid AuthenticationBroker");
            _storageManager = storageManager ?? throw new ArgumentException("Invalid StorageManager");
            _passwordManager = passwordManager ?? throw new ArgumentException("Invalid PasswordManager");
        }
#if WINRT
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedInDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        /// <param name="requiredPermissions">Required permissions for the session.</param>
        public LinkedInDataProvider(LinkedInOAuthTokens tokens, LinkedInPermissions requiredPermissions)
        : this(tokens, requiredPermissions, new UwpAuthenticationBroker(), new UwpPasswordManager(), new UwpStorageManager())
        {
        }
#endif

#if NET462
        /// <summary>
        /// Initializes a new instance of the <see cref="LinkedInDataProvider"/> class.
        /// Constructor.
        /// </summary>
        /// <param name="tokens">OAuth tokens for request.</param>
        /// <param name="requiredPermissions">Required permissions for the session.</param>
        public LinkedInDataProvider(LinkedInOAuthTokens tokens, LinkedInPermissions requiredPermissions)
        : this(tokens, requiredPermissions, new NetFrameworkAuthenticationBroker(), new NetFrameworkPasswordManager(), new NetFrameworkStorageManager())
        {
        }
#endif

        /// <summary>
        /// Log user in to LinkedIn.
        /// </summary>
        /// <returns>Boolean indicating login success.</returns>
        public async Task<bool> LoginAsync()
        {
            var user = await _storageManager.GetAsync(LinkedInConstants.STORAGEKEYUSER);
            var credential = _passwordManager.Get(LinkedInConstants.STORAGEKEYACCESSTOKEN);
            if (!string.IsNullOrEmpty(user) && credential != null)
            {
                Tokens.AccessToken = credential.Password;
                Username = user;
                LoggedIn = true;
                return true;
            }

            string authorizeCode = await GetAuthorizeCodeAsync(Tokens, RequiredPermissions);

            if (!string.IsNullOrEmpty(authorizeCode))
            {
                var accessToken = await GetAccessTokenAsync(Tokens, authorizeCode);

                if (!string.IsNullOrEmpty(accessToken))
                {
                    Tokens.AccessToken = accessToken;

                    _passwordManager.Store(LinkedInConstants.STORAGEKEYACCESSTOKEN, new PasswordCredential { UserName = LinkedInConstants.STORAGEKEYUSER, Password = accessToken });
                    await _storageManager.SetAsync(LinkedInConstants.STORAGEKEYUSER, LinkedInConstants.STORAGEKEYUSER);
                    return true;
                }
            }

            LoggedIn = false;
            return false;
        }

        /// <summary>
        /// Log user out of LinkedIn.
        /// </summary>
        [Obsolete("Logout is deprecated, please use LogoutAsync instead.", true)]
        public void Logout()
        {
#pragma warning disable CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
            LogoutAsync();
#pragma warning restore CS4014 // Because this call is not awaited, execution of the current method continues before the call is completed
        }

        /// <summary>
        /// Log user out of LinkedIn.
        /// </summary>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public async Task LogoutAsync()
        {
            var crendential = _passwordManager.Get(LinkedInConstants.STORAGEKEYACCESSTOKEN);

            if (crendential != null)
            {
                _passwordManager.Remove(LinkedInConstants.STORAGEKEYACCESSTOKEN);
                await _storageManager.SetAsync(LinkedInConstants.STORAGEKEYUSER, null);
            }

            LoggedIn = false;
        }

        /// <summary>
        /// Wrapper around REST API for making data request.
        /// </summary>
        /// <typeparam name="TSchema">Schema to use</typeparam>
        /// <param name="config">Query configuration.</param>
        /// <param name="maxRecords">Upper limit for records returned.</param>
        /// <param name="startRecord">Index of paged results.</param>
        /// <param name="fields">A comma seperated string of required fields, which will have strongly typed representation in the model passed in.</param>
        /// <returns>Strongly typed list of results.</returns>
        public async Task<IEnumerable<TSchema>> GetDataAsync<TSchema>(LinkedInDataConfig config, int maxRecords, int startRecord = 0, string fields = "id")
        {
            var parser = new LinkedInParser<TSchema>();

            var url = $"{_baseUrl}{config.Query}/~:({fields})?oauth2_access_token={Tokens.AccessToken}&format=json&count={maxRecords}&start={startRecord}";

            using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, new Uri(url)))
            {
                request.Headers.Connection.TryParseAdd("Keep-Alive");

                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                    if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(data))
                    {
                        return parser.Parse(data);
                    }

                    throw new RequestFailedException((System.Net.HttpStatusCode)response.StatusCode, data);
                }
            }
        }

        /// <summary>
        /// Share data to LinkedIn.
        /// </summary>
        /// <typeparam name="T">Schema of data to share.</typeparam>
        /// <typeparam name="U">Type of response object.</typeparam>
        /// <param name="dataToShare">Share request content.</param>
        /// <returns>Boolean indicating success or failure.</returns>
        public async Task<U> ShareDataAsync<T, U>(T dataToShare)
        {
            var shareRequest = dataToShare as LinkedInShareRequest;
            if (shareRequest != null)
            {
                LinkedInVisibility.ParseVisibilityStringToEnum(shareRequest.Visibility.Code);

                var requestParser = new LinkedInParser<LinkedInShareRequest>();

                var url = $"{_baseUrl}/people/~/shares?oauth2_access_token={Tokens.AccessToken}&format=json";

                using (HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, new Uri(url)))
                {
                    request.Headers.Add("x-li-format", "json");
                    var stringContent = requestParser.Parse(shareRequest);
                    request.Content = new StringContent(stringContent, Encoding.UTF8, "application/json");

                    using (var response = await client.SendAsync(request).ConfigureAwait(false))
                    {
                        var data = await response.Content.ReadAsStringAsync().ConfigureAwait(false);

                        var responseParser = new LinkedInParser<U>();

                        var listResults = responseParser.Parse(data) as List<U>;
                        return listResults[0];
                    }
                }
            }

            return default(U);
        }

        /// <summary>
        /// Check validity of configuration.
        /// </summary>
        /// <param name="config">Query configuration.</param>
        protected void ValidateConfig(LinkedInDataConfig config)
        {
            if (config?.Query == null)
            {
                throw new ConfigParameterNullException(nameof(config.Query));
            }
        }

        private async Task<string> GetAccessTokenAsync(LinkedInOAuthTokens tokens, string authorizeCode)
        {
            var url = $"{_oAuthBaseUrl}accessToken?grant_type=authorization_code"
            + "&code=" + authorizeCode
            + "&redirect_uri=" + Uri.EscapeDataString(tokens.CallbackUri)
            + "&client_id=" + tokens.ClientId
            + "&client_secret=" + tokens.ClientSecret;

            using (var request = new HttpRequestMessage(HttpMethod.Post, new Uri(url)))
            {
                using (var response = await client.SendAsync(request).ConfigureAwait(false))
                {
                    var jsonString = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    var json = JObject.Parse(jsonString);
                    return json.GetValue("access_token").Value<string>();
                }
            }
        }

        private async Task<string> GetAuthorizeCodeAsync(LinkedInOAuthTokens tokens, LinkedInPermissions permissions)
        {
            string scopes = ConvertPermissionsToEncodedScopeString(permissions);

            var url = $"{_oAuthBaseUrl}authorization?response_type=code"
            + "&client_id=" + tokens.ClientId
            + "&state=STATE"
            + "&redirect_uri=" + Uri.EscapeDataString(tokens.CallbackUri)
            + "&" + scopes;

            var startUri = new Uri(url);
            var endUri = new Uri(tokens.CallbackUri);

            var result = await _authentication.Authenticate(startUri, endUri);
            switch (result.ResponseStatus)
            {
                case AuthenticationResultStatus.Success:
                    {
                        var response = result.ResponseData;
                        IDictionary<string, string> dictionary = new Dictionary<string, string>();
                        var split = response.Split('?');
                        foreach (var keyValue in split[split.Length - 1].Split('&'))
                        {
                            var keyValueSplit = keyValue.Split('=');
                            if (keyValueSplit.Length == 2)
                            {
                                dictionary.Add(keyValueSplit[0], keyValueSplit[1]);
                            }
                        }

                        return dictionary["code"];
                    }

                case AuthenticationResultStatus.ErrorHttp:
                    Debug.WriteLine("WAB failed, message={0}", result.ResponseErrorDetail.ToString());
                    return string.Empty;

                case AuthenticationResultStatus.UserCancel:
                    Debug.WriteLine("WAB user aborted.");
                    return string.Empty;
            }

            return string.Empty;
        }

        private string ConvertPermissionsToEncodedScopeString(LinkedInPermissions requiredPermissions)
        {
            StringBuilder scope = new StringBuilder();

            foreach (LinkedInPermissions value in Enum.GetValues(typeof(LinkedInPermissions)))
            {
                if ((requiredPermissions & value) != LinkedInPermissions.NotSet)
                {
                    var name = value.ToString().ToLower();
                    name = name.Replace("readwrite", "rw_");
                    name = name.Replace("read", "r_");
                    name = name.Replace("write", "w_");
                    name = name.Replace("companyadmin", "company_admin");

                    scope.Append($"{name} ");
                }
            }

            return "scope=" + Uri.EscapeDataString(scope.ToString());
        }
    }
}
