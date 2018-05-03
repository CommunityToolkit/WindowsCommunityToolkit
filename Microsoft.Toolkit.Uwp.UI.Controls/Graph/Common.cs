using System.Net.Http.Headers;
using System.Threading.Tasks;
using Microsoft.Graph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal class Common
    {
        private const string GraphAPIBaseUrl = "https://graph.microsoft.com/v1.0";

        internal static GraphServiceClient GetAuthenticatedClient(string accessToken)
        {
            if (string.IsNullOrEmpty(accessToken))
            {
                return null;
            }

            var graphClient = new GraphServiceClient(
                    GraphAPIBaseUrl,
                    new DelegateAuthenticationProvider(
                        (requestMessage) =>
                        {
                            requestMessage.Headers.Authorization = new AuthenticationHeaderValue("bearer", accessToken);

                            return Task.FromResult(0);
                        }));

            return graphClient;
        }
    }
}
