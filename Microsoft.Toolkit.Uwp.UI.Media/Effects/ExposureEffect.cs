﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// An exposure effect
    /// </summary>
    /// <remarks>This effect maps to the Win2D <see cref="Graphics.Canvas.Effects.ExposureEffect"/> effect</remarks>
    public sealed class ExposureEffect : ValueEffectBase
    {
        /// <inheritdoc/>
        public override PipelineBuilder AppendToPipeline(PipelineBuilder builder)
        {
            return builder.Exposure((float)Value);
        }
    }
}
