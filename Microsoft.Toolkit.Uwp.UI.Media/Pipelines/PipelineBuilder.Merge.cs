// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions;
using Windows.Graphics.Effects;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media.Pipelines
{
    /// <summary>
    /// A <see langword="class"/> that allows to build custom effects pipelines and create <see cref="CompositionBrush"/> instances from them
    /// </summary>
    public sealed partial class PipelineBuilder
    {
        /// <summary>
        /// Gets the aligned pipelines to merge for a given operation
        /// </summary>
        /// <param name="left">The left pipeline to merge</param>
        /// <param name="right">The right pipeline to merge</param>
        /// <param name="placement">The placemeht to use with the two input pipelines</param>
        /// <returns>A <see cref="ValueTuple{T1,T2}"/> instance with the aligned pipelines</returns>
        [Pure]
        [SuppressMessage("StyleCop.CSharp.SpacingRules", "SA1008", Justification = "ValueTuple<T1,T2> return type")]
        private static (PipelineBuilder Foreground, PipelineBuilder Background) GetMergePipeline(
            PipelineBuilder left,
            PipelineBuilder right,
            Placement placement)
        {
            return placement switch
            {
                Placement.Foreground => (left, right),
                Placement.Background => (right, left),
                _ => throw new ArgumentException($"Invalid placement value: {placement}")
            };
        }

        /// <summary>
        /// Blends two pipelines using a <see cref="BlendEffect"/> instance with the specified mode
        /// </summary>
        /// <param name="pipeline">The second <see cref="PipelineBuilder"/> instance to blend</param>
        /// <param name="mode">The desired <see cref="BlendEffectMode"/> to use to blend the input pipelines</param>
        /// <param name="placement">The placemeht to use with the two input pipelines</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Blend(PipelineBuilder pipeline, BlendEffectMode mode, Placement placement = Placement.Foreground)
        {
            var pipelines = GetMergePipeline(this, pipeline, placement);

            async ValueTask<IGraphicsEffectSource> Factory() => new BlendEffect
            {
                Foreground = await pipelines.Foreground.sourceProducer(),
                Background = await pipelines.Background.sourceProducer(),
                Mode = mode
            };

            return new PipelineBuilder(Factory, pipelines.Foreground, pipelines.Background);
        }

        /// <summary>
        /// Cross fades two pipelines using an <see cref="CrossFadeEffect"/> instance
        /// </summary>
        /// <param name="pipeline">The second <see cref="PipelineBuilder"/> instance to cross fade</param>
        /// <param name="factor">The cross fade factor to blend the input effects</param>
        /// <param name="placement">The placement to use with the two input pipelines</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder CrossFade(PipelineBuilder pipeline, float factor = 0.5f, Placement placement = Placement.Foreground)
        {
            var pipelines = GetMergePipeline(this, pipeline, placement);

            async ValueTask<IGraphicsEffectSource> Factory() => new CrossFadeEffect
            {
                CrossFade = factor,
                Source1 = await pipelines.Foreground.sourceProducer(),
                Source2 = await pipelines.Background.sourceProducer()
            };

            return new PipelineBuilder(Factory, pipelines.Foreground, pipelines.Background);
        }

        /// <summary>
        /// Cross fades two pipelines using an <see cref="CrossFadeEffect"/> instance
        /// </summary>
        /// <param name="pipeline">The second <see cref="PipelineBuilder"/> instance to cross fade</param>
        /// <param name="factor">The cross fade factor to blend the input effects</param>
        /// <param name="setter">The optional blur setter for the effect</param>
        /// <param name="placement">The placement to use with the two input pipelines</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder CrossFade(PipelineBuilder pipeline, float factor, out EffectSetter<float> setter, Placement placement = Placement.Foreground)
        {
            var pipelines = GetMergePipeline(this, pipeline, placement);

            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async ValueTask<IGraphicsEffectSource> Factory() => new CrossFadeEffect
            {
                CrossFade = factor,
                Source1 = await pipelines.Foreground.sourceProducer(),
                Source2 = await pipelines.Background.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(CrossFadeEffect.CrossFade)}", value);

            return new PipelineBuilder(Factory, pipelines.Foreground, pipelines.Background, new[] { $"{id}.{nameof(CrossFadeEffect.CrossFade)}" });
        }

        /// <summary>
        /// Cross fades two pipelines using an <see cref="CrossFadeEffect"/> instance
        /// </summary>
        /// <param name="pipeline">The second <see cref="PipelineBuilder"/> instance to cross fade</param>
        /// <param name="factor">The cross fade factor to blend the input effects</param>
        /// <param name="animation">The optional blur animation for the effect</param>
        /// <param name="placement">The placement to use with the two input pipelines</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder CrossFade(PipelineBuilder pipeline, float factor, out EffectAnimation<float> animation, Placement placement = Placement.Foreground)
        {
            var pipelines = GetMergePipeline(this, pipeline, placement);

            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async ValueTask<IGraphicsEffectSource> Factory() => new CrossFadeEffect
            {
                CrossFade = factor,
                Source1 = await pipelines.Foreground.sourceProducer(),
                Source2 = await pipelines.Background.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(CrossFadeEffect.CrossFade)}", value, duration);

            return new PipelineBuilder(Factory, pipelines.Foreground, pipelines.Background, new[] { $"{id}.{nameof(CrossFadeEffect.CrossFade)}" });
        }

        /// <summary>
        /// Blends two pipelines using the provided <see cref="Func{T1, T2, TResult}"/> to do so
        /// </summary>
        /// <param name="factory">The blend function to use</param>
        /// <param name="background">The background pipeline to blend with the current instance</param>
        /// <param name="animations">The list of optional animatable properties in the returned effect</param>
        /// <param name="initializers">The list of source parameters that require deferred initialization (see <see cref="CompositionEffectSourceParameter"/> for more info)</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Merge(
            Func<IGraphicsEffectSource, IGraphicsEffectSource, IGraphicsEffectSource> factory,
            PipelineBuilder background,
            IEnumerable<string> animations = null,
            IEnumerable<BrushProvider> initializers = null)
        {
            async ValueTask<IGraphicsEffectSource> Factory() => factory(await this.sourceProducer(), await background.sourceProducer());

            return new PipelineBuilder(Factory, this, background, animations?.ToArray(), initializers?.ToDictionary(item => item.Name, item => item.Initializer));
        }

        /// <summary>
        /// Blends two pipelines using the provided asynchronous <see cref="Func{T1, T2, TResult}"/> to do so
        /// </summary>
        /// <param name="factory">The asynchronous blend function to use</param>
        /// <param name="background">The background pipeline to blend with the current instance</param>
        /// <param name="animations">The list of optional animatable properties in the returned effect</param>
        /// <param name="initializers">The list of source parameters that require deferred initialization (see <see cref="CompositionEffectSourceParameter"/> for more info)</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Merge(
            Func<IGraphicsEffectSource, IGraphicsEffectSource, Task<IGraphicsEffectSource>> factory,
            PipelineBuilder background,
            IEnumerable<string> animations = null,
            IEnumerable<BrushProvider> initializers = null)
        {
            async ValueTask<IGraphicsEffectSource> Factory() => await factory(await this.sourceProducer(), await background.sourceProducer());

            return new PipelineBuilder(Factory, this, background, animations?.ToArray(), initializers?.ToDictionary(item => item.Name, item => item.Initializer));
        }
    }
}
