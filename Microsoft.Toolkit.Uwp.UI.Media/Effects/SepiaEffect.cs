// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A sepia effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.SepiaEffect"/> effect</remarks>
    public sealed class SepiaEffect : IPipelineEffect
    {
        private double intensity = 0.5;

        /// <summary>
        /// Gets or sets the intensity of the effect (defaults to 0.5, should be in the [0, 1] range).
        /// </summary>
        public double Intensity
        {
            get => this.intensity;
            set => this.intensity = Math.Clamp(value, 0, 1);
        }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.Sepia((float)Intensity);
        }
    }
}
