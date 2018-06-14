// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// OAuth token types
    /// </summary>
    public enum TwitterOAuthTokenType
    {
        /// <summary>
        /// Request or access token
        /// </summary>
        OAuthRequestOrAccessToken,

        /// <summary>
        /// Request or access token secret
        /// </summary>
        OAuthRequestOrAccessTokenSecret,

        /// <summary>
        /// Verifier
        /// </summary>
        OAuthVerifier,

        /// <summary>
        /// Callback confirmed
        /// </summary>
        OAuthCallbackConfirmed,

        /// <summary>
        /// Screen name
        /// </summary>
        ScreenName
    }
}
