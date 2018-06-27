using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal static class GraphServiceHelper
    {
        internal static async Task<GraphServiceClient> GetGraphServiceClient()
        {
            MicrosoftGraphService graphService = MicrosoftGraphService.Instance;
            if (await graphService.TryLoginAsync())
            {
                return graphService.GraphProvider;
            }

            return null;
        }
    }
}
