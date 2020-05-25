// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A gaussian blur effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.GaussianBlurEffect"/> effect</remarks>
    public sealed class BlurEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the amount of gaussian blur to apply to the background.
        /// </summary>
        public double Amount { get; set; }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.Blur((float)Amount);
        }
    }
}
