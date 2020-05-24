// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// A base <see langword="class"/> for an effect that exposes a single <see cref="float"/> parameter
    /// </summary>
    public abstract class ValueEffectBase : IPipelineEffect
    {
        /// <summary>
        /// Gets or sets the value of the parameter for the current effect
        /// </summary>
        public double Value { get; set; }

        /// <inheritdoc/>
        public abstract PipelineBuilder AppendToPipeline(PipelineBuilder builder);
    }
}
