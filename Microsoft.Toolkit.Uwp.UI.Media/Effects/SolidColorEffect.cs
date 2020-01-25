// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Effects.Interfaces;
using Windows.UI;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A simple effect that renders a solid color on the available surface
    /// </summary>
    public sealed class SolidColorEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the color to display
        /// </summary>
        public Color Color { get; set; }
    }
}
