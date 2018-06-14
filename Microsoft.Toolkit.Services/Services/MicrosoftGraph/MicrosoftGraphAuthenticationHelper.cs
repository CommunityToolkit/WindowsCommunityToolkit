// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
#if WINRT
using System.Net.Http;
#endif
using System.Threading.Tasks;
using Microsoft.Identity.Client;
#if WINRT
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Windows.Storage;
#endif
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

#if WINRT
        private const string LogoutUrl = "https://login.microsoftonline.com/common/oauth2/logout";
        private const string MicrosoftGraphResource = "https://graph.microsoft.com";

        /// <summary>
        /// Storage key name for user name.
        /// </summary>
        private static readonly string STORAGEKEYUSER = "user";

#endif

        private static MSAL.PublicClientApplication _identityClient = null;

#if WINRT
        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private readonly Windows.Security.Credentials.PasswordVault _vault;

        /// <summary>
        /// Azure Active Directory Authentication context use to get an access token [ADAL]
        /// </summary>
        private AuthenticationContext _azureAdContext = new AuthenticationContext(Authority);
#endif

        /// <summary>
        /// Gets or sets delegated permission Scopes
        /// </summary>
        protected string[] DelegatedPermissionScopes { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        public MicrosoftGraphAuthenticationHelper()
        {
#if WINRT
            _vault = new Windows.Security.Credentials.PasswordVault();
#endif
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        /// <param name="delegatedPermissionScopes">Delegated Permission Scopes</param>
        public MicrosoftGraphAuthenticationHelper(string[] delegatedPermissionScopes)
            : this()
        {
            DelegatedPermissionScopes = delegatedPermissionScopes;
        }

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
        internal void CleanToken()
        {
            TokenForUser = null;
#if WINRT
            _azureAdContext.TokenCache.Clear();
#endif
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

            try
            {
                authenticationResult = await _identityClient.AcquireTokenSilentAsync(DelegatedPermissionScopes, _identityClient.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    authenticationResult = await _identityClient.AcquireTokenAsync(DelegatedPermissionScopes, upnLoginHint, uiParent);
                }
                catch (MsalException)
                {
                    throw;
                }
            }

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

#if WINRT
        /// <summary>
        /// Get a Microsoft Graph access token from Azure AD.
        /// </summary>
        /// <param name="appClientId">Azure AD application client ID</param>
        /// <returns>An oauth2 access token.</returns>
        internal async Task<string> GetUserTokenAsync(string appClientId)
        {
            // For the first use get an access token prompting the user, after one hour
            // refresh silently the token
            if (TokenForUser == null)
            {
                IdentityModel.Clients.ActiveDirectory.AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenAsync(MicrosoftGraphResource, appClientId, new Uri(DefaultRedirectUri), new IdentityModel.Clients.ActiveDirectory.PlatformParameters(PromptBehavior.Always, false));
                TokenForUser = userAuthnResult.AccessToken;
                Expiration = userAuthnResult.ExpiresOn;
            }

            if (Expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                IdentityModel.Clients.ActiveDirectory.AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenSilentAsync(MicrosoftGraphResource, appClientId);
                TokenForUser = userAuthnResult.AccessToken;
                Expiration = userAuthnResult.ExpiresOn;
            }

            return TokenForUser;
        }

        /// <summary>
        /// Logout the user
        /// </summary>
        /// <param name="authenticationModel">Authentication version endPoint</param>
        /// <returns>Success or failure</returns>
        internal async Task<bool> LogoutAsync(string authenticationModel)
        {
            HttpResponseMessage response = null;
            ApplicationData.Current.LocalSettings.Values[STORAGEKEYUSER] = null;

            if (authenticationModel.Equals("V1"))
            {
                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, LogoutUrl);
                    response = await client.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
            }

            if (authenticationModel.Equals("V2"))
            {
                return Logout();
            }

            return true;
        }
#endif
    }
}
