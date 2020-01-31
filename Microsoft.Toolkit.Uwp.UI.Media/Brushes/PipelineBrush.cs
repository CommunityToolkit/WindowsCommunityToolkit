// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.Contracts;
using System.Linq;
using Microsoft.Toolkit.Uwp.UI.Media.Brushes.Base;
using Microsoft.Toolkit.Uwp.UI.Media.Effects;
using Microsoft.Toolkit.Uwp.UI.Media.Effects.Interfaces;
using Microsoft.Toolkit.Uwp.UI.Media.Pipelines;
using Windows.UI.Xaml.Media;
using BlendEffect = Microsoft.Toolkit.Uwp.UI.Media.Effects.BlendEffect;
using LuminanceToAlphaEffect = Microsoft.Toolkit.Uwp.UI.Media.Effects.LuminanceToAlphaEffect;
using OpacityEffect = Microsoft.Toolkit.Uwp.UI.Media.Effects.OpacityEffect;
using SaturationEffect = Microsoft.Toolkit.Uwp.UI.Media.Effects.SaturationEffect;
using TileEffect = Microsoft.Toolkit.Uwp.UI.Media.Effects.TileEffect;

namespace Microsoft.Toolkit.Uwp.UI.Media.Brushes
{
    /// <summary>
    /// A <see cref="Brush"/> that renders a customizable Composition/Win2D effects pipeline
    /// </summary>
    public sealed class PipelineBrush : XamlCompositionEffectBrushBase
    {
        /// <summary>
        /// Builds a new effects pipeline from the input effects sequence
        /// </summary>
        /// <param name="effects">The input collection of <see cref="IPipelineEffect"/> instance</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance with the items in <paramref name="effects"/></returns>
        [Pure]
        private static PipelineBuilder Build(IList<IPipelineEffect> effects)
        {
            if (effects.Count == 0)
            {
                throw new ArgumentException("An effects pipeline can't be empty");
            }

            return effects.Skip(1).Aggregate(Start(effects[0]), (b, e) => Append(e, b));
        }

        /// <summary>
        /// Starts a new composition pipeline from the given effect
        /// </summary>
        /// <param name="effect">The initial <see cref="IPipelineEffect"/> instance</param>
        /// <returns>A new <see cref="PipelineBuilder"/> instance starting from <paramref name="effect"/></returns>
        [Pure]
        private static PipelineBuilder Start(IPipelineEffect effect)
        {
            switch (effect)
            {
                case BackdropEffect backdrop when backdrop.Source == AcrylicBackgroundSource.Backdrop:
                    return PipelineBuilder.FromBackdrop();
                case BackdropEffect backdrop when backdrop.Source == AcrylicBackgroundSource.HostBackdrop:
                    return PipelineBuilder.FromHostBackdrop();
                case SolidColorEffect color:
                    return PipelineBuilder.FromColor(color.Color);
                case ImageEffect image:
                    return PipelineBuilder.FromImage(image.Uri, image.DPIMode, image.CacheMode);
                case TileEffect tile:
                    return PipelineBuilder.FromTiles(tile.Uri, tile.DPIMode, tile.CacheMode);
                case AcrylicEffect acrylic when acrylic.Source == AcrylicBackgroundSource.Backdrop:
                    return PipelineBuilder.FromBackdropAcrylic(acrylic.Tint, (float)acrylic.TintMix, (float)acrylic.BlurAmount, acrylic.TextureUri);
                case AcrylicEffect acrylic when acrylic.Source == AcrylicBackgroundSource.HostBackdrop:
                    return PipelineBuilder.FromHostBackdropAcrylic(acrylic.Tint, (float)acrylic.TintMix, acrylic.TextureUri);
                default:
                    throw new ArgumentException($"Invalid initial pipeline effect: {effect.GetType()}");
            }
        }

        /// <summary>
        /// Appends an effect to an existing composition pipeline
        /// </summary>
        /// <param name="effect">The <see cref="IPipelineEffect"/> instance to append to the current pipeline</param>
        /// <param name="builder">The target <see cref="PipelineBuilder"/> instance to modify</param>
        /// <returns>The target <see cref="PipelineBuilder"/> instance in use</returns>
        private static PipelineBuilder Append(IPipelineEffect effect, PipelineBuilder builder)
        {
            switch (effect)
            {
                case OpacityEffect opacity:
                    return builder.Opacity((float)opacity.Value);
                case LuminanceToAlphaEffect _:
                    return builder.LuminanceToAlpha();
                case InvertEffect _:
                    return builder.Invert();
                case GrayscaleEffect _:
                    return builder.Grayscale();
                case ExposureEffect exposure:
                    return builder.Exposure((float)exposure.Value);
                case SepiaEffect sepia:
                    return builder.Sepia((float)sepia.Value);
                case ShadeEffect shade:
                    return builder.Shade(shade.Color, (float)shade.Intensity);
                case HueRotationEffect hueRotation:
                    return builder.HueRotation((float)hueRotation.Angle);
                case TintEffect tint:
                    return builder.Tint(tint.Color);
                case TemperatureAndTintEffect temperatureAndTint:
                    return builder.TemperatureAndTint((float)temperatureAndTint.Temperature, (float)temperatureAndTint.Tint);
                case BlurEffect blur:
                    return builder.Blur((float)blur.Value);
                case SaturationEffect saturation:
                    return builder.Saturation((float)saturation.Value);
                case BlendEffect blend:
                    return builder.Blend(Build(blend.Input), blend.Mode, blend.Placement);
                default:
                    throw new ArgumentException($"Invalid pipeline effect: {effect.GetType()}");
            }
        }

        /// <inheritdoc/>
        protected override PipelineBuilder OnBrushRequested()
        {
            return Build(this.Effects);
        }

        /// <summary>
        /// Gets or sets the collection of effects to use in the current pipeline
        /// </summary>
        public IList<IPipelineEffect> Effects { get; set; } = new List<IPipelineEffect>();
    }
}
