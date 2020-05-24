// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// An effect that renders a standard 8bit SDR color on the available surface
    /// </summary>
    public sealed class SolidColorSource : IPipelineSource
    {
        /// <summary>
        /// Gets or sets the color to display
        /// </summary>
        public Color Color { get; set; }

        /// <inheritdoc/>
        public PipelineBuilder StartPipeline()
        {
            return PipelineBuilder.FromColor(Color);
        }
    }
}
