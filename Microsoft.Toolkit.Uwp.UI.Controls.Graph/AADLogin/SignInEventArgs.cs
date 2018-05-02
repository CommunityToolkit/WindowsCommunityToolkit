using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    public class SignInEventArgs
    {
        internal SignInEventArgs() { }

        public GraphServiceClient GraphClient
        {
            get; internal set;
        }

        public string GraphAccessToken
        {
            get; internal set;
        }

        public string CurrentSignInUserId
        {
            get; internal set;
        }
    }
}
