// ******************************************************************
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
// ******************************************************************

using System;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Newtonsoft.Json;
using Windows.Security.Authentication.Web;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// Authentication Helper Using Azure Active Directory V1.0 app Model
    /// and Azure Active Directory library for .NET
    /// </summary>
    internal class MicrosoftGraphAuthenticationHelper
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string Authority = "https://login.microsoftonline.com/common/";
        private const string LogoutUrl = "https://login.microsoftonline.com/common/oauth2/logout";
        private const string MicrosoftGraphResource = "https://graph.microsoft.com";
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        private const string AuthorityV2Model = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";
        private const string AuthorizationTokenService = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        private const string LogoutUrlV2Model = "https://login.microsoftonline.com/common/oauth2/v2.0/logout";
        private const string Scope = "openid+profile+https://graph.microsoft.com/Files.ReadWrite https://graph.microsoft.com/Mail.ReadWrite https://graph.microsoft.com/User.ReadWrite+offline_access";

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        public MicrosoftGraphAuthenticationHelper()
        {
            _vault = new PasswordVault();
        }

        /// <summary>
        /// Storage key name for access token.
        /// </summary>
        private static readonly string STORAGEKEYACCESSTOKEN = "AccessToken";

        /// <summary>
        /// Storage key name for token expiration.
        /// </summary>
        private static readonly string STORAGEKEYEXPIRATION = "Expiration";

        /// <summary>
        /// Storage key name for user name.
        /// </summary>
        private static readonly string STORAGEKEYUSER = "user";

        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private readonly PasswordVault _vault;

        /// <summary>
        /// Store the current connected user
        /// </summary>
        private Identity.Client.User _user;

        /// <summary>
        /// Store the Oauth2 access token.
        /// </summary>
        private string _tokenForUser = null;

        /// <summary>
        /// Store The lifetime in seconds of the access token.
        /// </summary>
        /// <remarks>By default the life time of the first access token is 3600 (1h)</remarks>
        private DateTimeOffset _expiration;

        private PasswordCredential _passwordCredential;

        /// <summary>
        /// Azure Active Directory Authentication context use to get an access token
        /// </summary>
        private AuthenticationContext _azureAdContext = new AuthenticationContext(Authority);

        /// <summary>
        /// Get a Microsoft Graph access token from Azure AD.
        /// </summary>
        /// <param name="appClientId">Azure AD application client ID</param>
        /// <returns>An oauth2 access token.</returns>
        internal async Task<string> GetUserTokenAsync(string appClientId)
        {
            // For the first use get an access token prompting the user, after one hour
            // refresh silently the token
            if (_tokenForUser == null)
            {
                IdentityModel.Clients.ActiveDirectory.AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenAsync(MicrosoftGraphResource, appClientId, new Uri(DefaultRedirectUri), new IdentityModel.Clients.ActiveDirectory.PlatformParameters(PromptBehavior.Always, false));
                _tokenForUser = userAuthnResult.AccessToken;
                _expiration = userAuthnResult.ExpiresOn;
            }

            if (_expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                IdentityModel.Clients.ActiveDirectory.AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenSilentAsync(MicrosoftGraphResource, appClientId);
                _tokenForUser = userAuthnResult.AccessToken;
                _expiration = userAuthnResult.ExpiresOn;
            }

            return _tokenForUser;
        }

        /// <summary>
        /// Clean the TokenCache
        /// </summary>
        internal void CleanToken()
        {
            _tokenForUser = null;
            _azureAdContext.TokenCache.Clear();
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        /// <param name="authenticationModel">Authentication version endPoint</param>
        /// <returns>Success or failure</returns>
        internal async Task<bool> LogoutAsync(string authenticationModel)
        {
            HttpResponseMessage response = null;
            if (authenticationModel.Equals("V1"))
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, LogoutUrl);
                    response = await client.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
            }
           else if (authenticationModel.Equals("V2"))
            {
                if (_user != null)
                {
                    _user.SignOut();
                }
            }

            ApplicationData.Current.LocalSettings.Values[STORAGEKEYUSER] = null;
            if (_passwordCredential != null)
            {
                _vault.Remove(_passwordCredential);
            }

            return true;
        }

        private string StoreCredential(Identity.Client.AuthenticationResult authResult)
        {
            _user = authResult.User;
            _expiration = authResult.ExpiresOn;
            ApplicationData.Current.LocalSettings.Values[STORAGEKEYEXPIRATION] = authResult.ExpiresOn;
            ApplicationData.Current.LocalSettings.Values[STORAGEKEYUSER] = authResult.User.DisplayableId;
            _passwordCredential = new PasswordCredential(STORAGEKEYACCESSTOKEN, authResult.User.DisplayableId, authResult.Token);
            _vault.Add(_passwordCredential);
            return authResult.Token;
        }

        /// <summary>
        /// Get a Microsoft Graph access token using the v2.0 Endpoint.
        /// </summary>
        /// <param name="appClientId">Application client ID</param>
        /// <returns>An oauth2 access token.</returns>
        private async Task<string> GetUserTokenV2Async(string appClientId)
        {
            string authorizationCode = null;
            string authorizationUrl = $"{AuthorityV2Model}?response_type=code&client_id={appClientId}&redirect_uri={DefaultRedirectUri}&scope={Scope}&response_mode=query";

            JwToken jwtToken = null;
            if (_tokenForUser == null)
            {
                var webAuthResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, new Uri(authorizationUrl), new Uri(DefaultRedirectUri));

                // Process the navigation result.
                if (webAuthResult.ResponseStatus == WebAuthenticationStatus.Success)
                {
                    authorizationCode = ParseAuthorizationCode(webAuthResult.ResponseData);
                    jwtToken = await RequestTokenAsync(appClientId, authorizationCode);
                    _tokenForUser = jwtToken.AccessToken;
                }
            }

            return _tokenForUser;
        }

        private async Task<JwToken> RequestTokenAsync(string appClientId, string code)
        {
            var requestBody = $"grant_type=authorization_code&client_id={appClientId}&code={code}&redirect_uri={DefaultRedirectUri}&scope={Scope}";
            var requestBytes = Encoding.UTF8.GetBytes(requestBody);

            // Build request.
            var request = HttpWebRequest.CreateHttp(AuthorizationTokenService);
            request.Method = "POST";
            request.ContentType = "application/x-www-form-urlencoded";
            var requestStream = await request.GetRequestStreamAsync()
                                                    .ConfigureAwait(continueOnCapturedContext: true);
            await requestStream.WriteAsync(requestBytes, 0, requestBytes.Length);

            // Get response.
            var response = await request.GetResponseAsync()
                                            .ConfigureAwait(continueOnCapturedContext: true)
                                as HttpWebResponse;
            var responseReader = new StreamReader(response.GetResponseStream());
            var responseBody = await responseReader.ReadToEndAsync()
                                                        .ConfigureAwait(continueOnCapturedContext: true);

            if (response.StatusCode == HttpStatusCode.OK)
            {
                // Parse the JWT.
                var jwt = JsonConvert.DeserializeObject<JwToken>(responseBody);
                return jwt;
            }

            // Consent was not obtained.
            return null;
        }

       /// <summary>
       /// Retrieve the authorisation code
       /// </summary>
       /// <param name="responseData">string contening the authorisation code</param>
       /// <returns>The authorization code</returns>
        private string ParseAuthorizationCode(string responseData)
        {
            string code = null;
            var queryParams = SplitQueryString(responseData);
            foreach (var param in queryParams)
            {
                // Split the current parameter into name and value.
                var parts = param.Split('=');
                var paramName = parts[0].ToLowerInvariant().Trim();
                var paramValue = WebUtility.UrlDecode(parts[1]).Trim();

                // Process the output parameter.
                if (paramName.Equals("code"))
                {
                    code = paramValue;
                }
            }

            return code;
        }

        private string[] SplitQueryString(string queryString)
        {
            // Do some hygiene on the query string upfront to ease the parsing.
            queryString.Trim();
            var queryStringBegin = queryString.IndexOf('?');

            if (queryStringBegin >= 0)
            {
                queryString = queryString.Substring(queryStringBegin + 1);
            }

            // Now split the query string.
            return queryString.Split('&');
        }
    }
}
