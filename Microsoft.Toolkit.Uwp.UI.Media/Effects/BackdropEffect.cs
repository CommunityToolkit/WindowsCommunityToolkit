﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Effects.Interfaces;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A backdrop effect that can sample from a specified source
    /// </summary>
    public sealed class BackdropEffect : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the backdrop source to use to render the effect
        /// </summary>
        public AcrylicBackgroundSource Source { get; set; }
    }
}
