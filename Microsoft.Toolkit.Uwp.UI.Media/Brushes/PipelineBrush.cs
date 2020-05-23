// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Microsoft.Toolkit.Uwp.UI.Media.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Xaml.Media;

namespace Microsoft.Toolkit.Uwp.UI.Media
{
    /// <summary>
    /// A <see cref="Brush"/> that renders a customizable Composition/Win2D effects pipeline
    /// </summary>
    public sealed class PipelineBrush : XamlCompositionEffectBrushBase
    {
        /// <summary>
        /// Gets or sets the input for the current pipeline
        /// </summary>
        public IPipelineInput Input { get; set; }

        /// <summary>
        /// Gets or sets the collection of effects to use in the current pipeline
        /// </summary>
        public IList<IPipelineNode> Effects { get; set; } = new List<IPipelineNode>();

        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested()
        {
            PipelineBuilder builder = Input.StartPipeline();

            foreach (IPipelineNode effect in Effects)
            {
                builder = effect.AppendToPipeline(builder);
            }

            return builder;
        }
    }
}
