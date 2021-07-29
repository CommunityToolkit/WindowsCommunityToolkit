﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// An exposure effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.ExposureEffect"/> effect</remarks>
    public sealed class ExposureEffect : IPipelineEffect
    {
        private double amount;

        /// <summary>
        /// Gets or sets the amount of exposure to apply to the background (defaults to 0, should be in the [-2, 2] range).
        /// </summary>
        public double Amount
        {
            get => this.amount;
            set => this.amount = Math.Clamp(value, -2, 2);
        }

        /// <inheritdoc/>
        public PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.Exposure((float)Amount);
        }
    }
}
