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
using Microsoft.OneDrive.Sdk.Authentication;
using Newtonsoft.Json;

namespace Microsoft.Toolkit.Uwp.Services.OneDrive
{
    /// <summary>
    /// Authentication Helper
    /// </summary>
    public class OneDriveAuthenticationHelper
    {
        /// <summary>
        /// const to store the redirect uri in order to get the authentication code
        /// </summary>
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        /// <summary>
        /// Fields to store the account provider
        /// </summary>
        private static IAuthenticationProvider _accountProvider;

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
        /// Authenticate the user with a Microsoft Account
        /// </summary>
        /// <returns>No object or value is returned by this method when it completes.</returns>
        internal static async Task AuthenticateMsaUserAsync()
        {
            try
            {
                await ((MsaAuthenticationProvider)OneDriveAuthenticationHelper.AuthenticationProvider).RestoreMostRecentFromCacheOrAuthenticateUserAsync();
            }
            catch (Exception)
            {
                await ((MsaAuthenticationProvider)OneDriveAuthenticationHelper.AuthenticationProvider).AuthenticateUserAsync();
            }
        }
    }
}
