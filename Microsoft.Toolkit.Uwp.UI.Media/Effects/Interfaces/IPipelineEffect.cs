// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// The base <see langword="interface"/> for all the builder effects to be used in a <see cref="PipelineBrush"/>
    /// </summary>
    public interface IPipelineEffect
    {
        /// <summary>
        /// Appends the current effect to the input <see cref="PipelineBuilder"/> instance.
        /// </summary>
        /// <param name="builder">The source <see cref="PipelineBuilder"/> instance to add the effect to.</param>
        /// <returns>A new <see cref="PipelineBuilder"/> with the new effects added to it.</returns>
        PipelineBuilder AppendToPipeline(PipelineBuilder builder);
    }
}
