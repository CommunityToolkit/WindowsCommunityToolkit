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
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Identity.Client;
using MSAL = Microsoft.Identity.Client;

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

        protected const string AuthorizationTokenService = "https://login.microsoftonline.com/common/oauth2/v2.0/token";
        protected const string LogoutUrlV2Model = "https://login.microsoftonline.com/common/oauth2/v2.0/logout";

        private static MSAL.PublicClientApplication _identityClient = null;

        /// <summary>
        /// Gets or sets delegated permission Scopes
        /// </summary>
        protected string[] DelegatedPermissionScopes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        public MicrosoftGraphAuthenticationHelper()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        /// <param name="delegatedPermissionScopes">Delegated Permission Scopes</param>
        public MicrosoftGraphAuthenticationHelper(string[] delegatedPermissionScopes) => DelegatedPermissionScopes = delegatedPermissionScopes;

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
        /// Get a Microsoft Graph access token using the v2.0 Endpoint
        /// </summary>
        /// <param name="appClientId">Application client Id</param>
        /// <param name="loginHint">UPN</param>
        /// <returns>An oauth2 access token.</returns>
        internal async Task<string> GetUserTokenV2Async(string appClientId, string loginHint)
        {
            return await GetUserTokenV2Async(appClientId, null, null, loginHint);
        }

        /// <summary>
        /// Get a Microsoft Graph access token using the v2.0 Endpoint.
        /// </summary>
        /// <param name="appClientId">Application client ID</param>
        /// <param name="uiParent">UiParent instance - required for Android</param>
        /// <param name="redirectUri">Redirect Uri - required for Android</param>
        /// <param name="loginHint">UPN</param>
        /// <returns>An oauth2 access token.</returns>
        internal async Task<string> GetUserTokenV2Async(string appClientId, UIParent uiParent = null, string redirectUri = null, string loginHint = null)
        {
            if (_identityClient == null)
            {
                _identityClient = new MSAL.PublicClientApplication(appClientId);
            }

            if (!string.IsNullOrEmpty(redirectUri))
            {
                _identityClient.RedirectUri = redirectUri;
            }

            var upnLoginHint = string.Empty;
            if (!string.IsNullOrEmpty(loginHint))
            {
                upnLoginHint = loginHint;
            }

            MSAL.AuthenticationResult authenticationResult = null;

            var user = _identityClient.Users.FirstOrDefault();

            authenticationResult = user != null ? await _identityClient.AcquireTokenSilentAsync(DelegatedPermissionScopes, user) : await _identityClient.AcquireTokenAsync(DelegatedPermissionScopes, upnLoginHint, uiParent);

            return authenticationResult?.AccessToken;
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        /// <returns>Success or failure</returns>
        internal bool Logout()
        {
            return LogoutV2();
        }

        internal bool LogoutV2()
        {
            try
            {
                _identityClient.Remove(_identityClient.Users.FirstOrDefault());
            }
            catch (MsalException)
            {
                return false;
            }

            return true;
        }
    }
}
