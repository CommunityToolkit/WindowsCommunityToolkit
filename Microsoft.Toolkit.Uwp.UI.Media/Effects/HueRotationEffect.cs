// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A hue rotation effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.HueRotationEffect"/> effect</remarks>
    public sealed class HueRotationEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the angle to rotate the hue, in radians
        /// </summary>
        public double Angle { get; set; }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.HueRotation((float)Angle);
        }
    }
}
