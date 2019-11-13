// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Threading.Tasks;
using Microsoft.Graph;
using Microsoft.Toolkit.Services.MicrosoftGraph;

namespace Microsoft.Toolkit.Uwp.UI.Controls.Graph
{
    internal static class GraphServiceHelper
    {
        internal static async Task<GraphServiceClient> GetGraphServiceClientAsync()
        {
            #pragma warning disable CS0618 // Type or member is obsolete
            MicrosoftGraphService graphService = MicrosoftGraphService.Instance;
            #pragma warning restore CS0618 // Type or member is obsolete
            if (await graphService.TryLoginAsync())
            {
                return graphService.GraphProvider;
            }

            return null;
        }
    }
}
