using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    /// <summary>
    /// Arguments relating to a sign-in event of AADlogin control
    /// </summary>
    public class SignInEventArgs
    {
        internal SignInEventArgs()
        {
        }

        /// <summary>
        /// Gets the graph service client with authorized token.
        /// </summary>
        public GraphServiceClient GraphClient
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the authorized access token.
        /// </summary>
        public string GraphAccessToken
        {
            get; internal set;
        }

        /// <summary>
        /// Gets the unique identifier for current signed in user.
        /// </summary>
        public string CurrentSignInUserId
        {
            get; internal set;
        }
    }
}
