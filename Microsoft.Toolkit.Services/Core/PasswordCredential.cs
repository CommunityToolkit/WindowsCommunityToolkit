namespace Microsoft.Toolkit.Services.Core
{
    /// <summary>
    /// PasswordCredential class composed of UserName and Password, both strings.
    /// </summary>
    public class PasswordCredential
    {
        /// <summary>
        /// Gets or sets the username from login form
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password from login form
        /// </summary>
        public string Password { get; set; }
    }
}
