// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// A <see cref="XamlCompositionBrush"/> that displays a tiled noise texture
    /// </summary>
    public sealed class TilesBrush : XamlCompositionEffectBrushBase
    {
        /// <summary>
        /// Gets or sets the <see cref="Uri"/> to the texture to use
        /// </summary>
        /// <remarks>This property must be initialized before using the brush</remarks>
        public Uri TextureUri { get; set; }

        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested() => PipelineBuilder.FromTiles(this.TextureUri);
    }
}
