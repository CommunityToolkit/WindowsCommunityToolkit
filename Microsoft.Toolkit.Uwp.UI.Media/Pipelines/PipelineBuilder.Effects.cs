// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Graphics.Canvas.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Extensions;
using Windows.Graphics.Effects;
using Windows.UI;
using Windows.UI.Composition;

namespace Microsoft.Toolkit.Uwp.UI.Media.Pipelines
{
    /// <summary>
    /// A <see langword="class"/> that allows to build custom effects pipelines and create <see cref="CompositionBrush"/> instances from them
    /// </summary>
    public sealed partial class PipelineBuilder
    {
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
            PipelineBuilder foreground, background;
            if (placement == Placement.Foreground)
            {
                foreground = this;
                background = pipeline;
            }
            else
            {
                foreground = pipeline;
                background = this;
            }

            async Task<IGraphicsEffectSource> Factory() => new BlendEffect
            {
                Foreground = await foreground.sourceProducer(),
                Background = await background.sourceProducer(),
                Mode = mode
            };

            return new PipelineBuilder(Factory, foreground, background);
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
            PipelineBuilder foreground, background;
            if (placement == Placement.Foreground)
            {
                foreground = this;
                background = pipeline;
            }
            else
            {
                foreground = pipeline;
                background = this;
            }

            async Task<IGraphicsEffectSource> Factory() => new CrossFadeEffect
            {
                CrossFade = factor,
                Source1 = await foreground.sourceProducer(),
                Source2 = await background.sourceProducer()
            };

            return new PipelineBuilder(Factory, foreground, background);
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
            PipelineBuilder foreground, background;
            if (placement == Placement.Foreground)
            {
                foreground = this;
                background = pipeline;
            }
            else
            {
                foreground = pipeline;
                background = this;
            }

            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new CrossFadeEffect
            {
                CrossFade = factor,
                Source1 = await foreground.sourceProducer(),
                Source2 = await background.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(CrossFadeEffect.CrossFade)}", value);

            return new PipelineBuilder(Factory, foreground, background, new[] { $"{id}.{nameof(CrossFadeEffect.CrossFade)}" });
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
            PipelineBuilder foreground, background;
            if (placement == Placement.Foreground)
            {
                foreground = this;
                background = pipeline;
            }
            else
            {
                foreground = pipeline;
                background = this;
            }

            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new CrossFadeEffect
            {
                CrossFade = factor,
                Source1 = await foreground.sourceProducer(),
                Source2 = await background.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(CrossFadeEffect.CrossFade)}", value, duration);

            return new PipelineBuilder(Factory, foreground, background, new[] { $"{id}.{nameof(CrossFadeEffect.CrossFade)}" });
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
            async Task<IGraphicsEffectSource> Factory() => factory(await this.sourceProducer(), await background.sourceProducer());

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
            async Task<IGraphicsEffectSource> Factory() => await factory(await this.sourceProducer(), await background.sourceProducer());

            return new PipelineBuilder(Factory, this, background, animations?.ToArray(), initializers?.ToDictionary(item => item.Name, item => item.Initializer));
        }

