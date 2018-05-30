// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.LinkedIn
{
    /// <summary>
    /// LinkedIn OAuth tokens.
    /// </summary>
    public class LinkedInOAuthTokens
    {
        /// <summary>
        /// Gets or sets clientId.
        /// </summary>
        public string ClientId { get; set; }

        /// <summary>
        /// Gets or sets clientSecret.
        /// </summary>
        public string ClientSecret { get; set; }

        /// <summary>
        /// Gets or sets callback Uri.
        /// </summary>
        public string CallbackUri { get; set; }

        /// <summary>
        /// Gets or sets access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets access token Secret.
        /// </summary>
        public string AccessTokenSecret { get; set; }
    }
}
