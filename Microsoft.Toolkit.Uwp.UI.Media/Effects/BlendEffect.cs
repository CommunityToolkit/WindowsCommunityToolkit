// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A blend effect that merges the current builder with an input one
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.BlendEffect"/> effect</remarks>
    public sealed class BlendEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the input to merge with the current instance
        /// </summary>
        public IPipelineInput Input { get; set; }

        /// <summary>
        /// Gets or sets the effects to apply to the input to merge with the current instance
        /// </summary>
        public List<IPipelineEffect> InputEffects { get; set; } = new List<IPipelineEffect>();

        /// <summary>
        /// Gets or sets the blending mode to use (the default mode is <see cref="ImageBlendMode.Multiply"/>)
        /// </summary>
        public ImageBlendMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the placement of the input builder with respect to the current one (the default is <see cref="Placement.Foreground"/>)
        /// </summary>
        public Placement Placement { get; set; } = Placement.Foreground;

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            PipelineBuilder inputPipeline = Input.StartPipeline();

            foreach (IPipelineEffect effect in InputEffects)
            {
                inputPipeline = effect.AppendToPipeline(inputPipeline);
            }

            return builder.Blend(inputPipeline, (BlendEffectMode)Mode, Placement);
        }
    }
}
