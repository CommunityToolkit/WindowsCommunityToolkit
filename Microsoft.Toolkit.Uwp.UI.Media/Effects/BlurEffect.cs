// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A gaussian blur effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.GaussianBlurEffect"/> effect</remarks>
    public sealed class BlurEffect : IPipelineEffect
    {
        /// <summary>
        /// The unique id for the effect, if <see cref="IsAnimatable"/> is set.
        /// </summary>
        private string id;

        /// <summary>
        /// The <see cref="CompositionBrush"/> in use, if any.
        /// </summary>
        private CompositionBrush brush;

        /// <summary>
        /// Gets or sets a value indicating whether the effect can be animated.
        /// </summary>
        public bool IsAnimatable { get; set; }

        private double amount;

        /// <summary>
        /// Gets or sets the blur amount for the effect (must be a positive value)
        /// </summary>
        public double Amount
        {
            get => this.amount;
            set => this.amount = Math.Max(value, 0);
        }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return IsAnimatable switch
            {
                true => builder.Blur((float)Amount, out this.id),
                false => builder.Blur((float)Amount)
            };
        }

        /// <inheritdoc/>
        void IPipelineEffect.NotifyCompositionBrushInUse(CompositionBrush brush)
        {
            this.brush = brush;
        }
    }
}
