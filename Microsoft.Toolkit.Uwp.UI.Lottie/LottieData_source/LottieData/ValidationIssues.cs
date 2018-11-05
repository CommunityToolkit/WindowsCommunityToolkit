// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData
{
    sealed class ValidationIssues
    {
        readonly HashSet<(string Code, string Description)> _issues = new HashSet<(string Code, string Description)>();

        internal (string Code, string Description)[] GetIssues() => _issues.ToArray();

        internal void LayerHasInPointAfterOutPoint(string layerName)
        {
            Report("LV0001", $"Layer {layerName} has in-point after out-point");
        }

        internal void LayerInCycle(string layerName)
        {
            Report("LV0002", $"Layer {layerName} is in a cycle");
        }

        internal void InvalidLayerParent(string layerParent)
        {
            Report("LV0003", $"Invalid layer parent: {layerParent}");
        }
        void Report(string code, string description)
        {
            _issues.Add((code, description));
        }

    }
}
