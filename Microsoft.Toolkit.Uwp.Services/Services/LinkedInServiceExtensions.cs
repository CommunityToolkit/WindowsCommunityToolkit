// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Services.LinkedIn;

namespace Microsoft.Toolkit.Uwp.Services
{
    /// <summary>
    /// LinkedIn service extensions to use for Uwp
    /// </summary>
    public static class LinkedInServiceExtensions
    {
        /// <summary>
        /// Initialize underlying provider with relevent token information for Uwp.
        /// </summary>
        /// <param name="service">The LinkedInService.</param>
        /// <param name="oAuthTokens">Token instance.</param>
        /// <param name="requiredPermissions">Scope / permissions app requires user to sign up for.</param>
        /// <returns>Success or failure.</returns>
        public static bool Initialize(this LinkedInService service, LinkedInOAuthTokens oAuthTokens, LinkedInPermissions requiredPermissions = LinkedInPermissions.NotSet)
        {
            return service.Initialize(oAuthTokens, new UwpAuthenticationBroker(), new UWpPasswordManager(), new UwpStorageManager(), requiredPermissions);
        }
    }
}
