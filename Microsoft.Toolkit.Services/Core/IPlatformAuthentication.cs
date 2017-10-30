using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// Interface for platform-specific implementation of authentication/authorization.
    /// </summary>
    public interface IPlatformAuthentication
    {
        /// <summary>
        /// Platform specific authentication.
        /// </summary>
        /// <param name="authorizationUrl">Authorization Url.</param>
        /// <param name="redirectUri">Redirect Uri.</param>
        /// <returns>String containing token.</returns>
        Task<string> AuthenticateAsync(Uri authorizationUrl, Uri redirectUri);
    }
}
