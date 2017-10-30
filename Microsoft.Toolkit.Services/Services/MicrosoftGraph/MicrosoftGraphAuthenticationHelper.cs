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
using Microsoft.Toolkit.Services.Exceptions;
using Newtonsoft.Json;
using Microsoft.Toolkit.Services.Core;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    /// <summary>
    /// Authentication Helper Using Azure Active Directory v2.0 app Model
    /// </summary>
    internal class MicrosoftGraphAuthenticationHelper
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        protected const string Authority = "https://login.microsoftonline.com/common/";
        protected const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        protected const string AuthorityV2Model = "https://login.microsoftonline.com/common/oauth2/v2.0/authorize";

        protected const string Scope = "openid+profile+https://graph.microsoft.com/Files.ReadWrite https://graph.microsoft.com/Mail.ReadWrite https://graph.microsoft.com/User.ReadWrite+offline_access";
        protected const string AuthorizationTokenService = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        protected const string LogoutUrlV2Model = "https://login.microsoftonline.com/common/oauth2/v2.0/logout";

        private static HttpClient httpClient;

        /// <summary>
        /// Gets or sets static instance of HttpClient.
        /// </summary>
        public static HttpClient HttpClient
        {
            get { return httpClient ?? (httpClient = new HttpClient()); }
            set { httpClient = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        public MicrosoftGraphAuthenticationHelper()
        {
        }

        /// <summary>
        /// Gets or sets platform-specific implementation for authentication.
        /// </summary>
        public static IPlatformAuthentication PlatformAuthentication { get; set; }

        /// <summary>
        /// Gets or sets the Oauth2 access token.
        /// </summary>
        protected string TokenForUser { get; set; }

        /// <summary>
        /// Gets or sets token expiration.  By default the life time of the first access token is 3600 (1h).
        /// </summary>
        public DateTimeOffset Expiration { get; set; }

        /// <summary>
        /// Clean the TokenCache
        /// </summary>
        internal virtual void CleanToken()
        {
            TokenForUser = null;
        }

        /// <summary>
        /// Get a Microsoft Graph access token using the v2.0 Endpoint.
        /// </summary>
        /// <param name="appClientId">Application client ID</param>
        /// <returns>An oauth2 access token.</returns>
        internal async Task<string> GetUserTokenV2Async(string appClientId)
        {
            string authorizationCode = null;
            string authorizationUrl = $"{AuthorityV2Model}?response_type=code&client_id={appClientId}&redirect_uri={DefaultRedirectUri}&scope={Scope}&response_mode=query";

            JwToken jwtToken = null;
            if (TokenForUser == null)
            {
                if (PlatformAuthentication == null)
                {
                    throw new NotSupportedException("MicrosoftGraphAuthenticationHelper::PlatformAuthentication not set. Unable to authenticate.");
                }

                var webAuthResult = await PlatformAuthentication.AuthenticateAsync(new Uri(authorizationUrl), new Uri(DefaultRedirectUri));
                authorizationCode = ParseAuthorizationCode(webAuthResult);
                jwtToken = await RequestTokenAsync(appClientId, authorizationCode);
                TokenForUser = jwtToken.AccessToken;
            }

            return TokenForUser;
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        /// <returns>Success or failure</returns>
        internal async Task<bool> LogoutAsync()
        {
            return await LogoutV2Async();
        }

        internal async Task<bool> LogoutV2Async()
        {
            HttpResponseMessage response = null;
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, LogoutUrlV2Model);
                response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }

        protected async Task<JwToken> RequestTokenAsync(string appClientId, string code)
        {
            var requestBody = $"grant_type=authorization_code&client_id={appClientId}&code={code}&redirect_uri={DefaultRedirectUri}&scope={Scope}";
            var requestBytes = Encoding.UTF8.GetBytes(requestBody);

            // Build request.
            using (var request = new HttpRequestMessage(HttpMethod.Post, AuthorizationTokenService))
            {
                request.Content = new ByteArrayContent(requestBytes);
                request.Content.Headers.Add("ContentType", "application/x-www-form-urlencoded");

                using (var response = await HttpClient.SendAsync(request).ConfigureAwait(false))
                {
                    string responseBody = response.Content == null ? null : await response.Content.ReadAsStringAsync();

                    if (response.IsSuccessStatusCode && !string.IsNullOrEmpty(responseBody))
                    {
                        // Parse the JWT.
                        var jwt = JsonConvert.DeserializeObject<JwToken>(responseBody);
                        return jwt;
                    }
                }
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