        /// <summary>
        /// Adds a new <see cref="GaussianBlurEffect"/> to the current pipeline
        /// </summary>
        /// <param name="blur">The blur amount to apply</param>
        /// <param name="mode">The <see cref="EffectBorderMode"/> parameter for the effect, defaults to <see cref="EffectBorderMode.Hard"/></param>
        /// <param name="optimization">The <see cref="EffectOptimization"/> parameter to use, defaults to <see cref="EffectOptimization.Balanced"/></param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Blur(float blur, EffectBorderMode mode = EffectBorderMode.Hard, EffectOptimization optimization = EffectOptimization.Balanced)
        {
            async Task<IGraphicsEffectSource> Factory() => new GaussianBlurEffect
            {
                BlurAmount = blur,
                BorderMode = mode,
                Optimization = optimization,
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Adds a new <see cref="GaussianBlurEffect"/> to the current pipeline
        /// </summary>
        /// <param name="blur">The initial blur amount</param>
        /// <param name="setter">The optional blur setter for the effect</param>
        /// <param name="mode">The <see cref="EffectBorderMode"/> parameter for the effect, defaults to <see cref="EffectBorderMode.Hard"/></param>
        /// <param name="optimization">The <see cref="EffectOptimization"/> parameter to use, defaults to <see cref="EffectOptimization.Balanced"/></param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Blur(float blur, out EffectSetter<float> setter, EffectBorderMode mode = EffectBorderMode.Hard, EffectOptimization optimization = EffectOptimization.Balanced)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new GaussianBlurEffect
            {
                BlurAmount = blur,
                BorderMode = mode,
                Optimization = optimization,
                Source = await this.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(GaussianBlurEffect.BlurAmount)}", value);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(GaussianBlurEffect.BlurAmount)}" });
        }

        /// <summary>
        /// Adds a new <see cref="GaussianBlurEffect"/> to the current pipeline
        /// </summary>
        /// <param name="blur">The initial blur amount</param>
        /// <param name="animation">The optional blur animation for the effect</param>
        /// <param name="mode">The <see cref="EffectBorderMode"/> parameter for the effect, defaults to <see cref="EffectBorderMode.Hard"/></param>
        /// <param name="optimization">The <see cref="EffectOptimization"/> parameter to use, defaults to <see cref="EffectOptimization.Balanced"/></param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Blur(float blur, out EffectAnimation<float> animation, EffectBorderMode mode = EffectBorderMode.Hard, EffectOptimization optimization = EffectOptimization.Balanced)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new GaussianBlurEffect
            {
                BlurAmount = blur,
                BorderMode = mode,
                Optimization = optimization,
                Source = await this.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(GaussianBlurEffect.BlurAmount)}", value, duration);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(GaussianBlurEffect.BlurAmount)}" });
        }

        /// <summary>
        /// Adds a new <see cref="SaturationEffect"/> to the current pipeline
        /// </summary>
        /// <param name="saturation">The saturation amount for the new effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Saturation(float saturation)
        {
            async Task<IGraphicsEffectSource> Factory() => new SaturationEffect
            {
                Saturation = saturation,
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Adds a new <see cref="SaturationEffect"/> to the current pipeline
        /// </summary>
        /// <param name="saturation">The initial saturation amount for the new effect</param>
        /// <param name="setter">The optional saturation setter for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Saturation(float saturation, out EffectSetter<float> setter)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new SaturationEffect
            {
                Saturation = saturation,
                Source = await this.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(SaturationEffect.Saturation)}", value);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(SaturationEffect.Saturation)}" });
        }

        /// <summary>
        /// Adds a new <see cref="SaturationEffect"/> to the current pipeline
        /// </summary>
        /// <param name="saturation">The initial saturation amount for the new effect</param>
        /// <param name="animation">The optional saturation animation for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Saturation(float saturation, out EffectAnimation<float> animation)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new SaturationEffect
            {
                Saturation = saturation,
                Source = await this.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(SaturationEffect.Saturation)}", value, duration);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(SaturationEffect.Saturation)}" });
        }

        /// <summary>
        /// Adds a new <see cref="SepiaEffect"/> to the current pipeline
        /// </summary>
        /// <param name="intensity">The sepia effect intensity for the new effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Sepia(float intensity)
        {
            async Task<IGraphicsEffectSource> Factory() => new SepiaEffect
            {
                Intensity = intensity,
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Adds a new <see cref="SepiaEffect"/> to the current pipeline
        /// </summary>
        /// <param name="intensity">The sepia effect intensity for the new effect</param>
        /// <param name="setter">The optional sepia intensity setter for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Sepia(float intensity, out EffectSetter<float> setter)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new SepiaEffect
            {
                Intensity = intensity,
                Source = await this.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(SepiaEffect.Intensity)}", value);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(SepiaEffect.Intensity)}" });
        }

        /// <summary>
        /// Adds a new <see cref="SepiaEffect"/> to the current pipeline
        /// </summary>
        /// <param name="intensity">The sepia effect intensity for the new effect</param>
        /// <param name="animation">The sepia intensity animation for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Sepia(float intensity, out EffectAnimation<float> animation)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new SepiaEffect
            {
                Intensity = intensity,
                Source = await this.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(SepiaEffect.Intensity)}", value, duration);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(SepiaEffect.Intensity)}" });
        }

        /// <summary>
        /// Adds a new <see cref="OpacityEffect"/> to the current pipeline
        /// </summary>
        /// <param name="opacity">The opacity value to apply to the pipeline</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Opacity(float opacity)
        {
            async Task<IGraphicsEffectSource> Factory() => new OpacityEffect
            {
                Opacity = opacity,
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Adds a new <see cref="OpacityEffect"/> to the current pipeline
        /// </summary>
        /// <param name="opacity">The opacity value to apply to the pipeline</param>
        /// <param name="setter">The optional opacity setter for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Opacity(float opacity, out EffectSetter<float> setter)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new OpacityEffect
            {
                Opacity = opacity,
                Source = await this.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(OpacityEffect.Opacity)}", value);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(OpacityEffect.Opacity)}" });
        }

        /// <summary>
        /// Adds a new <see cref="OpacityEffect"/> to the current pipeline
        /// </summary>
        /// <param name="opacity">The opacity value to apply to the pipeline</param>
        /// <param name="animation">The optional opacity animation for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Opacity(float opacity, out EffectAnimation<float> animation)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new OpacityEffect
            {
                Opacity = opacity,
                Source = await this.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(OpacityEffect.Opacity)}", value, duration);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(OpacityEffect.Opacity)}" });
        }

        /// <summary>
        /// Applies an exposure effect on the current pipeline
        /// </summary>
        /// <param name="amount">The amount of exposure to apply over the current effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Exposure(float amount)
        {
            async Task<IGraphicsEffectSource> Factory() => new ExposureEffect
            {
                Exposure = amount,
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Applies an exposure effect on the current pipeline
        /// </summary>
        /// <param name="amount">The initial exposure of tint to apply over the current effect</param>
        /// <param name="setter">The optional amount setter for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Exposure(float amount, out EffectSetter<float> setter)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new ExposureEffect
            {
                Exposure = amount,
                Source = await this.sourceProducer(),
                Name = id
            };

            setter = (brush, value) => brush.Properties.InsertScalar($"{id}.{nameof(ExposureEffect.Exposure)}", value);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(ExposureEffect.Exposure)}" });
        }

        /// <summary>
        /// Applies an exposure effect on the current pipeline
        /// </summary>
        /// <param name="amount">The initial exposure of tint to apply over the current effect</param>
        /// <param name="animation">The optional amount animation for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Exposure(float amount, out EffectAnimation<float> animation)
        {
            string id = Guid.NewGuid().ToUppercaseAsciiLetters();

            async Task<IGraphicsEffectSource> Factory() => new ExposureEffect
            {
                Exposure = amount,
                Source = await this.sourceProducer(),
                Name = id
            };

            animation = (brush, value, duration) => brush.StartAnimationAsync($"{id}.{nameof(ExposureEffect.Exposure)}", value, duration);

            return new PipelineBuilder(this, Factory, new[] { $"{id}.{nameof(ExposureEffect.Exposure)}" });
        }

        /// <summary>
        /// Applies a tint color on the current pipeline
        /// </summary>
        /// <param name="color">The tint color to use</param>
        /// <param name="mix">The amount of tint to apply over the current effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Tint(Color color, float mix)
        {
            return FromColor(color).CrossFade(this, mix);
        }

        /// <summary>
        /// Applies a tint color on the current pipeline
        /// </summary>
        /// <param name="color">The tint color to use</param>
        /// <param name="mix">The initial amount of tint to apply over the current effect</param>
        /// <param name="setter">The optional tint mix setter for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Tint(Color color, float mix, out EffectSetter<float> setter)
        {
            return FromColor(color).CrossFade(this, mix, out setter);
        }

        /// <summary>
        /// Applies a tint color on the current pipeline
        /// </summary>
        /// <param name="color">The tint color to use</param>
        /// <param name="mix">The initial amount of tint to apply over the current effect</param>
        /// <param name="animation">The optional tint mix animation for the effect</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Tint(Color color, float mix, out EffectAnimation<float> animation)
        {
            return FromColor(color).CrossFade(this, mix, out animation);
        }

        /// <summary>
        /// Applies a luminance to alpha effect on the current pipeline
        /// </summary>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder LuminanceToAlpha()
        {
            async Task<IGraphicsEffectSource> Factory() => new LuminanceToAlphaEffect
            {
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Applies an invert effect on the current pipeline
        /// </summary>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Invert()
        {
            async Task<IGraphicsEffectSource> Factory() => new InvertEffect
            {
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Applies a grayscale on the current pipeline
        /// </summary>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Grayscale()
        {
            async Task<IGraphicsEffectSource> Factory() => new GrayscaleEffect
            {
                Source = await this.sourceProducer()
            };

            return new PipelineBuilder(this, Factory);
        }

        /// <summary>
        /// Applies a custom effect to the current pipeline
        /// </summary>
        /// <param name="factory">A <see cref="Func{T, TResult}"/> that takes the current <see cref="IGraphicsEffectSource"/> instance and produces a new effect to display</param>
        /// <param name="animations">The list of optional animatable properties in the returned effect</param>
        /// <param name="initializers">The list of source parameters that require deferred initialization (see <see cref="CompositionEffectSourceParameter"/> for more info)</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Effect(
            Func<IGraphicsEffectSource, IGraphicsEffectSource> factory,
            IEnumerable<string> animations = null,
            IEnumerable<BrushProvider> initializers = null)
        {
            async Task<IGraphicsEffectSource> Factory() => factory(await this.sourceProducer());

            return new PipelineBuilder(this, Factory, animations?.ToArray(), initializers?.ToDictionary(item => item.Name, item => item.Initializer));
        }

        /// <summary>
        /// Applies a custom effect to the current pipeline
        /// </summary>
        /// <param name="factory">An asynchronous <see cref="Func{T, TResult}"/> that takes the current <see cref="IGraphicsEffectSource"/> instance and produces a new effect to display</param>
        /// <param name="animations">The list of optional animatable properties in the returned effect</param>
        /// <param name="initializers">The list of source parameters that require deferred initialization (see <see cref="CompositionEffectSourceParameter"/> for more info)</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance to use to keep adding new effects</returns>
        [Pure]
        public PipelineBuilder Effect(
            Func<IGraphicsEffectSource, Task<IGraphicsEffectSource>> factory,
            IEnumerable<string> animations = null,
            IEnumerable<BrushProvider> initializers = null)
        {
            async Task<IGraphicsEffectSource> Factory() => await factory(await this.sourceProducer());

            return new PipelineBuilder(this, Factory, animations?.ToArray(), initializers?.ToDictionary(item => item.Name, item => item.Initializer));
        }
    }
}
