// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Effects.Interfaces;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A blend effect that merges the current pipeline with an input one
    /// </summary>
    public sealed class BlendEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the input pipeline to merge with the current instance
        /// </summary>
        public IList<IPipelineEffect> Input { get; set; } = new List<IPipelineEffect>();

        /// <summary>
        /// Gets or sets the blending mode to use (the default mode is <see cref="BlendEffectMode.Multiply"/>)
        /// </summary>
        public BlendEffectMode Mode { get; set; }

        /// <summary>
        /// Gets or sets the placement of the input pipeline with respect to the current one (the default is <see cref="Media.Placement.Foreground"/>)
        /// </summary>
        public Placement Placement { get; set; } = Placement.Foreground;
    }
}
