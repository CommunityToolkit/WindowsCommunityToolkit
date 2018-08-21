// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

#if WINRT || WINDOWS_UWP
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using Windows.Storage;
#endif

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
#if WINRT || WINDOWS_UWP
    internal class AuthenticationHelperV1 : IAuthenticationHelper
    {
        /// <summary>
        /// Default Redirect Uri
        /// </summary>
        private const string DefaultRedirectUri = "urn:ietf:wg:oauth:2.0:oob";

        private readonly AuthenticationContext _authContext;
        private readonly string clientId;
        private readonly string logoutUrl;

        public AuthenticationHelperV1(string clientId, string authority, string logoutUrl)
        {
            _authContext = new AuthenticationContext(authority);
            this.clientId = clientId;
            this.logoutUrl = logoutUrl;
        }

        public async Task<string> AquireTokenAsync(IEnumerable<string> scopes, Identity.Client.UIParent uiParent = null, string redirectUri = null, string loginHint = null)
        {
            try
            {
                AuthenticationResult userAuthnResult = await _authContext.AcquireTokenSilentAsync(scopes.First(), clientId);
                return userAuthnResult.AccessToken;
            }
            catch (AdalSilentTokenAcquisitionException)
            {
                try
                {
                    AuthenticationResult userAuthnResult = await _authContext.AcquireTokenAsync(scopes.First(), clientId, new Uri(redirectUri ?? DefaultRedirectUri), new IdentityModel.Clients.ActiveDirectory.PlatformParameters(PromptBehavior.SelectAccount, false));
                    return userAuthnResult.AccessToken;
                }
                catch (AdalException)
                {
                    throw;
                }
            }
        }

        public Task<string> AquireTokenAsync(string resourceId, Identity.Client.UIParent uiParent = null, string redirectUri = null, string loginHint = null)
        {
            return AquireTokenAsync(new[] { resourceId }, uiParent, redirectUri, loginHint);
        }

        public async Task<bool> LogoutAsync()
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage(HttpMethod.Get, logoutUrl);
                var response = await client.SendAsync(request);
                return response.IsSuccessStatusCode;
            }
        }
    }

#endif
}
