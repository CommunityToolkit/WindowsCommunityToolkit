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
    /// <summary>
    /// Authentication helper for Azure AD resources
    /// </summary>
    public interface IAuthenticationHelper
    {
        /// <summary>
        /// Aquire's an acess token
        /// </summary>
        /// <param name="resourceId">Resource id or single scope to retrieve</param>
        /// <param name="uiParent">Rrequired for Android</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <param name="loginHint">Login Hint</param>
        /// <returns>Access Token</returns>
        Task<string> AquireTokenAsync(string resourceId, UIParent uiParent = null, string redirectUri = null, string loginHint = null);

        /// <summary>
        /// Aquire's an acess token
        /// </summary>
        /// <param name="scopes">Scope to retrieve</param>
        /// <param name="uiParent">Rrequired for Android</param>
        /// <param name="redirectUri">RedirectUri</param>
        /// <param name="loginHint">Login Hint</param>
        /// <returns>Access Token</returns>
        Task<string> AquireTokenAsync(IEnumerable<string> scopes, UIParent uiParent = null, string redirectUri = null, string loginHint = null);

        /// <summary>
        /// Log out
        /// </summary>
        /// <returns>True if succeeded</returns>
        Task<bool> LogoutAsync();
    }
}
