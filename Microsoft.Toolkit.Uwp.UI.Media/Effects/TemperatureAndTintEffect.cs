﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Effects.Abstract;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A temperature and tint effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.TemperatureAndTintEffect"/> effect</remarks>
    public sealed class TemperatureAndTintEffect : ValueEffectBase
    {
        /// <summary>
        /// Gets or sets the value of the temperature for the current effect
        /// </summary>
        public double Temperature { get; set; }

        /// <summary>
        /// Gets or sets the value of the tint for the current effect
        /// </summary>
        public double Tint { get; set; }
    }
}
