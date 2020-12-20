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

        /// <summary>
        /// Gets the unique id for the effect, if <see cref="IsAnimatable"/> is set.
        /// </summary>
        internal string Id { get; private set; }

        /// <summary>
        /// Gets <see cref="CompositionBrush"/> in use, if any.
        /// </summary>
        internal CompositionBrush Brush { get; private set; }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            if (IsAnimatable)
            {
                builder = builder.Blur((float)Amount, out string id);

                Id = id;

                return builder;
            }

            return builder.Blur((float)Amount);
        }

        /// <inheritdoc/>
        void IPipelineEffect.NotifyCompositionBrushInUse(CompositionBrush brush)
        {
            Brush = brush;
        }
    }
}
