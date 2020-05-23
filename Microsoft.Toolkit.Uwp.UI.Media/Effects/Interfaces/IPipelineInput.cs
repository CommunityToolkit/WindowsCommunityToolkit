// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;

namespace Microsoft.Toolkit.Uwp.UI.Media.Effects
{
    /// <summary>
    /// The base <see langword="interface"/> for all the pipeline inputs to be used in a <see cref="PipelineBrush"/>.
    /// </summary>
    public interface IPipelineInput
    {
        /// <summary>
        /// Creates a new <see cref="PipelineBuilder"/> instance from the current input.
        /// </summary>
        /// <returns>A new <see cref="PipelineBuilder"/> starting from the current input.</returns>
        PipelineBuilder StartPipeline();
    }
}
