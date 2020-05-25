// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A sepia effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.SepiaEffect"/> effect</remarks>
    public sealed class SepiaEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the intensity of the effect (defaults to 0.5, should be in the [0, 1] range).
        /// </summary>
        public double Intensity { get; set; } = 0.5;

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.Sepia((float)Intensity);
        }
    }
}
