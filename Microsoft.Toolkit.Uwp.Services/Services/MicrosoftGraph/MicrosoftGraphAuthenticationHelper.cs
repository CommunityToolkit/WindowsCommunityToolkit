// ******************************************************************
//
// Copyright (c) Microsoft. All rights reserved.
// This code is licensed under the MIT License (MIT).
// THE CODE IS PROVIDED “AS IS”, WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED,
// INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.
// IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM,
// DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF CONTRACT,
// TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH
// THE CODE OR THE USE OR OTHER DEALINGS IN THE CODE.
//
// ******************************************************************

using System;
using System.Threading.Tasks;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Windows.Security.Authentication.Web;

namespace Microsoft.Toolkit.Uwp.Services.AzureAD
    {
    /// <summary>
    /// Authentication Helper Using Azure Active Directory V1.0 app Model
    /// and Azure Active Directory library for .NET
    /// <see cref="Http://github.com/AzureAD/azure-activedirectory-library-for-dotnet"/>
    /// </summary>
    internal class MicrosoftGraphAuthenticationHelper
    {
        /// <summary>
        /// Base Url for service.
        /// </summary>
        private const string Authority = "https://login.microsoftonline.com/common";
        private const string MicrosoftGraphResource = "https://graph.microsoft.com";
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        private MicrosoftGraphAuthenticationHelper()
        {
        }

        private static MicrosoftGraphAuthenticationHelper instance;

        /// <summary>
        /// Gets public singleton property.
        /// </summary>
        public static MicrosoftGraphAuthenticationHelper Instance => instance ?? (instance = new MicrosoftGraphAuthenticationHelper());

        /// <summary>
        /// Store the Oauth2 access token.
        /// </summary>
        private string tokenForUser = null;

        /// <summary>
        /// Store the refresh token
        /// </summary>
        private string refreshToken = null;

        /// <summary>
        /// Store The lifetime in seconds of the access token.
        /// </summary>
        /// <remarks>By default the life time of the first access token is 3600 (1h)</remarks>
        private DateTimeOffset expiration;

        private AuthenticationContext azureAdContext = new AuthenticationContext(Authority);

        /// <summary>
        /// Get a Microsoft Graph access token from Azure AD.
        /// </summary>
        /// <param name="appClientId">Azure AD application client ID</param>
        /// <returns>An oauth2 access token.</returns>
        internal async Task<string> GetUserTokenAsync(string appClientId)
        {
            // For the first use get an access token prompting the user, after one hour
            // refresh silently the token
            if (tokenForUser == null)
            {
                AuthenticationResult userAuthnResult = await azureAdContext.AcquireTokenAsync(MicrosoftGraphResource, appClientId, new Uri(DefaultRedirectUri), PromptBehavior.RefreshSession);
                tokenForUser = userAuthnResult.AccessToken;
                expiration = userAuthnResult.ExpiresOn;
                refreshToken = userAuthnResult.RefreshToken;
            }

            if (expiration <= DateTimeOffset.UtcNow.AddMinutes(5))
            {
                AuthenticationResult userAuthnResult = await azureAdContext.AcquireTokenByRefreshTokenAsync(refreshToken, appClientId);
                tokenForUser = userAuthnResult.AccessToken;
                expiration = userAuthnResult.ExpiresOn;
                refreshToken = userAuthnResult.RefreshToken;
            }

            return tokenForUser;
        }
    }

}
