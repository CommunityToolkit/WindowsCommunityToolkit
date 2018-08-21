// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.Identity.Client;

namespace Microsoft.Toolkit.Services.MicrosoftGraph
{
    internal class AuthenticationHelperV2 : IAuthenticationHelper
    {
        private readonly PublicClientApplication _identityClient;
        private readonly string logoutUrl;

        public AuthenticationHelperV2(string clientId, string authority, string logoutUrl)
        {
            _identityClient = new PublicClientApplication(clientId, authority);
            this.logoutUrl = logoutUrl;
        }

        public async Task<string> AquireTokenAsync(IEnumerable<string> scopes, UIParent uiParent = null, string redirectUri = null, string loginHint = null)
        {
            if (!string.IsNullOrEmpty(redirectUri))
            {
                _identityClient.RedirectUri = redirectUri;
            }

            string upnLoginHint = null;
            if (!string.IsNullOrEmpty(loginHint))
            {
                upnLoginHint = loginHint;
            }

            AuthenticationResult authenticationResult = null;

            try
            {
                authenticationResult = await _identityClient.AcquireTokenSilentAsync(scopes, _identityClient.Users.FirstOrDefault());
            }
            catch (MsalUiRequiredException)
            {
                try
                {
                    authenticationResult = await _identityClient.AcquireTokenAsync(scopes, upnLoginHint, UIBehavior.SelectAccount, null, uiParent);
                }
                catch (MsalException)
                {
                    throw;
                }
            }

            return authenticationResult?.AccessToken;
        }

        public async Task<bool> LogoutAsync()
        {
            try
            {
                _identityClient.Remove(_identityClient.Users.FirstOrDefault());

                using (var client = new HttpClient())
                {
                    var request = new HttpRequestMessage(HttpMethod.Get, logoutUrl);
                    var response = await client.SendAsync(request);
                    return response.IsSuccessStatusCode;
                }
            }
            catch (MsalException)
            {
                return false;
            }
        }

        public Task<string> AquireTokenAsync(string resourceId, UIParent uiParent = null, string redirectUri = null, string loginHint = null)
        {
            return AquireTokenAsync(new[] { resourceId }, uiParent, redirectUri, loginHint);
        }
    }
}
