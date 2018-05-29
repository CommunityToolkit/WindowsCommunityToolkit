// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Microsoft.Toolkit.Uwp.Services.Twitter
{
    /// <summary>
    /// Twitter OAuth tokens.
    /// </summary>
    public class TwitterOAuthTokens
    {
        /// <summary>
        /// Gets or sets consumer Key.
        /// </summary>
        public string ConsumerKey { get; set; }

        /// <summary>
        /// Gets or sets consumer Secret.
        /// </summary>
        public string ConsumerSecret { get; set; }

        /// <summary>
        /// Gets or sets access token.
        /// </summary>
        public string AccessToken { get; set; }

        /// <summary>
        /// Gets or sets access token Secret.
        /// </summary>
        public string AccessTokenSecret { get; set; }

        /// <summary>
        /// Gets or sets access Request Token.
        /// </summary>
        public string RequestToken { get; set; }

        /// <summary>
        /// Gets or sets access Request Token Secret.
        /// </summary>
        public string RequestTokenSecret { get; set; }

        /// <summary>
        /// Gets or sets callback Uri.
        /// </summary>
        public string CallbackUri { get; set; }
    }
}
