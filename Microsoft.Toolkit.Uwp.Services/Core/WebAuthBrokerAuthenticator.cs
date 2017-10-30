using System;
using System.Threading.Tasks;
using Microsoft.Toolkit.Services.Core;
using Windows.Security.Authentication.Web;

namespace Microsoft.Toolkit.Uwp.Services.Core
{
    /// <summary>
    /// Authenticator for Web Auth Broker.
    /// </summary>
    public class WebAuthBrokerAuthenticator : IPlatformAuthentication
    {
        /// <summary>
        /// Authentication method.
        /// </summary>
        /// <param name="authorizationUrl">Authorization Url.</param>
        /// <param name="redirectUri">Redirect Url.</param>
        /// <returns>Token contained in string.</returns>
        public async Task<string> AuthenticateAsync(Uri authorizationUrl, Uri redirectUri)
        {
            var webAuthResult = await WebAuthenticationBroker.AuthenticateAsync(WebAuthenticationOptions.None, authorizationUrl, redirectUri);

            if (webAuthResult.ResponseStatus == WebAuthenticationStatus.Success)
            {
                return webAuthResult.ResponseData;
            }

            return string.Empty;
        }
    }
}
