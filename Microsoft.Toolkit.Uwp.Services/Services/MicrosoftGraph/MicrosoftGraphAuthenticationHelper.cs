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
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Windows.Security.Credentials;
using Windows.Storage;

namespace Microsoft.Toolkit.Uwp.Services.MicrosoftGraph
{
    /// <summary>
    /// Authentication Helper Using Azure Active Directory v1.0 and v2.0 app Model
    /// </summary>
    internal class MicrosoftGraphAuthenticationHelper : Toolkit.Services.MicrosoftGraph.MicrosoftGraphAuthenticationHelper
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string LogoutUrl = "https://login.microsoftonline.com/common/oauth2/logout";
        private const string MicrosoftGraphResource = "https://graph.microsoft.com";

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        public MicrosoftGraphAuthenticationHelper()
        {
            _vault = new PasswordVault();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MicrosoftGraphAuthenticationHelper"/> class.
        /// </summary>
        /// <param name="delegatedPermissionScopes">Delegated Permission Scopes</param>
        public MicrosoftGraphAuthenticationHelper(string[] delegatedPermissionScopes)
            : base(delegatedPermissionScopes)
        {
            _vault = new PasswordVault();
        }

        /// <summary>
        /// Storage key name for user name.
        /// </summary>
        private static readonly string STORAGEKEYUSER = "user";

        /// <summary>
        /// Password vault used to store access tokens
        /// </summary>
        private readonly PasswordVault _vault;

        /// <summary>
        /// Azure Active Directory Authentication context use to get an access token [ADAL]
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
        /// Clean the TokenCache
        /// </summary>
        internal override void CleanToken()
        {
            TokenForUser = null;
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
    }
}
