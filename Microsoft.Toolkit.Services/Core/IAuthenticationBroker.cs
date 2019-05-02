// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// This gets an Uri value.
    /// </summary>
    public interface IAuthenticationBroker
    {
        /// <summary>
        /// Returns the authentication status, it could be UserCancel, ErrorHttp and Success.
        /// </summary>
        /// <param name="requestUri"> Autorization base url</param>
        /// <param name="callbackUri"> LinkedInOAuthTokens callbackUri</param>
        /// <returns> Returns a status </returns>
        Task<AuthenticationResult> Authenticate(Uri requestUri, Uri callbackUri);
    }
}
