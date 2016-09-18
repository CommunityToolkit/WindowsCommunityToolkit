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
