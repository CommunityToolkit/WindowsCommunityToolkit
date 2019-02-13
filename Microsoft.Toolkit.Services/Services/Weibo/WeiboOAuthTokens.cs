// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Services.Weibo
{
    /// <summary>
    /// Weibo OAuth tokens.
    /// </summary>
    public class WeiboOAuthTokens
    {
        /// <summary>
        /// Gets or sets app Key.
        /// </summary>
        public string AppKey { get; set; }

        /// <summary>
        /// Gets or sets app Secret.
        /// </summary>
        public string AppSecret { get; set; }

        /// <summary>
        /// Gets or sets access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets redirect Uri.
        /// </summary>
        public string RedirectUri { get; set; }
    }
}