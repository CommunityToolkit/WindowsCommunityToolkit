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
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Windows.Web.Http;

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
        private const string Authority = "https://login.microsoftonline.com/common";
        private const string LogoutUrl = "https://login.microsoftonline.com/common/oauth2/logout";
        private const string MicrosoftGraphResource = "https://graph.microsoft.com";
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        public MicrosoftGraphAuthenticationHelper()
        {
        }

        /// <summary>
        /// Store the Oauth2 access token.
        /// </summary>
        private string _tokenForUser = null;

        /// <summary>
        /// Store the refresh token
        /// </summary>
        private string _refreshToken = null;

        /// <summary>
        /// Store The lifetime in seconds of the access token.
        /// </summary>
        /// <remarks>By default the life time of the first access token is 3600 (1h)</remarks>
        private DateTimeOffset _expiration;

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
                AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenAsync(MicrosoftGraphResource, appClientId, new Uri(DefaultRedirectUri), PromptBehavior.Always);
                _tokenForUser = userAuthnResult.AccessToken;
                _expiration = userAuthnResult.ExpiresOn;
                _refreshToken = userAuthnResult.RefreshToken;
            }

            if (_expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenByRefreshTokenAsync(_refreshToken, appClientId);
                _tokenForUser = userAuthnResult.AccessToken;
                _expiration = userAuthnResult.ExpiresOn;
                _refreshToken = userAuthnResult.RefreshToken;
            }

            return _tokenForUser;
        }

        /// <summary>
        /// Clean the TokenCache
        /// </summary>
        internal void CleanToken()
        {
            _tokenForUser = null;
            _refreshToken = null;
            _azureAdContext.TokenCache.Clear();
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        /// <returns>Success or failure</returns>
        internal async Task<bool> LogoutAsync()
        {
            using (var request = new HttpHelperRequest(new Uri(LogoutUrl), HttpMethod.Get))
            {
                using (var response = await HttpHelper.Instance.SendRequestAsync(request).ConfigureAwait(false))
                {
                    return response.Success;
                }
            }
        }
    }
}
