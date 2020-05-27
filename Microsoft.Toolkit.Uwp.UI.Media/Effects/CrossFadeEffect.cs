﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Xaml.Markup;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A blend effect that merges the current builder with an input one
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.CrossFadeEffect"/> effect</remarks>
    [ContentProperty(Name = nameof(Effects))]
    public sealed class CrossFadeEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the input to merge with the current instance (defaults to a <see cref="BackdropSourceExtension"/> with <see cref="Windows.UI.Xaml.Media.AcrylicBackgroundSource.Backdrop"/> source).
        /// </summary>
        public PipelineBuilder Source { get; set; }

        /// <summary>
        /// Gets or sets the effects to apply to the input to merge with the current instance
        /// </summary>
        public List<IPipelineEffect> Effects { get; set; } = new List<IPipelineEffect>();

        private double factor = 0.5;

        /// <summary>
        /// Gets or sets the The cross fade factor to blend the input effects (default to 0.5, should be in the [0, 1] range)
        /// </summary>
        public double Factor
        {
            get => this.factor;
            set => this.factor = Math.Clamp(value, 0, 1);
        }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            PipelineBuilder inputBuilder = Source ?? PipelineBuilder.FromBackdrop();

            foreach (IPipelineEffect effect in this.Effects)
            {
                inputBuilder = effect.AppendToPipeline(inputBuilder);
            }

            return builder.CrossFade(inputBuilder, (float)Factor);
        }
    }
}
