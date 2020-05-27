// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// An effect that overlays a color layer over the current builder, with a specified intensity
    /// </summary>
    public sealed class ShadeEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the color to use
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the intensity of the color layer (default to 0.5, should be in the [0, 1] range)
        /// </summary>
        public double Intensity { get; set; } = 0.5;

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.Shade(Color, (float)Intensity);
        }
    }
}
