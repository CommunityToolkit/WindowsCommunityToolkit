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
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;
using Newtonsoft.Json;
using ADAL = Microsoft.IdentityModel.Clients.ActiveDirectory;
using MSAL = Microsoft.Identity.Client;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Authentication Helper Using Azure Active Directory V1.0 app Model
    /// and Azure Active Directory library for .NET
    /// </summary>
    public class GraphOneDriveAuthenticationHelper
    {
        /// <summary>
        /// const to store the redirect uri in order to get the authentication code
        /// </summary>
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private const string Authority = "https://login.microsoftonline.com/common/";
        private const string DiscoveryResourceUri = "https://api.office.com/discovery";

        /// <summary>
        /// Azure Active Directory Authentication context use to get an access token
        /// </summary>
        private static ADAL.AuthenticationContext _azureAdContext = new ADAL.AuthenticationContext(Authority);

        /// <summary>
        /// Gets Azure Active Directory Context
        /// </summary>
        public static ADAL.AuthenticationContext AzureAdContext
        {
            get { return _azureAdContext; }
        }

        private static MSAL.PublicClientApplication _identityClient = null;

        /// <summary>
        /// Gets MSAL identity client
        /// </summary>
        public static MSAL.PublicClientApplication IdentityClient
        {
            get { return _identityClient; }
        }

        /// <summary>
        /// Fields to store the account provider
        /// </summary>
        private static IAuthenticationProvider _accountProvider;

        /// <summary>
        /// Store the Client Application Id
        /// </summary>
        private static string _appClientId;

        /// <summary>
        /// Gets Authentication Provider
        /// </summary>
        internal static IAuthenticationProvider AuthenticationProvider
        {
            get { return _accountProvider; }
        }

        private static string _resourceUri;

        /// <summary>
        /// Gets or sets the resource uri to get an access token
        /// </summary>
        internal static string ResourceUri
        {
            get { return _resourceUri; } set { _resourceUri = value; }
        }

        /// <summary>
        /// Create an ADAL Authentication provider
        /// </summary>
        /// <param name="appClientId">client application id</param>
        /// <returns>an authentication provider for Azure Active Directory</returns>
        internal static IAuthenticationProvider CreateAdalAuthenticationProvider(string appClientId)
        {
            _appClientId = appClientId;
            _accountProvider = new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization =
                                           new AuthenticationHeaderValue(
                                                    "bearer",
                                                    await GraphOneDriveAuthenticationHelper.AuthenticateAdalUserAsync().ConfigureAwait(false));
                        return;
                    });
            return _accountProvider;
        }

        /// <summary>
        /// Create an ADAL Authentication provider
        /// </summary>
        /// <param name="appClientId">client application id</param>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user</param>
        /// <returns>an authentication provider for Azure Active Directory</returns>
        internal static IAuthenticationProvider CreateMsalAuthenticationProvider(string appClientId, string[] scopes)
        {
            _appClientId = appClientId;
            _accountProvider = new DelegateAuthenticationProvider(
                    async (requestMessage) =>
                    {
                        requestMessage.Headers.Authorization =
                                           new AuthenticationHeaderValue(
                                                    "bearer",
                                                    await GraphOneDriveAuthenticationHelper.AuthenticateMsalUserAsync(scopes).ConfigureAwait(false));
                        return;
                    });
            return _accountProvider;
        }

        /// <summary>
        /// Get a Microsoft Graph access token from Azure AD V1.
        /// </summary>
        /// <returns>An oauth2 access token.</returns>
        internal static async Task<string> AuthenticateAdalUserAsync()
        {
            ADAL.AuthenticationResult authenticationResult = null;
            try
            {
                authenticationResult = await _azureAdContext.AcquireTokenSilentAsync(_resourceUri, _appClientId);
            }
            catch (Exception)
            {
                authenticationResult = await _azureAdContext.AcquireTokenAsync(_resourceUri, _appClientId, new Uri(DefaultRedirectUri), new ADAL.PlatformParameters(ADAL.PromptBehavior.RefreshSession, false));
            }

            return authenticationResult.AccessToken;
        }

        /// <summary>
        /// Get a Microsoft Graph access token from Azure AD V2.
        /// </summary>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user</param>
        /// <returns>An oauth2 access token.</returns>
        internal static async Task<string> AuthenticateMsalUserAsync(string[] scopes)
        {
            if (_identityClient == null)
            {
                _identityClient = new MSAL.PublicClientApplication(_appClientId);
            }

            MSAL.AuthenticationResult authenticationResult = null;
            try
            {
                authenticationResult = await _identityClient.AcquireTokenSilentAsync(scopes, _identityClient.Users.First());
            }
            catch (Exception)
            {
                authenticationResult = await _identityClient.AcquireTokenAsync(scopes);
            }

            return authenticationResult.AccessToken;
        }
    }
}
