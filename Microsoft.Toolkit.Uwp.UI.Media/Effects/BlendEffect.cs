// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A blend effect that merges the current builder with an input one
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.BlendEffect"/> effect</remarks>
    [ContentProperty(Name = nameof(Effects))]
    public sealed class BlendEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the input to merge with the current instance (defaults to a <see cref="BackdropSourceExtension"/> with <see cref="Windows.UI.Xaml.Media.AcrylicBackgroundSource.Backdrop"/> source).
        /// </summary>
        public PipelineBuilder Source { get; set; }

        /// <summary>
        /// Gets or sets the effects to apply to the input to merge with the current instance
        /// </summary>
        public List<IPipelineEffect> Effects { get; set; } = new List<IPipelineEffect>();

        /// <summary>
        /// Gets or sets the blending mode to use (the default mode is <see cref="ImageBlendMode.Multiply"/>)
        /// </summary>
        public ImageBlendMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the placement of the input builder with respect to the current one (the default is <see cref="Media.Placement.Foreground"/>)
        /// </summary>
        public Placement Placement { get; set; } = Placement.Foreground;

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            PipelineBuilder inputBuilder = Source ?? PipelineBuilder.FromBackdrop();

            foreach (IPipelineEffect effect in this.Effects)
            {
                inputBuilder = effect.AppendToPipeline(inputBuilder);
            }

            return builder.Blend(inputBuilder, (BlendEffectMode)Mode, Placement);
        }
    }
}
