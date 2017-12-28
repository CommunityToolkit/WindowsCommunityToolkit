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
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Microsoft.OneDrive.Sdk.Authentication;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Authentication Helper Using Azure Active Directory V1.0 app Model
    /// and Azure Active Directory library for .NET
    /// </summary>
    public class OneDriveAuthenticationHelper
    {
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";
        private const string Authority = "https://login.microsoftonline.com/common/";
        private const string DiscoveryResourceUri = "https://api.office.com/discovery";

        /// <summary>
        /// Azure Active Directory Authentication context use to get an access token
        /// </summary>
        private static AuthenticationContext _azureAdContext = new AuthenticationContext(Authority);

        /// <summary>
        /// Gets Azure Active Directory Context
        /// </summary>
        public static AuthenticationContext AzureAdContext
        {
            get { return _azureAdContext; }
        }

        /// <summary>
        /// Store connected user's data.
        /// </summary>
        private static UserInfoSettings _userInfoSettings = UserInfoSettings.Load();

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
        /// Create an Microsoft Account authentication provider
        /// </summary>
        /// <param name="appClientId">client application id</param>
        /// <param name="scopes">Scopes represent various permission levels that an app can request from a user</param>
        /// <returns>An instance of the MSAAuthenticationProvider</returns>
        internal static IAuthenticationProvider CreateMSAAuthenticationProvider(string appClientId, string[] scopes)
        {
            _accountProvider = new MsaAuthenticationProvider(appClientId, DefaultRedirectUri, scopes, new CredentialVault(appClientId));
            return _accountProvider;
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
                                                    await OneDriveAuthenticationHelper.AuthenticateAdalUserAsync().ConfigureAwait(false));
                        return;
                    });
            return _accountProvider;
        }

        /// <summary>
        /// Gets an access token in order to discover the onedrive for business endpoint of the authenticated user.
        /// </summary>
        /// <param name="appClientId">client application id</param>
        /// <returns>An instance of AuthenticationResult with the access token</returns>
        internal static async Task<IdentityModel.Clients.ActiveDirectory.AuthenticationResult> AuthenticateAdalUserForDiscoveryAsync(string appClientId)
        {
            var discoveryResourceUri = "https://api.office.com/discovery/";
            _appClientId = appClientId;
            IdentityModel.Clients.ActiveDirectory.AuthenticationResult userAuthnResult = await _azureAdContext.AcquireTokenAsync(discoveryResourceUri, appClientId, new Uri(DefaultRedirectUri), new IdentityModel.Clients.ActiveDirectory.PlatformParameters(PromptBehavior.Auto, true));
            _userInfoSettings = SaveUserInfo(userAuthnResult);
            return userAuthnResult;
        }

        /// <summary>
        /// Gets the user's service resources
        /// </summary>
        /// <param name="authenticationResult">An instance of AuthenticationResult containing the access token</param>
        /// <returns>The onedrive for business service endpoint</returns>
        internal static async Task<DiscoveryService> GetUserServiceResource(IdentityModel.Clients.ActiveDirectory.AuthenticationResult authenticationResult)
        {
            DiscoveryService discoveryService = null;
            HttpClient client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue(authenticationResult.AccessTokenType, authenticationResult.AccessToken);
            var response = await client.GetAsync($"{DiscoveryResourceUri}/v2.0/me/services");

            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var discoveryResponse = JsonConvert.DeserializeObject<DiscoveryServiceResponse>(jsonData);
                var values = discoveryResponse.Value;
                var query = from service in values where service.Capability == "MyFiles" && service.ServiceApiVersion == "v2.0" select service;
                discoveryService = query.FirstOrDefault();
                if (discoveryService == null)
                {
                    throw new ServiceException(new Error { Message = "This user don't have access to OneDrive For Business", Code = "DiscoveryError", ThrowSite = "UWP Community Toolkit" });
                }
            }
            else
            {
                throw new HttpRequestException(response.ReasonPhrase);
            }

            return discoveryService;
        }

        /// <summary>
        /// Get a Microsoft Graph access token from Azure AD.
        /// </summary>
        /// <param name="refreshToken">Flag indicating if the token has to be redeem</param>
        /// <returns>An oauth2 access token.</returns>
        internal static async Task<string> AuthenticateAdalUserAsync(bool refreshToken = false)
        {
            if (_userInfoSettings == null && refreshToken == false)
            {
                _userInfoSettings = SaveUserInfo(await _azureAdContext.AcquireTokenAsync(_resourceUri, _appClientId, new Uri(DefaultRedirectUri), new IdentityModel.Clients.ActiveDirectory.PlatformParameters(PromptBehavior.RefreshSession, false)));
            }

            if (_userInfoSettings.Expiration <= DateTimeOffset.UtcNow.AddMinutes(5) || refreshToken == true)
            {
                _userInfoSettings = SaveUserInfo(await _azureAdContext.AcquireTokenSilentAsync(_resourceUri, _appClientId));
            }

            return _userInfoSettings.AccessToken;
        }

        private static UserInfoSettings SaveUserInfo(AuthenticationResult authResult)
        {
            var userInfo = new UserInfoSettings { Expiration = authResult.ExpiresOn, UserPrincipalName = authResult.UserInfo.DisplayableId, AccessToken = authResult.AccessToken };
            userInfo.Save();
            return userInfo;
        }
    }
}
