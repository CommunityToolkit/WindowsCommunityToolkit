// Copyright(c) Microsoft Corporation.All rights reserved.
// Licensed under the MIT License.

using System.Collections.Generic;
using System.Linq;

namespace Microsoft.Toolkit.Uwp.UI.Lottie.LottieData.Serialization
{
    sealed class ParsingIssues
    {
        readonly HashSet<(string Code, string Description)> _issues = new HashSet<(string Code, string Description)>();

        internal (string Code, string Description)[] GetIssues() => _issues.ToArray();

        internal void FailedToParseJson(string message)
        {
            Report("LP0001", $"Failed to parse JSON. {message}");

        }

        internal void FatalError(string message)
        {
            Report("LP0002", $"Fatal error: {message}");
        }

        internal void AssetType(string type)
        {
            Report("LP0005", $"Unsupported asset type: {type}");
        }

        internal void LayerWithRenderFalse()
        {
            Report("LP0006", "Layer with render = false");
        }

        internal void IllustratorLayers()
        {
            Report("LP0007", "Illustrator layers");
        }

        internal void LayerEffects()
        {
            Report("LP0008", "Layer effects");
        }

        internal void Mattes()
        {
            Report("LP0009", "Mattes");
        }

        // LP0010: Masks has been removed. We now support masks.

        internal void TimeRemappingOfPreComps()
        {
            Report("LP0011", "Time remapping of precomp layers");
        }

        internal void UnexpectedShapeContentType(string type)
        {
            Report("LP0012", $"Unexpected shape content type: {type}");
        }

        internal void GradientStrokes()
        {
            Report("LP0013", "Gradient strokes");
        }

        internal void PolystarAnimation(string property)
        {
            Report("LP0014", $"Polystar {property} animation");
        }

        internal void Expressions()
        {
            Report("LP0015", "Expressions");
        }

        internal void IgnoredField(string field)
        {
            Report("LP0016", $"Ignored field: {field}");
        }

        internal void UnexpectedField(string field)
        {
            Report("LP0017", $"Unexpected field: {field}");
        }

        void Report(string code, string description)
        {
            _issues.Add((code, description));
        }

    }
}

