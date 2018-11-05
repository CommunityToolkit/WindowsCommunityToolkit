// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.WinCompData.Tools
{
    /// <summary>
    /// Analyzes a tree to determine the features of the runtime required to instantiate it.
    /// </summary>
#if !WINDOWS_UWP
    public
#endif
    sealed class ApiCompatibility
    {
        ApiCompatibility(bool requiresCompositionGeometricClip)
        {
            RequiresCompositionGeometricClip = requiresCompositionGeometricClip;
        }

        /// <summary>
        /// Analyzes the given tree and returns information about its compatibility with a runtime.
        /// </summary>
        public static ApiCompatibility Analyze(CompositionObject graphRoot)
        {
            var graph = Graph.FromCompositionObject(graphRoot, includeVertices: false);

            var requiresCompositionGeometricClip = graph.CompositionObjectNodes.Where(obj => obj.Object.Type == CompositionObjectType.CompositionGeometricClip).Any();
            // Require CompostionGeometryClip anyway - this ensures that we are never compatible with
            // RS4 (geometries are flaky in RS4, and CompositionGeometryClip is new in RS5).
            requiresCompositionGeometricClip = true;

            return new ApiCompatibility(requiresCompositionGeometricClip: requiresCompositionGeometricClip);
        }

        public bool RequiresCompositionGeometricClip { get; }
    }
}
