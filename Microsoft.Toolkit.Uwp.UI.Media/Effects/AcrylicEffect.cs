// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Microsoft.Toolkit.Uwp.UI.Media.Effects.Interfaces;
using Microsoft.UI.Xaml.Media;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A custom acrylic effect that can be inserted into a pipeline
    /// </summary>
    /// <remarks>This effect mirrors the look of the default <see cref="AcrylicBrush"/> implementation</remarks>
    public sealed class AcrylicEffect : IPipelineEffect
    {
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
