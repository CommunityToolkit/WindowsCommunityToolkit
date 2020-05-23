// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// An effect that overlays a color layer over the current pipeline, with a specified intensity
    /// </summary>
    public sealed class ShadeEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the color to use
        /// </summary>
        public Color Color { get; set; }

        /// <summary>
        /// Gets or sets the intensity of the color layer
        /// </summary>
        public double Intensity { get; set; }
    }
}
