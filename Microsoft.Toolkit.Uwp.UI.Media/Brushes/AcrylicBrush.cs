// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// A <see cref="XamlCompositionBrush"/> that implements an acrylic effect with customizable parameters
    /// </summary>
    public sealed class AcrylicBrush : XamlCompositionEffectBrushBase
    {
        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested()
        {
            switch (this.Source)
            {
                case AcrylicBackgroundSource.Backdrop: return PipelineBuilder.FromBackdropAcrylic(this.Tint, (float)this.TintMix, (float)this.BlurAmount, this.TextureUri);
                case AcrylicBackgroundSource.HostBackdrop: return PipelineBuilder.FromHostBackdropAcrylic(this.Tint, (float)this.TintMix, this.TextureUri);
                default: throw new ArgumentOutOfRangeException(nameof(this.Source), $"Invalid acrylic source: {this.Source}");
            }
        }

        /// <summary>
        /// Gets or sets the source mode for the effect
        /// </summary>
        public AcrylicBackgroundSource Source { get; set; }

        /// <summary>
        /// Gets or sets the blur amount for the effect
        /// </summary>
        /// <remarks>This property is ignored when the active mode is <see cref="AcrylicBackgroundSource.HostBackdrop"/></remarks>
        public double BlurAmount { get; set; }

        /// <summary>
        /// Gets or sets the tint for the effect
        /// </summary>
        public Color Tint { get; set; }

        /// <summary>
        /// Gets or sets the color for the tint effect
        /// </summary>
        public double TintMix { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="Uri"/> to the texture to use
        /// </summary>
        public Uri TextureUri { get; set; }
    }
}
