// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Graphics.Canvas;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

#nullable enable

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// A border effect useful for tiling a surface, it is not animatable directly, animate the incoming surface.
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.BorderEffect"/> effect</remarks>
    public sealed class BorderEffect : PipelineEffect
    {
        /// <summary>
        /// Gets or sets the wrapping behavior in the horizontal direction.
        /// </summary>
        public CanvasEdgeBehavior ExtendX { get; set; } = CanvasEdgeBehavior.Wrap;

        /// <summary>
        /// Gets or sets the wrapping behavior in the vertical direction.
        /// </summary>
        public CanvasEdgeBehavior ExtendY { get; set; } = CanvasEdgeBehavior.Wrap;

        /// <inheritdoc/>
        public override PipelineBuilder AppendToBuilder(PipelineBuilder builder)
        {
            return builder.Border(ExtendX, ExtendY);
        }
    }
}
